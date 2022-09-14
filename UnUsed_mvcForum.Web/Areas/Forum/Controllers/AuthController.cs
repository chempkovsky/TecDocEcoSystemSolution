// mvcForum.Web.Areas.Forum.Controllers.AuthController
using ApplicationBoilerplate.DataProvider;
using MVCBootstrap.Web.Mvc.Interfaces;
using mvcForum.Core;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Core.Specifications;
using SimpleAuthentication.Mvc;
using System;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.Forum.Controllers
{

    public class AuthController : IAuthenticationCallbackProvider
    {
        private readonly mvcForum.Core.Interfaces.Services.IMembershipService memberService;

        private readonly IFormsAuthenticationService forms;

        private readonly IContext context;

        public AuthController(mvcForum.Core.Interfaces.Services.IMembershipService memberService, IContext context, IFormsAuthenticationService forms)
        {
            this.memberService = memberService;
            this.context = context;
            this.forms = forms;
        }

        public ActionResult OnRedirectToAuthenticationProviderError(HttpContextBase context, string errorMessage)
        {
            ViewResult viewResult = new ViewResult();
            viewResult.ViewName = "AuthenticateCallback";
            viewResult.ViewData = new ViewDataDictionary(new
            {
                ErrorMessage = errorMessage
            });
            return viewResult;
        }

        public ActionResult Process(HttpContextBase context, AuthenticateCallbackData model)
        {
            string text = $"[{model.AuthenticatedClient.ProviderName}][{model.AuthenticatedClient.UserInformation.Id}]";
            IAccount account = memberService.GetAccount(text);
            if (account == null)
            {
                MailAddress mailAddress = new MailAddress($"{DateTime.UtcNow:ddMMyyyy-hhmmss}repl@ce.this");
                try
                {
                    mailAddress = new MailAddress(model.AuthenticatedClient.UserInformation.Email);
                }
                catch
                {
                }
                if (!memberService.CreateAccount(text, default(Guid).ToString(), mailAddress.Address, out string _))
                {
                    return new HttpStatusCodeResult(500);
                }
                account = memberService.GetAccount(text);
                if (account == null)
                {
                    return new HttpStatusCodeResult(500);
                }
                IRepository<ForumUser> repository = this.context.GetRepository<ForumUser>();
                ForumUser forumUser = repository.ReadOne(new ForumUserSpecifications.SpecificProviderUserKey(account.ProviderUserKey.ToString()));
                if (forumUser != null)
                {
                    forumUser.Name = model.AuthenticatedClient.UserInformation.UserName;
                    forumUser.EmailAddress = (account.EmailAddress.EndsWith("repl@ce.this") ? account.EmailAddress : (account.EmailAddress + "repl@ce.this"));
                    forumUser.FullName = model.AuthenticatedClient.UserInformation.Name;
                    forumUser.Active = true;
                    forumUser.ExternalAccount = true;
                    forumUser.ExternalProvider = model.AuthenticatedClient.ProviderName;
                    forumUser.ExternalProviderId = model.AuthenticatedClient.UserInformation.Id;
                    repository.Update(forumUser);
                    this.context.SaveChanges();
                }
            }
            else
            {
                IRepository<ForumUser> repository2 = this.context.GetRepository<ForumUser>();
                ForumUser forumUser2 = repository2.ReadOne(new ForumUserSpecifications.SpecificProviderUserKey(account.ProviderUserKey.ToString()));
                if (forumUser2 == null)
                {
                    return new HttpStatusCodeResult(500);
                }
                if (!forumUser2.ExternalAccount || (forumUser2.ExternalAccount && forumUser2.ExternalProvider != model.AuthenticatedClient.ProviderName))
                {
                    return new RedirectResult("/forum/account/external");
                }
            }
            forms.SignIn(text, createPersistentCookie: false);
            return new RedirectResult("/forum");
        }
    }

}
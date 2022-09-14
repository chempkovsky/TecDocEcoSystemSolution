using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvcForum.Web.Areas.Forum.Controllers
{
    // mvcForum.Web.Areas.Forum.Controllers.ProfileController
    using ApplicationBoilerplate.DataProvider;
    using mvcForum.Core;
    using mvcForum.Core.Abstractions.Interfaces;
    using mvcForum.Core.Interfaces.Services;
    using mvcForum.Core.Specifications;
    using mvcForum.Web.Controllers;
    using mvcForum.Web.Helpers;
    using mvcForum.Web.Interfaces;
    using mvcForum.Web.ViewModels;
    using mvcForum.Web.ViewModels.Delete;
    using mvcForum.Web.ViewModels.Update;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using System.Web.Mvc;

    public class ProfileController : ThemedForumBaseController
    {
        private readonly IConfiguration config;

        private readonly IMembershipService membershipService;

        public ProfileController(IWebUserProvider userProvider, IContext context, IConfiguration config, IMembershipService membershipService)
            : base(userProvider, context)
        {
            this.config = config;
            this.membershipService = membershipService;
        }

        public ActionResult Index(int id, string name)
        {
            try
            {
                ForumUser user = GetRepository<ForumUser>().Read(id);
                UserViewModel userViewModel = new UserViewModel(user);
                userViewModel.Path = new Dictionary<string, string>();
                return View(userViewModel);
            }
            catch
            {
            }
            return View();
        }

        [Authorize]
        public ActionResult Update()
        {
            UpdateUserViewModel model = new UpdateUserViewModel(base.ActiveUser, base.ActiveUser.ExternalAccount, config.AllowUserDefinedTheme, base.Server.MapPath("~/themes"));
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Update(UpdateUserViewModel model)
        {
            if (base.ModelState.IsValid)
            {
                string namespc = "mvcForum.Web.Profile.Update";
                if (model.Id == base.ActiveUser.Id)
                {
                    bool flag = false;
                    if (base.ActiveUser.ExternalAccount)
                    {
                        if (base.ActiveUser.EmailAddress.ToLowerInvariant().EndsWith("repl@ce.this"))
                        {
                            if (string.IsNullOrWhiteSpace(model.Name))
                            {
                                base.ModelState.AddModelError("Name", ForumHelper.GetString("MissingUsernameExternalUser", null, namespc));
                                flag = true;
                            }
                            else
                            {
                                IEnumerable<ForumUser> source = base.ForumUserRepository.ReadMany(new ForumUserSpecifications.SpecificUsername(model.Name));
                                if (source.Any((ForumUser fu) => fu.Id != base.ActiveUser.Id))
                                {
                                    base.ModelState.AddModelError("Name", ForumHelper.GetString("NameInUseExternalUser", null, namespc));
                                    flag = true;
                                }
                            }
                        }
                        if (string.IsNullOrWhiteSpace(model.Email))
                        {
                            base.ModelState.AddModelError("Email", ForumHelper.GetString("MissingEmailExternalUser", null, namespc));
                            flag = true;
                        }
                        else
                        {
                            try
                            {
                                new MailAddress(model.Email);
                                string accountNameByEmailAddress = membershipService.GetAccountNameByEmailAddress(model.Email);
                                if (!string.IsNullOrWhiteSpace(accountNameByEmailAddress))
                                {
                                    base.ModelState.AddModelError("Email", ForumHelper.GetString("EmailInUse", null, namespc));
                                    flag = true;
                                }
                            }
                            catch
                            {
                                base.ModelState.AddModelError("Email", ForumHelper.GetString("InvalidEmailExternalUser", null, namespc));
                                flag = true;
                            }
                        }
                        if (!flag)
                        {
                            IAccount accountByName = membershipService.GetAccountByName(base.ControllerContext.RequestContext.HttpContext.User.Identity.Name, online: false);
                            ForumUser forumUser = base.ForumUserRepository.ReadOne(new ForumUserSpecifications.SpecificProviderUserKey(accountByName.ProviderUserKey.ToString()));
                            if (model.Email != accountByName.EmailAddress)
                            {
                                accountByName.EmailAddress = model.Email.ToLowerInvariant();
                                forumUser.EmailAddress = accountByName.EmailAddress;
                                membershipService.UpdateAccount(accountByName);
                            }
                            forumUser.Name = model.Name;
                            forumUser.FullName = model.FullName;
                            forumUser.UseFullName = !string.IsNullOrWhiteSpace(model.FullName);
                            forumUser.Culture = model.Culture;
                            forumUser.Timezone = model.Timezone;
                            if (config.AllowUserDefinedTheme)
                            {
                                forumUser.Theme = model.Theme;
                            }
                            base.ForumUserRepository.Update(forumUser);
                            base.Context.SaveChanges();
                            base.TempData.Add("Status", ForumHelper.GetString("ChangesSaved", null, namespc));
                        }
                    }
                    else
                    {
                        bool flag2 = false;
                        if (!string.IsNullOrEmpty(model.OldPassword))
                        {
                            flag2 = membershipService.ValidateAccount(base.ControllerContext.RequestContext.HttpContext.User.Identity.Name, model.OldPassword);
                        }
                        IAccount accountByName2 = membershipService.GetAccountByName(base.ControllerContext.RequestContext.HttpContext.User.Identity.Name, online: false);
                        if (base.ActiveUser.EmailAddress != model.Email)
                        {
                            string accountNameByEmailAddress2 = membershipService.GetAccountNameByEmailAddress(model.Email);
                            if (!string.IsNullOrWhiteSpace(accountNameByEmailAddress2))
                            {
                                base.ModelState.AddModelError("Email", ForumHelper.GetString("EmailInUse", null, namespc));
                                flag = true;
                            }
                        }
                        if (base.ActiveUser.EmailAddress != model.Email)
                        {
                            if (!flag2)
                            {
                                base.ModelState.AddModelError("Email", ForumHelper.GetString("EmailChangeMissingPassword", null, namespc));
                                flag = true;
                            }
                            else
                            {
                                accountByName2.EmailAddress = model.Email;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(model.NewPassword) && model.NewPassword != model.RepeatedNewPassword)
                        {
                            base.ModelState.AddModelError("NewPassword", ForumHelper.GetString("PasswordsDoesNotMatch", null, namespc));
                            flag = true;
                        }
                        if (!string.IsNullOrWhiteSpace(model.NewPassword) && model.NewPassword == model.RepeatedNewPassword && !flag2)
                        {
                            base.ModelState.AddModelError("Password", ForumHelper.GetString("OldPasswordsRequired", null, namespc));
                            flag = true;
                        }
                        if (!flag)
                        {
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(model.OldPassword) && !string.IsNullOrWhiteSpace(model.NewPassword))
                                {
                                    accountByName2.ChangePassword(model.OldPassword, model.NewPassword);
                                }
                                membershipService.UpdateAccount(accountByName2);
                                ForumUser forumUser2 = base.ForumUserRepository.ReadOne(new ForumUserSpecifications.SpecificProviderUserKey(accountByName2.ProviderUserKey.ToString()));
                                if (forumUser2 != null)
                                {
                                    forumUser2.Culture = model.Culture;
                                    forumUser2.Timezone = model.Timezone;
                                    forumUser2.FullName = model.FullName;
                                    forumUser2.UseFullName = !string.IsNullOrWhiteSpace(forumUser2.FullName);
                                    forumUser2.EmailAddress = model.Email;
                                    if (config.AllowUserDefinedTheme)
                                    {
                                        forumUser2.Theme = model.Theme;
                                    }
                                }
                                base.Context.SaveChanges();
                                base.TempData.Add("Status", ForumHelper.GetString("ChangesSaved", null, namespc));
                            }
                            catch (FormatException)
                            {
                                base.ModelState.AddModelError("", ForumHelper.GetString("EditProfile.InvalidEmailAddress"));
                            }
                        }
                    }
                }
            }
            model = new UpdateUserViewModel(base.ActiveUser, base.ActiveUser.ExternalAccount, config.AllowUserDefinedTheme, base.Server.MapPath("~/themes"));
            return View(model);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            DeleteUserViewModel model = new DeleteUserViewModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(DeleteUserViewModel model)
        {
            if (model.Confirm)
            {
                membershipService.DeleteAccount(base.ActiveUser.Name, deleteAllRelatedData: true);
                return Redirect("/");
            }
            return View(model);
        }
    }

}
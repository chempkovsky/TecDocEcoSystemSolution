// mvcForum.Web.Areas.Forum.Controllers.AccountController
using ApplicationBoilerplate.DataProvider;
using CreativeMinds.Security.Cryptography;
using MVCBootstrap.Web.Mvc.Interfaces;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Core.Specifications;
using mvcForum.Web;
using mvcForum.Web.Controllers;
using mvcForum.Web.Filters;
using mvcForum.Web.Helpers;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using System;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.Forum.Controllers
{

    [ConfigurationControlled(ConfigurationArea.AccountController, "")]
    public class AccountController : ThemedForumBaseController
    {
        private readonly IAuthenticationService authService;

        private readonly mvcForum.Core.Interfaces.Services.IMembershipService membershipService;

        private readonly IRepository<BannedIP> bipRepo;

        private readonly IConfiguration config;

        private readonly IMailService mailService;

        public AccountController(IContext context, IWebUserProvider userProvider, IAuthenticationService authService, mvcForum.Core.Interfaces.Services.IMembershipService membershipService, IRepository<BannedIP> bipRepo, IConfiguration config, IMailService mailService)
            : base(userProvider, context)
        {
            this.authService = authService;
            this.membershipService = membershipService;
            this.config = config;
            this.bipRepo = bipRepo;
            this.mailService = mailService;
        }

        public ActionResult LogOff()
        {
            authService.SignOut();
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        [ConfigurationControlled(ConfigurationArea.AccountController, "SignUp")]
        public ActionResult Register()
        {
            if (!config.UseForumAccountController || !config.AllowLocalUsers || !config.AllowSignUp)
            {
                return new HttpNotFoundResult();
            }
            return View(new RegisterModel
            {
                RequireRulesAccept = config.RequireRulesAccept,
                SignUpRules = config.SignUpRules
            });
        }

        [ConfigurationControlled(ConfigurationArea.AccountController, "SignUp")]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (!config.UseForumAccountController || !config.AllowLocalUsers || !config.AllowSignUp)
            {
                return new HttpNotFoundResult();
            }
            if (base.ModelState.IsValid)
            {
                if ((config.RequireRulesAccept && model.RulesAccepted) || !config.RequireRulesAccept)
                {
                    if (membershipService.CreateAccount(model.Username, model.Password, model.EmailAddress, out string errorMessage))
                    {
                        if (config.RequireEmailValidation)
                        {
                            IAccount account = membershipService.GetAccount(model.Username);
                            string data = string.Format("{1}#|#{0}#|#{2}", account.AccountName, account.EmailAddress, account.ProviderUserKey);
                            PrivatePrivateCrypto privatePrivateCrypto = new PrivatePrivateCrypto();
                            privatePrivateCrypto.Phrase = config.DefaultTimezone;
                            string text = config.SiteURL;
                            if (!text.EndsWith("/"))
                            {
                                text += "/";
                            }
                            string newValue = $"{text}forum/account/activate?code={HttpUtility.UrlEncode(privatePrivateCrypto.Encrypt(data))}";
                            mailService.Send(new MailAddress(config.RobotEmailAddress, config.RobotName), new MailAddress[1]
                            {
                            new MailAddress(model.EmailAddress, model.Username)
                            }.ToList(), config.ValidationSubject, config.ValidationBody.Replace("{Email}", model.EmailAddress).Replace("{Password}", model.Password).Replace("{Url}", newValue));
                            base.TempData.Add("Status", ForumHelper.GetString<ForumConfigurator>("Register.EmailActivation"));
                        }
                        else
                        {
                            IRepository<ForumUser> repository = base.Context.GetRepository<ForumUser>();
                            ForumUser forumUser = repository.ReadOne(new ForumUserSpecifications.SpecificEmailAddress(model.EmailAddress));
                            forumUser.Active = true;
                            base.Context.SaveChanges();
                            base.TempData.Add("Status", ForumHelper.GetString<ForumConfigurator>("Register.AccountReady"));
                        }
                        return RedirectToAction("success", "account", new
                        {
                            area = "forum"
                        });
                    }
                    base.ModelState.AddModelError("", errorMessage);
                }
                else
                {
                    base.ModelState.AddModelError("RulesAccepted", ForumHelper.GetString<ForumConfigurator>("Register.RulesMustBeAccepted"));
                }
            }
            model.RequireRulesAccept = config.RequireRulesAccept;
            model.SignUpRules = config.SignUpRules;
            return View(model);
        }

        [ConfigurationControlled(ConfigurationArea.AccountController, "LocalOrOpenAuth")]
        [HttpGet]
        public ActionResult LogOn()
        {
            if (!config.UseForumAccountController || (!config.AllowLocalUsers && !config.AllowOpenAuthUsers))
            {
                return new HttpNotFoundResult();
            }
            BannedIP bannedIP = bipRepo.ReadOne(new BannedIPSpecifications.SpecificIP(base.Request.UserHostAddress));
            if (bannedIP != null)
            {
                base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.BannedIP", new
                {
                    IPAddress = base.Request.UserHostAddress
                }));
                return RedirectToRoute("NoAccess");
            }
            return View(new LogOnModel
            {
                AllowLocalUsers = config.AllowLocalUsers,
                AllowSignUp = config.AllowSignUp,
                AllowOpenAuthUsers = config.AllowOpenAuthUsers
            });
        }

        [ConfigurationControlled(ConfigurationArea.AccountController, "Local")]
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (!config.UseForumAccountController || !config.AllowLocalUsers)
            {
                return new HttpNotFoundResult();
            }
            if (base.ModelState.IsValid)
            {
                BannedIP bannedIP = bipRepo.ReadOne(new BannedIPSpecifications.SpecificIP(base.Request.UserHostAddress));
                if (bannedIP != null)
                {
                    base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.BannedIP"));
                    return RedirectToRoute("NoAccess");
                }
                string accountNameByEmailAddress = membershipService.GetAccountNameByEmailAddress(model.EmailAddress);
                if (!string.IsNullOrWhiteSpace(accountNameByEmailAddress) && membershipService.ValidateAccount(accountNameByEmailAddress, model.Password))
                {
                    ForumUser forumUser = base.ForumUserRepository.ReadOne(new ForumUserSpecifications.SpecificUsername(accountNameByEmailAddress));
                    if (forumUser != null)
                    {
                        if (!forumUser.ExternalAccount)
                        {
                            authService.SignIn(membershipService.GetAccountByName(accountNameByEmailAddress), model.RememberMe);
                            if (base.Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            return RedirectToAction("Index", "Home", new
                            {
                                area = "forum"
                            });
                        }
                        base.ModelState.AddModelError(string.Empty, "The account is an external account, please log on using the right provider.");
                    }
                }
                else
                {
                    base.ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
                }
            }
            model.AllowLocalUsers = config.AllowLocalUsers;
            model.AllowSignUp = config.AllowSignUp;
            model.AllowOpenAuthUsers = config.AllowOpenAuthUsers;
            return View(model);
        }

        [HttpPost]
        [ConfigurationControlled(ConfigurationArea.AccountController, "Local")]
        public ActionResult ForgottenPassword(ForgottenPassword model)
        {
            if (base.ModelState.IsValid)
            {
                IAccount accountByEmailAddress = membershipService.GetAccountByEmailAddress(model.EmailAddress);
                string newValue = accountByEmailAddress.ResetPassword();
                membershipService.UpdateAccount(accountByEmailAddress);
                mailService.Send(new MailAddress(config.RobotEmailAddress, config.RobotName), new MailAddress[1]
                {
                new MailAddress(accountByEmailAddress.EmailAddress, accountByEmailAddress.AccountName)
                }.ToList(), config.ForgottenPasswordSubject, config.ForgottenPasswordBody.Replace("{Email}", model.EmailAddress).Replace("{Password}", newValue));
                base.TempData.Add("ForgottenStatus", ForumHelper.GetString("PasswordChanged", null, "mvcForum.Web.ForgottenPassword"));
            }
            return RedirectToAction("logon");
        }

        public ActionResult External()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        [ConfigurationControlled(ConfigurationArea.AccountController, "SignUp")]
        public ActionResult Activate(string code)
        {
            bool flag = false;
            if (!string.IsNullOrWhiteSpace(code))
            {
                PrivatePrivateCrypto privatePrivateCrypto = new PrivatePrivateCrypto();
                privatePrivateCrypto.Phrase = config.DefaultTimezone;
                try
                {
                    string text = privatePrivateCrypto.Decrypt(code);
                    string[] array = text.Split(new string[1]
                    {
                    "#|#"
                    }, StringSplitOptions.RemoveEmptyEntries);
                    if (array.Length == 3)
                    {
                        IAccount account = membershipService.GetAccount(array[1]);
                        if (account != null && account.EmailAddress == array[0] && account.ProviderUserKey.ToString() == array[2])
                        {
                            account.IsApproved = true;
                            membershipService.UpdateAccount(account);
                            flag = true;
                            ForumUser forumUser = base.ForumUserRepository.ReadOne(new ForumUserSpecifications.SpecificProviderUserKey(account.ProviderUserKey.ToString()));
                            forumUser.Active = true;
                            base.Context.SaveChanges();
                        }
                    }
                }
                catch
                {
                }
            }
            return View(flag);
        }
    }

}
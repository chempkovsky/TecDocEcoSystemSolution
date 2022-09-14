// CarShop.Controllers.AccountController
using CarShop.Controllers;
using CarShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
//using mvcForum.DataProvider.EntityFramework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarShop.Controllers
{
/*
    [Authorize]
    public class AccountController_old : Controller
    {
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public string LoginProvider
            {
                get;
                set;
            }

            public string RedirectUri
            {
                get;
                set;
            }

            public string UserId
            {
                get;
                set;
            }

            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                AuthenticationProperties authenticationProperties = new AuthenticationProperties();
                authenticationProperties.RedirectUri = RedirectUri;
                AuthenticationProperties authenticationProperties2 = authenticationProperties;
                if (UserId != null)
                {
                    authenticationProperties2.Dictionary["XsrfId"] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(authenticationProperties2, LoginProvider);
            }
        }

        private const string XsrfKey = "XsrfId";

        private MVCForumContext mvcContext = new MVCForumContext("mvcForum.DataProvider.MainDB");

        public UserManager<ApplicationUser> UserManager
        {
            get;
            private set;
        }

        private IAuthenticationManager AuthenticationManager => base.HttpContext.GetOwinContext().Authentication;

        public AccountController_old()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController_old(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            base.ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (base.ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                base.ModelState.AddModelError("", "Неверное имя пользователя или пароль.");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (base.ModelState.IsValid)
            {
                DateTime dt = DateTime.UtcNow;
                ApplicationUser user2 = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.UserName + "@t.t",
                    AccessFailedCount = 0,
                    LockoutEnabled = true,
                    LastActivityDate = dt,
                    Approved = true,
                    CreationDate = dt,
                    LastLockoutDate = dt,
                    LastLoginDate = new DateTime(1970, 1, 1),
                    LockoutEndDateUtc = dt
                };
                IdentityResult result = await UserManager.CreateAsync(user2, model.Password);
                if (result.Succeeded)
                {
                    user2 = UserManager.FindByName(model.UserName);
                    if (user2 != null)
                    {
                        try
                        {
                            IConfiguration service = DependencyResolver.Current.GetService<IConfiguration>();
                            ForumUser forumUser = new ForumUser(user2.Id, user2.UserName, user2.Email, "::1");
                            forumUser.Timezone = service.DefaultTimezone;
                            forumUser.Culture = service.DefaultCulture;
                            forumUser.Active = true;
                            if (string.IsNullOrEmpty(forumUser.EmailAddress))
                            {
                                forumUser.EmailAddress = model.UserName + "@t.t";
                            }
                            mvcContext.ForumUsers.Add(forumUser);
                            mvcContext.SaveChanges();
                            foreach (int newUserGroup in service.NewUserGroups)
                            {
                                if (newUserGroup > 0)
                                {
                                    Group group = mvcContext.Groups.Find(newUserGroup);
                                    mvcContext.GroupMembers.Add(new GroupMember(group, forumUser));
                                }
                            }
                            mvcContext.SaveChanges();
                        }
                        catch
                        {
                        }
                        await SignInAsync(user2, isPersistent: false);
                        return RedirectToAction("Index", "GuestProfile");
                    }
                }
                else
                {
                    AddErrors(result);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = (!(await UserManager.RemoveLoginAsync(base.User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey))).Succeeded) ? new ManageMessageId?(ManageMessageId.Error) : new ManageMessageId?(ManageMessageId.RemoveLoginSuccess);
            return RedirectToAction("Manage", new
            {
                Message = message
            });
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            dynamic viewBag = base.ViewBag;
            ManageMessageId? manageMessageId = message;
            viewBag.StatusMessage = ((manageMessageId.GetValueOrDefault() == ManageMessageId.ChangePasswordSuccess && manageMessageId.HasValue) ? "Ваш пароль изменен." : ((message == ManageMessageId.SetPasswordSuccess) ? "Пароль задан." : ((message == ManageMessageId.RemoveLoginSuccess) ? "Внешнее имя входа удалено." : ((message == ManageMessageId.Error) ? "Произошла ошибка." : ""))));
            base.ViewBag.HasLocalPassword = HasPassword();
            base.ViewBag.ReturnUrl = base.Url.Action("Manage");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            base.ViewBag.HasLocalPassword = hasPassword;
            base.ViewBag.ReturnUrl = base.Url.Action("Manage");
            if (hasPassword)
            {
                if (base.ModelState.IsValid)
                {
                    IdentityResult result2 = await UserManager.ChangePasswordAsync(base.User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result2.Succeeded)
                    {
                        return RedirectToAction("Manage", new
                        {
                            Message = ManageMessageId.ChangePasswordSuccess
                        });
                    }
                    AddErrors(result2);
                }
            }
            else
            {
                base.ModelState["OldPassword"]?.Errors.Clear();
                if (base.ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(base.User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new
                        {
                            Message = ManageMessageId.SetPasswordSuccess
                        });
                    }
                    AddErrors(result);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, base.Url.Action("ExternalLoginCallback", "Account", new
            {
                ReturnUrl = returnUrl
            }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            ApplicationUser user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            base.ViewBag.ReturnUrl = returnUrl;
            base.ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel
            {
                UserName = loginInfo.DefaultUserName
            });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LinkLogin(string provider)
        {
            return new ChallengeResult(provider, base.Url.Action("LinkLoginCallback", "Account"), base.User.Identity.GetUserId());
        }

        public async Task<ActionResult> LinkLoginCallback()
        {
            ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync("XsrfId", base.User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new
                {
                    Message = ManageMessageId.Error
                });
            }
            if ((await UserManager.AddLoginAsync(base.User.Identity.GetUserId(), loginInfo.Login)).Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new
            {
                Message = ManageMessageId.Error
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (base.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }
            if (base.ModelState.IsValid)
            {
                ExternalLoginInfo info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                DateTime dt = DateTime.UtcNow;
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.UserName + "@t.t",
                    AccessFailedCount = 0,
                    LockoutEnabled = true,
                    LastActivityDate = dt,
                    Approved = true,
                    CreationDate = dt,
                    LastLockoutDate = dt,
                    LastLoginDate = new DateTime(1970, 1, 1),
                    LockoutEndDateUtc = dt
                };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }
            base.ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            IList<UserLoginInfo> logins = UserManager.GetLogins(base.User.Identity.GetUserId());
            base.ViewBag.ShowRemoveButton = (HasPassword() || logins.Count > 1);
            return PartialView("_RemoveAccountPartial", logins);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            if (disposing && mvcContext != null)
            {
                mvcContext.Dispose();
                mvcContext = null;
            }
            base.Dispose(disposing);
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut("ExternalCookie");
            ClaimsIdentity identity = await UserManager.CreateIdentityAsync(user, "ApplicationCookie");
            AuthenticationManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = isPersistent
            }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                base.ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            ApplicationUser applicationUser = UserManager.FindById(base.User.Identity.GetUserId());
            if (applicationUser != null)
            {
                return applicationUser.PasswordHash != null;
            }
            return false;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (base.Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
*/
}
// mvcForum.Web.Areas.ForumAdmin.Controllers.SettingsController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Web.Areas.ForumAdmin.ViewModels.Update;
using mvcForum.Web.Controllers;
using mvcForum.Web.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAdmin.Controllers
{

    public class SettingsController : ForumAdminBaseController
    {
        private readonly IConfiguration config;

        public SettingsController(IWebUserProvider userProvider, IContext context, IConfiguration config)
            : base(userProvider, context)
        {
            this.config = config;
        }

        [HttpGet]
        public ViewResult Index()
        {
            ForumSettingsViewModel model = new ForumSettingsViewModel(config, base.Server.MapPath("~/themes"));
            return View(model);
        }

        [HttpPost]
        public ViewResult Index(ForumSettingsViewModel model)
        {
            if (base.ModelState.IsValid)
            {
                config.MessagesPerPage = model.MessagesPerPage;
                config.TopicsPerPage = model.TopicsPerPage;
                config.AllowUserDefinedTheme = model.AllowUserDefinedTheme;
                config.AsynchronousAntiSpam = model.AsynchronousAntiSpam;
                config.DefaultCulture = model.DefaultCulture;
                config.DefaultTimezone = model.DefaultTimezone;
                config.MaxAttachmentsSize = model.MaxAttachmentsSize * 1000000;
                config.MaxFileSize = model.MaxFileSize * 1000000;
                config.PostsOnReply = model.PostsOnReply;
                config.RobotEmailAddress = model.RobotEmailAddress;
                config.RobotName = model.RobotName;
                config.ShowOldPostsOnReply = model.ShowOldPostsOnReply;
                config.SiteURL = model.SiteURL;
                config.AllowedExtensions = model.AllowedExtensions.Split(new char[1]
                {
                ';'
                }, StringSplitOptions.RemoveEmptyEntries).ToList();
                config.Theme = model.Theme;
                config.CheckForNews = model.CheckForNews;
                config.Editor = model.Editor;
                config.NewUserGroups = (from i in model.NewUserGroups
                                        select int.Parse(i)).ToArray();
                config.ShowDeletedMessages = model.ShowDeletedMessages;
                config.ShowOnlineUsers = model.ShowOnlineUsers;
                config.InformOnQuarantine = model.InformOnQuarantine;
                config.Save();
                base.TempData["Saved"] = true;
            }
            string themeRoot = base.Server.MapPath("~/themes");
            if (Directory.Exists(themeRoot))
            {
                model.Themes = (from d in Directory.GetDirectories(themeRoot)
                                select d.Substring(themeRoot.Length + 1)).ToArray();
            }
            else
            {
                model.Themes = new string[0];
            }
            return View(model);
        }

        [HttpGet]
        public ViewResult UserRegistration()
        {
            UserRegistrationViewModel model = new UserRegistrationViewModel(config);
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ViewResult UserRegistration(UserRegistrationViewModel model)
        {
            if (base.ModelState.IsValid)
            {
                config.UseForumAccountController = model.UseForumAccountController;
                config.AllowLocalUsers = (model.UseForumAccountController && model.AllowLocalUsers);
                config.AllowSignUp = (model.UseForumAccountController && model.AllowLocalUsers && model.AllowSignUp);
                config.AllowOpenAuthUsers = (model.UseForumAccountController && model.AllowOpenAuthUsers);
                config.RequireEmailValidation = (model.UseForumAccountController && model.AllowLocalUsers && model.AllowSignUp && model.RequireEmailValidation);
                config.RequireRulesAccept = (model.UseForumAccountController && model.AllowLocalUsers && model.AllowSignUp && model.RequireRulesAccept);
                config.ValidationSubject = model.ValidationSubject;
                config.ValidationBody = model.ValidationBody;
                config.ForgottenPasswordBody = model.ForgottenPasswordBody;
                config.ForgottenPasswordSubject = model.ForgottenPasswordSubject;
                config.SignUpRules = model.SignUpRules;
                config.Save();
                base.TempData["Saved"] = true;
            }
            return View(model);
        }
    }

}
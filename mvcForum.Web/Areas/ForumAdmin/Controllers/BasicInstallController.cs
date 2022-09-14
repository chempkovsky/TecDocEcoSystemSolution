// mvcForum.Web.Areas.ForumAdmin.Controllers.BasicInstallController
using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Events;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Interfaces;
using mvcForum.Core.Interfaces.Search;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Core.Specifications;
using mvcForum.Web.Controllers;
using mvcForum.Web.Events;
using mvcForum.Web.Helpers;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml;

namespace mvcForum.Web.Areas.ForumAdmin.Controllers
{

    public class BasicInstallController : ForumBaseController
    {
        private readonly IConfiguration config;

        private readonly IEnumerable<IInstallable> installers;

        private readonly IMembershipService membershipService;

        public BasicInstallController(IConfiguration config, IContext context, IWebUserProvider userProvider, IEnumerable<IInstallable> installers, IMembershipService membershipService)
            : base(userProvider, context)
        {
            this.config = config;
            this.installers = installers;
            this.membershipService = membershipService;
        }

        public ActionResult Index()
        {
            IRepository<ForumSettings> repository = base.Context.GetRepository<ForumSettings>();
            try
            {
                ForumSettings forumSettings = repository.ReadOne(new ForumSettingsSpecifications.SpecificKey("Version"));
                if (forumSettings != null)
                {
                    return RedirectToAction("Index", "Home", new
                    {
                        area = "Forum"
                    });
                }
            }
            catch
            {
            }
            BasicInstallViewModel basicInstallViewModel = new BasicInstallViewModel();
            basicInstallViewModel.CanCreateAdmin = true;
            basicInstallViewModel.CanCreateGroups = true;
            basicInstallViewModel.CanCreateSimpleForum = true;
            BasicInstallViewModel basicInstallViewModel2 = basicInstallViewModel;
            try
            {
                ForumUser forumUser = base.ForumUserRepository.ReadOne(new ForumUserSpecifications.SpecificUsername("Administrator"));
                if (forumUser != null)
                {
                    basicInstallViewModel2.CanCreateAdmin = false;
                }
            }
            catch
            {
            }
            IRepository<Group> repository2 = base.Context.GetRepository<Group>();
            try
            {
                IEnumerable<Group> source = repository2.ReadAll();
                if (source.Count() > 0)
                {
                    basicInstallViewModel2.CanCreateGroups = false;
                }
            }
            catch
            {
            }
            IRepository<Board> repository3 = base.Context.GetRepository<Board>();
            try
            {
                IEnumerable<Board> source2 = repository3.ReadAll();
                if (source2.Count() > 0)
                {
                    basicInstallViewModel2.CanCreateSimpleForum = false;
                }
            }
            catch
            {
            }
            basicInstallViewModel2.SubmitInstallation = !base.Request.IsLocal;
            return View(basicInstallViewModel2);
        }

        [HttpPost]
        public ActionResult Index(BasicInstallViewModel model)
        {
            if (base.ModelState.IsValid)
            {
                try
                {
                    IRepository<ForumSettings> repository = base.Context.GetRepository<ForumSettings>();
                    try
                    {
                        ForumSettings forumSettings = repository.ReadOne(new ForumSettingsSpecifications.SpecificKey("Version"));
                        if (forumSettings != null)
                        {
                            return RedirectToAction("Index", "Home", new
                            {
                                area = "Forum"
                            });
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        foreach (IIndexer service in DependencyResolver.Current.GetServices<IIndexer>())
                        {
                            service.Clear();
                        }
                    }
                    catch (Exception)
                    {
                    }
                    config.DefaultCulture = "en-GB";
                    config.DefaultTimezone = "GMT Standard Time";
                    config.MessagesPerPage = 20;
                    config.TopicsPerPage = 15;
                    config.CheckForNews = true;
                    config.Editor = "Regular";
                    ForumUser forumUser = null;
                    if (model.CreateAdmin && !model.ImportMembershipUsers)
                    {
                        string text = "Administrator";
                        string password = "123456";
                        string emailAddress = "admin@bogus.com";
                        if (!string.IsNullOrWhiteSpace(model.AdminUsername))
                        {
                            text = model.AdminUsername;
                        }
                        if (!string.IsNullOrWhiteSpace(model.AdminPassword))
                        {
                            password = model.AdminPassword;
                        }
                        if (!string.IsNullOrWhiteSpace(model.AdminEmail))
                        {
                            emailAddress = model.AdminEmail;
                        }
                        membershipService.CreateRole("Board Administrator");
                        membershipService.CreateRole("Solution Administrator");
                        membershipService.CreateAccount(text, password, emailAddress, out string _);
                        membershipService.AddAccountToRoles(text, new string[1]
                        {
                        "Solution Administrator"
                        });
                        forumUser = base.ForumUserRepository.ReadOne(new ForumUserSpecifications.SpecificUsername(text));
                        forumUser.Timezone = config.DefaultTimezone;
                        forumUser.Culture = config.DefaultCulture;
                        forumUser.Active = true;
                        IAccount accountByName = membershipService.GetAccountByName(text);
                        DependencyResolver.Current.GetService<IAuthenticationService>().SignIn(accountByName, createPersistentCookie: false);
                    }
                    Group group = null;
                    Group group2 = null;
                    Group group3 = null;
                    Group group4 = null;
                    IRepository<Group> repository2 = base.Context.GetRepository<Group>();
                    if (model.CreateGroups)
                    {
                        group = new Group("Guest");
                        repository2.Create(group);
                        group2 = new Group("Member");
                        repository2.Create(group2);
                        group3 = new Group("Moderator");
                        repository2.Create(group3);
                        group4 = new Group("Administrator");
                        repository2.Create(group4);
                        if (model.CreateAdmin && !model.ImportMembershipUsers)
                        {
                            IRepository<GroupMember> repository3 = base.Context.GetRepository<GroupMember>();
                            GroupMember newEntity = new GroupMember(group4, forumUser);
                            repository3.Create(newEntity);
                        }
                        base.Context.SaveChanges();
                        if (config.Editable)
                        {
                            config.NewUserGroups.Add(group2.Id);
                        }
                    }
                    if (model.CreateSimpleForum)
                    {
                        IRepository<Board> repository4 = GetRepository<Board>();
                        Board board = new Board();
                        board.Name = "Demo Board";
                        board.Description = "My Description";
                        board.Disabled = false;
                        Board board2 = board;
                        repository4.Create(board2);
                        IRepository<AccessMask> repository5 = GetRepository<AccessMask>();
                        AccessMask accessMask = new AccessMask(board2, "Readonly access", AccessFlag.Read);
                        repository5.Create(accessMask);
                        AccessMask newEntity2 = new AccessMask(board2, "Read/reply access", AccessFlag.Read | AccessFlag.Reply);
                        repository5.Create(newEntity2);
                        AccessMask accessMask2 = new AccessMask(board2, "Member access", AccessFlag.Read | AccessFlag.Post | AccessFlag.Reply | AccessFlag.Edit);
                        repository5.Create(accessMask2);
                        AccessMask accessMask3 = new AccessMask(board2, "Moderator access", AccessFlag.Read | AccessFlag.Post | AccessFlag.Reply | AccessFlag.Moderator | AccessFlag.Edit | AccessFlag.Delete);
                        repository5.Create(accessMask3);
                        AccessMask accessMask4 = new AccessMask(board2, "Admin access", AccessFlag.Read | AccessFlag.Post | AccessFlag.Reply | AccessFlag.Priority | AccessFlag.Poll | AccessFlag.Vote | AccessFlag.Moderator | AccessFlag.Edit | AccessFlag.Delete | AccessFlag.Upload);
                        repository5.Create(accessMask4);
                        IRepository<Category> repository6 = GetRepository<Category>();
                        Category category = new Category(board2, "Category 1", 10);
                        repository6.Create(category);
                        Category category2 = new Category(board2, "Category 2", 20);
                        repository6.Create(category2);
                        IRepository<mvcForum.Core.Forum> repository7 = GetRepository<mvcForum.Core.Forum>();
                        mvcForum.Core.Forum forum = new mvcForum.Core.Forum(category, "Forum 1", 10, "My first forum");
                        repository7.Create(forum);
                        mvcForum.Core.Forum forum2 = new mvcForum.Core.Forum(category, "Forum 2", 20, "My second forum");
                        repository7.Create(forum2);
                        mvcForum.Core.Forum forum3 = new mvcForum.Core.Forum(category2, "Forum 3", 10, "The third forum, in the second category");
                        repository7.Create(forum3);
                        if (model.CreateGroups)
                        {
                            IRepository<ForumAccess> repository8 = GetRepository<ForumAccess>();
                            ForumAccess newEntity3 = new ForumAccess(forum, group4, accessMask4);
                            repository8.Create(newEntity3);
                            newEntity3 = new ForumAccess(forum, group, accessMask);
                            repository8.Create(newEntity3);
                            newEntity3 = new ForumAccess(forum, group3, accessMask3);
                            repository8.Create(newEntity3);
                            newEntity3 = new ForumAccess(forum, group2, accessMask2);
                            repository8.Create(newEntity3);
                            newEntity3 = new ForumAccess(forum2, group4, accessMask4);
                            repository8.Create(newEntity3);
                            newEntity3 = new ForumAccess(forum2, group, accessMask);
                            repository8.Create(newEntity3);
                            newEntity3 = new ForumAccess(forum2, group3, accessMask3);
                            repository8.Create(newEntity3);
                            newEntity3 = new ForumAccess(forum2, group2, accessMask2);
                            repository8.Create(newEntity3);
                            newEntity3 = new ForumAccess(forum3, group4, accessMask4);
                            repository8.Create(newEntity3);
                            newEntity3 = new ForumAccess(forum3, group, accessMask);
                            repository8.Create(newEntity3);
                            newEntity3 = new ForumAccess(forum3, group3, accessMask3);
                            repository8.Create(newEntity3);
                            newEntity3 = new ForumAccess(forum3, group2, accessMask2);
                            repository8.Create(newEntity3);
                        }
                    }
                    config.SiteURL = model.SiteURL;
                    config.Save();
                    ForumSettings newEntity4 = new ForumSettings("Version", typeof(Post).Assembly.GetName().Version.ToString());
                    repository.Create(newEntity4);
                    newEntity4 = new ForumSettings("InstallDate", XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Utc));
                    repository.Create(newEntity4);
                    base.Context.SaveChanges();
                    foreach (IInstallable installer in installers)
                    {
                        try
                        {
                            installer.Install();
                            base.Context.SaveChanges();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (model.ImportMembershipUsers)
                    {
                        try
                        {
                            int total;
                            IEnumerable<IAccount> allAccounts = membershipService.GetAllAccounts(0, int.MaxValue, out total);
                            IRepository<ForumUser> repository9 = GetRepository<ForumUser>();
                            IRepository<GroupMember> repository10 = GetRepository<GroupMember>();
                            List<Group> list = new List<Group>();
                            foreach (int newUserGroup in config.NewUserGroups)
                            {
                                list.Add(repository2.Read(newUserGroup));
                            }
                            List<string> list2 = new List<string>();
                            foreach (IAccount item in allAccounts)
                            {
                                try
                                {
                                    ForumUser forumUser2 = new ForumUser();
                                    forumUser2.Culture = config.DefaultCulture;
                                    forumUser2.Deleted = false;
                                    forumUser2.EmailAddress = item.EmailAddress;
                                    forumUser2.FirstVisit = item.CreationDate;
                                    forumUser2.LastVisit = item.LastActivityDate;
                                    forumUser2.Name = item.AccountName;
                                    forumUser2.ProviderId = item.ProviderUserKey.ToString();
                                    forumUser2.Timezone = config.DefaultTimezone;
                                    forumUser2.UseFullName = false;
                                    forumUser2.UserFlag = UserFlag.None;
                                    repository9.Create(forumUser2);
                                    base.Context.SaveChanges();
                                    foreach (Group item2 in list)
                                    {
                                        repository10.Create(new GroupMember(item2, forumUser2));
                                    }
                                    base.Context.SaveChanges();
                                }
                                catch (Exception)
                                {
                                    list2.Add($"User {item.AccountName} ({item.EmailAddress}) with id {item.ProviderUserKey} was not created");
                                }
                            }
                            if (list2.Count > 0)
                            {
                                base.TempData.Add("ImportErrors", string.Join("<br />", list2));
                            }
                        }
                        catch (Exception)
                        {
                        }
                        if (!string.IsNullOrWhiteSpace(model.ExistingUserEmail) && group4 != null)
                        {
                            ForumUser forumUser3 = base.ForumUserRepository.ReadOne(new ForumUserSpecifications.SpecificEmailAddress(model.ExistingUserEmail));
                            IRepository<GroupMember> repository11 = base.Context.GetRepository<GroupMember>();
                            GroupMember newEntity5 = new GroupMember(group4, forumUser3);
                            repository11.Create(newEntity5);
                            membershipService.CreateRole("Board Administrator");
                            membershipService.CreateRole("Solution Administrator");
                            membershipService.AddAccountToRoles(forumUser3.Name, new string[1]
                            {
                            "Solution Administrator"
                            });
                            base.Context.SaveChanges();
                        }
                    }
                    if (!base.Request.IsLocal && model.SubmitInstallation)
                    {
                        DependencyResolver.Current.GetService<IEventPublisher>().Publish(new InstallationEvent
                        {
                            Version = ForumHelper.GetVersion()
                        });
                    }
                    return RedirectToAction("status", "basicinstall", new
                    {
                        area = "forumadmin"
                    });
                }
                catch (Exception)
                {
                }
            }
            return View(model);
        }

        public ActionResult Status()
        {
            return View();
        }
    }

}
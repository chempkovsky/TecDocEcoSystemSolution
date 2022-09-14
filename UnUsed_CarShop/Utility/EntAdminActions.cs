// CarShop.Utility.EntAdminActions
using CarShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.DataProvider.EntityFramework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Utility
{

    public static class EntAdminActions
    {
        public const string EcoSystemAdminName = "EcoSystemAdmin";

        public const string EnterpriseAdminName = "EnterpriseAdmin";

        public const string EnterpriseAuditName = "EnterpriseAudit";

        public const string BranchAdminName = "BranchAdmin";

        public const string BranchAuditName = "BranchAudit";

        public const string BranchSellerName = "BranchSeller";

        public const string BranchBookerName = "BranchBooker";

        public const string EcoSystemDisabled = "EcoSystemDisabled";

        public const string ForumSulushAdminName = "Solution Administrator";

        public const string ForumBoardAdminName = "Board Administrator";

        public static void CreateRoles()
        {
            using (RoleManager<IdentityRole> rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())))
            {
                CreateRoles(rm);
            }
        }

        public static void CreateRoles(RoleManager<IdentityRole> rm)
        {
            if (!rm.RoleExists("EcoSystemAdmin"))
            {
                rm.Create(new IdentityRole("EcoSystemAdmin"));
            }
            if (!rm.RoleExists("EnterpriseAdmin"))
            {
                rm.Create(new IdentityRole("EnterpriseAdmin"));
            }
            if (!rm.RoleExists("EnterpriseAudit"))
            {
                rm.Create(new IdentityRole("EnterpriseAudit"));
            }
            if (!rm.RoleExists("BranchAdmin"))
            {
                rm.Create(new IdentityRole("BranchAdmin"));
            }
            if (!rm.RoleExists("BranchAudit"))
            {
                rm.Create(new IdentityRole("BranchAudit"));
            }
            if (!rm.RoleExists("BranchSeller"))
            {
                rm.Create(new IdentityRole("BranchSeller"));
            }
            if (!rm.RoleExists("BranchBooker"))
            {
                rm.Create(new IdentityRole("BranchBooker"));
            }
            if (!rm.RoleExists("EcoSystemDisabled"))
            {
                rm.Create(new IdentityRole("EcoSystemDisabled"));
            }
        }

        public static void CreateEnterpriseUserAccount(EnterpriseUserTDES aUser)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                using (UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context)))
                {
                    userManager.PasswordValidator = new MinimumLengthValidator(3);
                    using (RoleManager<IdentityRole> rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)))
                    {
                        ApplicationUser applicationUser = userManager.FindByName(aUser.EntUserNic);
                        if (applicationUser == null)
                        {
                            DateTime utcNow = DateTime.UtcNow;
                            ApplicationUser applicationUser2 = new ApplicationUser();
                            applicationUser2.UserName = aUser.EntUserNic;
                            applicationUser2.Email = aUser.EntUserNic + "@t.t";
                            applicationUser2.AccessFailedCount = 0;
                            applicationUser2.LockoutEnabled = true;
                            applicationUser2.LastActivityDate = utcNow;
                            applicationUser2.Approved = true;
                            applicationUser2.CreationDate = utcNow;
                            applicationUser2.LastLockoutDate = utcNow;
                            applicationUser2.LastLoginDate = new DateTime(1970, 1, 1);
                            applicationUser2.LockoutEndDateUtc = utcNow;
                            applicationUser = applicationUser2;
                            IdentityResult identityResult = userManager.Create(applicationUser, aUser.Password);
                            if (!identityResult.Succeeded)
                            {
                                throw new Exception("Не могу создать пользователя");
                            }
                        }
                        CreateRoles(rm);
                        if (!aUser.IsActive && !userManager.IsInRole(applicationUser.Id, "EcoSystemDisabled"))
                        {
                            userManager.AddToRole(applicationUser.Id, "EcoSystemDisabled");
                        }
                        if (aUser.IsActive && userManager.IsInRole(applicationUser.Id, "EcoSystemDisabled"))
                        {
                            userManager.RemoveFromRole(applicationUser.Id, "EcoSystemDisabled");
                        }
                        if (aUser.IsAdmin && !userManager.IsInRole(applicationUser.Id, "EnterpriseAdmin"))
                        {
                            userManager.AddToRole(applicationUser.Id, "EnterpriseAdmin");
                        }
                        if (!aUser.IsAdmin && userManager.IsInRole(applicationUser.Id, "EnterpriseAdmin"))
                        {
                            userManager.RemoveFromRole(applicationUser.Id, "EnterpriseAdmin");
                        }
                        if (aUser.IsAudit && !userManager.IsInRole(applicationUser.Id, "EnterpriseAudit"))
                        {
                            userManager.AddToRole(applicationUser.Id, "EnterpriseAudit");
                        }
                        if (!aUser.IsAudit && userManager.IsInRole(applicationUser.Id, "EnterpriseAudit"))
                        {
                            userManager.RemoveFromRole(applicationUser.Id, "EnterpriseAudit");
                        }
                        if (!userManager.IsInRole(applicationUser.Id, "Board Administrator"))
                        {
                            userManager.AddToRole(applicationUser.Id, "Board Administrator");
                        }
                        try
                        {
                            applicationUser = userManager.FindByName(aUser.EntUserNic);
                            IConfiguration service = DependencyResolver.Current.GetService<IConfiguration>();
                            ForumUser forumUser = new ForumUser(applicationUser.Id, applicationUser.UserName, applicationUser.Email, "::1");
                            forumUser.Timezone = service.DefaultTimezone;
                            forumUser.Culture = service.DefaultCulture;
                            forumUser.Active = true;
                            if (string.IsNullOrEmpty(forumUser.EmailAddress))
                            {
                                forumUser.EmailAddress = aUser.EntUserNic + "@t.t";
                            }
                            MVCForumContext mVCForumContext = new MVCForumContext("mvcForum.DataProvider.MainDB");
                            mVCForumContext.ForumUsers.Add(forumUser);
                            mVCForumContext.SaveChanges();
                            Group group = mVCForumContext.Groups.Find(1);
                            mVCForumContext.GroupMembers.Add(new GroupMember(group, forumUser));
                            mVCForumContext.SaveChanges();
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        public static void DeleteEnterpriseUserAccount(EnterpriseUserTDES aUser)
        {
            using (ApplicationDbContext applicationDbContext = new ApplicationDbContext())
            {
                using (UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(applicationDbContext))
                {
                    using (UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(store))
                    {
                        ApplicationUser applicationUser = manager.FindByName(aUser.EntUserNic);
                        if (applicationUser != null)
                        {
                            IList<string> roles = manager.GetRoles(applicationUser.Id);
                            foreach (string item in roles)
                            {
                                manager.RemoveFromRole(applicationUser.Id, item);
                            }
                            if (manager.HasPassword(applicationUser.Id))
                            {
                                manager.RemovePassword(applicationUser.Id);
                            }
                            IList<UserLoginInfo> logins = manager.GetLogins(applicationUser.Id);
                            foreach (UserLoginInfo item2 in logins)
                            {
                                manager.RemoveLogin(applicationUser.Id, item2);
                            }
                            IList<Claim> claims = manager.GetClaims(applicationUser.Id);
                            foreach (Claim item3 in claims)
                            {
                                manager.RemoveClaim(applicationUser.Id, item3);
                            }
                            ApplicationUser entity = applicationDbContext.Users.Find(applicationUser.Id);
                            applicationDbContext.Users.Remove(entity);
                            applicationDbContext.SaveChanges();
                        }
                    }
                }
            }
        }

        public static void CreateEnterpriseBranchUserAccount(EnterpriseBranchUserTDES aUser)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                using (UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context)))
                {
                    userManager.PasswordValidator = new MinimumLengthValidator(3);
                    using (RoleManager<IdentityRole> rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)))
                    {
                        ApplicationUser applicationUser = userManager.FindByName(aUser.EntUserNic);
                        if (applicationUser == null)
                        {
                            DateTime utcNow = DateTime.UtcNow;
                            ApplicationUser applicationUser2 = new ApplicationUser();
                            applicationUser2.UserName = aUser.EntUserNic;
                            applicationUser2.Email = aUser.EntUserNic + "@t.t";
                            applicationUser2.AccessFailedCount = 0;
                            applicationUser2.LockoutEnabled = true;
                            applicationUser2.LastActivityDate = utcNow;
                            applicationUser2.Approved = true;
                            applicationUser2.CreationDate = utcNow;
                            applicationUser2.LastLockoutDate = utcNow;
                            applicationUser2.LastLoginDate = new DateTime(1970, 1, 1);
                            applicationUser2.LockoutEndDateUtc = utcNow;
                            applicationUser = applicationUser2;
                            userManager.Create(applicationUser, aUser.Password);
                        }
                        CreateRoles(rm);
                        if (!aUser.IsActive && !userManager.IsInRole(applicationUser.Id, "EcoSystemDisabled"))
                        {
                            userManager.AddToRole(applicationUser.Id, "EcoSystemDisabled");
                        }
                        if (aUser.IsActive && userManager.IsInRole(applicationUser.Id, "EcoSystemDisabled"))
                        {
                            userManager.RemoveFromRole(applicationUser.Id, "EcoSystemDisabled");
                        }
                        if (aUser.IsAdmin && !userManager.IsInRole(applicationUser.Id, "BranchAdmin"))
                        {
                            userManager.AddToRole(applicationUser.Id, "BranchAdmin");
                        }
                        if (!aUser.IsAdmin && userManager.IsInRole(applicationUser.Id, "BranchAdmin"))
                        {
                            userManager.RemoveFromRole(applicationUser.Id, "BranchAdmin");
                        }
                        if (aUser.IsAudit && !userManager.IsInRole(applicationUser.Id, "BranchAudit"))
                        {
                            userManager.AddToRole(applicationUser.Id, "BranchAudit");
                        }
                        if (!aUser.IsAudit && userManager.IsInRole(applicationUser.Id, "BranchAudit"))
                        {
                            userManager.RemoveFromRole(applicationUser.Id, "BranchAudit");
                        }
                        if (aUser.IsSeller && !userManager.IsInRole(applicationUser.Id, "BranchSeller"))
                        {
                            userManager.AddToRole(applicationUser.Id, "BranchSeller");
                        }
                        if (!aUser.IsSeller && userManager.IsInRole(applicationUser.Id, "BranchSeller"))
                        {
                            userManager.RemoveFromRole(applicationUser.Id, "BranchSeller");
                        }
                        if (aUser.IsBooker && !userManager.IsInRole(applicationUser.Id, "BranchBooker"))
                        {
                            userManager.AddToRole(applicationUser.Id, "BranchBooker");
                        }
                        if (!aUser.IsBooker && userManager.IsInRole(applicationUser.Id, "BranchBooker"))
                        {
                            userManager.RemoveFromRole(applicationUser.Id, "BranchBooker");
                        }
                        try
                        {
                            applicationUser = userManager.FindByName(aUser.EntUserNic);
                            IConfiguration service = DependencyResolver.Current.GetService<IConfiguration>();
                            ForumUser forumUser = new ForumUser(applicationUser.Id, applicationUser.UserName, applicationUser.Email, "::1");
                            forumUser.Timezone = service.DefaultTimezone;
                            forumUser.Culture = service.DefaultCulture;
                            forumUser.Active = true;
                            if (string.IsNullOrEmpty(forumUser.EmailAddress))
                            {
                                forumUser.EmailAddress = aUser.EntUserNic + "@t.t";
                            }
                            MVCForumContext mVCForumContext = new MVCForumContext("mvcForum.DataProvider.MainDB");
                            mVCForumContext.ForumUsers.Add(forumUser);
                            mVCForumContext.SaveChanges();
                            Group group = mVCForumContext.Groups.Find(4);
                            mVCForumContext.GroupMembers.Add(new GroupMember(group, forumUser));
                            mVCForumContext.SaveChanges();
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        public static void DeleteEnterpriseBranchUserAccount(EnterpriseBranchUserTDES aUser)
        {
            using (ApplicationDbContext applicationDbContext = new ApplicationDbContext())
            {
                using (UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(applicationDbContext))
                {
                    using (UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(store))
                    {
                        ApplicationUser applicationUser = manager.FindByName(aUser.EntUserNic);
                        if (applicationUser != null)
                        {
                            IList<string> roles = manager.GetRoles(applicationUser.Id);
                            foreach (string item in roles)
                            {
                                manager.RemoveFromRole(applicationUser.Id, item);
                            }
                            if (manager.HasPassword(applicationUser.Id))
                            {
                                manager.RemovePassword(applicationUser.Id);
                            }
                            IList<UserLoginInfo> logins = manager.GetLogins(applicationUser.Id);
                            foreach (UserLoginInfo item2 in logins)
                            {
                                manager.RemoveLogin(applicationUser.Id, item2);
                            }
                            IList<Claim> claims = manager.GetClaims(applicationUser.Id);
                            foreach (Claim item3 in claims)
                            {
                                manager.RemoveClaim(applicationUser.Id, item3);
                            }
                            ApplicationUser entity = applicationDbContext.Users.Find(applicationUser.Id);
                            applicationDbContext.Users.Remove(entity);
                            applicationDbContext.SaveChanges();
                        }
                    }
                }
            }
        }

        public static void CreateEcoSystemAdmin()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                using (UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context))
                {
                    using (UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(store))
                    {
                        using (RoleManager<IdentityRole> rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)))
                        {
                            ApplicationUser applicationUser = manager.FindByName("EcoSystemAdmin");
                            if (applicationUser == null)
                            {
                                DateTime utcNow = DateTime.UtcNow;
                                ApplicationUser applicationUser2 = new ApplicationUser();
                                applicationUser2.UserName = "EcoSystemAdmin";
                                applicationUser2.Email = "EcoSystemAdmin@t.t";
                                applicationUser2.AccessFailedCount = 0;
                                applicationUser2.LockoutEnabled = true;
                                applicationUser2.LastActivityDate = utcNow;
                                applicationUser2.Approved = true;
                                applicationUser2.CreationDate = utcNow;
                                applicationUser2.LastLockoutDate = utcNow;
                                applicationUser2.LastLoginDate = new DateTime(1970, 1, 1);
                                applicationUser2.LockoutEndDateUtc = utcNow;
                                applicationUser = applicationUser2;
                                manager.Create(applicationUser, "EcoSystemAdmin");
                            }
                            CreateRoles(rm);
                            if (!manager.IsInRole(applicationUser.Id, "EcoSystemAdmin"))
                            {
                                manager.AddToRole(applicationUser.Id, "EcoSystemAdmin");
                            }
                        }
                    }
                }
            }
        }
    }
}
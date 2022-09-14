// CarShop.MvcForumIdentity.MembershipService
using ApplicationBoilerplate.Events;
using CarShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Events;
using mvcForum.Core.Interfaces.Services;
using mvcForum.DataProvider.EntityFramework;
using mvcForum.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarShop.MvcForumIdentity
{

    public class MembershipService : IMembershipService
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IWebUserProvider userProvider;

        private readonly MVCForumContext context;

        public MembershipService(IWebUserProvider userProvider)
        {
            this.userProvider = userProvider;
            context = new MVCForumContext("mvcForum.DataProvider.MainDB");
            ApplicationDbContext applicationDbContext = new ApplicationDbContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDbContext));
            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(applicationDbContext));
        }

        public void AddAccountToRoles(string accountName, string[] roles)
        {
            ApplicationUser applicationUser = userManager.FindByName(accountName);
            foreach (string role in roles)
            {
                userManager.AddToRole(applicationUser.Id, role);
            }
        }

        public bool CreateAccount(string accountName, string password, string emailAddress, out string errorMessage)
        {
            IEventPublisher service = DependencyResolver.Current.GetService<IEventPublisher>();
            NewUserEvent newUserEvent = new NewUserEvent();
            newUserEvent.Username = accountName;
            newUserEvent.EmailAddress = emailAddress;
            newUserEvent.IPAddress = HttpContext.Current.Request.UserHostAddress;
            NewUserEvent newUserEvent2 = newUserEvent;
            service.Publish(newUserEvent2);
            if (newUserEvent2.Bot)
            {
                errorMessage = "User was rejected";
                return false;
            }
            ApplicationUser applicationUser = userManager.FindByName(accountName);
            IdentityResult identityResult = null;
            identityResult = ((applicationUser != null) ? IdentityResult.Success : userManager.Create(new ApplicationUser
            {
                UserName = accountName,
                Email = emailAddress,
                AccessFailedCount = 0,
                LockoutEnabled = true,
                LastActivityDate = DateTime.UtcNow,
                Approved = true,
                CreationDate = DateTime.UtcNow,
                LastLockoutDate = DateTime.UtcNow,
                LastLoginDate = new DateTime(1970, 1, 1)
            }, password));
            errorMessage = string.Empty;
            if (!identityResult.Succeeded)
            {
                errorMessage = string.Join(",", identityResult.Errors);
            }
            else
            {
                applicationUser = userManager.FindByName(accountName);
                IConfiguration service2 = DependencyResolver.Current.GetService<IConfiguration>();
                ForumUser forumUser = new ForumUser(applicationUser.Id, applicationUser.UserName, applicationUser.Email, HttpContext.Current.Request.UserHostAddress);
                forumUser.Timezone = service2.DefaultTimezone;
                forumUser.Culture = service2.DefaultCulture;
                if (!string.IsNullOrEmpty(emailAddress) && string.IsNullOrEmpty(forumUser.EmailAddress))
                {
                    forumUser.EmailAddress = emailAddress;
                }
                context.ForumUsers.Add(forumUser);
                context.SaveChanges();
                foreach (int newUserGroup in service2.NewUserGroups)
                {
                    if (newUserGroup > 0)
                    {
                        Group group = context.Groups.Find(newUserGroup);
                        context.GroupMembers.Add(new GroupMember(group, forumUser));
                    }
                }
                context.SaveChanges();
            }
            return identityResult.Succeeded;
        }

        public void CreateRole(string roleName)
        {
            roleManager.Create(new IdentityRole
            {
                Name = roleName
            });
        }

        public void DeleteAccount(string accountName, bool deleteAllRelatedData)
        {
            ApplicationUser user = userManager.FindByName(accountName);
            userManager.Delete(user);
        }

        public void DeleteAccount(string accountName)
        {
            DeleteAccount(accountName, deleteAllRelatedData: true);
        }

        public IAccount GetAccount(bool online)
        {
            if (online)
            {
                ApplicationUser applicationUser = userManager.FindById(userProvider.ActiveUser.ProviderId);
                applicationUser.LastActivityDate = DateTime.UtcNow;
                userManager.Update(applicationUser);
                return GetAccount(applicationUser);
            }
            return GetAccount(userProvider.ActiveUser.ProviderId);
        }

        public IAccount GetAccount(object id)
        {
            ApplicationUser user = userManager.FindById(id.ToString());
            return GetAccount(user);
        }

        private IAccount GetAccount(ApplicationUser user)
        {
            Account account = new Account();
            account.AccountName = user.UserName;
            account.CreationDate = user.CreationDate;
            account.EmailAddress = user.Email;
            account.IsApproved = user.Approved;
            account.IsLockedOut = (user.LockoutEndDateUtc > DateTime.UtcNow);
            account.LastActivityDate = user.LastActivityDate;
            account.LastLockoutDate = user.LastLockoutDate;
            account.LastLoginDate = user.LastLoginDate;
            account.ProviderUserKey = user.Id;
            return account;
        }

        public IAccount GetAccountByEmailAddress(string emailAddress)
        {
            ApplicationUser user = userManager.FindByEmail(emailAddress);
            return GetAccount(user);
        }

        public IAccount GetAccountByName(string accountName, bool online)
        {
            ApplicationUser applicationUser = userManager.FindByName(accountName);
            if (online)
            {
                applicationUser.LastActivityDate = DateTime.UtcNow;
                userManager.Update(applicationUser);
            }
            return GetAccount(applicationUser);
        }

        public IAccount GetAccountByName(string accountName)
        {
            return GetAccountByName(accountName, online: false);
        }

        public string GetAccountNameByEmailAddress(string emailAddress)
        {
            ApplicationUser applicationUser = userManager.FindByEmail(emailAddress);
            if (applicationUser == null)
            {
                return string.Empty;
            }
            return applicationUser.UserName;
        }

        public IEnumerable<IAccount> GetAllAccounts(int page, int pageSize, out int total)
        {
            total = userManager.Users.Count();
            return from u in userManager.Users.Skip((page - 1) * pageSize).Take(pageSize)
                   select GetAccount(u);
        }

        public string[] GetAllRoles()
        {
            return (from r in roleManager.Roles
                    select r.Name).ToArray();
        }

        public string[] GetRolesForAccount(string accountName)
        {
            ApplicationUser applicationUser = userManager.FindByName(accountName);
            IEnumerable<string> roleIds = (from r in applicationUser.Roles
                                           select r.RoleId).ToList();
            return (from r in roleManager.Roles
                    where roleIds.Contains(r.Id)
                    select r.Name).ToArray();
        }

        public string[] GetRolesForAccount()
        {
            ApplicationUser applicationUser = userManager.FindById(userProvider.ActiveUser.ProviderId);
            return GetRolesForAccount(applicationUser.UserName);
        }

        public bool IsAccountInRole(string accountName, string roleName)
        {
            ApplicationUser applicationUser = userManager.FindByName(accountName);
            if (applicationUser != null)
            {
                return userManager.IsInRole(userManager.FindByName(accountName).Id, roleName);
            }
            return false;
        }

        public void RemoveAccountFromRoles(string accountName, string[] roles)
        {
            ApplicationUser applicationUser = userManager.FindByName(accountName);
            foreach (string role in roles)
            {
                userManager.RemoveFromRole(applicationUser.Id, role);
            }
        }

        public void UnlockAccount(string accountName)
        {
            ApplicationUser applicationUser = userManager.FindByName(accountName);
            applicationUser.LockoutEndDateUtc = DateTime.UtcNow;
            userManager.Update(applicationUser);
        }

        public void UpdateAccount(IAccount account)
        {
            ApplicationUser applicationUser = userManager.FindById(account.ProviderUserKey.ToString());
            applicationUser.Approved = account.IsApproved;
            applicationUser.Email = account.EmailAddress;
            applicationUser.LastActivityDate = account.LastActivityDate;
            applicationUser.LastLockoutDate = account.LastLockoutDate;
            applicationUser.LastLoginDate = account.LastLoginDate;
            userManager.Update(applicationUser);
        }

        public bool ValidateAccount(string accountName, string password)
        {
            ApplicationUser applicationUser = userManager.Find(accountName, password);
            return applicationUser != null;
        }
    }
}
// CarShop.MvcForumIdentity.IdentityUserProvider
using ApplicationBoilerplate.DataProvider;
using CarShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using mvcForum.Core;
using mvcForum.Core.Specifications;
using mvcForum.Web.Interfaces;
using System.Data.Entity;
using System.Web;

namespace CarShop.MvcForumIdentity
{

    public class IdentityUserProvider : IWebUserProvider
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IRepository<ForumUser> userRepo;

        protected ForumUser user;

        protected bool checkedAuthenticated;

        protected bool authenticated;

        public ForumUser ActiveUser
        {
            get
            {
                if (Authenticated)
                {
                    return user;
                }
                return null;
            }
        }

        public bool Authenticated
        {
            get
            {
                if (!checkedAuthenticated)
                {
                    if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        ApplicationUser applicationUser = userManager.FindByName(HttpContext.Current.User.Identity.Name);
                        authenticated = (applicationUser != null);
                        if (authenticated)
                        {
                            try
                            {
                                user = userRepo.ReadOne(new ForumUserSpecifications.SpecificProviderUserKey(applicationUser.Id));
                            }
                            catch
                            {
                            }
                            authenticated = (user != null);
                        }
                    }
                    checkedAuthenticated = true;
                }
                return authenticated;
            }
        }

        public IdentityUserProvider(IRepository<ForumUser> userRepo, DbContext context)
        {
            this.userRepo = userRepo;
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
    }
}
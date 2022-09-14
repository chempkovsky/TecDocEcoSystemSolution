using CarShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using mvcForum.Core.Interfaces.Services;
using System.Data.Entity;
using System.Security.Claims;
using System.Web;

namespace CarShop.MvcForumIdentity
{
    // CarShop.MvcForumIdentity.AuthenticationService

    public class AuthenticationService : IAuthenticationService
    {
        private readonly DbContext context;

        private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;

        public AuthenticationService(DbContext context)
        {
            this.context = new ApplicationDbContext();
        }

        public void SignIn(IAccount account, bool createPersistentCookie)
        {
            UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = manager.FindByName(account.AccountName);
            AuthenticationManager.SignOut("ExternalCookie");
            ClaimsIdentity claimsIdentity = manager.CreateIdentity(user, "ApplicationCookie");
            AuthenticationManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = createPersistentCookie
            }, claimsIdentity);
        }

        public void SignOut()
        {
            AuthenticationManager.SignOut();
        }
    }
}
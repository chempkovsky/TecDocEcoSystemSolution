// CarShop.Models.CustomApplicationDbContextInitializer
using CarShop.Models;
using CarShop.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace CarShop.Models
{

    public class CustomApplicationDbContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            CreateEcoSystemAdmin(context);
            base.Seed(context);
        }

        protected void CreateEcoSystemAdmin(ApplicationDbContext appDbCnt)
        {
            UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(appDbCnt));
            RoleManager<IdentityRole> rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(appDbCnt));
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
            EntAdminActions.CreateRoles(rm);
            if (!manager.IsInRole(applicationUser.Id, "EcoSystemAdmin"))
            {
                manager.AddToRole(applicationUser.Id, "EcoSystemAdmin");
            }
        }
    }
}
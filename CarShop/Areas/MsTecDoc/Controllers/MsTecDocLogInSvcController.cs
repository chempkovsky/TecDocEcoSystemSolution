// CarShop.Areas.MsTecDoc.Controllers.MsTecDocLogInSvcController
using CarShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Areas.MsTecDoc.Controllers
{

    public class MsTecDocLogInSvcController : ApiController
    {
        private bool ViewBag_IsEcoSystemAdmin;

        private bool ViewBag_IsEnterpriseAdmin;

        private bool ViewBag_IsBranchAdmin;

        private CarShopContext db = new CarShopContext();

        private IAuthenticationManager Authentication => base.Request.GetOwinContext().Authentication;

        protected void UserIsInRoles(string aUserName)
        {
            ViewBag_IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            ViewBag_IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            ViewBag_IsBranchAdmin = base.User.IsInRole("BranchAdmin");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> WebLogOn(string UserName, string Password)
        {
            using (ApplicationDbContext appDbCnt = new ApplicationDbContext())
            {
                using (UserManager<ApplicationUser> um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(appDbCnt)))
                {
                    ApplicationUser user = await um.FindAsync(UserName, Password);
                    if (user == null)
                    {
                        base.ModelState.AddModelError("", "Не найду такого пользователя.");
                        return base.Request.CreateResponse(HttpStatusCode.BadRequest, base.ModelState);
                    }
                    Authentication.SignOut("ExternalCookie");
                    ClaimsIdentity identity = await um.CreateIdentityAsync(user, "ApplicationCookie");
                    Authentication.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, identity);
                    UserIsInRoles(UserName);
                    if (!ViewBag_IsEcoSystemAdmin && !ViewBag_IsEnterpriseAdmin && ViewBag_IsBranchAdmin)
                    {
                        Authentication.SignOut();
                        base.ModelState.AddModelError("", "Не найду такого пользователя с правами EcoSystemAdmin или EnterpriseAdmin или BranchAdmin.");
                        return base.Request.CreateResponse(HttpStatusCode.BadRequest, base.ModelState);
                    }
                    if (!ViewBag_IsEcoSystemAdmin)
                    {
                        if (ViewBag_IsEnterpriseAdmin)
                        {
                            EnterpriseUserTDES enterpriseUserTDES = db.EnterpriseUserTDES.Where((EnterpriseUserTDES e) => e.EntUserNic == UserName).FirstOrDefault();
                            if (enterpriseUserTDES == null)
                            {
                                Authentication.SignOut();
                                base.ModelState.AddModelError("", "Не найду такого пользователя.");
                                return base.Request.CreateResponse(HttpStatusCode.BadRequest, base.ModelState);
                            }
                            if (!enterpriseUserTDES.IsActive)
                            {
                                Authentication.SignOut();
                                base.ModelState.AddModelError("", "Данная учетная запись заблокирована администратором предприятия.");
                                return base.Request.CreateResponse(HttpStatusCode.BadRequest, base.ModelState);
                            }
                        }
                        else if (ViewBag_IsBranchAdmin)
                        {
                            EnterpriseBranchUserTDES enterpriseBranchUserTDES = db.EnterpriseBranchUserTDES.Where((EnterpriseBranchUserTDES e) => e.EntUserNic == UserName).FirstOrDefault();
                            if (enterpriseBranchUserTDES == null)
                            {
                                Authentication.SignOut();
                                base.ModelState.AddModelError("", "Не найду такого пользователя.");
                                return base.Request.CreateResponse(HttpStatusCode.BadRequest, base.ModelState);
                            }
                            if (!enterpriseBranchUserTDES.IsActive)
                            {
                                Authentication.SignOut();
                                base.ModelState.AddModelError("", "Данная учетная запись заблокирована администратором подразделения.");
                                return base.Request.CreateResponse(HttpStatusCode.BadRequest, base.ModelState);
                            }
                        }
                    }
                    return base.Request.CreateResponse(HttpStatusCode.OK);
                }
            }
        }
    }
}
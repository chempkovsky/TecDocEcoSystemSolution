// CarShop.Areas.RestSvc.Controllers.BranchLogInSvcController
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

namespace CarShop.Areas.RestSvc.Controllers
{

    public class BranchLogInSvcController : ApiController
    {
        private bool ViewBag_IsEcoSystemAdmin;

        private bool ViewBag_IsEnterpriseAdmin;

        private bool ViewBag_IsEnterpriseAudit;

        private bool ViewBag_IsBranchAdmin;

        private bool ViewBag_IsBranchBooker;

        private bool ViewBag_IsBranchAudit;

        private bool ViewBag_IsBranchSeller;

        private CarShopContext db = new CarShopContext();

        private IAuthenticationManager Authentication => base.Request.GetOwinContext().Authentication;

        protected void UserIsInRoles(string aUserName)
        {
            ViewBag_IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            ViewBag_IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            ViewBag_IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
            ViewBag_IsBranchAdmin = base.User.IsInRole("BranchAdmin");
            ViewBag_IsBranchAudit = base.User.IsInRole("BranchAudit");
            ViewBag_IsBranchSeller = base.User.IsInRole("BranchSeller");
            ViewBag_IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<HttpResponseMessage> WebLogOn(string UserName, string Password)
        {
            using (ApplicationDbContext appDbCnt = new ApplicationDbContext())
            {
                using (UserManager<ApplicationUser> um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(appDbCnt)))
                {
                    ApplicationUser user = await um.FindAsync(UserName, Password);
                    if (user == null)
                    {
                        base.ModelState.AddModelError("", "???? ?????????? ???????????? ????????????????????????.");
                        return base.Request.CreateResponse(HttpStatusCode.BadRequest, base.ModelState);
                    }
                    Authentication.SignOut("ExternalCookie");
                    ClaimsIdentity identity = await um.CreateIdentityAsync(user, "ApplicationCookie");
                    Authentication.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, identity);
                    UserIsInRoles(UserName);
                    if (!ViewBag_IsEcoSystemAdmin)
                    {
                        if (ViewBag_IsEnterpriseAdmin || ViewBag_IsEnterpriseAudit)
                        {
                            EnterpriseUserTDES enterpriseUserTDES = db.EnterpriseUserTDES.Where((EnterpriseUserTDES e) => e.EntUserNic == UserName).FirstOrDefault();
                            if (enterpriseUserTDES == null)
                            {
                                Authentication.SignOut();
                                base.ModelState.AddModelError("", "???? ?????????? ???????????? ????????????????????????.");
                                return base.Request.CreateResponse(HttpStatusCode.BadRequest, base.ModelState);
                            }
                            if (!enterpriseUserTDES.IsActive)
                            {
                                Authentication.SignOut();
                                base.ModelState.AddModelError("", "???????????? ?????????????? ???????????? ?????????????????????????? ?????????????????????????????? ??????????????????????.");
                                return base.Request.CreateResponse(HttpStatusCode.BadRequest, base.ModelState);
                            }
                        }
                        else if (ViewBag_IsBranchAdmin || ViewBag_IsBranchAudit || ViewBag_IsBranchSeller || ViewBag_IsBranchBooker)
                        {
                            EnterpriseBranchUserTDES enterpriseBranchUserTDES = db.EnterpriseBranchUserTDES.Where((EnterpriseBranchUserTDES e) => e.EntUserNic == UserName).FirstOrDefault();
                            if (enterpriseBranchUserTDES == null)
                            {
                                Authentication.SignOut();
                                base.ModelState.AddModelError("", "???? ?????????? ???????????? ????????????????????????.");
                                return base.Request.CreateResponse(HttpStatusCode.BadRequest, base.ModelState);
                            }
                            if (!enterpriseBranchUserTDES.IsActive)
                            {
                                Authentication.SignOut();
                                base.ModelState.AddModelError("", "???????????? ?????????????? ???????????? ?????????????????????????? ?????????????????????????????? ??????????????????????????.");
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
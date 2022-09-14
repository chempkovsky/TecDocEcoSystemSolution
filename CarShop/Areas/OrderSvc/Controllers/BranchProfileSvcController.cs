// CarShop.Areas.OrderSvc.Controllers.BranchProfileSvcController
using CarShop.Models;
using CarShop.Properties;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Areas.OrderSvc.Controllers
{

    public class BranchProfileSvcController : ApiController
    {
        private Guid guestEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private Guid guestEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private CarShopContext db = new CarShopContext();

        private string CarShopOrderContextCatalog;

        private CarShopOrdersContext _dbOrders;

        private bool ViewBag_IsEcoSystemAdmin;

        private bool ViewBag_IsEnterpriseAdmin;

        private bool ViewBag_IsEnterpriseAudit;

        private bool ViewBag_IsBranchAdmin;

        private bool ViewBag_IsBranchBooker;

        private CarShopOrdersContext dbOrders
        {
            get
            {
                if (_dbOrders == null)
                {
                    _dbOrders = new CarShopOrdersContext(CarShopOrderContextCatalog);
                }
                return _dbOrders;
            }
        }

        private void UserIsInRoles()
        {
            ViewBag_IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            ViewBag_IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            ViewBag_IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
            ViewBag_IsBranchAdmin = base.User.IsInRole("BranchAdmin");
            ViewBag_IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        private void ViewBagHelper(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            EnterpriseBranchTDES enterpriseBranchTDES = (from e in db.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                         select e).FirstOrDefault();
            if (enterpriseBranchTDES != null)
            {
                if (!enterpriseBranchTDES.IsActive || !enterpriseBranchTDES.EnterpriseTDES.IsActive)
                {
                    base.ModelState.AddModelError("", Resources.SendEnterpriseBranchTDES_ISNOTACTIVE);
                }
            }
            else
            {
                base.ModelState.AddModelError("", Resources.SendEnterpriseBranchTDES_NOTFOUND);
            }
            enterpriseBranchTDES = (from e in db.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                    where e.EntBranchGuid == guestEntBranchGuid && e.EntGuid == guestEntGuid
                                    select e).FirstOrDefault();
            if (enterpriseBranchTDES != null)
            {
                if (enterpriseBranchTDES.IsActive && enterpriseBranchTDES.EnterpriseTDES.IsActive)
                {
                    CarShopOrderContextCatalog = enterpriseBranchTDES.OrderCatalog;
                }
                else
                {
                    base.ModelState.AddModelError("", Resources.OrderBranchTDES_ISNOTACTIVE);
                }
            }
            else
            {
                base.ModelState.AddModelError("", Resources.OrderBranchTDES_NOTFOUND);
            }
        }

        public HttpResponseMessage GetGuestProfileTDES(Guid? searchEntBranchGuid, string searchGestUserNic)
        {
            Guid? searchEntGuid = null;
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if (!ViewBag_IsEcoSystemAdmin)
            {
                string aName = base.User.Identity.Name;
                if (ViewBag_IsEnterpriseAdmin || ViewBag_IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES != null)
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                    else
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                }
            }
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            GuestProfileTDES guestProfileTDES = dbOrders.GuestProfileTDES.Where((GuestProfileTDES e) => e.GestUserNic == searchGestUserNic).FirstOrDefault();
            if (guestProfileTDES == null)
            {
                throw new HttpResponseException(base.Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return base.Request.CreateResponse(HttpStatusCode.OK, guestProfileTDES);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            if (_dbOrders != null)
            {
                _dbOrders.Dispose();
                _dbOrders = null;
            }
            base.Dispose(disposing);
        }
    }
}
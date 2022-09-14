// CarShop.Areas.OrderSvc.Controllers.BranchOrderSvcController
using CarShop.Models;
using CarShop.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Areas.OrderSvc.Controllers
{

    public class BranchOrderSvcController : ApiController
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

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public HttpResponseMessage GetGuestOrderTDES(Guid? searchEntBranchGuid)
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
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                }
            }
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            List<GuestOrderTmp> value = (from e in dbOrders.GuestOrderTDES
                                         where e.IsReplicated > 0 && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                         select new GuestOrderTmp
                                         {
                                             GuestOrderGuid = e.GuestOrderGuid,
                                             GestUserNic = e.GestUserNic,
                                             EntBranchGuid = e.EntBranchGuid,
                                             EntBranchDescription = e.EntBranchDescription,
                                             IsActive = e.IsActive,
                                             IsDone = e.IsDone,
                                             LastUpdated = e.LastUpdated,
                                             LastReplicated = e.LastReplicated
                                         }).Take(1).ToList();
            return base.Request.CreateResponse(HttpStatusCode.OK, value);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public HttpResponseMessage GetGuestOrderTDES(Guid? searchEntBranchGuid, Guid? searchGuestOrderGuid)
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
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                }
            }
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            GuestOrderTmp guestOrderTmp = (from e in dbOrders.GuestOrderTDES
                                           where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid
                                           select new GuestOrderTmp
                                           {
                                               GuestOrderGuid = e.GuestOrderGuid,
                                               GestUserNic = e.GestUserNic,
                                               EntBranchGuid = e.EntBranchGuid,
                                               EntBranchDescription = e.EntBranchDescription,
                                               IsActive = e.IsActive,
                                               IsDone = e.IsDone,
                                               LastUpdated = e.LastUpdated,
                                               LastReplicated = e.LastReplicated
                                           }).FirstOrDefault();
            if (guestOrderTmp == null)
            {
                throw new HttpResponseException(base.Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return base.Request.CreateResponse(HttpStatusCode.OK, guestOrderTmp);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public HttpResponseMessage PutGuestOrderTDES(Guid? searchEntBranchGuid, Guid searchGuestOrderGuid, GuestOrderTmp guestordertdes)
        {
            if (!base.ModelState.IsValid)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
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
            if (searchGuestOrderGuid != guestordertdes.GuestOrderGuid)
            {
                return base.Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (searchEntBranchGuid.Value != guestordertdes.EntBranchGuid)
            {
                return base.Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            GuestOrderTDES guestOrderTDES = dbOrders.GuestOrderTDES.Where((GuestOrderTDES e) => e.GuestOrderGuid == searchGuestOrderGuid).FirstOrDefault();
            if (guestOrderTDES == null)
            {
                return base.Request.CreateResponse(HttpStatusCode.OK);
            }
            guestOrderTDES.IsActive = guestordertdes.IsActive;
            guestOrderTDES.IsDone = guestordertdes.IsDone;
            guestOrderTDES.LastUpdated = guestordertdes.LastUpdated;
            guestOrderTDES.LastReplicated = guestordertdes.LastReplicated;
            try
            {
                dbOrders.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.NotFound, exception);
            }
            return base.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage PostGuestOrderTDES(Guid? searchEntBranchGuid, GuestOrderTmp guestordertdes)
        {
            if (!base.ModelState.IsValid)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
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
            if (base.ModelState.IsValid)
            {
                GuestOrderTDES guestOrderTDES = new GuestOrderTDES();
                guestordertdes.CopyTo(guestOrderTDES);
                dbOrders.GuestOrderTDES.Add(guestOrderTDES);
                dbOrders.SaveChanges();
                HttpResponseMessage httpResponseMessage = base.Request.CreateResponse(HttpStatusCode.Created, guestordertdes);
                httpResponseMessage.Headers.Location = new Uri(base.Url.Link("DefaultApi", new
                {
                    searchEntBranchGuid = guestordertdes.EntBranchGuid,
                    searchGuestOrderGuid = guestordertdes.GuestOrderGuid
                }));
                return httpResponseMessage;
            }
            return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public HttpResponseMessage DeleteGuestOrderTDES(Guid? searchEntBranchGuid, Guid? searchGuestOrderGuid)
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
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                }
            }
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            GuestOrderTDES guestOrderTDES = dbOrders.GuestOrderTDES.Where((GuestOrderTDES e) => (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid).FirstOrDefault();
            if (guestOrderTDES == null)
            {
                return base.Request.CreateResponse(HttpStatusCode.NotFound);
            }
            dbOrders.GuestOrderTDES.Remove(guestOrderTDES);
            try
            {
                dbOrders.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.NotFound, exception);
            }
            return base.Request.CreateResponse(HttpStatusCode.OK, guestOrderTDES);
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
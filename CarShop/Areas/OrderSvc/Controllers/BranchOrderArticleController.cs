// CarShop.Areas.OrderSvc.Controllers.BranchOrderArticleController
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

    public class BranchOrderArticleController : ApiController
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
        public HttpResponseMessage GetGuestOrderArticleTDES(Guid? searchEntBranchGuid, Guid? searchGuestOrderGuid)
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
                base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND);
                return base.Request.CreateErrorResponse(HttpStatusCode.NotFound, base.ModelState);
            }
            List<GuestOrderArticleTmp> value = (from e in dbOrders.GuestOrderArticleTDES
                                                where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && e.IsReplicated > 0
                                                select new GuestOrderArticleTmp
                                                {
                                                    GuestOrderGuid = e.GuestOrderGuid,
                                                    EntBranchArticle = e.EntBranchArticle,
                                                    EntBranchSup = e.EntBranchSup,
                                                    EntArticleDescription = e.EntArticleDescription,
                                                    ART_ARTICLE_NR = e.ART_ARTICLE_NR,
                                                    SUP_TEXT = e.SUP_TEXT,
                                                    ExternArticleEAN = e.ExternArticleEAN,
                                                    ArtAmount = e.ArtAmount,
                                                    ArtPrice = e.ArtPrice,
                                                    EntBranchGuid = e.EntBranchGuid,
                                                    LastUpdated = e.LastUpdated,
                                                    LastReplicated = e.LastReplicated
                                                }).ToList();
            return base.Request.CreateResponse(HttpStatusCode.OK, value);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public HttpResponseMessage GetGuestOrderArticleTDES(Guid? searchEntBranchGuid, Guid? searchGuestOrderGuid, string searchEntBranchArticle, string searchEntBranchSup)
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
                base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND);
                return base.Request.CreateErrorResponse(HttpStatusCode.NotFound, base.ModelState);
            }
            GuestOrderArticleTDES guestOrderArticleTDES = dbOrders.GuestOrderArticleTDES.Where((GuestOrderArticleTDES e) => (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && e.EntBranchArticle == searchEntBranchArticle && e.EntBranchSup == searchEntBranchSup).FirstOrDefault();
            if (guestOrderArticleTDES == null)
            {
                throw new HttpResponseException(base.Request.CreateResponse(HttpStatusCode.NotFound));
            }
            GuestOrderArticleTmp guestOrderArticleTmp = new GuestOrderArticleTmp();
            guestOrderArticleTmp.CopyFrom(guestOrderArticleTDES);
            return base.Request.CreateResponse(HttpStatusCode.OK, guestOrderArticleTmp);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public HttpResponseMessage PutGuestOrderArticleTDES(Guid? searchEntBranchGuid, Guid searchGuestOrderGuid, string searchEntBranchArticle, string searchEntBranchSup, GuestOrderArticleTmp guestorderarticletmp)
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
            GuestOrderTDES guestOrderTDES = dbOrders.GuestOrderTDES.Where((GuestOrderTDES e) => (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.GuestOrderGuid == searchGuestOrderGuid).FirstOrDefault();
            if (guestOrderTDES == null)
            {
                base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND);
                return base.Request.CreateErrorResponse(HttpStatusCode.NotFound, base.ModelState);
            }
            GuestOrderArticleTDES guestOrderArticleTDES = dbOrders.GuestOrderArticleTDES.Where((GuestOrderArticleTDES e) => e.GuestOrderGuid == searchGuestOrderGuid && e.EntBranchArticle == searchEntBranchArticle && e.EntBranchSup == searchEntBranchSup).FirstOrDefault();
            if (guestOrderArticleTDES == null)
            {
                return base.Request.CreateResponse(HttpStatusCode.OK);
            }
            guestOrderArticleTDES.LastUpdated = guestorderarticletmp.LastUpdated;
            guestOrderArticleTDES.LastReplicated = guestorderarticletmp.LastReplicated;
            guestOrderArticleTDES.ArtPrice = guestorderarticletmp.ArtPrice;
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

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public HttpResponseMessage PostGuestOrderArticleTDES(Guid? searchEntBranchGuid, Guid searchGuestOrderGuid, GuestOrderArticleTmp guestorderarticletmp)
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
            GuestOrderTDES guestOrderTDES = dbOrders.GuestOrderTDES.Where((GuestOrderTDES e) => (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.GuestOrderGuid == searchGuestOrderGuid).FirstOrDefault();
            if (guestOrderTDES == null)
            {
                base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND);
                return base.Request.CreateErrorResponse(HttpStatusCode.NotFound, base.ModelState);
            }
            if (base.ModelState.IsValid)
            {
                GuestOrderArticleTDES guestOrderArticleTDES = new GuestOrderArticleTDES();
                guestorderarticletmp.CopyTo(guestOrderArticleTDES);
                dbOrders.GuestOrderArticleTDES.Add(guestOrderArticleTDES);
                dbOrders.SaveChanges();
                HttpResponseMessage httpResponseMessage = base.Request.CreateResponse(HttpStatusCode.Created, guestorderarticletmp);
                httpResponseMessage.Headers.Location = new Uri(base.Url.Link("DefaultApi", new
                {
                    searchEntBranchGuid = guestOrderArticleTDES.EntBranchGuid,
                    searchGuestOrderGuid = guestOrderArticleTDES.GuestOrderGuid,
                    searchEntBranchArticle = guestOrderArticleTDES.EntBranchArticle,
                    searchEntBranchSup = guestOrderArticleTDES.EntBranchSup
                }));
                return httpResponseMessage;
            }
            return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public HttpResponseMessage DeleteGuestOrderArticleTDES(Guid? searchEntBranchGuid, Guid searchGuestOrderGuid, string searchEntBranchArticle, string searchEntBranchSup)
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
            GuestOrderTDES guestOrderTDES = dbOrders.GuestOrderTDES.Where((GuestOrderTDES e) => (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.GuestOrderGuid == searchGuestOrderGuid).FirstOrDefault();
            if (guestOrderTDES == null)
            {
                base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND);
                return base.Request.CreateErrorResponse(HttpStatusCode.NotFound, base.ModelState);
            }
            GuestOrderArticleTDES guestOrderArticleTDES = dbOrders.GuestOrderArticleTDES.Where((GuestOrderArticleTDES e) => e.GuestOrderGuid == searchGuestOrderGuid && e.EntBranchArticle == searchEntBranchArticle && e.EntBranchSup == searchEntBranchSup).FirstOrDefault();
            if (guestOrderArticleTDES == null)
            {
                return base.Request.CreateResponse(HttpStatusCode.NotFound);
            }
            dbOrders.GuestOrderArticleTDES.Remove(guestOrderArticleTDES);
            try
            {
                dbOrders.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.NotFound, exception);
            }
            GuestOrderArticleTmp guestOrderArticleTmp = new GuestOrderArticleTmp();
            guestOrderArticleTmp.CopyFrom(guestOrderArticleTDES);
            return base.Request.CreateResponse(HttpStatusCode.OK, guestOrderArticleTmp);
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
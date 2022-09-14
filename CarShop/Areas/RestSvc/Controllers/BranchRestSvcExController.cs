// CarShop.Areas.RestSvc.Controllers.BranchRestSvcExController
using CarShop.Models;
using CarShop.Properties;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Areas.RestSvc.Controllers
{

    public class BranchRestSvcExController : ApiController
    {
        private Guid guestEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private Guid guestEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private CarShopContext db = new CarShopContext();

        private string CarShopRestContextCatalog;

        private CarShopRestContext _dbRest;

        private bool ViewBag_IsEcoSystemAdmin;

        private bool ViewBag_IsEnterpriseAdmin;

        private bool ViewBag_IsEnterpriseAudit;

        private bool ViewBag_IsBranchAdmin;

        private bool ViewBag_IsBranchBooker;

        private CarShopRestContext dbRest
        {
            get
            {
                if (_dbRest == null)
                {
                    _dbRest = new CarShopRestContext(CarShopRestContextCatalog);
                }
                return _dbRest;
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
                    CarShopRestContextCatalog = enterpriseBranchTDES.TecDocCatalog;
                }
                else
                {
                    base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_ISNOTACTIVE);
                }
            }
            else
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        [HttpGet]
        public HttpResponseMessage GetBranchRestTDES(Guid? searchEntBranchGuid = null, string searchEntBranchArticle = null, string searchEntBranchSup = null)
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
            if (string.IsNullOrEmpty(CarShopRestContextCatalog))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            BranchRestTmp branchRestTmp = (from e in dbRest.BranchRestTDES.Include("BranchRestArticleDescriptionTDES")
                                           where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntBranchArticle == searchEntBranchArticle && e.EntBranchSup == searchEntBranchSup
                                           select new BranchRestTmp
                                           {
                                               EntBranchGuid = e.EntBranchGuid,
                                               EntBranchArticle = e.EntBranchArticle,
                                               EntBranchSup = e.EntBranchSup,
                                               ART_ARTICLE_NR = e.ART_ARTICLE_NR,
                                               SUP_TEXT = e.SUP_TEXT,
                                               ArtAmount = e.ArtAmount,
                                               ArtPrice = e.ArtPrice,
                                               LastUpdated = e.LastUpdated,
                                               LastReplicated = e.LastReplicated,
                                               TSConcClmn = e.TSConcClmn,
                                               ExternArticleEAN = e.ExternArticleEAN,
                                               EntArticleDescriptionId = e.EntArticleDescriptionId,
                                               EntArticleDescription = e.BranchRestArticleDescriptionTDES.EntArticleDescription
                                           }).FirstOrDefault();
            if (branchRestTmp == null)
            {
                throw new HttpResponseException(base.Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return base.Request.CreateResponse(HttpStatusCode.OK, branchRestTmp);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public HttpResponseMessage PostBranchRestTDES(BranchRestTmp branchresttdes)
        {
            if (!base.ModelState.IsValid)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            Guid? searchEntGuid = null;
            Guid? guid = null;
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if (ViewBag_IsEcoSystemAdmin)
            {
                guid = branchresttdes.EntBranchGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (ViewBag_IsEnterpriseAdmin || ViewBag_IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                    guid = branchresttdes.EntBranchGuid;
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
                        guid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                    else
                    {
                        searchEntGuid = null;
                        guid = null;
                    }
                }
            }
            ViewBagHelper(searchEntGuid, guid);
            if (!base.ModelState.IsValid)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            if (string.IsNullOrEmpty(CarShopRestContextCatalog))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            branchresttdes.EntBranchGuid = guid.Value;
            BranchRestArticleDescriptionTDES branchRestArticleDescriptionTDES = null;
            BranchRestTDES branchRestTDES = (from e in dbRest.BranchRestTDES.Include("BranchRestArticleDescriptionTDES")
                                             where e.EntBranchGuid == branchresttdes.EntBranchGuid && e.EntBranchArticle == branchresttdes.EntBranchArticle && e.EntBranchSup == branchresttdes.EntBranchSup
                                             select e).FirstOrDefault();
            if (branchRestTDES == null)
            {
                branchRestArticleDescriptionTDES = dbRest.BranchRestArticleDescriptionTDES.Where((BranchRestArticleDescriptionTDES e) => e.EntArticleDescription == branchresttdes.EntArticleDescription).FirstOrDefault();
                if (branchRestArticleDescriptionTDES == null)
                {
                    BranchRestArticleDescriptionTDES branchRestArticleDescriptionTDES2 = new BranchRestArticleDescriptionTDES();
                    branchRestArticleDescriptionTDES2.EntArticleDescription = branchresttdes.EntArticleDescription;
                    branchRestArticleDescriptionTDES = branchRestArticleDescriptionTDES2;
                    dbRest.BranchRestArticleDescriptionTDES.Add(branchRestArticleDescriptionTDES);
                    dbRest.SaveChanges();
                }
                branchresttdes.EntArticleDescriptionId = branchRestArticleDescriptionTDES.EntArticleDescriptionId;
                BranchRestTDES branchRestTDES2 = new BranchRestTDES();
                branchresttdes.CopyTo(branchRestTDES2, doCreateDescr: false);
                branchRestTDES2.TSConcClmn = null;
                dbRest.BranchRestTDES.Add(branchRestTDES2);
                dbRest.SaveChanges();
                branchresttdes.CopyFrom(branchRestTDES2);
                HttpResponseMessage httpResponseMessage = base.Request.CreateResponse(HttpStatusCode.Created, branchresttdes);
                httpResponseMessage.Headers.Location = new Uri(base.Url.Link("DefaultApi", new
                {
                    searchEntBranchGuid = branchresttdes.EntBranchGuid,
                    searchEntBranchArticle = branchresttdes.EntBranchArticle,
                    searchEntBranchSup = branchresttdes.EntBranchSup
                }));
                return httpResponseMessage;
            }
            if (branchRestTDES.ArtAmount < 1)
            {
                if (branchRestTDES.BranchRestArticleDescriptionTDES.EntArticleDescription != branchresttdes.EntArticleDescription)
                {
                    branchRestArticleDescriptionTDES = dbRest.BranchRestArticleDescriptionTDES.Where((BranchRestArticleDescriptionTDES e) => e.EntArticleDescription == branchresttdes.EntArticleDescription).FirstOrDefault();
                    if (branchRestArticleDescriptionTDES == null)
                    {
                        BranchRestArticleDescriptionTDES branchRestArticleDescriptionTDES3 = new BranchRestArticleDescriptionTDES();
                        branchRestArticleDescriptionTDES3.EntArticleDescription = branchresttdes.EntArticleDescription;
                        branchRestArticleDescriptionTDES = branchRestArticleDescriptionTDES3;
                        dbRest.BranchRestArticleDescriptionTDES.Add(branchRestArticleDescriptionTDES);
                        dbRest.SaveChanges();
                    }
                    branchRestTDES.EntArticleDescriptionId = branchRestArticleDescriptionTDES.EntArticleDescriptionId;
                    branchRestTDES.BranchRestArticleDescriptionTDES = branchRestArticleDescriptionTDES;
                }
                branchRestTDES.ART_ARTICLE_NR = branchresttdes.ART_ARTICLE_NR;
                branchRestTDES.SUP_TEXT = branchresttdes.SUP_TEXT;
                branchRestTDES.ArtAmount = branchresttdes.ArtAmount;
                branchRestTDES.ArtPrice = branchresttdes.ArtPrice;
                branchRestTDES.LastUpdated = branchresttdes.LastUpdated;
                branchRestTDES.LastReplicated = branchresttdes.LastReplicated;
                branchRestTDES.ExternArticleEAN = branchresttdes.ExternArticleEAN;
                try
                {
                    dbRest.Entry(branchRestTDES).State = EntityState.Modified;
                    dbRest.SaveChanges();
                }
                catch (DbUpdateConcurrencyException exception)
                {
                    return base.Request.CreateErrorResponse(HttpStatusCode.NotFound, exception);
                }
            }
            HttpResponseMessage httpResponseMessage2 = base.Request.CreateResponse(HttpStatusCode.Created, branchresttdes);
            httpResponseMessage2.Headers.Location = new Uri(base.Url.Link("DefaultApi", new
            {
                searchEntBranchGuid = branchresttdes.EntBranchGuid,
                searchEntBranchArticle = branchresttdes.EntBranchArticle,
                searchEntBranchSup = branchresttdes.EntBranchSup
            }));
            return httpResponseMessage2;
        }
    }
}
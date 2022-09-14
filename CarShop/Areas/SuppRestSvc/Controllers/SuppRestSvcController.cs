// CarShop.Areas.SuppRestSvc.Controllers.SuppRestSvcController
using CarShop.Models;
using CarShop.Properties;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Areas.SuppRestSvc.Controllers
{

    public class SuppRestSvcController : ApiController
    {
        private Guid guestEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private Guid guestEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private CarShopContext db = new CarShopContext();

        private string CarShopRestContextCatalog;

        private CarShopRestContext _dbRest;

        private string CarShopArticleContextCatalog;

        private CarShopArticleContext _dbArticleContext;

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

        private CarShopArticleContext dbArticleContext
        {
            get
            {
                if (_dbArticleContext == null)
                {
                    _dbArticleContext = new CarShopArticleContext(CarShopArticleContextCatalog);
                }
                return _dbArticleContext;
            }
        }

        protected void PrepareArticleContext(Guid searchEntGuid)
        {
            EnterpriseTDES enterpriseTDES = db.EnterpriseTDES.Where((EnterpriseTDES e) => e.EntGuid == searchEntGuid).FirstOrDefault();
            if (enterpriseTDES != null)
            {
                CarShopArticleContextCatalog = enterpriseTDES.ArticleCatalog;
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

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [HttpPost]
        public HttpResponseMessage PostSuppRestTDES(SuppRestTDESTmp suppresttdes)
        {
            if (!base.ModelState.IsValid)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            UserIsInRoles();
            ViewBagHelper(guestEntGuid, guestEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            if (string.IsNullOrEmpty(CarShopRestContextCatalog))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            Guid? searchEntGuid = null;
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
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                    }
                }
            }
            else
            {
                searchEntGuid = suppresttdes.EntGuid;
            }
            if (!searchEntGuid.HasValue)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            try
            {
                EnterpriseSupplierTDES enterpriseSupplierTDES = db.EnterpriseSupplierTDES.Where((EnterpriseSupplierTDES e) => e.EntSupplierId == suppresttdes.EntSupplierId && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid).FirstOrDefault();
                if (enterpriseSupplierTDES == null)
                {
                    base.ModelState.AddModelError("", Resources.EnterpriseSupplierTDES_NOTFOUND);
                    return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
                }
                SuppRestTDES suppresttdestoSave = dbRest.SuppRestTDES.Where((SuppRestTDES e) => e.EntSupplierId == suppresttdes.EntSupplierId && e.EntBranchArticle == suppresttdes.EntBranchArticle && e.EntBranchSup == suppresttdes.EntBranchSup).FirstOrDefault();
                if (suppresttdes.ExternArticleEAN != null)
                {
                    suppresttdes.ExternArticleEAN = suppresttdes.ExternArticleEAN.Replace(" ", "");
                }
                if (suppresttdestoSave == null)
                {
                    suppresttdestoSave = new SuppRestTDES();
                    suppresttdes.CopyTo(suppresttdestoSave, doCreateDescr: false);
                    BranchRestArticleDescriptionTDES branchRestArticleDescriptionTDES = dbRest.BranchRestArticleDescriptionTDES.Where((BranchRestArticleDescriptionTDES e) => e.EntArticleDescription == suppresttdes.EntArticleDescription).FirstOrDefault();
                    if (branchRestArticleDescriptionTDES == null)
                    {
                        BranchRestArticleDescriptionTDES branchRestArticleDescriptionTDES2 = new BranchRestArticleDescriptionTDES();
                        branchRestArticleDescriptionTDES2.EntArticleDescription = suppresttdes.EntArticleDescription;
                        branchRestArticleDescriptionTDES = branchRestArticleDescriptionTDES2;
                        dbRest.BranchRestArticleDescriptionTDES.Add(branchRestArticleDescriptionTDES);
                        dbRest.SaveChanges();
                    }
                    suppresttdestoSave.EntArticleDescriptionId = branchRestArticleDescriptionTDES.EntArticleDescriptionId;
                    dbRest.SuppRestTDES.Add(suppresttdestoSave);
                    dbRest.SaveChanges();
                }
                else
                {
                    _ = suppresttdestoSave.TSConcClmn;
                    suppresttdes.CopyTo(suppresttdestoSave, doCreateDescr: false);
                    suppresttdestoSave.ART_ARTICLE_NR = suppresttdes.ART_ARTICLE_NR;
                    suppresttdestoSave.SUP_TEXT = suppresttdes.SUP_TEXT;
                    suppresttdestoSave.ArtAmount = suppresttdes.ArtAmount;
                    suppresttdestoSave.ArtPrice = suppresttdes.ArtPrice;
                    suppresttdestoSave.LastUpdated = suppresttdes.LastUpdated;
                    suppresttdestoSave.LastReplicated = suppresttdes.LastReplicated;
                    suppresttdestoSave.ExternArticleEAN = suppresttdes.ExternArticleEAN;
                    BranchRestArticleDescriptionTDES branchRestArticleDescriptionTDES3 = dbRest.BranchRestArticleDescriptionTDES.Where((BranchRestArticleDescriptionTDES e) => e.EntArticleDescription == suppresttdes.EntArticleDescription).FirstOrDefault();
                    if (branchRestArticleDescriptionTDES3 == null)
                    {
                        BranchRestArticleDescriptionTDES branchRestArticleDescriptionTDES4 = new BranchRestArticleDescriptionTDES();
                        branchRestArticleDescriptionTDES4.EntArticleDescription = suppresttdes.EntArticleDescription;
                        branchRestArticleDescriptionTDES3 = branchRestArticleDescriptionTDES4;
                        dbRest.BranchRestArticleDescriptionTDES.Add(branchRestArticleDescriptionTDES3);
                        dbRest.SaveChanges();
                    }
                    suppresttdestoSave.EntArticleDescriptionId = branchRestArticleDescriptionTDES3.EntArticleDescriptionId;
                    suppresttdestoSave.BranchRestArticleDescriptionTDES = branchRestArticleDescriptionTDES3;
                    dbRest.Entry(suppresttdestoSave).State = EntityState.Modified;
                    dbRest.SaveChanges();
                }
                suppresttdes.CopyFrom(suppresttdestoSave);
                try
                {
                    PrepareArticleContext(searchEntGuid.Value);
                    if (CarShopArticleContextCatalog != null)
                    {
                        EnterpriseArticleTDES enterpriseArticleTDES = dbArticleContext.EnterpriseArticleTDES.Where((EnterpriseArticleTDES e) => (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == suppresttdestoSave.EntBranchSup && e.EntArticle == suppresttdestoSave.EntBranchArticle).Include("EnterpriseArticleDescriptionTDES").FirstOrDefault();
                        EnterpriseArticleDescriptionTDES enterpriseArticleDescriptionTDES = dbArticleContext.EnterpriseArticleDescriptionTDES.Where((EnterpriseArticleDescriptionTDES e) => e.EntArticleDescription == suppresttdes.EntArticleDescription).FirstOrDefault();
                        if (enterpriseArticleDescriptionTDES == null)
                        {
                            EnterpriseArticleDescriptionTDES enterpriseArticleDescriptionTDES2 = new EnterpriseArticleDescriptionTDES();
                            enterpriseArticleDescriptionTDES2.EntArticleDescription = suppresttdes.EntArticleDescription;
                            enterpriseArticleDescriptionTDES = enterpriseArticleDescriptionTDES2;
                            dbArticleContext.EnterpriseArticleDescriptionTDES.Add(enterpriseArticleDescriptionTDES);
                            dbArticleContext.SaveChanges();
                        }
                        if (enterpriseArticleTDES == null)
                        {
                            enterpriseArticleTDES = new EnterpriseArticleTDES();
                            enterpriseArticleTDES.EntArticleDescriptionId = enterpriseArticleDescriptionTDES.EntArticleDescriptionId;
                            enterpriseArticleTDES.EnterpriseArticleDescriptionTDES = enterpriseArticleDescriptionTDES;
                            enterpriseArticleTDES.EntGuid = searchEntGuid.Value;
                            enterpriseArticleTDES.EntBrandNic = suppresttdestoSave.EntBranchSup;
                            enterpriseArticleTDES.EntArticle = suppresttdestoSave.EntBranchArticle;
                            enterpriseArticleTDES.ExternArticle = suppresttdes.ART_ARTICLE_NR;
                            enterpriseArticleTDES.ExternBrandNic = suppresttdes.SUP_TEXT;
                            enterpriseArticleTDES.ExternArticleEAN = suppresttdes.ExternArticleEAN;
                            dbArticleContext.EnterpriseArticleTDES.Add(enterpriseArticleTDES);
                            dbArticleContext.SaveChanges();
                        }
                        else
                        {
                            enterpriseArticleTDES.EntArticleDescriptionId = enterpriseArticleDescriptionTDES.EntArticleDescriptionId;
                            enterpriseArticleTDES.EnterpriseArticleDescriptionTDES = enterpriseArticleDescriptionTDES;
                            enterpriseArticleTDES.ExternArticle = suppresttdes.ART_ARTICLE_NR;
                            enterpriseArticleTDES.ExternBrandNic = suppresttdes.SUP_TEXT;
                            enterpriseArticleTDES.ExternArticleEAN = suppresttdes.ExternArticleEAN;
                            dbArticleContext.Entry(enterpriseArticleTDES).State = EntityState.Modified;
                            dbArticleContext.SaveChanges();
                        }
                    }
                }
                catch
                {
                }
                return base.Request.CreateResponse(HttpStatusCode.Created, suppresttdes);
            }
            catch (Exception ex)
            {
                base.ModelState.AddModelError("", ex.Message);
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbRest != null)
            {
                _dbRest.Dispose();
                _dbRest = null;
            }
            if (_dbArticleContext != null)
            {
                _dbArticleContext.Dispose();
                _dbArticleContext = null;
            }
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
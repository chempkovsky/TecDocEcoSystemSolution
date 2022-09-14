// CarShop.Areas.ArticleSvc.Controllers.ArticleSvcController
using CarShop.Models;
using CarShop.Properties;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Areas.ArticleSvc.Controllers
{

    public class ArticleSvcController : ApiController
    {
        private CarShopContext db = new CarShopContext();

        private Guid EntGuid = default(Guid);

        private string CarShopArticleContextCatalog;

        private CarShopArticleContext _dbBrand = new CarShopArticleContext();

        private bool ViewBag_IsEcoSystemAdmin;

        private bool ViewBag_IsEnterpriseAdmin;

        private CarShopArticleContext dbBrand
        {
            get
            {
                if (_dbBrand == null)
                {
                    _dbBrand = new CarShopArticleContext(CarShopArticleContextCatalog);
                }
                return _dbBrand;
            }
        }

        private void UserIsInRoles()
        {
            ViewBag_IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            ViewBag_IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
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
                    return;
                }
                EntGuid = enterpriseBranchTDES.EnterpriseTDES.EntGuid;
                CarShopArticleContextCatalog = enterpriseBranchTDES.EnterpriseTDES.ArticleCatalog;
            }
            else
            {
                base.ModelState.AddModelError("", Resources.SendEnterpriseBranchTDES_NOTFOUND);
            }
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [HttpPost]
        public HttpResponseMessage PostArticle(Guid? searchEntBranchGuid = null, string EntArticle = null, string EntBrandNic = null, string EntArticleDescription = null, string ExternArticle = null, string ExternBrandNic = null, string ExternArticleEAN = null)
        {
            if (!base.ModelState.IsValid)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if (!ViewBag_IsEcoSystemAdmin)
            {
                string aName = base.User.Identity.Name;
                if (ViewBag_IsEnterpriseAdmin)
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
            if (!base.ModelState.IsValid || CarShopArticleContextCatalog == null)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            if (searchEntGuid.HasValue && EntGuid.CompareTo(searchEntGuid.Value) != 0)
            {
                base.ModelState.AddModelError("", "Указано не свое подразделение");
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            if (string.IsNullOrEmpty(EntArticle) || string.IsNullOrEmpty(EntBrandNic) || string.IsNullOrEmpty(EntArticleDescription) || string.IsNullOrEmpty(ExternArticle) || string.IsNullOrEmpty(ExternBrandNic))
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            if (ExternArticleEAN != null)
            {
                ExternArticleEAN = ExternArticleEAN.Replace(" ", "").Replace("-", "").Replace(".", "");
            }
            try
            {
                EnterpriseBrandTDES enterpriseBrandTDES = dbBrand.EnterpriseBrandTDES.Where((EnterpriseBrandTDES e) => e.EntGuid == EntGuid && e.EntBrandNic == EntBrandNic).FirstOrDefault();
                if (enterpriseBrandTDES == null)
                {
                    enterpriseBrandTDES = new EnterpriseBrandTDES();
                    enterpriseBrandTDES.EntBrandNic = EntBrandNic;
                    enterpriseBrandTDES.EntBrandDescription = EntBrandNic;
                    enterpriseBrandTDES.EntGuid = EntGuid;
                    dbBrand.EnterpriseBrandTDES.Add(enterpriseBrandTDES);
                    dbBrand.SaveChanges();
                }
                EnterpriseArticleTDES enterpriseArticleTDES = dbBrand.EnterpriseArticleTDES.Where((EnterpriseArticleTDES e) => e.EntGuid == EntGuid && e.EntBrandNic == EntBrandNic && e.EntArticle == EntArticle).Include("EnterpriseArticleDescriptionTDES").FirstOrDefault();
                if (enterpriseArticleTDES == null)
                {
                    EnterpriseArticleDescriptionTDES enterpriseArticleDescriptionTDES = dbBrand.EnterpriseArticleDescriptionTDES.Where((EnterpriseArticleDescriptionTDES e) => e.EntArticleDescription == EntArticleDescription).FirstOrDefault();
                    if (enterpriseArticleDescriptionTDES == null)
                    {
                        enterpriseArticleDescriptionTDES = new EnterpriseArticleDescriptionTDES();
                        enterpriseArticleDescriptionTDES.EntArticleDescription = EntArticleDescription;
                        dbBrand.EnterpriseArticleDescriptionTDES.Add(enterpriseArticleDescriptionTDES);
                        dbBrand.SaveChanges();
                    }
                    enterpriseArticleTDES = new EnterpriseArticleTDES();
                    enterpriseArticleTDES.EntArticleDescriptionId = enterpriseArticleDescriptionTDES.EntArticleDescriptionId;
                    enterpriseArticleTDES.EntArticle = EntArticle;
                    enterpriseArticleTDES.EntBrandNic = EntBrandNic;
                    enterpriseArticleTDES.EntGuid = EntGuid;
                    enterpriseArticleTDES.ExternArticle = ExternArticle;
                    enterpriseArticleTDES.ExternBrandNic = ExternBrandNic;
                    enterpriseArticleTDES.ExternArticleEAN = ExternArticleEAN;
                    dbBrand.EnterpriseArticleTDES.Add(enterpriseArticleTDES);
                    dbBrand.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                base.ModelState.AddModelError("", ex.Message);
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            return base.Request.CreateResponse(HttpStatusCode.OK);
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbBrand != null)
            {
                _dbBrand.Dispose();
                _dbBrand = null;
            }
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
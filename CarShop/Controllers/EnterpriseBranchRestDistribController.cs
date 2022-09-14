// CarShop.Controllers.EnterpriseBranchRestDistribController
using CarShop.Models;
using CarShop.Properties;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterpriseBranchRestDistribController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopIncomeContextCatalog;

        private CarShopIncomeContext _dbIncome;

        private string CarShopSalesContextCatalog;

        private CarShopSalesContext _dbSales;

        private CarShopIncomeContext dbIncome
        {
            get
            {
                if (_dbIncome == null)
                {
                    _dbIncome = new CarShopIncomeContext(CarShopIncomeContextCatalog);
                }
                return _dbIncome;
            }
        }

        private CarShopSalesContext dbSales
        {
            get
            {
                if (_dbSales == null)
                {
                    _dbSales = new CarShopSalesContext(CarShopSalesContextCatalog);
                }
                return _dbSales;
            }
        }

        public async Task ViewBagHelper(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            EnterpriseBranchTDES enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefaultAsync();
            if (enterprisebranchtdes != null)
            {
                base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
                base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
                base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
                base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
                CarShopIncomeContextCatalog = enterprisebranchtdes.IncomeCatalog;
                CarShopSalesContextCatalog = enterprisebranchtdes.SalesCatalog;
                return;
            }
            base.ViewBag.EntBranchDescription = Resources.EnterpriseBranchTDES_NOTFOUND;
            base.ViewBag.SearchEntBranchGuid = null;
            base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            EnterpriseTDES enterprises = await (from e in db.EnterpriseTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
            if (enterprises != null)
            {
                base.ViewBag.EntDescription = enterprises.EntDescription;
                base.ViewBag.SearchEntGuid = enterprises.EntGuid;
            }
            else
            {
                base.ViewBag.EntDescription = Resources.ENTERPRISE_NOT_DEFINED;
                base.ViewBag.SearchEntGuid = null;
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
        }

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
            base.ViewBag.IsBranchAdmin = base.User.IsInRole("BranchAdmin");
            base.ViewBag.IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        public async Task<ActionResult> Index(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchEntArticle, string searchEntBrandNic, int? page)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid? reference = searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            IPagedList<IncomeArticleTDES> aResult = null;
            if (searchEntBranchGuid.HasValue)
            {
                page = (page ?? 1);
                IOrderedQueryable<IncomeArticleTDES> q = from e in dbIncome.IncomeArticleTDES.Include("IncomePayRollTDES")
                                                         where e.EntArticle == searchEntArticle && e.EntBrandNic == searchEntBrandNic && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                         orderby e.IncomePayRollTDES.CreatedAt descending
                                                         select e;
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await q.ToPagedListAsync(pageNumber, pageSize);
            }
            base.ViewBag.SearchEntArticle = searchEntArticle;
            base.ViewBag.SearchEntBrandNic = searchEntBrandNic;
            return View(aResult);
        }

        public async Task<ActionResult> Sales(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntArticle = null, string searchEntBrandNic = null, int? page = default(int?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            IPagedList<SaleBasketArticleTDES> aResult = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                IOrderedQueryable<SaleBasketArticleTDES> salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES.Include("SaleArticleDescriptionTDES")
                                                                                 where e.EntArticle == searchEntArticle && e.EntBrandNic == searchEntBrandNic && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                 orderby e.PaidAt descending
                                                                                 select e;
                int pageSize = 20;
                int pageNumber = page ?? 1;
                aResult = await salebasketarticletdes.ToPagedListAsync(pageNumber, pageSize);
            }
            base.ViewBag.SearchEntArticle = searchEntArticle;
            base.ViewBag.SearchEntBrandNic = searchEntBrandNic;
            return View(aResult);
        }

        public async Task<ActionResult> Revaluations(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntArticle = null, string searchEntBrandNic = null, int? page = default(int?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            IPagedList<RevaluationArticleTDES> aResult = null;
            if (CarShopIncomeContextCatalog != null)
            {
                page = (page ?? 1);
                IOrderedQueryable<RevaluationArticleTDES> q = from e in dbIncome.RevaluationArticleTDES.Include("SheetRevaluationTDES")
                                                              where e.EntArticle == searchEntArticle && e.EntBrandNic == searchEntBrandNic && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                              orderby e.SheetRevaluationTDES.CreatedAt descending
                                                              select e;
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await q.ToPagedListAsync(pageNumber, pageSize);
            }
            base.ViewBag.SearchEntArticle = searchEntArticle;
            base.ViewBag.SearchEntBrandNic = searchEntBrandNic;
            return View(aResult);
        }

        public async Task<ActionResult> Returns(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntArticle = null, string searchEntBrandNic = null, int? page = default(int?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            IPagedList<ReturnBasketArticleTDES> aResult = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                IOrderedQueryable<ReturnBasketArticleTDES> salebasketarticletdes = from e in dbSales.ReturnBasketArticleTDES.Include("SaleArticleDescriptionTDES")
                                                                                   where e.EntArticle == searchEntArticle && e.EntBrandNic == searchEntBrandNic && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                   orderby e.PaidAt descending
                                                                                   select e;
                int pageSize = 20;
                int pageNumber = page ?? 1;
                aResult = await salebasketarticletdes.ToPagedListAsync(pageNumber, pageSize);
            }
            base.ViewBag.SearchEntArticle = searchEntArticle;
            base.ViewBag.SearchEntBrandNic = searchEntBrandNic;
            return View(aResult);
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbIncome != null)
            {
                _dbIncome.Dispose();
                _dbIncome = null;
            }
            if (_dbSales != null)
            {
                _dbSales.Dispose();
                _dbSales = null;
            }
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
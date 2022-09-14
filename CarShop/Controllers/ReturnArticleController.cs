// CarShop.Controllers.ReturnArticleController
using CarShop.Models;
using CarShop.Properties;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class ReturnArticleController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopSalesContextCatalog;

        private CarShopSalesContext _dbSales;

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
            base.ViewBag.IsBranchSeller = base.User.IsInRole("BranchSeller");
            base.ViewBag.IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        protected SelectList SelectListHelper(int showIs)
        {
            return new SelectList(new SelectListItem[3]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.ArticleSearchByEntArticle,
                Selected = (showIs == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.ArticleSearchByEntArticleDescription,
                Selected = (showIs == 2)
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.ArticleSearchByBrandNic,
                Selected = (showIs == 3)
            }
            }, "Value", "Text", showIs);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Autocomplete(Guid? searchSpellGuid, Guid? searchEntGuid, Guid? searchEntBranchGuid, int? currentFilterBy, string term)
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
            currentFilterBy = (currentFilterBy ?? 1);
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog == null)
            {
                List<string> source = new List<string>();
                var data = from e in source
                           select new
                           {
                               value = e
                           };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrEmpty(term))
            {
                term = term.Trim();
            }
            IQueryable<ReturnBasketArticleTDES> enterprisearticletdes = from e in dbSales.ReturnBasketArticleTDES
                                                                        where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid
                                                                        select e;
            if (!string.IsNullOrEmpty(term))
            {
                int? num = currentFilterBy;
                int valueOrDefault = num.GetValueOrDefault();
                if (!num.HasValue)
                {
                    goto IL_09d6;
                }
                switch (valueOrDefault)
                {
                    case 1:
                        break;
                    case 2:
                        goto IL_092f;
                    default:
                        goto IL_09d6;
                }
                enterprisearticletdes = from e in enterprisearticletdes
                                        where e.EntArticle.StartsWith(term)
                                        select e;
            }
            goto IL_0a64;
        IL_0a64:
            int? num2 = currentFilterBy;
            int valueOrDefault2 = num2.GetValueOrDefault();
            if (num2.HasValue)
            {
                switch (valueOrDefault2)
                {
                    case 1:
                        {
                            var ret2 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.EntArticle
                                        }).Take(10);
                            return Json(await ret2.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 2:
                        {
                            var ret3 = (from e in enterprisearticletdes.Include("SaleArticleDescriptionTDES")
                                        select new
                                        {
                                            value = e.SaleArticleDescriptionTDES.EntArticleDescription
                                        }).Take(10);
                            return Json(await ret3.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                }
            }
            var ret = (from e in enterprisearticletdes
                       select new
                       {
                           value = e.EntBrandNic
                       }).Take(10);
            return Json(await ret.ToListAsync(), JsonRequestBehavior.AllowGet);
        IL_09d6:
            enterprisearticletdes = from e in enterprisearticletdes
                                    where e.EntBrandNic.StartsWith(term)
                                    select e;
            goto IL_0a64;
        IL_092f:
            enterprisearticletdes = from e in enterprisearticletdes
                                    where e.SaleArticleDescriptionTDES.EntArticleDescription.StartsWith(term)
                                    select e;
            goto IL_0a64;
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchSpellGuid, Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page)
        {
            searchStringBy = (searchStringBy ?? 1);
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
            IQueryable<ReturnBasketArticleTDES> salebasketarticletdes;
            if (CarShopSalesContextCatalog != null)
            {
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    page = (page ?? 1);
                    searchString = currentFilter;
                }
                salebasketarticletdes = (from e in dbSales.ReturnBasketArticleTDES
                                         where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid
                                         orderby e.EntArticle, e.EntBrandNic
                                         select e).Include("SaleArticleDescriptionTDES");
                if (!string.IsNullOrEmpty(searchString))
                {
                    int? num = searchStringBy;
                    int valueOrDefault = num.GetValueOrDefault();
                    if (!num.HasValue)
                    {
                        goto IL_0cc8;
                    }
                    switch (valueOrDefault)
                    {
                        case 1:
                            break;
                        case 2:
                            goto IL_0aec;
                        default:
                            goto IL_0cc8;
                    }
                    salebasketarticletdes = from e in dbSales.ReturnBasketArticleTDES
                                            where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.EntArticle.StartsWith(searchString)
                                            orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                            select e;
                    goto IL_0e8b;
                }
                goto IL_0ea1;
            }
            base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            searchStringBy = (searchStringBy ?? 1);
            goto IL_0f84;
        IL_0f84:
            base.ViewBag.searchSpellGuid = searchSpellGuid;
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.currentFilterBy = searchStringBy;
            return View(aResult);
        IL_0e8b:
            salebasketarticletdes = salebasketarticletdes.Include("SaleArticleDescriptionTDES");
            goto IL_0ea1;
        IL_0cc8:
            salebasketarticletdes = from e in dbSales.ReturnBasketArticleTDES
                                    where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.EntBrandNic.StartsWith(searchString)
                                    orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                    select e;
            goto IL_0e8b;
        IL_0aec:
            salebasketarticletdes = from e in dbSales.ReturnBasketArticleTDES
                                    where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.SaleArticleDescriptionTDES.EntArticleDescription.StartsWith(searchString)
                                    orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                    select e;
            goto IL_0e8b;
        IL_0ea1:
            int pageSize = 20;
            int pageNumber = page.Value;
            aResult = await salebasketarticletdes.ToPagedListAsync(pageNumber, pageSize);
            goto IL_0f84;
        }

        protected override void Dispose(bool disposing)
        {
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
// CarShop.Controllers.SaleArticleController
using CarShop.Models;
using CarShop.Properties;
using Newtonsoft.Json;
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

    public class SaleArticleController : Controller
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
            return new SelectList(new SelectListItem[6]
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
                Text = Resources.ArticleSearchByExternArticle,
                Selected = (showIs == 3)
            },
            new SelectListItem
            {
                Value = "4",
                Text = Resources.ArticleSearchByExternBrandNic,
                Selected = (showIs == 4)
            },
            new SelectListItem
            {
                Value = "5",
                Text = Resources.ArticleSearchByExternArticleEAN,
                Selected = (showIs == 5)
            },
            new SelectListItem
            {
                Value = "6",
                Text = Resources.ArticleSearchByBrandNic,
                Selected = (showIs == 6)
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
            IQueryable<SaleBasketArticleTDES> enterprisearticletdes = from e in dbSales.SaleBasketArticleTDES.Include("SaleArticleDescriptionTDES")
                                                                      where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid
                                                                      select e;
            if (!string.IsNullOrEmpty(term))
            {
                int? num = currentFilterBy;
                int valueOrDefault = num.GetValueOrDefault();
                if (num.HasValue)
                {
                    switch (valueOrDefault)
                    {
                        case 1:
                            enterprisearticletdes = from e in enterprisearticletdes
                                                    where e.EntArticle.StartsWith(term)
                                                    select e;
                            break;
                        case 2:
                            enterprisearticletdes = from e in enterprisearticletdes
                                                    where e.SaleArticleDescriptionTDES.EntArticleDescription.StartsWith(term)
                                                    select e;
                            break;
                        case 3:
                            enterprisearticletdes = from e in enterprisearticletdes
                                                    where e.ExternArticle.StartsWith(term)
                                                    select e;
                            break;
                        case 4:
                            enterprisearticletdes = from e in enterprisearticletdes
                                                    where e.ExternBrandNic.StartsWith(term)
                                                    select e;
                            break;
                        case 5:
                            enterprisearticletdes = from e in enterprisearticletdes
                                                    where e.ExternArticleEAN.StartsWith(term)
                                                    select e;
                            break;
                        case 6:
                            enterprisearticletdes = from e in enterprisearticletdes
                                                    where e.EntBrandNic.StartsWith(term)
                                                    select e;
                            break;
                    }
                }
            }
            int? num2 = currentFilterBy;
            int valueOrDefault2 = num2.GetValueOrDefault();
            if (num2.HasValue)
            {
                switch (valueOrDefault2)
                {
                    case 1:
                        {
                            var ret3 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.EntArticle,
                                            va0 = e.EntArticle,
                                            va1 = e.EntBrandNic,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.SaleArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ArtAmount,
                                            va5 = e.SalePrice
                                        }).Take(10);
                            return Json(await ret3.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 2:
                        {
                            var ret6 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.SaleArticleDescriptionTDES.EntArticleDescription,
                                            va0 = e.EntArticle,
                                            va1 = e.EntBrandNic,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.SaleArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ArtAmount,
                                            va5 = e.SalePrice
                                        }).Take(10);
                            return Json(await ret6.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 3:
                        {
                            var ret5 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.ExternArticle,
                                            va0 = e.EntArticle,
                                            va1 = e.EntBrandNic,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.SaleArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ArtAmount,
                                            va5 = e.SalePrice
                                        }).Take(10);
                            return Json(await ret5.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 4:
                        {
                            var ret4 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.ExternBrandNic,
                                            va0 = e.EntArticle,
                                            va1 = e.EntBrandNic,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.SaleArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ArtAmount,
                                            va5 = e.SalePrice
                                        }).Take(10);
                            return Json(await ret4.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 6:
                        {
                            var ret2 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.EntBrandNic,
                                            va0 = e.EntArticle,
                                            va1 = e.EntBrandNic,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.SaleArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ArtAmount,
                                            va5 = e.SalePrice
                                        }).Take(10);
                            return Json(await ret2.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                }
            }
            var ret = (from e in enterprisearticletdes
                       select new
                       {
                           value = e.ExternArticleEAN,
                           va0 = e.EntArticle,
                           va1 = e.EntBrandNic,
                           va2 = e.ExternArticleEAN,
                           va3 = e.SaleArticleDescriptionTDES.EntArticleDescription,
                           va4 = e.ArtAmount,
                           va5 = e.SalePrice
                       }).Take(10);
            return Json(await ret.ToListAsync(), JsonRequestBehavior.AllowGet);
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
            IPagedList<SaleBasketArticleTDES> aResult = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            IQueryable<SaleBasketArticleTDES> salebasketarticletdes;
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
                salebasketarticletdes = (from e in dbSales.SaleBasketArticleTDES
                                         where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid
                                         orderby e.EntArticle, e.EntBrandNic
                                         select e).Include("SaleArticleDescriptionTDES");
                if (!string.IsNullOrEmpty(searchString))
                {
                    int? num = searchStringBy;
                    int valueOrDefault = num.GetValueOrDefault();
                    if (!num.HasValue)
                    {
                        goto IL_1230;
                    }
                    switch (valueOrDefault)
                    {
                        case 1:
                            break;
                        case 2:
                            goto IL_0afc;
                        case 3:
                            goto IL_0cd8;
                        case 4:
                            goto IL_0ea0;
                        case 6:
                            goto IL_1068;
                        default:
                            goto IL_1230;
                    }
                    salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES
                                            where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.EntArticle.StartsWith(searchString)
                                            orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                            select e;
                    goto IL_13f3;
                }
                goto IL_1409;
            }
            base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            searchStringBy = (searchStringBy ?? 1);
            goto IL_14ec;
        IL_1409:
            int pageSize = 20;
            int pageNumber = page.Value;
            aResult = await salebasketarticletdes.ToPagedListAsync(pageNumber, pageSize);
            goto IL_14ec;
        IL_1068:
            salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES
                                    where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.EntBrandNic.StartsWith(searchString)
                                    orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                    select e;
            goto IL_13f3;
        IL_0ea0:
            salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES
                                    where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.ExternBrandNic.StartsWith(searchString)
                                    orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                    select e;
            goto IL_13f3;
        IL_0afc:
            salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES
                                    where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.SaleArticleDescriptionTDES.EntArticleDescription.StartsWith(searchString)
                                    orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                    select e;
            goto IL_13f3;
        IL_1230:
            salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES
                                    where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.ExternArticleEAN.StartsWith(searchString)
                                    orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                    select e;
            goto IL_13f3;
        IL_0cd8:
            salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES
                                    where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.ExternArticle.StartsWith(searchString)
                                    orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                    select e;
            goto IL_13f3;
        IL_13f3:
            salebasketarticletdes = salebasketarticletdes.Include("SaleArticleDescriptionTDES");
            goto IL_1409;
        IL_14ec:
            base.ViewBag.searchSpellGuid = searchSpellGuid;
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.currentFilterBy = searchStringBy;
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> LookForSpellArticle(string redirecData, string redirectContriller, string redirectAction, Guid? searchSpellGuid, Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page)
        {
            base.ViewBag.RedirecData = redirecData;
            base.ViewBag.RedirectContriller = redirectContriller;
            base.ViewBag.RedirectAction = redirectAction;
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
            IPagedList<SaleBasketArticleTDES> aResult = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            IQueryable<SaleBasketArticleTDES> salebasketarticletdes;
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
                salebasketarticletdes = (from e in dbSales.SaleBasketArticleTDES
                                         where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid
                                         orderby e.EntArticle, e.EntBrandNic
                                         select e).Include("SaleArticleDescriptionTDES");
                if (!string.IsNullOrEmpty(searchString))
                {
                    int? num = searchStringBy;
                    int valueOrDefault = num.GetValueOrDefault();
                    if (!num.HasValue)
                    {
                        goto IL_1374;
                    }
                    switch (valueOrDefault)
                    {
                        case 1:
                            break;
                        case 2:
                            goto IL_0c40;
                        case 3:
                            goto IL_0e1c;
                        case 4:
                            goto IL_0fe4;
                        case 6:
                            goto IL_11ac;
                        default:
                            goto IL_1374;
                    }
                    salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES
                                            where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.EntArticle.StartsWith(searchString)
                                            orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                            select e;
                    goto IL_1537;
                }
                goto IL_154d;
            }
            base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            searchStringBy = (searchStringBy ?? 1);
            goto IL_1630;
        IL_0e1c:
            salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES
                                    where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.ExternArticle.StartsWith(searchString)
                                    orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                    select e;
            goto IL_1537;
        IL_1537:
            salebasketarticletdes = salebasketarticletdes.Include("SaleArticleDescriptionTDES");
            goto IL_154d;
        IL_1630:
            base.ViewBag.searchSpellGuid = searchSpellGuid;
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.currentFilterBy = searchStringBy;
            return View(aResult);
        IL_154d:
            int pageSize = 20;
            int pageNumber = page.Value;
            aResult = await salebasketarticletdes.ToPagedListAsync(pageNumber, pageSize);
            goto IL_1630;
        IL_11ac:
            salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES
                                    where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.EntBrandNic.StartsWith(searchString)
                                    orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                    select e;
            goto IL_1537;
        IL_0fe4:
            salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES
                                    where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.ExternBrandNic.StartsWith(searchString)
                                    orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                    select e;
            goto IL_1537;
        IL_0c40:
            salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES
                                    where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.SaleArticleDescriptionTDES.EntArticleDescription.StartsWith(searchString)
                                    orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                    select e;
            goto IL_1537;
        IL_1374:
            salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES
                                    where (Guid)(Guid?)e.SpellGuid == (Guid)searchSpellGuid && e.ExternArticleEAN.StartsWith(searchString)
                                    orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                    select e;
            goto IL_1537;
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult LookForSpellArticleSelected(string redirecData, string redirectContriller, string redirectAction, string DOCANCEL_TITLE, string DOSELECT_TITLE, string ArticleTmp)
        {
            if (string.IsNullOrEmpty(redirectAction))
            {
                redirectAction = "GetReturnArticuleForCreate";
            }
            if (string.IsNullOrEmpty(redirectContriller))
            {
                redirectContriller = "ReturnBasketArticle";
            }
            if (DOSELECT_TITLE != null)
            {
                string entArticle = null;
                string entBrandNic = null;
                Guid? entBasketGuid = null;
                SaleBasketArticleTmp saleBasketArticleTmp = JsonConvert.DeserializeObject<SaleBasketArticleTmp>(ArticleTmp);
                if (saleBasketArticleTmp != null)
                {
                    entArticle = saleBasketArticleTmp.EntArticle;
                    entBrandNic = saleBasketArticleTmp.EntBrandNic;
                    entBasketGuid = saleBasketArticleTmp.EntBasketGuid;
                }
                return RedirectToAction(redirectAction, redirectContriller, new
                {
                    redirecData = redirecData,
                    EntArticle = entArticle,
                    EntBrandNic = entBrandNic,
                    EntBasketGuid = entBasketGuid
                });
            }
            return RedirectToAction(redirectAction, redirectContriller, new
            {
                redirecData
            });
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
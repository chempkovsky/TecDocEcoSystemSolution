// CarShop.Controllers.EnterpriseSuppArticleAndRestController
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

    public class EnterpriseSuppArticleAndRestController : Controller
    {
        private Guid? guestEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private Guid? guestEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private CarShopContext db = new CarShopContext();

        private string CarShopRestContextCatalog;

        private CarShopRestContext _dbRest;

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
                CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
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
                base.ViewBag.SearchEntGuid = searchEntGuid;
            }
            else
            {
                base.ViewBag.EntDescription = Resources.ENTERPRISE_NOT_DEFINED;
                base.ViewBag.SearchEntGuid = null;
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
        }

        public async Task PreparedbRest()
        {
            EnterpriseBranchTDES enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)guestEntBranchGuid
                                                               select e).FirstOrDefaultAsync();
            if (enterprisebranchtdes != null)
            {
                CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
                return;
            }
            CarShopRestContextCatalog = null;
            base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
        }

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
            base.ViewBag.IsBranchAdmin = base.User.IsInRole("BranchAdmin");
            base.ViewBag.IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Autocomplete(Guid? searchEntGuid, Guid? searchEntBranchGuid, int? currentFilterBy, string term)
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
            await PreparedbRest();
            if (CarShopRestContextCatalog == null)
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
            IQueryable<SuppRestTDES> enterprisearticletdes = from e in dbRest.SuppRestTDES.Include("BranchRestArticleDescriptionTDES")
                                                             join b in dbRest.BranchSuppTDES on e.EntSupplierId equals b.EntSupplierId
                                                             where (Guid)(Guid?)b.EntBranchGuid == (Guid)searchEntBranchGuid
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
                                                    where e.EntBranchArticle.StartsWith(term)
                                                    select e;
                            break;
                        case 2:
                            enterprisearticletdes = from e in enterprisearticletdes
                                                    where e.BranchRestArticleDescriptionTDES.EntArticleDescription.StartsWith(term)
                                                    select e;
                            break;
                        case 3:
                            enterprisearticletdes = from e in enterprisearticletdes
                                                    where e.ART_ARTICLE_NR.StartsWith(term)
                                                    select e;
                            break;
                        case 4:
                            enterprisearticletdes = from e in enterprisearticletdes
                                                    where e.SUP_TEXT.StartsWith(term)
                                                    select e;
                            break;
                        case 5:
                            enterprisearticletdes = from e in enterprisearticletdes
                                                    where e.ExternArticleEAN.StartsWith(term)
                                                    select e;
                            break;
                        case 6:
                            enterprisearticletdes = from e in enterprisearticletdes
                                                    where e.EntBranchSup.StartsWith(term)
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
                            var ret5 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.EntBranchArticle,
                                            va0 = e.EntBranchArticle,
                                            va1 = e.EntBranchSup,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ArtAmount,
                                            va5 = e.ArtPrice
                                        }).Take(10);
                            return Json(await ret5.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 2:
                        {
                            var ret6 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                                            va0 = e.EntBranchArticle,
                                            va1 = e.EntBranchSup,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ArtAmount,
                                            va5 = e.ArtPrice
                                        }).Take(10);
                            return Json(await ret6.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 3:
                        {
                            var ret4 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.ART_ARTICLE_NR,
                                            va0 = e.EntBranchArticle,
                                            va1 = e.EntBranchSup,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ArtAmount,
                                            va5 = e.ArtPrice
                                        }).Take(10);
                            return Json(await ret4.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 4:
                        {
                            var ret3 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.SUP_TEXT,
                                            va0 = e.EntBranchArticle,
                                            va1 = e.EntBranchSup,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ArtAmount,
                                            va5 = e.ArtPrice
                                        }).Take(10);
                            return Json(await ret3.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 6:
                        {
                            var ret2 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.EntBranchSup,
                                            va0 = e.EntBranchArticle,
                                            va1 = e.EntBranchSup,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ArtAmount,
                                            va5 = e.ArtPrice
                                        }).Take(10);
                            return Json(await ret2.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                }
            }
            var ret = (from e in enterprisearticletdes
                       select new
                       {
                           value = e.ExternArticleEAN,
                           va0 = e.EntBranchArticle,
                           va1 = e.EntBranchSup,
                           va2 = e.ExternArticleEAN,
                           va3 = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                           va4 = e.ArtAmount,
                           va5 = e.ArtPrice
                       }).Take(10);
            return Json(await ret.ToListAsync(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page)
        {
            IPagedList<SuppRestTDES> aResult2 = null;
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
            await PreparedbRest();
            if (CarShopRestContextCatalog == null)
            {
                return View(aResult2);
            }
            if (searchString != null)
            {
                page = 1;
                searchStringBy = (searchStringBy ?? 1);
            }
            else
            {
                page = (page ?? 1);
                searchString = currentFilter;
                searchStringBy = (currentFilterBy ?? 1);
            }
            IQueryable<SuppRestTDES> enterprisearticletdes = from e in dbRest.SuppRestTDES.Include("BranchRestArticleDescriptionTDES")
                                                             join b in dbRest.BranchSuppTDES on e.EntSupplierId equals b.EntSupplierId
                                                             where (Guid)(Guid?)b.EntBranchGuid == (Guid)searchEntBranchGuid
                                                             orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                                                             select e;
            if (!string.IsNullOrEmpty(searchString))
            {
                int? num = searchStringBy;
                int valueOrDefault = num.GetValueOrDefault();
                if (!num.HasValue)
                {
                    goto IL_23f5;
                }
                switch (valueOrDefault)
                {
                    case 1:
                        break;
                    case 2:
                        goto IL_1022;
                    case 3:
                        goto IL_1429;
                    case 4:
                        goto IL_181c;
                    case 5:
                        goto IL_1c0f;
                    case 6:
                        goto IL_2002;
                    default:
                        goto IL_23f5;
                }
                enterprisearticletdes = from e in dbRest.SuppRestTDES.Include("BranchRestArticleDescriptionTDES")
                                        where e.EntBranchArticle.StartsWith(searchString)
                                        join b in dbRest.BranchSuppTDES on e.EntSupplierId equals b.EntSupplierId
                                        where (Guid)(Guid?)b.EntBranchGuid == (Guid)searchEntBranchGuid
                                        orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                                        select e;
            }
            goto IL_27e3;
        IL_2002:
            enterprisearticletdes = from e in dbRest.SuppRestTDES.Include("BranchRestArticleDescriptionTDES")
                                    where e.EntBranchSup.StartsWith(searchString)
                                    join b in dbRest.BranchSuppTDES on e.EntSupplierId equals b.EntSupplierId
                                    where (Guid)(Guid?)b.EntBranchGuid == (Guid)searchEntBranchGuid
                                    orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                                    select e;
            goto IL_27e3;
        IL_1c0f:
            enterprisearticletdes = from e in dbRest.SuppRestTDES.Include("BranchRestArticleDescriptionTDES")
                                    where e.ExternArticleEAN.StartsWith(searchString)
                                    join b in dbRest.BranchSuppTDES on e.EntSupplierId equals b.EntSupplierId
                                    where (Guid)(Guid?)b.EntBranchGuid == (Guid)searchEntBranchGuid
                                    orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                                    select e;
            goto IL_27e3;
        IL_1429:
            enterprisearticletdes = from e in dbRest.SuppRestTDES.Include("BranchRestArticleDescriptionTDES")
                                    where e.ART_ARTICLE_NR.StartsWith(searchString)
                                    join b in dbRest.BranchSuppTDES on e.EntSupplierId equals b.EntSupplierId
                                    where (Guid)(Guid?)b.EntBranchGuid == (Guid)searchEntBranchGuid
                                    orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                                    select e;
            goto IL_27e3;
        IL_1022:
            enterprisearticletdes = from e in dbRest.SuppRestTDES.Include("BranchRestArticleDescriptionTDES")
                                    where e.BranchRestArticleDescriptionTDES.EntArticleDescription.StartsWith(searchString)
                                    join b in dbRest.BranchSuppTDES on e.EntSupplierId equals b.EntSupplierId
                                    where (Guid)(Guid?)b.EntBranchGuid == (Guid)searchEntBranchGuid
                                    orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                                    select e;
            goto IL_27e3;
        IL_23f5:
            enterprisearticletdes = from e in dbRest.SuppRestTDES.Include("BranchRestArticleDescriptionTDES")
                                    where e.ExternArticleEAN.StartsWith(searchString)
                                    join b in dbRest.BranchSuppTDES on e.EntSupplierId equals b.EntSupplierId
                                    where (Guid)(Guid?)b.EntBranchGuid == (Guid)searchEntBranchGuid
                                    orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                                    select e;
            goto IL_27e3;
        IL_27e3:
            int pageSize = 20;
            int pageNumber = page.Value;
            aResult2 = await enterprisearticletdes.ToPagedListAsync(pageNumber, pageSize);
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.currentFilterBy = searchStringBy;
            return View(aResult2);
        IL_181c:
            enterprisearticletdes = from e in dbRest.SuppRestTDES.Include("BranchRestArticleDescriptionTDES")
                                    where e.SUP_TEXT.StartsWith(searchString)
                                    join b in dbRest.BranchSuppTDES on e.EntSupplierId equals b.EntSupplierId
                                    where (Guid)(Guid?)b.EntBranchGuid == (Guid)searchEntBranchGuid
                                    orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                                    select e;
            goto IL_27e3;
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbRest != null)
            {
                _dbRest.Dispose();
                _dbRest = null;
            }
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
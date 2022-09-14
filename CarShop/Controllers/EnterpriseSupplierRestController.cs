// CarShop.Controllers.EnterpriseSupplierRestController
using CarShop.Models;
using CarShop.Properties;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterpriseSupplierRestController : Controller
    {
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

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
        }

        public async Task ViewBagHelper(Guid? searchEntGuid, string searchEntSupplierId)
        {
            EnterpriseSupplierTDES enterprisesuppliercontacttdes = await (from e in db.EnterpriseSupplierTDES
                                                                          where e.EntSupplierId == searchEntSupplierId && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                          select e).Include((EnterpriseSupplierTDES x) => x.EnterpriseTDES).FirstOrDefaultAsync();
            if (enterprisesuppliercontacttdes != null)
            {
                base.ViewBag.EntDescription = enterprisesuppliercontacttdes.EnterpriseTDES.EntDescription;
                base.ViewBag.EntSupplierDescription = enterprisesuppliercontacttdes.EntSupplierDescription;
                base.ViewBag.SearchEntGuid = searchEntGuid;
                base.ViewBag.SearchEntSupplierId = searchEntSupplierId;
                EnterpriseBranchTDES enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                                   where (Guid)(Guid?)e.EntBranchGuid == (Guid)guestEntBranchGuid
                                                                   select e).FirstOrDefaultAsync();
                if (enterprisebranchtdes != null)
                {
                    CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
                }
                return;
            }
            base.ViewBag.EntSupplierDescription = Resources.EnterpriseSupplierTDES_NOTFOUND;
            base.ViewBag.SearchEntSupplierId = null;
            CarShopRestContextCatalog = null;
            base.ModelState.AddModelError("", Resources.EnterpriseSupplierTDES_NOTFOUND);
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
        public async Task<ActionResult> Autocomplete(Guid? searchEntGuid, string searchEntSupplierId, int? currentFilterBy, string term)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                searchEntGuid = ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? new Guid?(await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                           where e.EntUserNic == aName
                                                                                                                                           select e.EntGuid).FirstOrDefaultAsync()) : new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                                                                                                       where e.EntUserNic == aName
                                                                                                                                                                                                       select e.EntGuid).FirstOrDefaultAsync()));
            }
            await ViewBagHelper(searchEntGuid, searchEntSupplierId);
            if (!base.ModelState.IsValid)
            {
                List<string> source = new List<string>();
                var data = from e in source
                           select new
                           {
                               value = e
                           };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            currentFilterBy = (currentFilterBy ?? 1);
            if (!string.IsNullOrEmpty(term))
            {
                term = term.Trim();
            }
            IQueryable<SuppRestTDES> suppresttdes = (from e in dbRest.SuppRestTDES
                                                     where e.EntSupplierId == searchEntSupplierId
                                                     orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                                                     select e).Include("BranchRestArticleDescriptionTDES");
            if (!string.IsNullOrEmpty(term))
            {
                int? num = currentFilterBy;
                int valueOrDefault = num.GetValueOrDefault();
                if (!num.HasValue)
                {
                    goto IL_117c;
                }
                switch (valueOrDefault)
                {
                    case 1:
                        break;
                    case 2:
                        goto IL_0a84;
                    case 3:
                        goto IL_0c51;
                    case 4:
                        goto IL_0e0a;
                    case 6:
                        goto IL_0fc3;
                    default:
                        goto IL_117c;
                }
                suppresttdes = from e in dbRest.SuppRestTDES
                               where e.EntSupplierId == searchEntSupplierId && e.EntBranchArticle.StartsWith(term)
                               orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                               select e;
                goto IL_1330;
            }
            goto IL_1346;
        IL_1330:
            suppresttdes = suppresttdes.Include("BranchRestArticleDescriptionTDES");
            goto IL_1346;
        IL_0a84:
            suppresttdes = from e in dbRest.SuppRestTDES
                           where e.EntSupplierId == searchEntSupplierId && e.BranchRestArticleDescriptionTDES.EntArticleDescription.StartsWith(term)
                           orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                           select e;
            goto IL_1330;
        IL_0e0a:
            suppresttdes = from e in dbRest.SuppRestTDES
                           where e.EntSupplierId == searchEntSupplierId && e.SUP_TEXT.StartsWith(term)
                           orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                           select e;
            goto IL_1330;
        IL_1346:
            int? num2 = currentFilterBy;
            int valueOrDefault2 = num2.GetValueOrDefault();
            if (num2.HasValue)
            {
                switch (valueOrDefault2)
                {
                    case 1:
                        {
                            var ret4 = (from e in suppresttdes
                                        select new
                                        {
                                            value = e.EntBranchArticle,
                                            va0 = e.EntBranchArticle,
                                            va1 = e.EntBranchSup,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ART_ARTICLE_NR,
                                            va5 = e.SUP_TEXT
                                        }).Take(10);
                            return Json(await ret4.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 2:
                        {
                            var ret6 = (from e in suppresttdes
                                        select new
                                        {
                                            value = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                                            va0 = e.EntBranchArticle,
                                            va1 = e.EntBranchSup,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ART_ARTICLE_NR,
                                            va5 = e.SUP_TEXT
                                        }).Take(10);
                            return Json(await ret6.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 3:
                        {
                            var ret5 = (from e in suppresttdes
                                        select new
                                        {
                                            value = e.ART_ARTICLE_NR,
                                            va0 = e.EntBranchArticle,
                                            va1 = e.EntBranchSup,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ART_ARTICLE_NR,
                                            va5 = e.SUP_TEXT
                                        }).Take(10);
                            return Json(await ret5.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 4:
                        {
                            var ret3 = (from e in suppresttdes
                                        select new
                                        {
                                            value = e.SUP_TEXT,
                                            va0 = e.EntBranchArticle,
                                            va1 = e.EntBranchSup,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ART_ARTICLE_NR,
                                            va5 = e.SUP_TEXT
                                        }).Take(10);
                            return Json(await ret3.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 6:
                        {
                            var ret2 = (from e in suppresttdes
                                        select new
                                        {
                                            value = e.EntBranchSup,
                                            va0 = e.EntBranchArticle,
                                            va1 = e.EntBranchSup,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ART_ARTICLE_NR,
                                            va5 = e.SUP_TEXT
                                        }).Take(10);
                            return Json(await ret2.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                }
            }
            var ret = (from e in suppresttdes
                       select new
                       {
                           value = e.ExternArticleEAN,
                           va0 = e.EntBranchArticle,
                           va1 = e.EntBranchSup,
                           va2 = e.ExternArticleEAN,
                           va3 = e.BranchRestArticleDescriptionTDES.EntArticleDescription,
                           va4 = e.ART_ARTICLE_NR,
                           va5 = e.SUP_TEXT
                       }).Take(10);
            return Json(await ret.ToListAsync(), JsonRequestBehavior.AllowGet);
        IL_0c51:
            suppresttdes = from e in dbRest.SuppRestTDES
                           where e.EntSupplierId == searchEntSupplierId && e.ART_ARTICLE_NR.StartsWith(term)
                           orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                           select e;
            goto IL_1330;
        IL_0fc3:
            suppresttdes = from e in dbRest.SuppRestTDES
                           where e.EntSupplierId == searchEntSupplierId && e.EntBranchSup.StartsWith(term)
                           orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                           select e;
            goto IL_1330;
        IL_117c:
            suppresttdes = from e in dbRest.SuppRestTDES
                           where e.EntSupplierId == searchEntSupplierId && e.ExternArticleEAN.StartsWith(term)
                           orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                           select e;
            goto IL_1330;
        }

        public async Task<ActionResult> Index(Guid? searchEntGuid, string searchEntSupplierId, string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page = default(int?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                searchEntGuid = ((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                       where e.EntUserNic == aName
                                                                                                                       select e.EntGuid).FirstOrDefaultAsync()) : (await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                                                          where e.EntUserNic == aName
                                                                                                                                                                          select new EnterpriseBranchIdsTDES
                                                                                                                                                                          {
                                                                                                                                                                              EntGuid = e.EntGuid,
                                                                                                                                                                              EntBranchGuid = e.EntBranchGuid
                                                                                                                                                                          }).FirstOrDefaultAsync())?.EntGuid);
            }
            IPagedList<SuppRestTDES> aResult = null;
            await ViewBagHelper(searchEntGuid, searchEntSupplierId);
            if (!base.ModelState.IsValid)
            {
                searchStringBy = (searchStringBy ?? 1);
                base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
                return View(aResult);
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
            IQueryable<SuppRestTDES> suppresttdes2;
            if (!string.IsNullOrEmpty(searchString))
            {
                (from e in dbRest.SuppRestTDES
                 where e.EntSupplierId == searchEntSupplierId
                 orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                 select e).Include("BranchRestArticleDescriptionTDES");
                int? num = searchStringBy;
                int valueOrDefault = num.GetValueOrDefault();
                if (!num.HasValue)
                {
                    goto IL_12ec;
                }
                switch (valueOrDefault)
                {
                    case 1:
                        break;
                    case 2:
                        goto IL_0bf4;
                    case 3:
                        goto IL_0dc1;
                    case 4:
                        goto IL_0f7a;
                    case 6:
                        goto IL_1133;
                    default:
                        goto IL_12ec;
                }
                suppresttdes2 = from e in dbRest.SuppRestTDES
                                where e.EntSupplierId == searchEntSupplierId && e.EntBranchArticle.StartsWith(searchString)
                                orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                                select e;
                goto IL_14a0;
            }
            IQueryable<SuppRestTDES> suppresttdes = (from e in dbRest.SuppRestTDES
                                                     where e.EntSupplierId == searchEntSupplierId
                                                     orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                                                     select e).Include("BranchRestArticleDescriptionTDES");
            int pageSize = 20;
            int pageNumber = page.Value;
            aResult = await suppresttdes.ToPagedListAsync(pageNumber, pageSize);
            goto IL_175a;
        IL_14a0:
            suppresttdes2 = suppresttdes2.Include("BranchRestArticleDescriptionTDES");
            int pageSize2 = 20;
            int pageNumber2 = page.Value;
            aResult = await suppresttdes2.ToPagedListAsync(pageNumber2, pageSize2);
            goto IL_175a;
        IL_175a:
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.currentFilterBy = searchStringBy;
            return View(aResult);
        IL_1133:
            suppresttdes2 = from e in dbRest.SuppRestTDES
                            where e.EntSupplierId == searchEntSupplierId && e.EntBranchSup.StartsWith(searchString)
                            orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                            select e;
            goto IL_14a0;
        IL_0bf4:
            suppresttdes2 = from e in dbRest.SuppRestTDES
                            where e.EntSupplierId == searchEntSupplierId && e.BranchRestArticleDescriptionTDES.EntArticleDescription.StartsWith(searchString)
                            orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                            select e;
            goto IL_14a0;
        IL_0dc1:
            suppresttdes2 = from e in dbRest.SuppRestTDES
                            where e.EntSupplierId == searchEntSupplierId && e.ART_ARTICLE_NR.StartsWith(searchString)
                            orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                            select e;
            goto IL_14a0;
        IL_0f7a:
            suppresttdes2 = from e in dbRest.SuppRestTDES
                            where e.EntSupplierId == searchEntSupplierId && e.SUP_TEXT.StartsWith(searchString)
                            orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                            select e;
            goto IL_14a0;
        IL_12ec:
            suppresttdes2 = from e in dbRest.SuppRestTDES
                            where e.EntSupplierId == searchEntSupplierId && e.ExternArticleEAN.StartsWith(searchString)
                            orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                            select e;
            goto IL_14a0;
        }

        public async Task<ActionResult> Details(Guid? searchEntGuid, string searchEntSupplierId, string searchEntArticle, string searchEntBrandNic)
        {
            if (string.IsNullOrEmpty(searchEntSupplierId) || string.IsNullOrEmpty(searchEntArticle) || string.IsNullOrEmpty(searchEntBrandNic))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                searchEntGuid = ((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                       where e.EntUserNic == aName
                                                                                                                       select e.EntGuid).FirstOrDefaultAsync()) : (await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                                                          where e.EntUserNic == aName
                                                                                                                                                                          select new EnterpriseBranchIdsTDES
                                                                                                                                                                          {
                                                                                                                                                                              EntGuid = e.EntGuid,
                                                                                                                                                                              EntBranchGuid = e.EntBranchGuid
                                                                                                                                                                          }).FirstOrDefaultAsync())?.EntGuid);
            }
            await ViewBagHelper(searchEntGuid, searchEntSupplierId);
            if (!base.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuppRestTDES suppresttdes = await (from e in dbRest.SuppRestTDES
                                               where e.EntSupplierId == searchEntSupplierId && e.EntBranchArticle == searchEntArticle && e.EntBranchSup == searchEntBrandNic
                                               select e).FirstOrDefaultAsync();
            if (suppresttdes == null)
            {
                return HttpNotFound();
            }
            return View(suppresttdes);
        }

        public async Task<ActionResult> Create(Guid? searchEntGuid, string searchEntSupplierId)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                searchEntGuid = ((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                       where e.EntUserNic == aName
                                                                                                                       select e.EntGuid).FirstOrDefaultAsync()) : (await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                                                          where e.EntUserNic == aName
                                                                                                                                                                          select new EnterpriseBranchIdsTDES
                                                                                                                                                                          {
                                                                                                                                                                              EntGuid = e.EntGuid,
                                                                                                                                                                              EntBranchGuid = e.EntBranchGuid
                                                                                                                                                                          }).FirstOrDefaultAsync())?.EntGuid);
            }
            await ViewBagHelper(searchEntGuid, searchEntSupplierId);
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            SuppRestTDESTmp suppresttdes = new SuppRestTDESTmp
            {
                EntSupplierId = searchEntSupplierId
            };
            DateTime aDate2 = DateTime.Now;
            aDate2 = (suppresttdes.LastReplicated = (suppresttdes.LastUpdated = aDate2.AddMilliseconds(-aDate2.Millisecond)));
            if (searchEntGuid.HasValue)
            {
                suppresttdes.EntGuid = searchEntGuid.Value;
            }
            return View(suppresttdes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EntSupplierId,EntBranchArticle,EntBranchSup,ART_ARTICLE_NR,SUP_TEXT,ArtAmount,ArtPrice,LastUpdated,LastReplicated,TSConcClmn,EntArticleDescriptionId,EntArticleDescription,ExternArticleEAN,EntGuid")] SuppRestTDESTmp suppresttdes, string ART_ARTICLE_NRLookUp)
        {
            if (ART_ARTICLE_NRLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(suppresttdes);
                string aRT_ARTICLE_NR = suppresttdes.ART_ARTICLE_NR;
                return RedirectToAction("LookUpArticleByID", "TecDoc", new
                {
                    redirecData = redirecData,
                    redirectContriller = "EnterpriseSupplierRest",
                    redirectAction = "GetArticleTDForCreate",
                    artId = aRT_ARTICLE_NR,
                    srchTp = 0
                });
            }
            UserIsInRoles();
            Guid? searchEntGuid;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                searchEntGuid = ((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                       where e.EntUserNic == aName
                                                                                                                       select e.EntGuid).FirstOrDefaultAsync()) : (await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                                                          where e.EntUserNic == aName
                                                                                                                                                                          select new EnterpriseBranchIdsTDES
                                                                                                                                                                          {
                                                                                                                                                                              EntGuid = e.EntGuid,
                                                                                                                                                                              EntBranchGuid = e.EntBranchGuid
                                                                                                                                                                          }).FirstOrDefaultAsync())?.EntGuid);
            }
            else
            {
                searchEntGuid = suppresttdes.EntGuid;
            }
            if (base.ModelState.IsValid && suppresttdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            await ViewBagHelper(suppresttdes.EntGuid, suppresttdes.EntSupplierId);
            if (base.ModelState.IsValid && CarShopRestContextCatalog != null)
            {
                if (suppresttdes.ExternArticleEAN != null)
                {
                    suppresttdes.ExternArticleEAN = suppresttdes.ExternArticleEAN.Replace(" ", "");
                }
                SuppRestTDES suppresttdesToSave = new SuppRestTDES();
                suppresttdes.CopyTo(suppresttdesToSave, doCreateDescr: false);
                BranchRestArticleDescriptionTDES desr = await (from e in dbRest.BranchRestArticleDescriptionTDES
                                                               where e.EntArticleDescription == suppresttdes.EntArticleDescription
                                                               select e).FirstOrDefaultAsync();
                if (desr == null)
                {
                    desr = new BranchRestArticleDescriptionTDES
                    {
                        EntArticleDescription = suppresttdes.EntArticleDescription
                    };
                    dbRest.BranchRestArticleDescriptionTDES.Add(desr);
                    await dbRest.SaveChangesAsync();
                }
                suppresttdesToSave.EntArticleDescriptionId = desr.EntArticleDescriptionId;
                dbRest.SuppRestTDES.Add(suppresttdesToSave);
                await dbRest.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = suppresttdes.EntGuid,
                    searchEntSupplierId = suppresttdes.EntSupplierId,
                    searchString = suppresttdes.EntBranchArticle
                });
            }
            return View(suppresttdes);
        }

        public async Task<ActionResult> GetArticleTDForCreate(string redirecData, string ART_ARTICLE_NR, string SUP_TEXT, string MASTER_BEZ, string EAN)
        {
            return await GetArticleTDForCreateUpdate(redirecData, ART_ARTICLE_NR, SUP_TEXT, MASTER_BEZ, EAN, "Create");
        }

        public async Task<ActionResult> GetArticleTDForUpdate(string redirecData, string ART_ARTICLE_NR, string SUP_TEXT, string MASTER_BEZ, string EAN)
        {
            return await GetArticleTDForCreateUpdate(redirecData, ART_ARTICLE_NR, SUP_TEXT, MASTER_BEZ, EAN, "Edit");
        }

        public async Task<ActionResult> GetArticleTDForCreateUpdate(string redirecData, string ART_ARTICLE_NR, string SUP_TEXT, string MASTER_BEZ, string EAN, string actionName)
        {
            SuppRestTDESTmp suppresttdes = JsonConvert.DeserializeObject<SuppRestTDESTmp>(redirecData);
            if (ART_ARTICLE_NR != null)
            {
                suppresttdes.ART_ARTICLE_NR = ART_ARTICLE_NR;
            }
            if (SUP_TEXT != null)
            {
                suppresttdes.SUP_TEXT = SUP_TEXT;
            }
            if (MASTER_BEZ != null)
            {
                suppresttdes.EntArticleDescription = MASTER_BEZ;
            }
            suppresttdes.ExternArticleEAN = EAN;
            await ViewBagHelper(suppresttdes.EntGuid, suppresttdes.EntSupplierId);
            return View(actionName, suppresttdes);
        }

        public async Task<ActionResult> Edit(Guid? searchEntGuid, string searchEntSupplierId, string searchEntArticle, string searchEntBrandNic)
        {
            if (string.IsNullOrEmpty(searchEntSupplierId) || string.IsNullOrEmpty(searchEntArticle) || string.IsNullOrEmpty(searchEntBrandNic))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                searchEntGuid = ((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                       where e.EntUserNic == aName
                                                                                                                       select e.EntGuid).FirstOrDefaultAsync()) : (await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                                                          where e.EntUserNic == aName
                                                                                                                                                                          select new EnterpriseBranchIdsTDES
                                                                                                                                                                          {
                                                                                                                                                                              EntGuid = e.EntGuid,
                                                                                                                                                                              EntBranchGuid = e.EntBranchGuid
                                                                                                                                                                          }).FirstOrDefaultAsync())?.EntGuid);
            }
            await ViewBagHelper(searchEntGuid, searchEntSupplierId);
            if (!base.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuppRestTDES suppresttdes = await (from e in dbRest.SuppRestTDES
                                               where e.EntSupplierId == searchEntSupplierId && e.EntBranchArticle == searchEntArticle && e.EntBranchSup == searchEntBrandNic
                                               select e).FirstOrDefaultAsync();
            if (suppresttdes == null)
            {
                return HttpNotFound();
            }
            SuppRestTDESTmp suppresttdestmp = new SuppRestTDESTmp();
            suppresttdestmp.CopyFrom(suppresttdes);
            if (searchEntGuid.HasValue)
            {
                suppresttdestmp.EntGuid = searchEntGuid.Value;
            }
            return View(suppresttdestmp);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "EntSupplierId,EntBranchArticle,EntBranchSup,ART_ARTICLE_NR,SUP_TEXT,ArtAmount,ArtPrice,LastUpdated,LastReplicated,TSConcClmn,EntArticleDescriptionId,EntArticleDescription,ExternArticleEAN,EntGuid")] SuppRestTDESTmp suppresttdes, string ART_ARTICLE_NRLookUp)
        {
            if (ART_ARTICLE_NRLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(suppresttdes);
                string aRT_ARTICLE_NR = suppresttdes.ART_ARTICLE_NR;
                return RedirectToAction("LookUpArticleByID", "TecDoc", new
                {
                    redirecData = redirecData,
                    redirectContriller = "EnterpriseSupplierRest",
                    redirectAction = "GetArticleTDForUpdate",
                    artId = aRT_ARTICLE_NR,
                    srchTp = 0
                });
            }
            UserIsInRoles();
            Guid? searchEntGuid;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                searchEntGuid = ((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                       where e.EntUserNic == aName
                                                                                                                       select e.EntGuid).FirstOrDefaultAsync()) : (await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                                                          where e.EntUserNic == aName
                                                                                                                                                                          select new EnterpriseBranchIdsTDES
                                                                                                                                                                          {
                                                                                                                                                                              EntGuid = e.EntGuid,
                                                                                                                                                                              EntBranchGuid = e.EntBranchGuid
                                                                                                                                                                          }).FirstOrDefaultAsync())?.EntGuid);
            }
            else
            {
                searchEntGuid = suppresttdes.EntGuid;
            }
            if (base.ModelState.IsValid && suppresttdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            await ViewBagHelper(suppresttdes.EntGuid, suppresttdes.EntSupplierId);
            if (base.ModelState.IsValid && CarShopRestContextCatalog != null)
            {
                if (suppresttdes.ExternArticleEAN != null)
                {
                    suppresttdes.ExternArticleEAN = suppresttdes.ExternArticleEAN.Replace(" ", "");
                }
                SuppRestTDES suppresttdesToSave = new SuppRestTDES();
                suppresttdes.CopyTo(suppresttdesToSave, doCreateDescr: false);
                BranchRestArticleDescriptionTDES desr = await (from e in dbRest.BranchRestArticleDescriptionTDES
                                                               where e.EntArticleDescription == suppresttdes.EntArticleDescription
                                                               select e).FirstOrDefaultAsync();
                if (desr == null)
                {
                    desr = new BranchRestArticleDescriptionTDES
                    {
                        EntArticleDescription = suppresttdes.EntArticleDescription
                    };
                    dbRest.BranchRestArticleDescriptionTDES.Add(desr);
                    await dbRest.SaveChangesAsync();
                }
                suppresttdesToSave.EntArticleDescriptionId = desr.EntArticleDescriptionId;
                dbRest.Entry(suppresttdesToSave).State = EntityState.Modified;
                await dbRest.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = suppresttdes.EntGuid,
                    searchEntSupplierId = suppresttdes.EntSupplierId,
                    searchString = suppresttdes.EntBranchArticle
                });
            }
            return View(suppresttdes);
        }

        public async Task<ActionResult> Delete(Guid? searchEntGuid, string searchEntSupplierId, string searchEntArticle, string searchEntBrandNic)
        {
            if (string.IsNullOrEmpty(searchEntSupplierId) || string.IsNullOrEmpty(searchEntArticle) || string.IsNullOrEmpty(searchEntBrandNic))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                searchEntGuid = ((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                       where e.EntUserNic == aName
                                                                                                                       select e.EntGuid).FirstOrDefaultAsync()) : (await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                                                          where e.EntUserNic == aName
                                                                                                                                                                          select new EnterpriseBranchIdsTDES
                                                                                                                                                                          {
                                                                                                                                                                              EntGuid = e.EntGuid,
                                                                                                                                                                              EntBranchGuid = e.EntBranchGuid
                                                                                                                                                                          }).FirstOrDefaultAsync())?.EntGuid);
            }
            await ViewBagHelper(searchEntGuid, searchEntSupplierId);
            if (!base.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuppRestTDES suppresttdes = await (from e in dbRest.SuppRestTDES
                                               where e.EntSupplierId == searchEntSupplierId && e.EntBranchArticle == searchEntArticle && e.EntBranchSup == searchEntBrandNic
                                               select e).FirstOrDefaultAsync();
            if (suppresttdes == null)
            {
                return HttpNotFound();
            }
            return View(suppresttdes);
        }

        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(Guid? searchEntGuid, string searchEntSupplierId, string searchEntArticle, string searchEntBrandNic)
        {
            if (string.IsNullOrEmpty(searchEntSupplierId) || string.IsNullOrEmpty(searchEntArticle) || string.IsNullOrEmpty(searchEntBrandNic))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                searchEntGuid = ((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                       where e.EntUserNic == aName
                                                                                                                       select e.EntGuid).FirstOrDefaultAsync()) : (await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                                                          where e.EntUserNic == aName
                                                                                                                                                                          select new EnterpriseBranchIdsTDES
                                                                                                                                                                          {
                                                                                                                                                                              EntGuid = e.EntGuid,
                                                                                                                                                                              EntBranchGuid = e.EntBranchGuid
                                                                                                                                                                          }).FirstOrDefaultAsync())?.EntGuid);
            }
            await ViewBagHelper(searchEntGuid, searchEntSupplierId);
            if (!base.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuppRestTDES suppresttdes = await (from e in dbRest.SuppRestTDES
                                               where e.EntSupplierId == searchEntSupplierId && e.EntBranchArticle == searchEntArticle && e.EntBranchSup == searchEntBrandNic
                                               select e).FirstOrDefaultAsync();
            dbRest.SuppRestTDES.Remove(suppresttdes);
            await dbRest.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid,
                searchEntSupplierId
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            if (_dbRest != null)
            {
                _dbRest.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
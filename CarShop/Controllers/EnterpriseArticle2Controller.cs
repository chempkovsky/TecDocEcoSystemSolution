// CarShop.Controllers.EnterpriseArticle2Controller
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

    public class EnterpriseArticle2Controller : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopArticleContextCatalog;

        private CarShopArticleContext _dbBrand = new CarShopArticleContext();

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

        protected async Task ViewBagHelper(Guid? searchEntGuid)
        {
            EnterpriseTDES enterprises = await (from e in db.EnterpriseTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
            if (enterprises != null)
            {
                base.ViewBag.EntDescription = enterprises.EntDescription;
                base.ViewBag.SearchEntGuid = searchEntGuid;
                CarShopArticleContextCatalog = enterprises.ArticleCatalog;
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

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Autocomplete(Guid? searchEntGuid, int? currentFilterBy, string term)
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
                    Guid? reference2 = searchEntGuid;
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value2;
                }
            }
            currentFilterBy = (currentFilterBy ?? 1);
            await ViewBagHelper(searchEntGuid);
            if (CarShopArticleContextCatalog == null)
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
            IQueryable<EnterpriseArticleTDES> enterprisearticletdes = from e in dbBrand.EnterpriseArticleTDES.Include("EnterpriseArticleDescriptionTDES")
                                                                      where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
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
                                                    where e.EnterpriseArticleDescriptionTDES.EntArticleDescription.StartsWith(term)
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
                            var ret4 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.EntArticle,
                                            va0 = e.EntArticle,
                                            va1 = e.EntBrandNic,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.EnterpriseArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ExternArticle,
                                            va5 = e.ExternBrandNic
                                        }).Take(10);
                            return Json(await ret4.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 2:
                        {
                            var ret6 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.EnterpriseArticleDescriptionTDES.EntArticleDescription,
                                            va0 = e.EntArticle,
                                            va1 = e.EntBrandNic,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.EnterpriseArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ExternArticle,
                                            va5 = e.ExternBrandNic
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
                                            va3 = e.EnterpriseArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ExternArticle,
                                            va5 = e.ExternBrandNic
                                        }).Take(10);
                            return Json(await ret5.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 4:
                        {
                            var ret3 = (from e in enterprisearticletdes
                                        select new
                                        {
                                            value = e.ExternBrandNic,
                                            va0 = e.EntArticle,
                                            va1 = e.EntBrandNic,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.EnterpriseArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ExternArticle,
                                            va5 = e.ExternBrandNic
                                        }).Take(10);
                            return Json(await ret3.ToListAsync(), JsonRequestBehavior.AllowGet);
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
                                            va3 = e.EnterpriseArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ExternArticle,
                                            va5 = e.ExternBrandNic
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
                           va3 = e.EnterpriseArticleDescriptionTDES.EntArticleDescription,
                           va4 = e.ExternArticle,
                           va5 = e.ExternBrandNic
                       }).Take(10);
            return Json(await ret.ToListAsync(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid, string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page)
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
                    Guid? reference2 = searchEntGuid;
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value2;
                }
            }
            IPagedList<EnterpriseArticleTDES> aResult = null;
            await ViewBagHelper(searchEntGuid);
            IQueryable<EnterpriseArticleTDES> enterprisearticletdes2;
            if (CarShopArticleContextCatalog != null)
            {
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
                if (!string.IsNullOrEmpty(searchString))
                {
                    (from e in dbBrand.EnterpriseArticleTDES
                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                     select e).Include("EnterpriseArticleDescriptionTDES");
                    int? num = searchStringBy;
                    int valueOrDefault = num.GetValueOrDefault();
                    if (!num.HasValue)
                    {
                        goto IL_1238;
                    }
                    switch (valueOrDefault)
                    {
                        case 1:
                            break;
                        case 2:
                            goto IL_0b04;
                        case 3:
                            goto IL_0ce0;
                        case 4:
                            goto IL_0ea8;
                        case 6:
                            goto IL_1070;
                        default:
                            goto IL_1238;
                    }
                    enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntArticle.StartsWith(searchString)
                                             orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                             select e;
                    goto IL_13fb;
                }
                IQueryable<EnterpriseArticleTDES> enterprisearticletdes = (from e in dbBrand.EnterpriseArticleTDES
                                                                           where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntArticle == searchString
                                                                           orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                                                           select e).Include("EnterpriseArticleDescriptionTDES");
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await enterprisearticletdes.ToPagedListAsync(pageNumber, pageSize);
                base.ModelState.AddModelError("", Resources.SEARCH_STRING_IS_EMPTY);
            }
            else
            {
                base.ModelState.AddModelError("", Resources.EnterpriseTDES_NOTDEFINED);
                searchStringBy = (searchStringBy ?? 1);
            }
            goto IL_176f;
        IL_1238:
            enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.ExternArticleEAN.StartsWith(searchString)
                                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                     select e;
            goto IL_13fb;
        IL_1070:
            enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic.StartsWith(searchString)
                                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                     select e;
            goto IL_13fb;
        IL_176f:
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.currentFilterBy = searchStringBy;
            return View(aResult);
        IL_0ce0:
            enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.ExternArticle.StartsWith(searchString)
                                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                     select e;
            goto IL_13fb;
        IL_0b04:
            enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EnterpriseArticleDescriptionTDES.EntArticleDescription.StartsWith(searchString)
                                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                     select e;
            goto IL_13fb;
        IL_0ea8:
            enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.ExternBrandNic.StartsWith(searchString)
                                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                     select e;
            goto IL_13fb;
        IL_13fb:
            enterprisearticletdes2 = enterprisearticletdes2.Include("EnterpriseArticleDescriptionTDES");
            int pageSize2 = 20;
            int pageNumber2 = page.Value;
            aResult = await enterprisearticletdes2.ToPagedListAsync(pageNumber2, pageSize2);
            goto IL_176f;
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(string searchEntArticle = null, Guid? searchEntGuid = default(Guid?), string searchEntBrandNic = null)
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
                    Guid? reference2 = searchEntGuid;
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value2;
                }
            }
            EnterpriseArticleTDES enterprisearticletdes = null;
            await ViewBagHelper(searchEntGuid);
            if (CarShopArticleContextCatalog != null)
            {
                enterprisearticletdes = await (from e in dbBrand.EnterpriseArticleTDES
                                               where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == searchEntBrandNic && e.EntArticle == searchEntArticle
                                               select e).Include("EnterpriseArticleDescriptionTDES").FirstOrDefaultAsync();
            }
            if (enterprisearticletdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisearticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Create(Guid? searchEntGuid = default(Guid?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                }
            }
            EnterpriseArticleTmp enterprisearticletdes = null;
            await ViewBagHelper(searchEntGuid);
            if (base.ViewBag.SearchEntGuid != null)
            {
                enterprisearticletdes = new EnterpriseArticleTmp
                {
                    EntGuid = searchEntGuid.Value
                };
            }
            return View(enterprisearticletdes);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        public async Task<ActionResult> Create(EnterpriseArticleTmp enterprisearticletdes, string ART_ARTICLE_NRLookUp, string EnterpriseBrandLookUp)
        {
            if (EnterpriseBrandLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(enterprisearticletdes);
                string entBrandNic = enterprisearticletdes.EntBrandNic;
                return RedirectToAction("LookUpEnterpriseBrand", "EnterpriseBrand", new
                {
                    redirecData = redirecData,
                    redirectContriller = "EnterpriseArticle2",
                    redirectAction = "GetEnterpriseBrandForCreate",
                    searchEntGuid = enterprisearticletdes.EntGuid,
                    searchString = entBrandNic,
                    searchStringBy = 1
                });
            }
            if (ART_ARTICLE_NRLookUp != null)
            {
                string redirecData2 = JsonConvert.SerializeObject(enterprisearticletdes);
                string externArticle = enterprisearticletdes.ExternArticle;
                return RedirectToAction("LookUpArticleByID", "TecDoc", new
                {
                    redirecData = redirecData2,
                    redirectContriller = "EnterpriseArticle2",
                    redirectAction = "GetArticleTDForCreate",
                    artId = externArticle,
                    srchTp = 0
                });
            }
            UserIsInRoles();
            Guid? searchEntGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                }
            }
            else
            {
                searchEntGuid = enterprisearticletdes.EntGuid;
            }
            if (base.ModelState.IsValid && enterprisearticletdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            await ViewBagHelper(searchEntGuid);
            if (base.ModelState.IsValid && CarShopArticleContextCatalog != null)
            {
                if (enterprisearticletdes.ExternArticleEAN != null)
                {
                    enterprisearticletdes.ExternArticleEAN = enterprisearticletdes.ExternArticleEAN.Replace(" ", "");
                }
                EnterpriseArticleTDES enterprisearticletdesToSave = new EnterpriseArticleTDES();
                enterprisearticletdes.CopyTo(enterprisearticletdesToSave, doCreateDescr: false);
                EnterpriseArticleDescriptionTDES desr = await (from e in dbBrand.EnterpriseArticleDescriptionTDES
                                                               where e.EntArticleDescription == enterprisearticletdes.EntArticleDescription
                                                               select e).FirstOrDefaultAsync();
                if (desr == null)
                {
                    desr = new EnterpriseArticleDescriptionTDES
                    {
                        EntArticleDescription = enterprisearticletdes.EntArticleDescription
                    };
                    dbBrand.EnterpriseArticleDescriptionTDES.Add(desr);
                    await dbBrand.SaveChangesAsync();
                }
                enterprisearticletdesToSave.EntArticleDescriptionId = desr.EntArticleDescriptionId;
                dbBrand.EnterpriseArticleTDES.Add(enterprisearticletdesToSave);
                await dbBrand.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = enterprisearticletdes.EntGuid,
                    searchString = enterprisearticletdes.EntArticle
                });
            }
            return View(enterprisearticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(string searchEntArticle = null, Guid? searchEntGuid = default(Guid?), string searchEntBrandNic = null)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    Guid? reference = searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
            }
            EnterpriseArticleTDES enterprisearticletdes = null;
            await ViewBagHelper(searchEntGuid);
            if (CarShopArticleContextCatalog != null)
            {
                enterprisearticletdes = await (from e in dbBrand.EnterpriseArticleTDES
                                               where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == searchEntBrandNic && e.EntArticle == searchEntArticle
                                               select e).Include("EnterpriseArticleDescriptionTDES").FirstOrDefaultAsync();
            }
            if (enterprisearticletdes == null)
            {
                return HttpNotFound();
            }
            EnterpriseArticleTmp ret = new EnterpriseArticleTmp();
            ret.CopyFrom(enterprisearticletdes);
            return View(ret);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EnterpriseArticleTmp enterprisearticletdes, string ART_ARTICLE_NRLookUp)
        {
            if (ART_ARTICLE_NRLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(enterprisearticletdes);
                string externArticle = enterprisearticletdes.ExternArticle;
                return RedirectToAction("LookUpArticleByID", "TecDoc", new
                {
                    redirecData = redirecData,
                    redirectContriller = "EnterpriseArticle2",
                    redirectAction = "GetArticleTDForUpdate",
                    artId = externArticle,
                    srchTp = 0
                });
            }
            UserIsInRoles();
            Guid? searchEntGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                }
            }
            else
            {
                searchEntGuid = enterprisearticletdes.EntGuid;
            }
            if (base.ModelState.IsValid && enterprisearticletdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            await ViewBagHelper(searchEntGuid);
            if (base.ModelState.IsValid && CarShopArticleContextCatalog != null)
            {
                if (enterprisearticletdes.ExternArticleEAN != null)
                {
                    enterprisearticletdes.ExternArticleEAN = enterprisearticletdes.ExternArticleEAN.Replace(" ", "");
                }
                EnterpriseArticleTDES enterprisearticletdesToSave = new EnterpriseArticleTDES();
                enterprisearticletdes.CopyTo(enterprisearticletdesToSave, doCreateDescr: false);
                EnterpriseArticleDescriptionTDES desr = await (from e in dbBrand.EnterpriseArticleDescriptionTDES
                                                               where e.EntArticleDescription == enterprisearticletdes.EntArticleDescription
                                                               select e).FirstOrDefaultAsync();
                if (desr == null)
                {
                    desr = new EnterpriseArticleDescriptionTDES
                    {
                        EntArticleDescription = enterprisearticletdes.EntArticleDescription
                    };
                    dbBrand.EnterpriseArticleDescriptionTDES.Add(desr);
                    await dbBrand.SaveChangesAsync();
                }
                enterprisearticletdesToSave.EntArticleDescriptionId = desr.EntArticleDescriptionId;
                dbBrand.Entry(enterprisearticletdesToSave).State = EntityState.Modified;
                await dbBrand.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = enterprisearticletdes.EntGuid,
                    searchString = enterprisearticletdes.EntArticle
                });
            }
            return View(enterprisearticletdes);
        }

        public async Task<ActionResult> GetArticleTDForCreateUpdate(string redirecData, string ART_ARTICLE_NR, string SUP_TEXT, string MASTER_BEZ, string EAN, string actionName)
        {
            EnterpriseArticleTmp enterprisearticletdes = JsonConvert.DeserializeObject<EnterpriseArticleTmp>(redirecData);
            if (ART_ARTICLE_NR != null)
            {
                enterprisearticletdes.ExternArticle = ART_ARTICLE_NR;
            }
            if (SUP_TEXT != null)
            {
                enterprisearticletdes.ExternBrandNic = SUP_TEXT;
            }
            if (MASTER_BEZ != null)
            {
                enterprisearticletdes.EntArticleDescription = MASTER_BEZ;
            }
            enterprisearticletdes.ExternArticleEAN = EAN;
            await ViewBagHelper(enterprisearticletdes.EntGuid);
            return View(actionName, enterprisearticletdes);
        }

        public async Task<ActionResult> GetArticleTDForCreate(string redirecData, string ART_ARTICLE_NR, string SUP_TEXT, string MASTER_BEZ, string EAN)
        {
            return await GetArticleTDForCreateUpdate(redirecData, ART_ARTICLE_NR, SUP_TEXT, MASTER_BEZ, EAN, "Create");
        }

        public async Task<ActionResult> GetArticleTDForUpdate(string redirecData, string ART_ARTICLE_NR, string SUP_TEXT, string MASTER_BEZ, string EAN)
        {
            return await GetArticleTDForCreateUpdate(redirecData, ART_ARTICLE_NR, SUP_TEXT, MASTER_BEZ, EAN, "Edit");
        }

        public async Task<ActionResult> GetEnterpriseBrandForCreateUpdate(string redirecData, string EntBrandNic, string EntBrandDescription, string actionName)
        {
            EnterpriseArticleTmp enterprisearticletdes = JsonConvert.DeserializeObject<EnterpriseArticleTmp>(redirecData);
            if (EntBrandNic != null)
            {
                enterprisearticletdes.EntBrandNic = EntBrandNic;
            }
            await ViewBagHelper(enterprisearticletdes.EntGuid);
            return View(actionName, enterprisearticletdes);
        }

        public async Task<ActionResult> GetEnterpriseBrandForCreate(string redirecData, string EntBrandNic, string EntBrandDescription)
        {
            return await GetEnterpriseBrandForCreateUpdate(redirecData, EntBrandNic, EntBrandDescription, "Create");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Delete(string searchEntArticle = null, Guid? searchEntGuid = default(Guid?), string searchEntBrandNic = null)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    Guid? reference = searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
            }
            EnterpriseArticleTDES enterprisearticletdes = null;
            await ViewBagHelper(searchEntGuid);
            if (CarShopArticleContextCatalog != null)
            {
                enterprisearticletdes = await (from e in dbBrand.EnterpriseArticleTDES
                                               where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == searchEntBrandNic && e.EntArticle == searchEntArticle
                                               select e).Include("EnterpriseArticleDescriptionTDES").FirstOrDefaultAsync();
            }
            if (enterprisearticletdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisearticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string searchEntArticle = null, Guid? searchEntGuid = default(Guid?), string searchEntBrandNic = null)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    Guid? reference = searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
            }
            await ViewBagHelper(searchEntGuid);
            if (CarShopArticleContextCatalog != null)
            {
                EnterpriseArticleTDES enterprisearticletdes = await (from e in dbBrand.EnterpriseArticleTDES
                                                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == searchEntBrandNic && e.EntArticle == searchEntArticle
                                                                     select e).FirstOrDefaultAsync();
                dbBrand.EnterpriseArticleTDES.Remove(enterprisearticletdes);
                await dbBrand.SaveChangesAsync();
            }
            return RedirectToAction("Index", new
            {
                searchEntGuid
            });
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
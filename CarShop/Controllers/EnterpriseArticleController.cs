// CarShop.Controllers.EnterpriseArticleController
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

    public class EnterpriseArticleController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopArticleContextCatalog;

        private CarShopArticleContext _dbBrand;

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

        protected async Task ViewBagHelper(Guid? searchEntGuid, string searchEntBrandNic)
        {
            await ViewBagHelper(searchEntGuid);
            if (CarShopArticleContextCatalog != null)
            {
                EnterpriseBrandTDES enterprisebrandtdes = await (from e in dbBrand.EnterpriseBrandTDES
                                                                 where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == searchEntBrandNic
                                                                 select e).FirstOrDefaultAsync();
                if (enterprisebrandtdes == null)
                {
                    base.ViewBag.EntBrandDescription = Resources.ENTERPRISE_NOT_DEFINED;
                    base.ViewBag.SearchEntBrandNic = null;
                }
                else
                {
                    base.ViewBag.EntBrandDescription = enterprisebrandtdes.EntBrandDescription;
                    base.ViewBag.SearchEntBrandNic = enterprisebrandtdes.EntBrandNic;
                }
            }
            else
            {
                base.ViewBag.EntBrandDescription = Resources.ENTERPRISE_NOT_DEFINED;
                base.ViewBag.SearchEntBrandNic = null;
            }
        }

        protected SelectList SelectListHelper(int showIs)
        {
            return new SelectList(new SelectListItem[5]
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
        public async Task<ActionResult> Autocomplete(Guid? searchEntGuid, string searchEntBrandNic, int? currentFilterBy, string term)
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
                                                                      where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == searchEntBrandNic
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
                                            va3 = e.EnterpriseArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ExternArticle,
                                            va5 = e.ExternBrandNic
                                        }).Take(10);
                            return Json(await ret3.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 2:
                        {
                            var ret5 = (from e in enterprisearticletdes
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
                            return Json(await ret5.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 3:
                        {
                            var ret4 = (from e in enterprisearticletdes
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
                            return Json(await ret4.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 4:
                        {
                            var ret2 = (from e in enterprisearticletdes
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
        public async Task<ActionResult> Index(Guid? searchEntGuid, string searchEntBrandNic, string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page)
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
            await ViewBagHelper(searchEntGuid, searchEntBrandNic);
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
                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == searchEntBrandNic
                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                     select e).Include("EnterpriseArticleDescriptionTDES");
                    int? num = searchStringBy;
                    int valueOrDefault = num.GetValueOrDefault();
                    if (!num.HasValue)
                    {
                        goto IL_11f6;
                    }
                    switch (valueOrDefault)
                    {
                        case 1:
                            break;
                        case 2:
                            goto IL_0bac;
                        case 3:
                            goto IL_0dd2;
                        case 4:
                            goto IL_0fe4;
                        default:
                            goto IL_11f6;
                    }
                    enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == searchEntBrandNic && e.EntArticle.StartsWith(searchString)
                                             orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                             select e;
                    goto IL_1403;
                }
                IQueryable<EnterpriseArticleTDES> enterprisearticletdes = (from e in dbBrand.EnterpriseArticleTDES
                                                                           where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == searchEntBrandNic && e.EntArticle == searchString
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
            goto IL_17c1;
        IL_11f6:
            enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == searchEntBrandNic && e.ExternArticleEAN.StartsWith(searchString)
                                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                     select e;
            goto IL_1403;
        IL_0fe4:
            enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == searchEntBrandNic && e.ExternBrandNic.StartsWith(searchString)
                                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                     select e;
            goto IL_1403;
        IL_0dd2:
            enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == searchEntBrandNic && e.ExternArticle.StartsWith(searchString)
                                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                     select e;
            goto IL_1403;
        IL_17c1:
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.currentFilterBy = searchStringBy;
            return View(aResult);
        IL_1403:
            enterprisearticletdes2 = enterprisearticletdes2.Include("EnterpriseArticleDescriptionTDES");
            int pageSize2 = 20;
            int pageNumber2 = page.Value;
            aResult = await enterprisearticletdes2.ToPagedListAsync(pageNumber2, pageSize2);
            goto IL_17c1;
        IL_0bac:
            enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == searchEntBrandNic && e.EnterpriseArticleDescriptionTDES.EntArticleDescription.StartsWith(searchString)
                                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                     select e;
            goto IL_1403;
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
            await ViewBagHelper(searchEntGuid, searchEntBrandNic);
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
        public async Task<ActionResult> Create(Guid? searchEntGuid = default(Guid?), string searchEntBrandNic = null)
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
            await ViewBagHelper(searchEntGuid, searchEntBrandNic);
            if (base.ViewBag.SearchEntBrandNic != null)
            {
                enterprisearticletdes = new EnterpriseArticleTmp
                {
                    EntBrandNic = searchEntBrandNic,
                    EntGuid = searchEntGuid.Value
                };
            }
            return View(enterprisearticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(EnterpriseArticleTmp enterprisearticletdes, string ART_ARTICLE_NRLookUp)
        {
            if (ART_ARTICLE_NRLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(enterprisearticletdes);
                string externArticle = enterprisearticletdes.ExternArticle;
                return RedirectToAction("LookUpArticleByID", "TecDoc", new
                {
                    redirecData = redirecData,
                    redirectContriller = "EnterpriseArticle",
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
            await ViewBagHelper(searchEntGuid, enterprisearticletdes.EntBrandNic);
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
                    searchEntBrandNic = enterprisearticletdes.EntBrandNic,
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
            await ViewBagHelper(searchEntGuid, searchEntBrandNic);
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(EnterpriseArticleTmp enterprisearticletdes, string ART_ARTICLE_NRLookUp)
        {
            if (ART_ARTICLE_NRLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(enterprisearticletdes);
                string externArticle = enterprisearticletdes.ExternArticle;
                return RedirectToAction("LookUpArticleByID", "TecDoc", new
                {
                    redirecData = redirecData,
                    redirectContriller = "EnterpriseArticle",
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
            await ViewBagHelper(searchEntGuid, enterprisearticletdes.EntBrandNic);
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
                    searchEntBrandNic = enterprisearticletdes.EntBrandNic,
                    searchString = enterprisearticletdes.EntArticle
                });
            }
            return View(enterprisearticletdes);
        }

        protected async Task<ActionResult> GetArticleTDForCreateUpdate(string redirecData, string ART_ARTICLE_NR, string SUP_TEXT, string MASTER_BEZ, string EAN, string actionName)
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
            await ViewBagHelper(enterprisearticletdes.EntGuid, enterprisearticletdes.EntBrandNic);
            return View(actionName, enterprisearticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> GetArticleTDForCreate(string redirecData, string ART_ARTICLE_NR, string SUP_TEXT, string MASTER_BEZ, string EAN)
        {
            return await GetArticleTDForCreateUpdate(redirecData, ART_ARTICLE_NR, SUP_TEXT, MASTER_BEZ, EAN, "Create");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> GetArticleTDForUpdate(string redirecData, string ART_ARTICLE_NR, string SUP_TEXT, string MASTER_BEZ, string EAN)
        {
            return await GetArticleTDForCreateUpdate(redirecData, ART_ARTICLE_NR, SUP_TEXT, MASTER_BEZ, EAN, "Edit");
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
            await ViewBagHelper(searchEntGuid, searchEntBrandNic);
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

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
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
            await ViewBagHelper(searchEntGuid, searchEntBrandNic);
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
                searchEntGuid,
                searchEntBrandNic
            });
        }

        public async Task<ActionResult> AutocompleteForLookUp(Guid? searchEntGuid, int? currentFilterBy, string term)
        {
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
                                            value = e.EntArticle,
                                            va0 = e.EntArticle,
                                            va1 = e.EntBrandNic,
                                            va2 = e.ExternArticleEAN,
                                            va3 = e.EnterpriseArticleDescriptionTDES.EntArticleDescription,
                                            va4 = e.ExternArticle,
                                            va5 = e.ExternBrandNic
                                        }).Take(10);
                            return Json(await ret5.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 2:
                        {
                            var ret4 = (from e in enterprisearticletdes
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
                            return Json(await ret4.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 3:
                        {
                            var ret3 = (from e in enterprisearticletdes
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
                            return Json(await ret3.ToListAsync(), JsonRequestBehavior.AllowGet);
                        }
                    case 4:
                        {
                            var ret2 = (from e in enterprisearticletdes
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

        public async Task<ActionResult> LookUpArticleByID(string redirecData, string redirectContriller, string redirectAction, Guid? searchEntGuid, string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page)
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
            base.ViewBag.RedirecData = redirecData;
            base.ViewBag.RedirectContriller = redirectContriller;
            base.ViewBag.RedirectAction = redirectAction;
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
                        goto IL_0f25;
                    }
                    switch (valueOrDefault)
                    {
                        case 1:
                            break;
                        case 2:
                            goto IL_09b9;
                        case 3:
                            goto IL_0b95;
                        case 4:
                            goto IL_0d5d;
                        default:
                            goto IL_0f25;
                    }
                    enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntArticle.StartsWith(searchString)
                                             orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                             select e;
                    goto IL_10e8;
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
            goto IL_145c;
        IL_0b95:
            enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.ExternArticle.StartsWith(searchString)
                                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                     select e;
            goto IL_10e8;
        IL_09b9:
            enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EnterpriseArticleDescriptionTDES.EntArticleDescription.StartsWith(searchString)
                                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                     select e;
            goto IL_10e8;
        IL_10e8:
            enterprisearticletdes2 = enterprisearticletdes2.Include("EnterpriseArticleDescriptionTDES");
            int pageSize2 = 20;
            int pageNumber2 = page.Value;
            aResult = await enterprisearticletdes2.ToPagedListAsync(pageNumber2, pageSize2);
            goto IL_145c;
        IL_0f25:
            enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.ExternArticleEAN.StartsWith(searchString)
                                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                     select e;
            goto IL_10e8;
        IL_0d5d:
            enterprisearticletdes2 = from e in dbBrand.EnterpriseArticleTDES
                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.ExternBrandNic.StartsWith(searchString)
                                     orderby e.EntArticle, e.EntBrandNic, e.EntGuid
                                     select e;
            goto IL_10e8;
        IL_145c:
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.currentFilterBy = searchStringBy;
            return View(aResult);
        }

        [HttpPost]
        public ActionResult LookUpArticleByIDSelected(string redirecData, string redirectContriller, string redirectAction, string SimpleArticle, string DOSELECT_TITLE, string DOCANCEL_TITLE)
        {
            if (DOSELECT_TITLE != null)
            {
                string entArticle = null;
                string entBrandNic = null;
                Guid entGuid = Guid.Empty;
                string entArticleDescription = null;
                string externArticle = null;
                string externBrandNic = null;
                string externArticleEAN = null;
                int entArticleDescriptionId = 0;
                if (SimpleArticle != null)
                {
                    EnterpriseArticleTmp enterpriseArticleTmp = JsonConvert.DeserializeObject<EnterpriseArticleTmp>(SimpleArticle);
                    entArticle = enterpriseArticleTmp.EntArticle;
                    entBrandNic = enterpriseArticleTmp.EntBrandNic;
                    entGuid = enterpriseArticleTmp.EntGuid;
                    entArticleDescriptionId = enterpriseArticleTmp.EntArticleDescriptionId;
                    entArticleDescription = enterpriseArticleTmp.EntArticleDescription;
                    externArticle = enterpriseArticleTmp.ExternArticle;
                    externBrandNic = enterpriseArticleTmp.ExternBrandNic;
                    externArticleEAN = enterpriseArticleTmp.ExternArticleEAN;
                }
                return RedirectToAction(redirectAction, redirectContriller, new
                {
                    redirecData = redirecData,
                    EntArticle = entArticle,
                    EntBrandNic = entBrandNic,
                    EntGuid = entGuid,
                    EntArticleDescription = entArticleDescription,
                    EntArticleDescriptionId = entArticleDescriptionId,
                    ExternArticle = externArticle,
                    ExternBrandNic = externBrandNic,
                    ExternArticleEAN = externArticleEAN
                });
            }
            return RedirectToAction(redirectAction, redirectContriller, new
            {
                redirecData
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
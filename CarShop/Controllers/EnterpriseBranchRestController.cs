// CarShop.Controllers.EnterpriseBranchRestController
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

    public class EnterpriseBranchRestController : Controller
    {
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

        protected SelectList SelectListHelper(int showIs)
        {
            return new SelectList(new SelectListItem[2]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.ArticleSearchByBranch,
                Selected = (showIs == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.ArticleSearchByTecDoc,
                Selected = (showIs == 2)
            }
            }, "Value", "Text", showIs);
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
            IQueryable<BranchRestTDES> branchresttdes = from e in dbRest.BranchRestTDES.Include("BranchRestArticleDescriptionTDES")
                                                        where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                        select e;
            if (!string.IsNullOrEmpty(term))
            {
                branchresttdes = ((currentFilterBy != 1) ? (from e in branchresttdes
                                                            where e.ART_ARTICLE_NR.StartsWith(term)
                                                            select e) : branchresttdes.Where((BranchRestTDES e) => e.EntBranchArticle.StartsWith(term)));
            }
            if (currentFilterBy == 1)
            {
                var ret2 = (from e in branchresttdes
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
                return Json(await ret2.ToListAsync(), JsonRequestBehavior.AllowGet);
            }
            var ret = (from e in branchresttdes
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
            return Json(await ret.ToListAsync(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page)
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
            IPagedList<BranchRestTDES> aResult = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopRestContextCatalog != null)
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
                IQueryable<BranchRestTDES> branchresttdes = (from e in dbRest.BranchRestTDES
                                                             where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                             orderby e.EntBranchGuid, e.EntBranchArticle, e.EntBranchSup
                                                             select e).Include("BranchRestArticleDescriptionTDES");
                if (!string.IsNullOrEmpty(searchString))
                {
                    branchresttdes = ((searchStringBy != 1) ? (from e in dbRest.BranchRestTDES
                                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.ART_ARTICLE_NR.StartsWith(searchString)
                                                               orderby e.EntBranchGuid, e.EntBranchArticle, e.EntBranchSup
                                                               select e) : dbRest.BranchRestTDES.Where((BranchRestTDES e) => (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntBranchArticle.StartsWith(searchString)).OrderBy((BranchRestTDES e) => e.EntBranchGuid).ThenBy((BranchRestTDES e) => e.EntBranchArticle)
                        .ThenBy((BranchRestTDES e) => e.EntBranchSup));
                    branchresttdes = branchresttdes.Include("BranchRestArticleDescriptionTDES");
                }
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await branchresttdes.ToPagedListAsync(pageNumber, pageSize);
            }
            else
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.currentFilterBy = searchStringBy;
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(string EntBranchArticle = null, Guid? searchEntBranchGuid = default(Guid?), string searchEntBranchSup = null)
        {
            BranchRestTDES branchresttdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
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
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchTDES
                                                                        where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
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
            await ViewBagHelper(null, searchEntBranchGuid);
            if (CarShopRestContextCatalog != null)
            {
                branchresttdes = await (from e in dbRest.BranchRestTDES
                                        where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntBranchArticle == EntBranchArticle && e.EntBranchSup == searchEntBranchSup
                                        select e).Include("BranchRestArticleDescriptionTDES").FirstOrDefaultAsync();
            }
            if (branchresttdes == null)
            {
                return HttpNotFound();
            }
            return View(branchresttdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Create(Guid? searchEntGuid, Guid? searchEntBranchGuid)
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
            BranchRestTmp branchresttmp = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopRestContextCatalog == null)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            else
            {
                DateTime now = DateTime.Now;
                now = now.AddMilliseconds(-now.Millisecond);
                branchresttmp = new BranchRestTmp
                {
                    EntBranchGuid = searchEntBranchGuid.Value,
                    LastReplicated = now,
                    LastUpdated = now
                };
            }
            return View(branchresttmp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Create(BranchRestTmp branchresttmp, string ART_ARTICLE_NRLookUp, string EnterpriseArticleLookUp, string EnterpriseBrandLookUp)
        {
            if (EnterpriseBrandLookUp != null)
            {
                string branchrest = JsonConvert.SerializeObject(branchresttmp);
                string EntBranchSup = branchresttmp.EntBranchSup;
                await ViewBagHelper(null, branchresttmp.EntBranchGuid);
                return RedirectToAction("LookUpEnterpriseBrand", "EnterpriseBrand", new
                {
                    redirecData = branchrest,
                    redirectContriller = "EnterpriseBranchRest",
                    redirectAction = "GetEnterpriseBrandForCreate",
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchString = EntBranchSup,
                    searchStringBy = 1
                });
            }
            if (EnterpriseArticleLookUp != null)
            {
                string branchrest2 = JsonConvert.SerializeObject(branchresttmp);
                string artId = branchresttmp.EntBranchArticle;
                await ViewBagHelper(null, branchresttmp.EntBranchGuid);
                return RedirectToAction("LookUpArticleByID", "EnterpriseArticle", new
                {
                    redirecData = branchrest2,
                    redirectContriller = "EnterpriseBranchRest",
                    redirectAction = "GetEnterpriseArticleForCreate",
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchString = artId,
                    searchStringBy = 1
                });
            }
            if (ART_ARTICLE_NRLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(branchresttmp);
                string aRT_ARTICLE_NR = branchresttmp.ART_ARTICLE_NR;
                return RedirectToAction("LookUpArticleByID", "TecDoc", new
                {
                    redirecData = redirecData,
                    redirectContriller = "EnterpriseBranchRest",
                    redirectAction = "GetArticleTDForCreate",
                    artId = aRT_ARTICLE_NR,
                    srchTp = 0
                });
            }
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = branchresttmp.EntBranchGuid;
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
                    EnterpriseBranchIdsTDES searchEntBranchIds2 = await (from e in db.EnterpriseBranchTDES
                                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                         select new EnterpriseBranchIdsTDES
                                                                         {
                                                                             EntGuid = e.EntGuid,
                                                                             EntBranchGuid = e.EntBranchGuid
                                                                         }).FirstOrDefaultAsync();
                    if (searchEntBranchIds2 == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds2.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds2.EntBranchGuid;
                    }
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds2 = await (from e in db.EnterpriseBranchUserTDES
                                                                         where e.EntUserNic == aName
                                                                         select new EnterpriseBranchIdsTDES
                                                                         {
                                                                             EntGuid = e.EntGuid,
                                                                             EntBranchGuid = e.EntBranchGuid
                                                                         }).FirstOrDefaultAsync();
                    if (searchEntBranchIds2 == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds2.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds2.EntBranchGuid;
                    }
                }
            }
            if (base.ModelState.IsValid)
            {
                if (!(searchEntBranchGuid != branchresttmp.EntBranchGuid))
                {
                    await ViewBagHelper(null, searchEntBranchGuid);
                }
                else
                {
                    base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                }
            }
            if (base.ModelState.IsValid && CarShopRestContextCatalog != null)
            {
                BranchRestTDES branchresttdes = new BranchRestTDES();
                branchresttmp.CopyTo(branchresttdes, doCreateDescr: false);
                BranchRestArticleDescriptionTDES desr = await (from e in dbRest.BranchRestArticleDescriptionTDES
                                                               where e.EntArticleDescription == branchresttmp.EntArticleDescription
                                                               select e).FirstOrDefaultAsync();
                if (desr == null)
                {
                    desr = new BranchRestArticleDescriptionTDES
                    {
                        EntArticleDescription = branchresttmp.EntArticleDescription
                    };
                    dbRest.BranchRestArticleDescriptionTDES.Add(desr);
                    await dbRest.SaveChangesAsync();
                }
                branchresttdes.EntArticleDescriptionId = desr.EntArticleDescriptionId;
                dbRest.BranchRestTDES.Add(branchresttdes);
                await dbRest.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchEntBranchGuid = branchresttdes.EntBranchGuid
                });
            }
            return View(branchresttmp);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Edit(string EntBranchArticle = null, Guid? searchEntBranchGuid = default(Guid?), string searchEntBranchSup = null)
        {
            BranchRestTDES branchresttdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
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
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchTDES
                                                                        where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
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
            await ViewBagHelper(null, searchEntBranchGuid);
            if (CarShopRestContextCatalog != null)
            {
                branchresttdes = await (from e in dbRest.BranchRestTDES
                                        where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntBranchArticle == EntBranchArticle && e.EntBranchSup == searchEntBranchSup
                                        orderby e.EntBranchGuid, e.EntBranchArticle, e.EntBranchSup
                                        select e).Include("BranchRestArticleDescriptionTDES").FirstOrDefaultAsync();
            }
            if (branchresttdes == null)
            {
                return HttpNotFound();
            }
            BranchRestTmp branchresttmp = new BranchRestTmp();
            branchresttmp.CopyFrom(branchresttdes);
            return View(branchresttmp);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BranchRestTmp branchresttmp, string ART_ARTICLE_NRLookUp)
        {
            if (ART_ARTICLE_NRLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(branchresttmp);
                string aRT_ARTICLE_NR = branchresttmp.ART_ARTICLE_NR;
                return RedirectToAction("LookUpArticleByID", "TecDoc", new
                {
                    redirecData = redirecData,
                    redirectContriller = "EnterpriseBranchRest",
                    redirectAction = "GetArticleTDForUpdate",
                    artId = aRT_ARTICLE_NR,
                    srchTp = 0
                });
            }
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = branchresttmp.EntBranchGuid;
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
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchTDES
                                                                        where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
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
            if (base.ModelState.IsValid && searchEntBranchGuid != branchresttmp.EntBranchGuid)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                await ViewBagHelper(null, branchresttmp.EntBranchGuid);
                BranchRestTDES branchresttdes = new BranchRestTDES();
                branchresttmp.CopyTo(branchresttdes, doCreateDescr: false);
                BranchRestArticleDescriptionTDES desr = await (from e in dbRest.BranchRestArticleDescriptionTDES
                                                               where e.EntArticleDescription == branchresttmp.EntArticleDescription
                                                               select e).FirstOrDefaultAsync();
                if (desr == null)
                {
                    desr = new BranchRestArticleDescriptionTDES
                    {
                        EntArticleDescription = branchresttmp.EntArticleDescription
                    };
                    dbRest.BranchRestArticleDescriptionTDES.Add(desr);
                    await dbRest.SaveChangesAsync();
                }
                branchresttdes.EntArticleDescriptionId = desr.EntArticleDescriptionId;
                dbRest.Entry(branchresttdes).State = EntityState.Modified;
                await dbRest.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchEntBranchGuid = branchresttdes.EntBranchGuid,
                    searchString = branchresttdes.EntBranchArticle,
                    searchStringBy = 1
                });
            }
            await ViewBagHelper(null, branchresttmp.EntBranchGuid);
            return View(branchresttmp);
        }

        protected async Task<ActionResult> GetArticleTDForCreateUpdate(string redirecData, string ART_ARTICLE_NR, string SUP_TEXT, string MASTER_BEZ, string EAN, string actionName)
        {
            BranchRestTmp branchresttdes = JsonConvert.DeserializeObject<BranchRestTmp>(redirecData);
            if (ART_ARTICLE_NR != null)
            {
                branchresttdes.ART_ARTICLE_NR = ART_ARTICLE_NR;
            }
            if (SUP_TEXT != null)
            {
                branchresttdes.SUP_TEXT = SUP_TEXT;
            }
            if (MASTER_BEZ != null)
            {
                branchresttdes.EntArticleDescription = MASTER_BEZ;
            }
            if (EAN != null)
            {
                branchresttdes.ExternArticleEAN = EAN;
            }
            await ViewBagHelper(null, branchresttdes.EntBranchGuid);
            return View(actionName, branchresttdes);
        }

        public async Task<ActionResult> GetArticleTDForCreate(string redirecData, string ART_ARTICLE_NR, string SUP_TEXT, string MASTER_BEZ, string EAN)
        {
            return await GetArticleTDForCreateUpdate(redirecData, ART_ARTICLE_NR, SUP_TEXT, MASTER_BEZ, EAN, "Create");
        }

        public async Task<ActionResult> GetArticleTDForUpdate(string redirecData, string ART_ARTICLE_NR, string SUP_TEXT, string MASTER_BEZ, string EAN)
        {
            return await GetArticleTDForCreateUpdate(redirecData, ART_ARTICLE_NR, SUP_TEXT, MASTER_BEZ, EAN, "Edit");
        }

        protected async Task<ActionResult> GetEnterpriseArticleForCreateUpdate(string redirecData, string EntArticle, string EntBrandNic, string EntGuid, string EntArticleDescription, string ExternArticle, string ExternBrandNic, string ExternArticleEAN, string actionName)
        {
            BranchRestTmp branchresttdes = JsonConvert.DeserializeObject<BranchRestTmp>(redirecData);
            if (EntArticle != null)
            {
                branchresttdes.EntBranchArticle = EntArticle;
            }
            if (EntBrandNic != null)
            {
                branchresttdes.EntBranchSup = EntBrandNic;
            }
            if (ExternArticle != null)
            {
                branchresttdes.ART_ARTICLE_NR = ExternArticle;
            }
            if (ExternBrandNic != null)
            {
                branchresttdes.SUP_TEXT = ExternBrandNic;
            }
            if (EntArticleDescription != null)
            {
                branchresttdes.EntArticleDescription = EntArticleDescription;
            }
            if (ExternArticleEAN != null)
            {
                branchresttdes.ExternArticleEAN = ExternArticleEAN;
            }
            await ViewBagHelper(null, branchresttdes.EntBranchGuid);
            return View(actionName, branchresttdes);
        }

        public async Task<ActionResult> GetEnterpriseArticleForCreate(string redirecData, string EntArticle, string EntBrandNic, string EntGuid, string EntArticleDescription, string ExternArticle, string ExternBrandNic, string ExternArticleEAN)
        {
            return await GetEnterpriseArticleForCreateUpdate(redirecData, EntArticle, EntBrandNic, EntGuid, EntArticleDescription, ExternArticle, ExternBrandNic, ExternArticleEAN, "Create");
        }

        protected async Task<ActionResult> GetEnterpriseBrandForCreateUpdate(string redirecData, string EntBrandNic, string EntBrandDescription, string actionName)
        {
            BranchRestTmp branchresttdes = JsonConvert.DeserializeObject<BranchRestTmp>(redirecData);
            if (EntBrandNic != null)
            {
                branchresttdes.EntBranchSup = EntBrandNic;
            }
            await ViewBagHelper(null, branchresttdes.EntBranchGuid);
            return View(actionName, branchresttdes);
        }

        public async Task<ActionResult> GetEnterpriseBrandForCreate(string redirecData, string EntBrandNic, string EntBrandDescription)
        {
            return await GetEnterpriseBrandForCreateUpdate(redirecData, EntBrandNic, EntBrandDescription, "Create");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Delete(string EntBranchArticle = null, Guid? searchEntBranchGuid = default(Guid?), string searchEntBranchSup = null)
        {
            BranchRestTDES branchresttdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
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
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchTDES
                                                                        where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
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
            await ViewBagHelper(null, searchEntBranchGuid);
            if (CarShopRestContextCatalog != null)
            {
                branchresttdes = await (from e in dbRest.BranchRestTDES
                                        where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntBranchArticle == EntBranchArticle && e.EntBranchSup == searchEntBranchSup
                                        orderby e.EntBranchGuid, e.EntBranchArticle, e.EntBranchSup
                                        select e).Include("BranchRestArticleDescriptionTDES").FirstOrDefaultAsync();
            }
            if (branchresttdes == null)
            {
                return HttpNotFound();
            }
            return View(branchresttdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(string EntBranchArticle = null, Guid? EntBranchGuid = default(Guid?), string EntBranchSup = null)
        {
            BranchRestTDES branchresttdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
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
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchTDES
                                                                        where (Guid)(Guid?)e.EntBranchGuid == (Guid)EntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        EntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        EntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
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
                        EntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        EntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(null, EntBranchGuid);
            if (CarShopRestContextCatalog != null)
            {
                branchresttdes = await (from e in dbRest.BranchRestTDES
                                        where (Guid)(Guid?)e.EntBranchGuid == (Guid)EntBranchGuid && e.EntBranchArticle == EntBranchArticle && e.EntBranchSup == EntBranchSup
                                        orderby e.EntBranchGuid, e.EntBranchArticle, e.EntBranchSup
                                        select e).FirstOrDefaultAsync();
            }
            dbRest.BranchRestTDES.Remove(branchresttdes);
            await dbRest.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                searchEntBranchGuid = branchresttdes.EntBranchGuid,
                searchString = branchresttdes.EntBranchArticle,
                searchStringBy = 1
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
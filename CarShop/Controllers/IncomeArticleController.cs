// CarShop.Controllers.IncomeArticleController
using CarShop.Models;
using CarShop.Properties;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class IncomeArticleController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopIncomeContextCatalog;

        private CarShopIncomeContext _dbIncome;

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
                Text = Resources.ArticleSearchByIncomePayRollTDESGuid,
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
        public async Task<ActionResult> Index(Guid? searchEntGuid, Guid? searchEntBranchGuid, Guid? searchIncomePayRollTDESGuid, int? page)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
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
            IncomePayRollTDES incomepayrolltdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                incomepayrolltdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbIncome.IncomePayRollTDES
                                                                                                    where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid
                                                                                                    select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.IncomePayRollTDES
                                                                                                                                                                                                                                         where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                         select e).FirstOrDefaultAsync()) : (await (from e in dbIncome.IncomePayRollTDES
                                                                                                                                                                                                                                                                                    where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                    select e).FirstOrDefaultAsync())));
            }
            IPagedList<IncomeArticleTDES> aResult = null;
            if (incomepayrolltdes == null)
            {
                base.ViewBag.searchIncomePayRollTDESGuid = null;
                base.ViewBag.searchIncomePayRollDescr = null;
                base.ViewBag.searchIncomePayRoll = null;
            }
            else
            {
                page = (page ?? 1);
                IOrderedQueryable<IncomeArticleTDES> q = from e in dbIncome.IncomeArticleTDES
                                                         where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid
                                                         orderby e.SupArticle, e.SupBrandNic
                                                         select e;
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await q.ToPagedListAsync(pageNumber, pageSize);
                base.ViewBag.searchIncomePayRollTDESGuid = incomepayrolltdes.IncomePayRollTDESGuid;
                base.ViewBag.searchIncomePayRollDescr = incomepayrolltdes.Description;
                base.ViewBag.searchIncomePayRoll = incomepayrolltdes;
            }
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(string SupArticle = null, string SupBrandNic = null, Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? searchIncomePayRollTDESGuid = default(Guid?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
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
            IncomeArticleTDES incomearticletdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                incomearticletdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbIncome.IncomeArticleTDES
                                                                                                    where e.SupArticle == SupArticle && e.SupBrandNic == SupBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid
                                                                                                    select e).Include((IncomeArticleTDES e) => e.IncomePayRollTDES).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.IncomeArticleTDES
                                                                                                                                                                                                                                                                                               where e.SupArticle == SupArticle && e.SupBrandNic == SupBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                                                                               select e).Include((IncomeArticleTDES e) => e.IncomePayRollTDES).FirstOrDefaultAsync()) : (await (from e in dbIncome.IncomeArticleTDES
                                                                                                                                                                                                                                                                                                                                                                                                where e.SupArticle == SupArticle && e.SupBrandNic == SupBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                                                                                                                                select e).Include((IncomeArticleTDES e) => e.IncomePayRollTDES).FirstOrDefaultAsync())));
            }
            if (incomearticletdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.searchIncomePayRollTDESGuid = incomearticletdes.IncomePayRollTDESGuid;
            base.ViewBag.searchIncomePayRollDescr = incomearticletdes.IncomePayRollTDES.Description;
            return View(incomearticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Create(Guid? searchEntGuid, Guid? searchEntBranchGuid, Guid? searchIncomePayRollTDESGuid)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
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
            IncomePayRollTDES incomepayrolltdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                incomepayrolltdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbIncome.IncomePayRollTDES
                                                                                                    where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid
                                                                                                    select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.IncomePayRollTDES
                                                                                                                                                                                                                                         where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                         select e).FirstOrDefaultAsync()) : (await (from e in dbIncome.IncomePayRollTDES
                                                                                                                                                                                                                                                                                    where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                    select e).FirstOrDefaultAsync())));
            }
            IncomeArticleTDES incomearticletdes = null;
            if (incomepayrolltdes == null)
            {
                base.ViewBag.searchIncomePayRollTDESGuid = null;
                base.ViewBag.searchIncomePayRollDescr = null;
            }
            else
            {
                incomearticletdes = new IncomeArticleTDES
                {
                    IncomePayRollTDESGuid = incomepayrolltdes.IncomePayRollTDESGuid,
                    EntBranchGuid = incomepayrolltdes.EntBranchGuid,
                    EntGuid = incomepayrolltdes.EntGuid
                };
                base.ViewBag.searchIncomePayRollTDESGuid = incomepayrolltdes.IncomePayRollTDESGuid;
                base.ViewBag.searchIncomePayRollDescr = incomepayrolltdes.Description;
            }
            return View(incomearticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(IncomeArticleTDES incomearticletdes, string EnterpriseArticleLookUp, string EnterpriseBrandLookUp)
        {
            if (EnterpriseBrandLookUp != null)
            {
                string branchrest = JsonConvert.SerializeObject(incomearticletdes);
                string EntBrandNic = incomearticletdes.EntBrandNic;
                await ViewBagHelper(incomearticletdes.EntGuid, incomearticletdes.EntBranchGuid);
                return RedirectToAction("LookUpEnterpriseBrand", "EnterpriseBrand", new
                {
                    redirecData = branchrest,
                    redirectContriller = "IncomeArticle",
                    redirectAction = "GetEnterpriseBrandForCreate",
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchString = EntBrandNic,
                    searchStringBy = 1
                });
            }
            if (EnterpriseArticleLookUp != null)
            {
                string branchrest2 = JsonConvert.SerializeObject(incomearticletdes);
                string artId = incomearticletdes.EntArticle;
                await ViewBagHelper(incomearticletdes.EntGuid, incomearticletdes.EntBranchGuid);
                return RedirectToAction("LookUpArticleByID", "EnterpriseArticle", new
                {
                    redirecData = branchrest2,
                    redirectContriller = "IncomeArticle",
                    redirectAction = "GetEnterpriseArticleForCreate",
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchString = artId,
                    searchStringBy = 1
                });
            }
            UserIsInRoles();
            Guid? searchEntBranchGuid = null;
            Guid? searchEntGuid;
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
            else
            {
                searchEntGuid = incomearticletdes.EntGuid;
                searchEntBranchGuid = incomearticletdes.EntBranchGuid;
            }
            Guid? searchIncomePayRollTDESGuid = incomearticletdes.IncomePayRollTDESGuid;
            if (base.ModelState.IsValid && (searchEntGuid != incomearticletdes.EntGuid || searchEntBranchGuid != incomearticletdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            IncomePayRollTDES incomepayrolltdes = null;
            if (CarShopIncomeContextCatalog != null)
            {
                incomepayrolltdes = await (from e in dbIncome.IncomePayRollTDES
                                           where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid
                                           select e).FirstOrDefaultAsync();
                if (incomepayrolltdes != null)
                {
                    base.ViewBag.searchIncomePayRollTDESGuid = incomepayrolltdes.IncomePayRollTDESGuid;
                    base.ViewBag.searchIncomePayRollDescr = incomepayrolltdes.Description;
                }
                else
                {
                    base.ModelState.AddModelError("", Resources.IncomePayRollTDES_NOTFOUND);
                }
            }
            if (base.ModelState.IsValid && incomepayrolltdes != null && incomepayrolltdes.IsProcessed)
            {
                base.ModelState.AddModelError("", Resources.IncomePayRollTDES_ISPOCESSES);
            }
            if (base.ModelState.IsValid)
            {
                incomearticletdes.ArtAmountRest = incomearticletdes.ArtAmount;
                incomearticletdes.CurrArtPrice = incomearticletdes.ArtPrice;
                dbIncome.IncomeArticleTDES.Add(incomearticletdes);
                await dbIncome.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = incomearticletdes.EntGuid,
                    searchEntBranchGuid = incomearticletdes.EntBranchGuid,
                    searchIncomePayRollTDESGuid = incomearticletdes.IncomePayRollTDESGuid
                });
            }
            return View(incomearticletdes);
        }

        protected async Task<ActionResult> GetEnterpriseArticleForCreateUpdate(string redirecData, string EntArticle, string EntBrandNic, string EntGuid, string EntArticleDescription, string ExternArticle, string ExternBrandNic, string ExternArticleEAN, string actionName)
        {
            IncomeArticleTDES incomearticletdes = JsonConvert.DeserializeObject<IncomeArticleTDES>(redirecData);
            if (EntArticle != null)
            {
                incomearticletdes.EntArticle = EntArticle;
            }
            if (EntBrandNic != null)
            {
                incomearticletdes.EntBrandNic = EntBrandNic;
            }
            if (EntArticleDescription != null)
            {
                incomearticletdes.EntArticleDescription = EntArticleDescription;
            }
            await ViewBagHelper(incomearticletdes.EntGuid, incomearticletdes.EntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                IncomePayRollTDES incomepayrolltdes = await (from e in dbIncome.IncomePayRollTDES
                                                             where e.IncomePayRollTDESGuid == incomearticletdes.IncomePayRollTDESGuid
                                                             select e).FirstOrDefaultAsync();
                if (incomepayrolltdes != null)
                {
                    base.ViewBag.searchIncomePayRollTDESGuid = incomepayrolltdes.IncomePayRollTDESGuid;
                    base.ViewBag.searchIncomePayRollDescr = incomepayrolltdes.Description;
                }
                else
                {
                    base.ModelState.AddModelError("", Resources.IncomePayRollTDES_NOTFOUND);
                }
            }
            return View(actionName, incomearticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> GetEnterpriseArticleForCreate(string redirecData, string EntArticle, string EntBrandNic, string EntGuid, string EntArticleDescription, string ExternArticle, string ExternBrandNic, string ExternArticleEAN)
        {
            return await GetEnterpriseArticleForCreateUpdate(redirecData, EntArticle, EntBrandNic, EntGuid, EntArticleDescription, ExternArticle, ExternBrandNic, ExternArticleEAN, "Create");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> GetEnterpriseArticleForUpdate(string redirecData, string EntArticle, string EntBrandNic, string EntGuid, string EntArticleDescription, string ExternArticle, string ExternBrandNic, string ExternArticleEAN)
        {
            return await GetEnterpriseArticleForCreateUpdate(redirecData, EntArticle, EntBrandNic, EntGuid, EntArticleDescription, ExternArticle, ExternBrandNic, ExternArticleEAN, "Edit");
        }

        protected async Task<ActionResult> GetEnterpriseBrandForCreateUpdate(string redirecData, string EntBrandNic, string EntBrandDescription, string actionName)
        {
            IncomeArticleTDES incomearticletdes = JsonConvert.DeserializeObject<IncomeArticleTDES>(redirecData);
            if (EntBrandNic != null)
            {
                incomearticletdes.EntBrandNic = EntBrandNic;
            }
            await ViewBagHelper(incomearticletdes.EntGuid, incomearticletdes.EntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                IncomePayRollTDES incomepayrolltdes = await (from e in dbIncome.IncomePayRollTDES
                                                             where e.IncomePayRollTDESGuid == incomearticletdes.IncomePayRollTDESGuid
                                                             select e).FirstOrDefaultAsync();
                if (incomepayrolltdes != null)
                {
                    base.ViewBag.searchIncomePayRollTDESGuid = incomepayrolltdes.IncomePayRollTDESGuid;
                    base.ViewBag.searchIncomePayRollDescr = incomepayrolltdes.Description;
                }
                else
                {
                    base.ModelState.AddModelError("", Resources.IncomePayRollTDES_NOTFOUND);
                }
            }
            return View(actionName, incomearticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> GetEnterpriseBrandForCreate(string redirecData, string EntBrandNic, string EntBrandDescription)
        {
            return await GetEnterpriseBrandForCreateUpdate(redirecData, EntBrandNic, EntBrandDescription, "Create");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> GetEnterpriseBrandForUpdate(string redirecData, string EntBrandNic, string EntBrandDescription)
        {
            return await GetEnterpriseBrandForCreateUpdate(redirecData, EntBrandNic, EntBrandDescription, "Edit");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Edit(string SupArticle = null, string SupBrandNic = null, Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? searchIncomePayRollTDESGuid = default(Guid?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
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
            IncomeArticleTDES incomearticletdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                incomearticletdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbIncome.IncomeArticleTDES
                                                                                                    where e.SupArticle == SupArticle && e.SupBrandNic == SupBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid
                                                                                                    select e).Include((IncomeArticleTDES e) => e.IncomePayRollTDES).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.IncomeArticleTDES
                                                                                                                                                                                                                                                                                               where e.SupArticle == SupArticle && e.SupBrandNic == SupBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                                                                               select e).Include((IncomeArticleTDES e) => e.IncomePayRollTDES).FirstOrDefaultAsync()) : (await (from e in dbIncome.IncomeArticleTDES
                                                                                                                                                                                                                                                                                                                                                                                                where e.SupArticle == SupArticle && e.SupBrandNic == SupBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                                                                                                                                select e).Include((IncomeArticleTDES e) => e.IncomePayRollTDES).FirstOrDefaultAsync())));
            }
            if (incomearticletdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.searchIncomePayRollTDESGuid = incomearticletdes.IncomePayRollTDESGuid;
            base.ViewBag.searchIncomePayRollDescr = incomearticletdes.IncomePayRollTDES.Description;
            if (incomearticletdes == null)
            {
                return HttpNotFound();
            }
            return View(incomearticletdes);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IncomeArticleTDES incomearticletdes, string EnterpriseArticleLookUp, string EnterpriseBrandLookUp)
        {
            if (EnterpriseBrandLookUp != null)
            {
                string branchrest = JsonConvert.SerializeObject(incomearticletdes);
                string EntBrandNic = incomearticletdes.EntBrandNic;
                await ViewBagHelper(incomearticletdes.EntGuid, incomearticletdes.EntBranchGuid);
                return RedirectToAction("LookUpEnterpriseBrand", "EnterpriseBrand", new
                {
                    redirecData = branchrest,
                    redirectContriller = "IncomeArticle",
                    redirectAction = "GetEnterpriseBrandForUpdate",
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchString = EntBrandNic,
                    searchStringBy = 1
                });
            }
            if (EnterpriseArticleLookUp != null)
            {
                string branchrest2 = JsonConvert.SerializeObject(incomearticletdes);
                string artId = incomearticletdes.EntArticle;
                await ViewBagHelper(incomearticletdes.EntGuid, incomearticletdes.EntBranchGuid);
                return RedirectToAction("LookUpArticleByID", "EnterpriseArticle", new
                {
                    redirecData = branchrest2,
                    redirectContriller = "IncomeArticle",
                    redirectAction = "GetEnterpriseArticleForUpdate",
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchString = artId,
                    searchStringBy = 1
                });
            }
            UserIsInRoles();
            Guid? searchEntBranchGuid = null;
            Guid? searchEntGuid;
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
            else
            {
                searchEntGuid = incomearticletdes.EntGuid;
                searchEntBranchGuid = incomearticletdes.EntBranchGuid;
            }
            Guid? searchIncomePayRollTDESGuid = incomearticletdes.IncomePayRollTDESGuid;
            if (base.ModelState.IsValid && (searchEntGuid != incomearticletdes.EntGuid || searchEntBranchGuid != incomearticletdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            IncomePayRollTDES incomepayrolltdes = null;
            if (CarShopIncomeContextCatalog != null)
            {
                incomepayrolltdes = await (from e in dbIncome.IncomePayRollTDES
                                           where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid
                                           select e).FirstOrDefaultAsync();
                if (incomepayrolltdes != null)
                {
                    base.ViewBag.searchIncomePayRollTDESGuid = incomepayrolltdes.IncomePayRollTDESGuid;
                    base.ViewBag.searchIncomePayRollDescr = incomepayrolltdes.Description;
                }
                else
                {
                    base.ModelState.AddModelError("", Resources.IncomePayRollTDES_NOTFOUND);
                }
            }
            if (base.ModelState.IsValid && incomepayrolltdes != null && incomepayrolltdes.IsProcessed)
            {
                base.ModelState.AddModelError("", Resources.IncomePayRollTDES_ISPOCESSES);
            }
            if (base.ModelState.IsValid)
            {
                dbIncome.Entry(incomearticletdes).State = EntityState.Modified;
                incomearticletdes.ArtAmountRest = incomearticletdes.ArtAmount;
                incomearticletdes.CurrArtPrice = incomearticletdes.ArtPrice;
                await dbIncome.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = incomearticletdes.EntGuid,
                    searchEntBranchGuid = incomearticletdes.EntBranchGuid,
                    searchIncomePayRollTDESGuid = incomearticletdes.IncomePayRollTDESGuid
                });
            }
            return View(incomearticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Delete(string SupArticle = null, string SupBrandNic = null, Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? searchIncomePayRollTDESGuid = default(Guid?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
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
            IncomeArticleTDES incomearticletdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                incomearticletdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbIncome.IncomeArticleTDES
                                                                                                    where e.SupArticle == SupArticle && e.SupBrandNic == SupBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid
                                                                                                    select e).Include((IncomeArticleTDES e) => e.IncomePayRollTDES).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.IncomeArticleTDES
                                                                                                                                                                                                                                                                                               where e.SupArticle == SupArticle && e.SupBrandNic == SupBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                                                                               select e).Include((IncomeArticleTDES e) => e.IncomePayRollTDES).FirstOrDefaultAsync()) : (await (from e in dbIncome.IncomeArticleTDES
                                                                                                                                                                                                                                                                                                                                                                                                where e.SupArticle == SupArticle && e.SupBrandNic == SupBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                                                                                                                                select e).Include((IncomeArticleTDES e) => e.IncomePayRollTDES).FirstOrDefaultAsync())));
            }
            if (incomearticletdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.searchIncomePayRollTDESGuid = incomearticletdes.IncomePayRollTDESGuid;
            base.ViewBag.searchIncomePayRollDescr = incomearticletdes.IncomePayRollTDES.Description;
            return View(incomearticletdes);
        }

        [ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> DeleteConfirmed(string SupArticle = null, string SupBrandNic = null, Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? searchIncomePayRollTDESGuid = default(Guid?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
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
            IncomeArticleTDES incomearticletdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                incomearticletdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbIncome.IncomeArticleTDES
                                                                                                    where e.SupArticle == SupArticle && e.SupBrandNic == SupBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid
                                                                                                    select e).Include((IncomeArticleTDES e) => e.IncomePayRollTDES).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.IncomeArticleTDES
                                                                                                                                                                                                                                                                                               where e.SupArticle == SupArticle && e.SupBrandNic == SupBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                                                                               select e).Include((IncomeArticleTDES e) => e.IncomePayRollTDES).FirstOrDefaultAsync()) : (await (from e in dbIncome.IncomeArticleTDES
                                                                                                                                                                                                                                                                                                                                                                                                where e.SupArticle == SupArticle && e.SupBrandNic == SupBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                                                                                                                                select e).Include((IncomeArticleTDES e) => e.IncomePayRollTDES).FirstOrDefaultAsync())));
            }
            if (incomearticletdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.searchIncomePayRollTDESGuid = incomearticletdes.IncomePayRollTDESGuid;
            base.ViewBag.searchIncomePayRollDescr = incomearticletdes.IncomePayRollTDES.Description;
            if (base.ModelState.IsValid && incomearticletdes.IsProcessed)
            {
                base.ModelState.AddModelError("", Resources.IncomePayRollTDES_ISPOCESSES);
                return View(incomearticletdes);
            }
            dbIncome.IncomeArticleTDES.Remove(incomearticletdes);
            await dbIncome.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid = incomearticletdes.EntGuid,
                searchEntBranchGuid = incomearticletdes.EntBranchGuid,
                searchIncomePayRollTDESGuid = incomearticletdes.IncomePayRollTDESGuid
            });
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> LookUpIncomeArticle(string redirecData, string redirectContriller, string redirectAction, Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchString = null, int? searchStringBy = default(int?), string currentFilter = null, int? currentFilterBy = default(int?), int? page = default(int?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
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
            base.ViewBag.RedirecData = redirecData;
            base.ViewBag.RedirectContriller = redirectContriller;
            base.ViewBag.RedirectAction = redirectAction;
            IPagedList<IncomeArticleTDES> aResult = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            IOrderedQueryable<IncomeArticleTDES> incomearticletdes;
            if (CarShopIncomeContextCatalog != null)
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
                incomearticletdes = from e in dbIncome.IncomeArticleTDES
                                    where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.IsProcessed == true && e.IsReversed == false && e.ArtAmountRest > 0
                                    orderby e.EntArticle, e.EntBrandNic
                                    select e;
                if (!string.IsNullOrEmpty(searchString))
                {
                    int? num = searchStringBy;
                    int valueOrDefault = num.GetValueOrDefault();
                    if (!num.HasValue)
                    {
                        goto IL_10f3;
                    }
                    switch (valueOrDefault)
                    {
                        case 2:
                            break;
                        case 3:
                            goto IL_0e86;
                        default:
                            goto IL_10f3;
                    }
                    Guid aFilter = Guid.Empty;
                    if (!Guid.TryParse(searchString, out aFilter))
                    {
                        aFilter = Guid.Empty;
                    }
                    incomearticletdes = from e in dbIncome.IncomeArticleTDES
                                        where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.IsProcessed == true && e.IsReversed == false && e.ArtAmountRest > 0 && e.IncomePayRollTDESGuid == aFilter
                                        orderby e.EntArticle, e.EntBrandNic
                                        select e;
                }
                goto IL_1368;
            }
            goto IL_1480;
        IL_0e86:
            incomearticletdes = from e in dbIncome.IncomeArticleTDES
                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.IsProcessed == true && e.IsReversed == false && e.ArtAmountRest > 0 && e.EntBrandNic == searchString
                                orderby e.EntArticle, e.EntBrandNic
                                select e;
            goto IL_1368;
        IL_10f3:
            incomearticletdes = from e in dbIncome.IncomeArticleTDES
                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.IsProcessed == true && e.IsReversed == false && e.ArtAmountRest > 0 && e.EntArticle.StartsWith(searchString)
                                orderby e.EntArticle, e.EntBrandNic
                                select e;
            goto IL_1368;
        IL_1480:
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.CurrentFilterBy = searchStringBy;
            return View(aResult);
        IL_1368:
            int pageSize = 20;
            int pageNumber = page.Value;
            aResult = await incomearticletdes.ToPagedListAsync(pageNumber, pageSize);
            base.ViewBag.sliSearchBy = SelectListHelper(searchStringBy.Value);
            goto IL_1480;
        }

        public ActionResult LookUpIncomeArticleSelected(string redirecData, string redirectContriller, string redirectAction, string SimpleArticle, string DOSELECT_TITLE, string DOCANCEL_TITLE)
        {
            if (DOSELECT_TITLE != null)
            {
                string entArticle = null;
                string entBrandNic = null;
                string entArticleDescription = null;
                double currArtPrice = 0.0;
                int artAmountRest = 0;
                Guid? incomePayRollTDESGuid = null;
                Guid? entBranchGuid = null;
                Guid? entGuid = null;
                if (SimpleArticle != null)
                {
                    IncomeArticleTDES incomeArticleTDES = JsonConvert.DeserializeObject<IncomeArticleTDES>(SimpleArticle);
                    entArticle = incomeArticleTDES.EntArticle;
                    entBrandNic = incomeArticleTDES.EntBrandNic;
                    entArticleDescription = incomeArticleTDES.EntArticleDescription;
                    currArtPrice = incomeArticleTDES.CurrArtPrice;
                    artAmountRest = incomeArticleTDES.ArtAmountRest;
                    entGuid = incomeArticleTDES.EntGuid;
                    entBranchGuid = incomeArticleTDES.EntBranchGuid;
                    incomePayRollTDESGuid = incomeArticleTDES.IncomePayRollTDESGuid;
                }
                return RedirectToAction(redirectAction, redirectContriller, new
                {
                    redirecData = redirecData,
                    EntArticle = entArticle,
                    EntBrandNic = entBrandNic,
                    EntArticleDescription = entArticleDescription,
                    CurrArtPrice = currArtPrice,
                    ArtAmountRest = artAmountRest,
                    EntGuid = entGuid,
                    EntBranchGuid = entBranchGuid,
                    IncomePayRollTDESGuid = incomePayRollTDESGuid
                });
            }
            return RedirectToAction(redirectAction, redirectContriller, new
            {
                redirecData
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbIncome != null)
            {
                _dbIncome.Dispose();
                _dbIncome = null;
            }
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
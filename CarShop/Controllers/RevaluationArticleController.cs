// CarShop.Controllers.RevaluationArticleController
using CarShop.Models;
using CarShop.Properties;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class RevaluationArticleController : Controller
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

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? searchSheetRevaluationTDESGuid = default(Guid?), int? page = default(int?))
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
            SheetRevaluationTDES sheetrevaluationtdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                sheetrevaluationtdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbIncome.SheetRevaluationTDES
                                                                                                       where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)searchSheetRevaluationTDESGuid
                                                                                                       select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.SheetRevaluationTDES
                                                                                                                                                                                                                                            where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)searchSheetRevaluationTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                            select e).FirstOrDefaultAsync()) : (await (from e in dbIncome.SheetRevaluationTDES
                                                                                                                                                                                                                                                                                       where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)searchSheetRevaluationTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                       select e).FirstOrDefaultAsync())));
            }
            IPagedList<RevaluationArticleTDES> aResult = null;
            if (sheetrevaluationtdes == null)
            {
                base.ViewBag.searchSheetRevaluationTDESGuid = null;
                base.ViewBag.searchSheetRevaluationDescr = null;
                base.ViewBag.searchSheetRevaluation = null;
            }
            else
            {
                page = (page ?? 1);
                IOrderedQueryable<RevaluationArticleTDES> q = from e in dbIncome.RevaluationArticleTDES
                                                              where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)searchSheetRevaluationTDESGuid
                                                              orderby e.EntArticle, e.EntBrandNic
                                                              select e;
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await q.ToPagedListAsync(pageNumber, pageSize);
                base.ViewBag.searchSheetRevaluationTDESGuid = sheetrevaluationtdes.SheetRevaluationTDESGuid;
                base.ViewBag.searchSheetRevaluationDescr = sheetrevaluationtdes.Description;
                base.ViewBag.searchSheetRevaluation = sheetrevaluationtdes;
            }
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(string EntArticle = null, string EntBrandNic = null, Guid? IncomePayRollTDESGuid = default(Guid?), Guid? SheetRevaluationTDESGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
            RevaluationArticleTDES revaluationarticletdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                revaluationarticletdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbIncome.RevaluationArticleTDES
                                                                                                         where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)IncomePayRollTDESGuid && (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)SheetRevaluationTDESGuid
                                                                                                         select e).Include((RevaluationArticleTDES e) => e.SheetRevaluationTDES).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.RevaluationArticleTDES
                                                                                                                                                                                                                                                                                                            where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)IncomePayRollTDESGuid && (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)SheetRevaluationTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                                                                                            select e).Include((RevaluationArticleTDES e) => e.SheetRevaluationTDES).FirstOrDefaultAsync()) : (await (from e in dbIncome.RevaluationArticleTDES
                                                                                                                                                                                                                                                                                                                                                                                                                     where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)IncomePayRollTDESGuid && (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)SheetRevaluationTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                                                                                                                                                     select e).Include((RevaluationArticleTDES e) => e.SheetRevaluationTDES).FirstOrDefaultAsync())));
            }
            if (revaluationarticletdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.searchSheetRevaluationTDESGuid = revaluationarticletdes.SheetRevaluationTDESGuid;
            base.ViewBag.searchSheetRevaluationDescr = revaluationarticletdes.SheetRevaluationTDES.Description;
            return View(revaluationarticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Create(Guid? searchEntGuid, Guid? searchEntBranchGuid, Guid? searchSheetRevaluationTDESGuid)
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
            SheetRevaluationTDES sheetrevaluationtdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                sheetrevaluationtdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbIncome.SheetRevaluationTDES
                                                                                                       where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)searchSheetRevaluationTDESGuid
                                                                                                       select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.SheetRevaluationTDES
                                                                                                                                                                                                                                            where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)searchSheetRevaluationTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                            select e).FirstOrDefaultAsync()) : (await (from e in dbIncome.SheetRevaluationTDES
                                                                                                                                                                                                                                                                                       where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)searchSheetRevaluationTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                       select e).FirstOrDefaultAsync())));
            }
            RevaluationArticleTDES revaluationarticletdes = null;
            if (sheetrevaluationtdes == null)
            {
                base.ViewBag.searchSheetRevaluationTDESGuid = null;
                base.ViewBag.searchSheetRevaluationDescr = null;
            }
            else
            {
                revaluationarticletdes = new RevaluationArticleTDES
                {
                    SheetRevaluationTDESGuid = sheetrevaluationtdes.SheetRevaluationTDESGuid,
                    EntBranchGuid = sheetrevaluationtdes.EntBranchGuid,
                    EntGuid = sheetrevaluationtdes.EntGuid
                };
                base.ViewBag.searchSheetRevaluationTDESGuid = sheetrevaluationtdes.SheetRevaluationTDESGuid;
                base.ViewBag.searchSheetRevaluationDescr = sheetrevaluationtdes.Description;
            }
            return View(revaluationarticletdes);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Create(RevaluationArticleTDES revaluationarticletdes, string IncomeArticleLookUp = null)
        {
            if (IncomeArticleLookUp != null)
            {
                string aData = JsonConvert.SerializeObject(revaluationarticletdes);
                string EntArticle = revaluationarticletdes.EntArticle;
                await ViewBagHelper(revaluationarticletdes.EntGuid, revaluationarticletdes.EntBranchGuid);
                return RedirectToAction("LookUpIncomeArticle", "IncomeArticle", new
                {
                    redirecData = aData,
                    redirectContriller = "RevaluationArticle",
                    redirectAction = "GetIncomeArticleForCreate",
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchEntBranchGuid = (object)base.ViewBag.SearchEntBranchGuid,
                    searchString = EntArticle,
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
                searchEntGuid = revaluationarticletdes.EntGuid;
                searchEntBranchGuid = revaluationarticletdes.EntBranchGuid;
            }
            Guid? searchSheetRevaluationTDESGuid = revaluationarticletdes.SheetRevaluationTDESGuid;
            if (base.ModelState.IsValid && (searchEntGuid != revaluationarticletdes.EntGuid || searchEntBranchGuid != revaluationarticletdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            SheetRevaluationTDES sheetrevaluationtdes = null;
            if (CarShopIncomeContextCatalog != null)
            {
                sheetrevaluationtdes = await (from e in dbIncome.SheetRevaluationTDES
                                              where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)searchSheetRevaluationTDESGuid
                                              select e).FirstOrDefaultAsync();
                if (sheetrevaluationtdes != null)
                {
                    base.ViewBag.searchSheetRevaluationTDESGuid = sheetrevaluationtdes.SheetRevaluationTDESGuid;
                    base.ViewBag.searchSheetRevaluationDescr = sheetrevaluationtdes.Description;
                }
                else
                {
                    base.ModelState.AddModelError("", Resources.IncomePayRollTDES_NOTFOUND);
                }
            }
            if (base.ModelState.IsValid && sheetrevaluationtdes != null && sheetrevaluationtdes.IsProcessed)
            {
                base.ModelState.AddModelError("", Resources.IncomePayRollTDES_ISPOCESSES);
            }
            if (base.ModelState.IsValid)
            {
                revaluationarticletdes.OperSum = (revaluationarticletdes.NewArtPrice - revaluationarticletdes.CurrArtPrice) * (double)revaluationarticletdes.ArtAmountRest;
                dbIncome.RevaluationArticleTDES.Add(revaluationarticletdes);
                try
                {
                    await dbIncome.SaveChangesAsync();
                    return RedirectToAction("Index", new
                    {
                        searchEntGuid = revaluationarticletdes.EntGuid,
                        searchEntBranchGuid = revaluationarticletdes.EntBranchGuid,
                        searchSheetRevaluationTDESGuid = revaluationarticletdes.SheetRevaluationTDESGuid
                    });
                }
                catch (DbUpdateException ex)
                {
                    base.ModelState.AddModelError("", Resources.SqlError + " : " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        base.ModelState.AddModelError("", Resources.SqlError + " : " + ex.InnerException.Message);
                        if (ex.InnerException.InnerException != null)
                        {
                            base.ModelState.AddModelError("", Resources.SqlError + " : " + ex.InnerException.InnerException.Message);
                        }
                    }
                }
                catch (SqlException ex2)
                {
                    if (ex2.Number == 2627)
                    {
                        base.ModelState.AddModelError("", Resources.SqlDublicateValue + " : " + ex2.Message);
                    }
                    else
                    {
                        base.ModelState.AddModelError("", Resources.SqlError + " : " + ex2.Message);
                    }
                }
            }
            return View(revaluationarticletdes);
        }

        public async Task<ActionResult> GetIncomeArticleForCreateUpdate(string redirecData, string EntArticle = null, string EntBrandNic = null, string EntArticleDescription = null, double? CurrArtPrice = default(double?), int? ArtAmountRest = default(int?), Guid? EntGuid = default(Guid?), Guid? EntBranchGuid = default(Guid?), Guid? IncomePayRollTDESGuid = default(Guid?), string actionName = null)
        {
            RevaluationArticleTDES revaluationarticletdes = JsonConvert.DeserializeObject<RevaluationArticleTDES>(redirecData);
            if (EntArticle != null)
            {
                revaluationarticletdes.EntArticle = EntArticle;
            }
            if (EntBrandNic != null)
            {
                revaluationarticletdes.EntBrandNic = EntBrandNic;
            }
            if (EntBrandNic != null)
            {
                revaluationarticletdes.EntBrandNic = EntBrandNic;
            }
            if (EntArticleDescription != null)
            {
                revaluationarticletdes.EntArticleDescription = EntArticleDescription;
            }
            if (CurrArtPrice.HasValue)
            {
                revaluationarticletdes.CurrArtPrice = CurrArtPrice.Value;
            }
            if (ArtAmountRest.HasValue)
            {
                revaluationarticletdes.ArtAmountRest = ArtAmountRest.Value;
            }
            if (IncomePayRollTDESGuid.HasValue)
            {
                revaluationarticletdes.IncomePayRollTDESGuid = IncomePayRollTDESGuid.Value;
            }
            await ViewBagHelper(revaluationarticletdes.EntGuid, revaluationarticletdes.EntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                SheetRevaluationTDES sheetrevaluationtdes = await (from e in dbIncome.SheetRevaluationTDES
                                                                   where e.SheetRevaluationTDESGuid == revaluationarticletdes.SheetRevaluationTDESGuid
                                                                   select e).FirstOrDefaultAsync();
                if (sheetrevaluationtdes != null)
                {
                    base.ViewBag.searchSheetRevaluationTDESGuid = sheetrevaluationtdes.SheetRevaluationTDESGuid;
                    base.ViewBag.searchSheetRevaluationDescr = sheetrevaluationtdes.Description;
                }
                else
                {
                    base.ModelState.AddModelError("", Resources.IncomePayRollTDES_NOTFOUND);
                }
            }
            return View(actionName, revaluationarticletdes);
        }

        public async Task<ActionResult> GetIncomeArticleForCreate(string redirecData, string EntArticle = null, string EntBrandNic = null, string EntArticleDescription = null, double? CurrArtPrice = default(double?), int? ArtAmountRest = default(int?), Guid? EntGuid = default(Guid?), Guid? EntBranchGuid = default(Guid?), Guid? IncomePayRollTDESGuid = default(Guid?))
        {
            return await GetIncomeArticleForCreateUpdate(redirecData, EntArticle, EntBrandNic, EntArticleDescription, CurrArtPrice, ArtAmountRest, EntGuid, EntBranchGuid, IncomePayRollTDESGuid, "Create");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Edit(string EntArticle = null, string EntBrandNic = null, Guid? IncomePayRollTDESGuid = default(Guid?), Guid? SheetRevaluationTDESGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
            RevaluationArticleTDES revaluationarticletdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                revaluationarticletdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbIncome.RevaluationArticleTDES
                                                                                                         where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)IncomePayRollTDESGuid && (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)SheetRevaluationTDESGuid
                                                                                                         select e).Include((RevaluationArticleTDES e) => e.SheetRevaluationTDES).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.RevaluationArticleTDES
                                                                                                                                                                                                                                                                                                            where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)IncomePayRollTDESGuid && (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)SheetRevaluationTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                                                                                            select e).Include((RevaluationArticleTDES e) => e.SheetRevaluationTDES).FirstOrDefaultAsync()) : (await (from e in dbIncome.RevaluationArticleTDES
                                                                                                                                                                                                                                                                                                                                                                                                                     where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)IncomePayRollTDESGuid && (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)SheetRevaluationTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                                                                                                                                                     select e).Include((RevaluationArticleTDES e) => e.SheetRevaluationTDES).FirstOrDefaultAsync())));
            }
            if (revaluationarticletdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.searchSheetRevaluationTDESGuid = revaluationarticletdes.SheetRevaluationTDESGuid;
            base.ViewBag.searchSheetRevaluationDescr = revaluationarticletdes.SheetRevaluationTDES.Description;
            return View(revaluationarticletdes);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [HttpPost]
        public async Task<ActionResult> Edit(RevaluationArticleTDES revaluationarticletdes)
        {
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
                searchEntGuid = revaluationarticletdes.EntGuid;
                searchEntBranchGuid = revaluationarticletdes.EntBranchGuid;
            }
            Guid? searchSheetRevaluationTDESGuid = revaluationarticletdes.SheetRevaluationTDESGuid;
            if (base.ModelState.IsValid && (searchEntGuid != revaluationarticletdes.EntGuid || searchEntBranchGuid != revaluationarticletdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            SheetRevaluationTDES sheetrevaluationtdes = null;
            if (CarShopIncomeContextCatalog != null)
            {
                sheetrevaluationtdes = await (from e in dbIncome.SheetRevaluationTDES
                                              where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)searchSheetRevaluationTDESGuid
                                              select e).FirstOrDefaultAsync();
                if (sheetrevaluationtdes != null)
                {
                    base.ViewBag.searchSheetRevaluationTDESGuid = sheetrevaluationtdes.SheetRevaluationTDESGuid;
                    base.ViewBag.searchSheetRevaluationDescr = sheetrevaluationtdes.Description;
                }
                else
                {
                    base.ModelState.AddModelError("", Resources.IncomePayRollTDES_NOTFOUND);
                }
            }
            if (base.ModelState.IsValid && sheetrevaluationtdes != null && sheetrevaluationtdes.IsProcessed)
            {
                base.ModelState.AddModelError("", Resources.IncomePayRollTDES_ISPOCESSES);
            }
            if (base.ModelState.IsValid)
            {
                revaluationarticletdes.OperSum = (revaluationarticletdes.NewArtPrice - revaluationarticletdes.CurrArtPrice) * (double)revaluationarticletdes.ArtAmountRest;
                dbIncome.Entry(revaluationarticletdes).State = EntityState.Modified;
                await dbIncome.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = revaluationarticletdes.EntGuid,
                    searchEntBranchGuid = revaluationarticletdes.EntBranchGuid,
                    searchSheetRevaluationTDESGuid = revaluationarticletdes.SheetRevaluationTDESGuid
                });
            }
            return View(revaluationarticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Delete(string EntArticle = null, string EntBrandNic = null, Guid? IncomePayRollTDESGuid = default(Guid?), Guid? SheetRevaluationTDESGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
            RevaluationArticleTDES revaluationarticletdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                revaluationarticletdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbIncome.RevaluationArticleTDES
                                                                                                         where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)IncomePayRollTDESGuid && (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)SheetRevaluationTDESGuid
                                                                                                         select e).Include((RevaluationArticleTDES e) => e.SheetRevaluationTDES).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.RevaluationArticleTDES
                                                                                                                                                                                                                                                                                                            where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)IncomePayRollTDESGuid && (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)SheetRevaluationTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                                                                                            select e).Include((RevaluationArticleTDES e) => e.SheetRevaluationTDES).FirstOrDefaultAsync()) : (await (from e in dbIncome.RevaluationArticleTDES
                                                                                                                                                                                                                                                                                                                                                                                                                     where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)IncomePayRollTDESGuid && (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)SheetRevaluationTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                                                                                                                                                     select e).Include((RevaluationArticleTDES e) => e.SheetRevaluationTDES).FirstOrDefaultAsync())));
            }
            if (revaluationarticletdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.searchSheetRevaluationTDESGuid = revaluationarticletdes.SheetRevaluationTDESGuid;
            base.ViewBag.searchSheetRevaluationDescr = revaluationarticletdes.SheetRevaluationTDES.Description;
            return View(revaluationarticletdes);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> DeleteConfirmed(string EntArticle = null, string EntBrandNic = null, Guid? IncomePayRollTDESGuid = default(Guid?), Guid? SheetRevaluationTDESGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
            RevaluationArticleTDES revaluationarticletdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                revaluationarticletdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbIncome.RevaluationArticleTDES
                                                                                                         where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)IncomePayRollTDESGuid && (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)SheetRevaluationTDESGuid
                                                                                                         select e).Include((RevaluationArticleTDES e) => e.SheetRevaluationTDES).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.RevaluationArticleTDES
                                                                                                                                                                                                                                                                                                            where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)IncomePayRollTDESGuid && (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)SheetRevaluationTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                                                                                            select e).Include((RevaluationArticleTDES e) => e.SheetRevaluationTDES).FirstOrDefaultAsync()) : (await (from e in dbIncome.RevaluationArticleTDES
                                                                                                                                                                                                                                                                                                                                                                                                                     where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)IncomePayRollTDESGuid && (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)SheetRevaluationTDESGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                                                                                                                                                     select e).Include((RevaluationArticleTDES e) => e.SheetRevaluationTDES).FirstOrDefaultAsync())));
            }
            if (revaluationarticletdes == null)
            {
                return HttpNotFound();
            }
            if (revaluationarticletdes.IsProcessed)
            {
                base.ModelState.AddModelError("", Resources.IncomePayRollTDES_ISPOCESSES);
                base.ViewBag.searchSheetRevaluationTDESGuid = revaluationarticletdes.SheetRevaluationTDESGuid;
                base.ViewBag.searchSheetRevaluationDescr = revaluationarticletdes.SheetRevaluationTDES.Description;
                return View(revaluationarticletdes);
            }
            dbIncome.RevaluationArticleTDES.Remove(revaluationarticletdes);
            await dbIncome.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid = revaluationarticletdes.EntGuid,
                searchEntBranchGuid = revaluationarticletdes.EntBranchGuid,
                searchSheetRevaluationTDESGuid = revaluationarticletdes.SheetRevaluationTDESGuid
            });
        }

        protected override void Dispose(bool disposing)
        {
            dbIncome.Dispose();
            base.Dispose(disposing);
        }
    }

}
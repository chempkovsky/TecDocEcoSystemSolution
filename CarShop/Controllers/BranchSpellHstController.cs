// CarShop.Controllers.BranchSpellHstController
using CarShop.Models;
using CarShop.Properties;
using CarShop.Utility;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class BranchSpellHstController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopIncomeContextCatalog;

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
            base.ViewBag.IsBranchSeller = base.User.IsInRole("BranchSeller");
            base.ViewBag.IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchString, string searchString1, string currentFilter, string currentFilter1, int? page)
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
            IPagedList<BranchSpellHstTDES> aResult = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                if (searchString != null || searchString1 != null)
                {
                    page = 1;
                }
                else
                {
                    page = (page ?? 1);
                    searchString = currentFilter;
                    searchString1 = currentFilter1;
                }
                DateTime? openAt = null;
                DateTime? closeAt = null;
                if (!string.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.Trim();
                    if (DateTime.TryParse(searchString, out DateTime result))
                    {
                        openAt = result;
                    }
                    else
                    {
                        base.ModelState.AddModelError("", Resources.CanNotParceDateFrom);
                    }
                }
                if (!string.IsNullOrEmpty(searchString1))
                {
                    searchString1 = searchString1.Trim();
                    if (DateTime.TryParse(searchString1, out DateTime result2))
                    {
                        closeAt = result2;
                    }
                    else
                    {
                        base.ModelState.AddModelError("", Resources.CanNotParceDateTil);
                    }
                }
                IQueryable<BranchSpellHstTDES> q2 = from e in dbSales.BranchSpellHstTDES
                                                    where e.EntBranchGuid == searchEntBranchGuid.Value
                                                    select e;
                if (openAt.HasValue)
                {
                    q2 = from e in q2
                         where (DateTime)(DateTime?)e.OpenAt >= (DateTime)openAt
                         select e;
                }
                if (closeAt.HasValue)
                {
                    q2 = from e in q2
                         where (DateTime)(DateTime?)e.OpenAt <= (DateTime)closeAt
                         select e;
                }
                q2 = from e in q2
                     orderby e.EntBranchGuid, e.OpenAt descending
                     select e;
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await q2.ToPagedListAsync(pageNumber, pageSize);
            }
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.CurrentFilter1 = searchString1;
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
            BranchSpellHstTDES branchspellhsttdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                branchspellhsttdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbSales.BranchSpellHstTDES
                                                                                                     where (Guid)(Guid?)e.SpellGuid == (Guid)id
                                                                                                     select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbSales.BranchSpellHstTDES
                                                                                                                                                                                                                                          where (Guid)(Guid?)e.SpellGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                          select e).FirstOrDefaultAsync()) : (await (from e in dbSales.BranchSpellHstTDES
                                                                                                                                                                                                                                                                                     where (Guid)(Guid?)e.SpellGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                     select e).FirstOrDefaultAsync())));
            }
            if (branchspellhsttdes == null)
            {
                return HttpNotFound();
            }
            return View(branchspellhsttdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Edit(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
            BranchSpellHstTDES branchspellhsttdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                branchspellhsttdes = await (from e in dbSales.BranchSpellHstTDES
                                            where (Guid)(Guid?)e.SpellGuid == (Guid)id
                                            select e).FirstOrDefaultAsync();
            }
            if (branchspellhsttdes == null)
            {
                return HttpNotFound();
            }
            return View(branchspellhsttdes);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BranchSpellHstTDES branchspellhsttdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = branchspellhsttdes.EntBranchGuid;
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
            if (base.ModelState.IsValid && (searchEntBranchGuid != branchspellhsttdes.EntBranchGuid || searchEntGuid != branchspellhsttdes.EntGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                await ViewBagHelper(branchspellhsttdes.EntGuid, branchspellhsttdes.EntBranchGuid);
            }
            if (CarShopSalesContextCatalog != null)
            {
                if (base.ModelState.IsValid && branchspellhsttdes.OpenAt > branchspellhsttdes.CloseAt)
                {
                    base.ModelState.AddModelError("", Resources.SpellCloseDateLessThanOpenDate);
                }
                if (base.ModelState.IsValid)
                {
                    dbSales.Entry(branchspellhsttdes).State = EntityState.Modified;
                    await dbSales.SaveChangesAsync();
                    return RedirectToAction("Index", new
                    {
                        searchEntGuid = branchspellhsttdes.EntGuid,
                        searchEntBranchGuid = branchspellhsttdes.EntBranchGuid
                    });
                }
            }
            return View(branchspellhsttdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> DoMakeSpellClosed(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
            BranchSpellHstTDES branchspellhsttdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                branchspellhsttdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbSales.BranchSpellHstTDES
                                                                                                     where (Guid)(Guid?)e.SpellGuid == (Guid)id
                                                                                                     select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbSales.BranchSpellHstTDES
                                                                                                                                                                                                                                          where (Guid)(Guid?)e.SpellGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                          select e).FirstOrDefaultAsync()) : (await (from e in dbSales.BranchSpellHstTDES
                                                                                                                                                                                                                                                                                     where (Guid)(Guid?)e.SpellGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                     select e).FirstOrDefaultAsync())));
            }
            if (branchspellhsttdes == null)
            {
                return HttpNotFound();
            }
            string aKey = branchspellhsttdes.SpellGuid.ToString() + "SPELLCLOSE";
            MakeSpellClosedData aData = System.Web.HttpContext.Current.Cache[aKey] as MakeSpellClosedData;
            if (aData != null)
            {
                if (aData.isDone)
                {
                    base.ModelState.AddModelError("", "Для данной смены был ранее запущен процесс, который успешно завершился.");
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else if (aData.hasError)
                {
                    base.ModelState.AddModelError("", "Для данной смены  был ранее запущен процесс, который завершился С ошибкой: " + aData.ErrorText);
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else
                {
                    base.ModelState.AddModelError("", "Для данной смены  был ранее запущен процесс, который еще не закончен.");
                }
            }
            else
            {
                base.ModelState.AddModelError("", "Для данной смены нет запущенного процесса.");
            }
            return View(branchspellhsttdes);
        }

        [ValidateAntiForgeryToken]
        [ActionName("DoMakeSpellClosed")]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [HttpPost]
        public async Task<ActionResult> DoReMakeSpellClosed(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
            BranchSpellHstTDES branchspellhsttdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                branchspellhsttdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbSales.BranchSpellHstTDES
                                                                                                     where (Guid)(Guid?)e.SpellGuid == (Guid)id
                                                                                                     select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbSales.BranchSpellHstTDES
                                                                                                                                                                                                                                          where (Guid)(Guid?)e.SpellGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                          select e).FirstOrDefaultAsync()) : (await (from e in dbSales.BranchSpellHstTDES
                                                                                                                                                                                                                                                                                     where (Guid)(Guid?)e.SpellGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                     select e).FirstOrDefaultAsync())));
            }
            if (branchspellhsttdes == null)
            {
                return HttpNotFound();
            }
            string aKey = branchspellhsttdes.SpellGuid.ToString() + "SPELLCLOSE";
            MakeSpellClosedData aData2 = System.Web.HttpContext.Current.Cache[aKey] as MakeSpellClosedData;
            if (aData2 != null)
            {
                if (aData2.isDone)
                {
                    base.ModelState.AddModelError("", "Для данной смены был ранее запущен процесс, который успешно завершился.");
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else if (aData2.hasError)
                {
                    base.ModelState.AddModelError("", "Для данной смены  был ранее запущен процесс, который завершился С ошибкой: " + aData2.ErrorText);
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else
                {
                    base.ModelState.AddModelError("", "Для данной смены  был ранее запущен процесс, который еще не закончен.");
                }
            }
            else
            {
                aData2 = new MakeSpellClosedData
                {
                    SpellGuid = branchspellhsttdes.SpellGuid,
                    hasError = false,
                    isDone = false,
                    ErrorText = null
                };
                try
                {
                    System.Web.HttpContext.Current.Cache.Add(aKey, aData2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(40.0), CacheItemPriority.Normal, null);
                }
                catch
                {
                    base.ModelState.AddModelError("", "Не могу добавить данные в КЭШ. Сервер перегружен. попробуйте позже");
                }
                if (base.ModelState.IsValid)
                {
                    aData2.dbSales = new CarShopSalesContext(CarShopSalesContextCatalog);
                    aData2.dbIncome = new CarShopIncomeContext();
                    Thread thread = new Thread(MakeSpellClosed.DoMakeSpellClosed);
                    thread.Start(aData2);
                    base.ModelState.AddModelError("", "Для данной смены запущен процесс обработки. Для просмотра результатов вернитесь на данную страницу в течение 40 мин.");
                }
            }
            return View(branchspellhsttdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> LookForSpell(string redirecData, string redirectContriller, string redirectAction, Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchString, string searchString1, string currentFilter, string currentFilter1, int? page)
        {
            base.ViewBag.RedirecData = redirecData;
            base.ViewBag.RedirectContriller = redirectContriller;
            base.ViewBag.RedirectAction = redirectAction;
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
            IPagedList<BranchSpellHstTDES> aResult = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                if (searchString != null || searchString1 != null)
                {
                    page = 1;
                }
                else
                {
                    page = (page ?? 1);
                    searchString = currentFilter;
                    searchString1 = currentFilter1;
                }
                DateTime? openAt = null;
                DateTime? closeAt = null;
                if (!string.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.Trim();
                    if (DateTime.TryParse(searchString, out DateTime result))
                    {
                        openAt = result;
                    }
                    else
                    {
                        base.ModelState.AddModelError("", Resources.CanNotParceDateFrom);
                    }
                }
                if (!string.IsNullOrEmpty(searchString1))
                {
                    searchString1 = searchString1.Trim();
                    if (DateTime.TryParse(searchString1, out DateTime result2))
                    {
                        closeAt = result2;
                    }
                    else
                    {
                        base.ModelState.AddModelError("", Resources.CanNotParceDateTil);
                    }
                }
                IQueryable<BranchSpellHstTDES> q2 = from e in dbSales.BranchSpellHstTDES
                                                    where e.EntBranchGuid == searchEntBranchGuid.Value
                                                    select e;
                if (openAt.HasValue)
                {
                    q2 = from e in q2
                         where (DateTime)(DateTime?)e.OpenAt >= (DateTime)openAt
                         select e;
                }
                if (closeAt.HasValue)
                {
                    q2 = from e in q2
                         where (DateTime)(DateTime?)e.OpenAt <= (DateTime)closeAt
                         select e;
                }
                q2 = from e in q2
                     orderby e.EntBranchGuid, e.OpenAt descending
                     select e;
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await q2.ToPagedListAsync(pageNumber, pageSize);
            }
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.CurrentFilter1 = searchString1;
            return View(aResult);
        }

        public ActionResult LookForSpellSelected(string redirecData, string redirectContriller, string redirectAction, string DOCANCEL_TITLE, Guid? searchEntGuid, Guid? searchEntBranchGuid, Guid? searchSpellGuid)
        {
            if (string.IsNullOrEmpty(redirectAction))
            {
                redirectAction = "GetReturnArticuleForCreate";
            }
            if (string.IsNullOrEmpty(redirectContriller))
            {
                redirectContriller = "ReturnBasketArticle";
            }
            if (DOCANCEL_TITLE != null)
            {
                return RedirectToAction(redirectAction, redirectContriller, new
                {
                    redirecData
                });
            }
            return RedirectToAction(redirectAction, redirectContriller, new
            {
                searchSpellGuid,
                searchEntGuid,
                searchEntBranchGuid,
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
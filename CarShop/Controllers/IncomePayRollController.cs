// CarShop.Controllers.IncomePayRollController
using CarShop.Models;
using CarShop.Properties;
using CarShop.Utility;
using Newtonsoft.Json;
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

    public class IncomePayRollController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopIncomeContextCatalog;

        private string CarShopArticleContextCatalog;

        private string CarShopRestContextCatalog;

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
                CarShopArticleContextCatalog = enterprisebranchtdes.EnterpriseTDES.ArticleCatalog;
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
            IPagedList<IncomePayRollTDES> aResult = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
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
                IQueryable<IncomePayRollTDES> q2 = from e in dbIncome.IncomePayRollTDES
                                                   where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e;
                if (openAt.HasValue)
                {
                    q2 = from e in q2
                         where (DateTime)(DateTime?)e.CreatedAt >= (DateTime)openAt
                         select e;
                }
                if (closeAt.HasValue)
                {
                    q2 = from e in q2
                         where (DateTime)(DateTime?)e.CreatedAt <= (DateTime)closeAt
                         select e;
                }
                q2 = from e in q2
                     orderby e.CreatedAt descending
                     select e;
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await q2.ToPagedListAsync(pageNumber, pageSize);
            }
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.CurrentFilter1 = searchString1;
            return View(aResult);
        }

        protected async Task<IncomePayRollTDES> GetCurrentIncomePayRollTDES(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
                                                                                                    where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)id
                                                                                                    select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.IncomePayRollTDES
                                                                                                                                                                                                                                         where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                         select e).FirstOrDefaultAsync()) : (await (from e in dbIncome.IncomePayRollTDES
                                                                                                                                                                                                                                                                                    where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                    select e).FirstOrDefaultAsync())));
            }
            return incomepayrolltdes;
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            IncomePayRollTDES incomepayrolltdes = await GetCurrentIncomePayRollTDES(id, searchEntGuid, searchEntBranchGuid);
            if (incomepayrolltdes == null)
            {
                return HttpNotFound();
            }
            return View(incomepayrolltdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> SheetRevaluation(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            IncomePayRollTDES incomepayrolltdes = await GetCurrentIncomePayRollTDES(id, searchEntGuid, searchEntBranchGuid);
            if (incomepayrolltdes == null)
            {
                return HttpNotFound();
            }
            string aKey = incomepayrolltdes.IncomePayRollTDESGuid.ToString() + "REVAL";
            CreateSheetRevaluationData aData = System.Web.HttpContext.Current.Cache[aKey] as CreateSheetRevaluationData;
            if (aData != null)
            {
                if (aData.isDone)
                {
                    base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который успешно завершился.");
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else if (aData.hasError)
                {
                    base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который завершился С ошибкой: " + aData.ErrorText);
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else
                {
                    base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который еще не закончен.");
                }
            }
            else
            {
                base.ModelState.AddModelError("", "Для данной ведомости нет запущенного процесса.");
            }
            return View(incomepayrolltdes);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [ActionName("SheetRevaluation")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReCreateSheetRevaluation(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            IncomePayRollTDES incomepayrolltdes = await GetCurrentIncomePayRollTDES(id, searchEntGuid, searchEntBranchGuid);
            if (incomepayrolltdes == null)
            {
                return HttpNotFound();
            }
            string aKey = incomepayrolltdes.IncomePayRollTDESGuid.ToString() + "REVAL";
            CreateSheetRevaluationData aData2 = System.Web.HttpContext.Current.Cache[aKey] as CreateSheetRevaluationData;
            if (aData2 == null)
            {
                aData2 = new CreateSheetRevaluationData
                {
                    IncomePayRollTDESGuid = incomepayrolltdes.IncomePayRollTDESGuid,
                    hasError = false,
                    isDone = false,
                    ErrorText = null
                };
                try
                {
                    System.Web.HttpContext.Current.Cache.Add(aKey, aData2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10.0), CacheItemPriority.Normal, null);
                }
                catch
                {
                    base.ModelState.AddModelError("", "Не могу добавить данные в КЭШ. Сервер перегружен. попробуйте позже");
                }
                if (base.ModelState.IsValid)
                {
                    aData2.dbIncome = new CarShopIncomeContext(CarShopIncomeContextCatalog);
                    Thread thread = new Thread(CreateSheetRevaluation.DoCreateSheetRevaluation);
                    thread.Start(aData2);
                    base.ModelState.AddModelError("", "Для данной приходной ведомости запущен процесс обработки. Для просмотра результатов вернитесь на данную страницу в течение 20 мин.");
                }
            }
            else if (aData2.isDone)
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который успешно завершился.");
                System.Web.HttpContext.Current.Cache.Remove(aKey);
            }
            else if (aData2.hasError)
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который завершился С ошибкой: " + aData2.ErrorText);
                System.Web.HttpContext.Current.Cache.Remove(aKey);
            }
            else
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который еще не закончен.");
            }
            return View(incomepayrolltdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> MakeIncome(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            IncomePayRollTDES incomepayrolltdes = await GetCurrentIncomePayRollTDES(id, searchEntGuid, searchEntBranchGuid);
            if (incomepayrolltdes == null)
            {
                return HttpNotFound();
            }
            string aKey = incomepayrolltdes.IncomePayRollTDESGuid.ToString() + "INCOME";
            MakeIncomePayRollData aData = System.Web.HttpContext.Current.Cache[aKey] as MakeIncomePayRollData;
            if (aData != null)
            {
                if (aData.isDone)
                {
                    base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который успешно завершился.");
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else if (aData.hasError)
                {
                    base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который завершился С ошибкой: " + aData.ErrorText);
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else
                {
                    base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который еще не закончен.");
                }
            }
            else
            {
                base.ModelState.AddModelError("", "Для данной ведомости нет запущенного процесса.");
            }
            return View(incomepayrolltdes);
        }

        [HttpPost]
        [ActionName("MakeIncome")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> ReMakeIncome(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            IncomePayRollTDES incomepayrolltdes = await GetCurrentIncomePayRollTDES(id, searchEntGuid, searchEntBranchGuid);
            if (incomepayrolltdes == null)
            {
                return HttpNotFound();
            }
            string aKey = incomepayrolltdes.IncomePayRollTDESGuid.ToString() + "INCOME";
            MakeIncomePayRollData aData2 = System.Web.HttpContext.Current.Cache[aKey] as MakeIncomePayRollData;
            if (aData2 == null)
            {
                aData2 = new MakeIncomePayRollData
                {
                    IncomePayRollTDESGuid = incomepayrolltdes.IncomePayRollTDESGuid,
                    hasError = false,
                    isDone = false,
                    ErrorText = null
                };
                try
                {
                    System.Web.HttpContext.Current.Cache.Add(aKey, aData2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10.0), CacheItemPriority.Normal, null);
                }
                catch
                {
                    base.ModelState.AddModelError("", "Не могу добавить данные в КЭШ. Сервер перегружен. попробуйте позже");
                }
                if (base.ModelState.IsValid)
                {
                    aData2.dbIncome = new CarShopIncomeContext(CarShopIncomeContextCatalog);
                    aData2.dbArticle = new CarShopArticleContext(CarShopArticleContextCatalog);
                    aData2.dbRest = new CarShopRestContext(CarShopRestContextCatalog);
                    Thread thread = new Thread(MakeIncomePayRoll.DoMakeIncomePayRoll);
                    thread.Start(aData2);
                    base.ModelState.AddModelError("", "Для данной приходной ведомости запущен процесс обработки. Для просмотра результатов вернитесь на данную страницу в течение 20 мин.");
                }
            }
            else if (aData2.isDone)
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который успешно завершился.");
                System.Web.HttpContext.Current.Cache.Remove(aKey);
            }
            else if (aData2.hasError)
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который завершился С ошибкой: " + aData2.ErrorText);
                System.Web.HttpContext.Current.Cache.Remove(aKey);
            }
            else
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который еще не закончен.");
            }
            return View(incomepayrolltdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> ReverseIncome(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            IncomePayRollTDES incomepayrolltdes = await GetCurrentIncomePayRollTDES(id, searchEntGuid, searchEntBranchGuid);
            if (incomepayrolltdes == null)
            {
                return HttpNotFound();
            }
            string aKey = incomepayrolltdes.IncomePayRollTDESGuid.ToString() + "REVERSEINCOME";
            MakeIncomePayRollData aData = System.Web.HttpContext.Current.Cache[aKey] as MakeIncomePayRollData;
            if (aData != null)
            {
                if (aData.isDone)
                {
                    base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который успешно завершился.");
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else if (aData.hasError)
                {
                    base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который завершился С ошибкой: " + aData.ErrorText);
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else
                {
                    base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который еще не закончен.");
                }
            }
            else
            {
                base.ModelState.AddModelError("", "Для данной ведомости нет запущенного процесса.");
            }
            return View(incomepayrolltdes);
        }

        [HttpPost]
        [ActionName("ReverseIncome")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> ReReverseIncome(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            IncomePayRollTDES incomepayrolltdes = await GetCurrentIncomePayRollTDES(id, searchEntGuid, searchEntBranchGuid);
            if (incomepayrolltdes == null)
            {
                return HttpNotFound();
            }
            string aKey = incomepayrolltdes.IncomePayRollTDESGuid.ToString() + "REVERSEINCOME";
            MakeIncomePayRollData aData2 = System.Web.HttpContext.Current.Cache[aKey] as MakeIncomePayRollData;
            if (aData2 == null)
            {
                aData2 = new MakeIncomePayRollData
                {
                    IncomePayRollTDESGuid = incomepayrolltdes.IncomePayRollTDESGuid,
                    hasError = false,
                    isDone = false,
                    ErrorText = null
                };
                try
                {
                    System.Web.HttpContext.Current.Cache.Add(aKey, aData2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10.0), CacheItemPriority.Normal, null);
                }
                catch
                {
                    base.ModelState.AddModelError("", "Не могу добавить данные в КЭШ. Сервер перегружен. попробуйте позже");
                }
                if (base.ModelState.IsValid)
                {
                    aData2.dbIncome = new CarShopIncomeContext(CarShopIncomeContextCatalog);
                    aData2.dbArticle = new CarShopArticleContext(CarShopArticleContextCatalog);
                    aData2.dbRest = new CarShopRestContext(CarShopRestContextCatalog);
                    Thread thread = new Thread(MakeIncomePayRoll.DoReverseIncomePayRoll);
                    thread.Start(aData2);
                    base.ModelState.AddModelError("", "Для данной приходной ведомости запущен процесс обработки. Для просмотра результатов вернитесь на данную страницу в течение 20 мин.");
                }
            }
            else if (aData2.isDone)
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который успешно завершился.");
                System.Web.HttpContext.Current.Cache.Remove(aKey);
            }
            else if (aData2.hasError)
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который завершился С ошибкой: " + aData2.ErrorText);
                System.Web.HttpContext.Current.Cache.Remove(aKey);
            }
            else
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который еще не закончен.");
            }
            return View(incomepayrolltdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Create(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
            IncomePayRollTDES incomepayrolltdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                DateTime now = DateTime.Now;
                now = now.AddMilliseconds(-now.Millisecond);
                incomepayrolltdes = new IncomePayRollTDES
                {
                    IncomePayRollTDESGuid = Guid.NewGuid(),
                    EntBranchGuid = searchEntBranchGuid.Value,
                    EntGuid = searchEntGuid.Value,
                    EntUserNic = base.User.Identity.Name,
                    CreatedAt = now
                };
            }
            return View(incomepayrolltdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(IncomePayRollTDES incomepayrolltdes, string EntSupplierIdLookUp = null, string EntSupplierDescriptionLookUp = null)
        {
            if (EntSupplierIdLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(incomepayrolltdes);
                string EntSupplierId = incomepayrolltdes.EntSupplierId;
                await ViewBagHelper(incomepayrolltdes.EntGuid, incomepayrolltdes.EntBranchGuid);
                return RedirectToAction("LookUpSupplierByID", "EnterpriseSupplier", new
                {
                    redirecData = redirecData,
                    redirectContriller = "IncomePayRoll",
                    redirectAction = "GetSupplierForCreate",
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchString = EntSupplierId,
                    searchStringBy = 1
                });
            }
            if (EntSupplierDescriptionLookUp != null)
            {
                string redirecData2 = JsonConvert.SerializeObject(incomepayrolltdes);
                string EntSupplierId2 = incomepayrolltdes.EntSupplierDescription;
                await ViewBagHelper(incomepayrolltdes.EntGuid, incomepayrolltdes.EntBranchGuid);
                return RedirectToAction("LookUpSupplierByID", "EnterpriseSupplier", new
                {
                    redirecData = redirecData2,
                    redirectContriller = "IncomePayRoll",
                    redirectAction = "GetSupplierForCreate",
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchString = EntSupplierId2,
                    searchStringBy = 2
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
                searchEntGuid = incomepayrolltdes.EntGuid;
                searchEntBranchGuid = incomepayrolltdes.EntBranchGuid;
            }
            string searchEntUserNic = base.User.Identity.Name;
            if (base.ModelState.IsValid && (searchEntGuid != incomepayrolltdes.EntGuid || searchEntBranchGuid != incomepayrolltdes.EntBranchGuid || searchEntUserNic != incomepayrolltdes.EntUserNic))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (base.ModelState.IsValid)
            {
                incomepayrolltdes.CreatedAt = incomepayrolltdes.CreatedAt.AddMilliseconds(-incomepayrolltdes.CreatedAt.Millisecond);
                dbIncome.IncomePayRollTDES.Add(incomepayrolltdes);
                await dbIncome.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = incomepayrolltdes.EntGuid,
                    searchEntBranchGuid = incomepayrolltdes.EntBranchGuid
                });
            }
            return View(incomepayrolltdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        protected async Task<ActionResult> GetSupplierForCreateUpdate(string redirecData, string searchEntSupplierId, string searchEntSupplierDescription, string actionName)
        {
            IncomePayRollTDES branchresttdes = JsonConvert.DeserializeObject<IncomePayRollTDES>(redirecData);
            if (searchEntSupplierId != null)
            {
                branchresttdes.EntSupplierId = searchEntSupplierId;
            }
            if (searchEntSupplierDescription != null)
            {
                branchresttdes.EntSupplierDescription = searchEntSupplierDescription;
            }
            await ViewBagHelper(branchresttdes.EntGuid, branchresttdes.EntBranchGuid);
            return View(actionName, branchresttdes);
        }

        public async Task<ActionResult> GetSupplierForCreate(string redirecData, string searchEntSupplierId, string searchEntSupplierDescription)
        {
            return await GetSupplierForCreateUpdate(redirecData, searchEntSupplierId, searchEntSupplierDescription, "Create");
        }

        public async Task<ActionResult> GetSupplierForUpdate(string redirecData, string searchEntSupplierId, string searchEntSupplierDescription)
        {
            return await GetSupplierForCreateUpdate(redirecData, searchEntSupplierId, searchEntSupplierDescription, "Edit");
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
                                                                                                    where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)id
                                                                                                    select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.IncomePayRollTDES
                                                                                                                                                                                                                                         where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                         select e).FirstOrDefaultAsync()) : (await (from e in dbIncome.IncomePayRollTDES
                                                                                                                                                                                                                                                                                    where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                    select e).FirstOrDefaultAsync())));
            }
            if (incomepayrolltdes == null)
            {
                return HttpNotFound();
            }
            return View(incomepayrolltdes);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IncomePayRollTDES incomepayrolltdes, string EntSupplierIdLookUp = null, string EntSupplierDescriptionLookUp = null)
        {
            if (EntSupplierIdLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(incomepayrolltdes);
                string EntSupplierId = incomepayrolltdes.EntSupplierId;
                await ViewBagHelper(incomepayrolltdes.EntGuid, incomepayrolltdes.EntBranchGuid);
                return RedirectToAction("LookUpSupplierByID", "EnterpriseSupplier", new
                {
                    redirecData = redirecData,
                    redirectContriller = "IncomePayRoll",
                    redirectAction = "GetSupplierForUpdate",
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchString = EntSupplierId,
                    searchStringBy = 1
                });
            }
            if (EntSupplierDescriptionLookUp != null)
            {
                string redirecData2 = JsonConvert.SerializeObject(incomepayrolltdes);
                string EntSupplierId2 = incomepayrolltdes.EntSupplierDescription;
                await ViewBagHelper(incomepayrolltdes.EntGuid, incomepayrolltdes.EntBranchGuid);
                return RedirectToAction("LookUpSupplierByID", "EnterpriseSupplier", new
                {
                    redirecData = redirecData2,
                    redirectContriller = "IncomePayRoll",
                    redirectAction = "GetSupplierForUpdate",
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchString = EntSupplierId2,
                    searchStringBy = 2
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
                searchEntGuid = incomepayrolltdes.EntGuid;
                searchEntBranchGuid = incomepayrolltdes.EntBranchGuid;
            }
            string searchEntUserNic = base.User.Identity.Name;
            if (base.ModelState.IsValid && (searchEntGuid != incomepayrolltdes.EntGuid || searchEntBranchGuid != incomepayrolltdes.EntBranchGuid || searchEntUserNic != incomepayrolltdes.EntUserNic))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (base.ModelState.IsValid && incomepayrolltdes.IsProcessed)
            {
                base.ModelState.AddModelError("", Resources.IncomePayRollTDES_CANNOTCHANGE);
                return View(incomepayrolltdes);
            }
            if (base.ModelState.IsValid)
            {
                incomepayrolltdes.CreatedAt = incomepayrolltdes.CreatedAt.AddMilliseconds(-incomepayrolltdes.CreatedAt.Millisecond);
                dbIncome.Entry(incomepayrolltdes).State = EntityState.Modified;
                await dbIncome.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(incomepayrolltdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Delete(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
                                                                                                    where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)id
                                                                                                    select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.IncomePayRollTDES
                                                                                                                                                                                                                                         where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                         select e).FirstOrDefaultAsync()) : (await (from e in dbIncome.IncomePayRollTDES
                                                                                                                                                                                                                                                                                    where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                    select e).FirstOrDefaultAsync())));
            }
            if (incomepayrolltdes == null)
            {
                return HttpNotFound();
            }
            return View(incomepayrolltdes);
        }

        [ActionName("Delete")]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(Guid id, Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
                                                                                                    where e.IncomePayRollTDESGuid == id
                                                                                                    select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbIncome.IncomePayRollTDES
                                                                                                                                                                                                                                         where e.IncomePayRollTDESGuid == id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                         select e).FirstOrDefaultAsync()) : (await (from e in dbIncome.IncomePayRollTDES
                                                                                                                                                                                                                                                                                    where e.IncomePayRollTDESGuid == id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                    select e).FirstOrDefaultAsync())));
            }
            if (incomepayrolltdes == null)
            {
                return HttpNotFound();
            }
            if (incomepayrolltdes.IsProcessed)
            {
                base.ModelState.AddModelError("", Resources.IncomePayRollTDES_CANNOTCHANGE);
                return View(incomepayrolltdes);
            }
            dbIncome.IncomePayRollTDES.Remove(incomepayrolltdes);
            await dbIncome.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid,
                searchEntBranchGuid
            });
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public async Task<ActionResult> MakeRestToIncome(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            IncomePayRollTDES incomepayrolltdes = await GetCurrentIncomePayRollTDES(id, searchEntGuid, searchEntBranchGuid);
            if (incomepayrolltdes == null)
            {
                return HttpNotFound();
            }
            string aKey = incomepayrolltdes.EntBranchGuid.ToString() + "REST2INCOME";
            Rest2IncomeData aData = System.Web.HttpContext.Current.Cache[aKey] as Rest2IncomeData;
            if (aData != null)
            {
                if (aData.isDone)
                {
                    base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который успешно завершился.");
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else if (aData.hasError)
                {
                    base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который завершился С ошибкой: " + aData.ErrorText);
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else
                {
                    base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который еще не закончен.");
                }
            }
            else
            {
                base.ModelState.AddModelError("", "Для данной ведомости нет запущенного процесса.");
            }
            return View(incomepayrolltdes);
        }

        [ActionName("MakeRestToIncome")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public async Task<ActionResult> ReMakeRestToIncome(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            IncomePayRollTDES incomepayrolltdes = await GetCurrentIncomePayRollTDES(id, searchEntGuid, searchEntBranchGuid);
            if (incomepayrolltdes == null)
            {
                return HttpNotFound();
            }
            string aKey = incomepayrolltdes.EntBranchGuid.ToString() + "REST2INCOME";
            Rest2IncomeData aData2 = System.Web.HttpContext.Current.Cache[aKey] as Rest2IncomeData;
            if (aData2 == null)
            {
                aData2 = new Rest2IncomeData
                {
                    IncomePayRollTDESGuid = incomepayrolltdes.IncomePayRollTDESGuid,
                    hasError = false,
                    isDone = false,
                    ErrorText = null
                };
                try
                {
                    System.Web.HttpContext.Current.Cache.Add(aKey, aData2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60.0), CacheItemPriority.Normal, null);
                }
                catch
                {
                    base.ModelState.AddModelError("", "Не могу добавить данные в КЭШ. Сервер перегружен. попробуйте позже");
                }
                if (base.ModelState.IsValid)
                {
                    aData2.dbIncome = new CarShopIncomeContext(CarShopIncomeContextCatalog);
                    aData2.dbArticle = new CarShopArticleContext(CarShopArticleContextCatalog);
                    aData2.dbRest = new CarShopRestContext(CarShopRestContextCatalog);
                    Thread thread = new Thread(Rest2Income.DoMakeRest2Income);
                    thread.Start(aData2);
                    base.ModelState.AddModelError("", "Для данной приходной ведомости запущен процесс обработки. Для просмотра результатов вернитесь на данную страницу в течение 60 мин.");
                }
            }
            else if (aData2.isDone)
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который успешно завершился.");
                System.Web.HttpContext.Current.Cache.Remove(aKey);
            }
            else if (aData2.hasError)
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который завершился С ошибкой: " + aData2.ErrorText);
                System.Web.HttpContext.Current.Cache.Remove(aKey);
            }
            else
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который еще не закончен.");
            }
            return View(incomepayrolltdes);
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
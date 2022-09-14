// CarShop.Controllers.SheetRevaluationController
using CarShop.Models;
using CarShop.Properties;
using CarShop.Utility;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{
    public class SheetRevaluationController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopIncomeContextCatalog;

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

        public void ViewBagHelper(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            EnterpriseBranchTDES enterpriseBranchTDES = (from e in db.EnterpriseBranchTDES
                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                         select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefault();
            if (enterpriseBranchTDES != null)
            {
                base.ViewBag.EntDescription = enterpriseBranchTDES.EnterpriseTDES.EntDescription;
                base.ViewBag.EntBranchDescription = enterpriseBranchTDES.EntBranchDescription;
                base.ViewBag.SearchEntGuid = enterpriseBranchTDES.EntGuid;
                base.ViewBag.SearchEntBranchGuid = enterpriseBranchTDES.EntBranchGuid;
                CarShopIncomeContextCatalog = enterpriseBranchTDES.IncomeCatalog;
                CarShopRestContextCatalog = enterpriseBranchTDES.TecDocCatalog;
                return;
            }
            base.ViewBag.EntBranchDescription = Resources.EnterpriseBranchTDES_NOTFOUND;
            base.ViewBag.SearchEntBranchGuid = null;
            base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            EnterpriseTDES enterpriseTDES = (from e in db.EnterpriseTDES
                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                             select e).FirstOrDefault();
            if (enterpriseTDES != null)
            {
                base.ViewBag.EntDescription = enterpriseTDES.EntDescription;
                base.ViewBag.SearchEntGuid = enterpriseTDES.EntGuid;
            }
            else
            {
                base.ViewBag.EntDescription = Resources.ENTERPRISE_NOT_DEFINED;
                base.ViewBag.SearchEntGuid = null;
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
            base.ViewBag.IsBranchAdmin = base.User.IsInRole("BranchAdmin");
            base.ViewBag.IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Index(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchString, string searchString1, string currentFilter, string currentFilter1, int? page)
        {
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                }
            }
            IPagedList<SheetRevaluationTDES> model = null;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
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
                IQueryable<SheetRevaluationTDES> source = from e in dbIncome.SheetRevaluationTDES
                                                          where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                          select e;
                if (openAt.HasValue)
                {
                    source = from e in source
                             where (DateTime)(DateTime?)e.CreatedAt >= (DateTime)openAt
                             select e;
                }
                if (closeAt.HasValue)
                {
                    source = from e in source
                             where (DateTime)(DateTime?)e.CreatedAt <= (DateTime)closeAt
                             select e;
                }
                source = from e in source
                         orderby e.CreatedAt descending
                         select e;
                int pageSize = 20;
                int value = page.Value;
                model = source.ToPagedList(value, pageSize);
            }
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.CurrentFilter1 = searchString1;
            return View(model);
        }

        protected SheetRevaluationTDES GetCurrentSheetRevaluationTDES(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                }
            }
            SheetRevaluationTDES result = null;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                result = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (from e in dbIncome.SheetRevaluationTDES
                                                                                  where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)id
                                                                                  select e).FirstOrDefault() : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (from e in dbIncome.SheetRevaluationTDES
                                                                                                                                                                                                          where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                          select e).FirstOrDefault() : (from e in dbIncome.SheetRevaluationTDES
                                                                                                                                                                                                                                        where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                        select e).FirstOrDefault()));
            }
            return result;
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Details(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            SheetRevaluationTDES currentSheetRevaluationTDES = GetCurrentSheetRevaluationTDES(id, searchEntGuid, searchEntBranchGuid);
            if (currentSheetRevaluationTDES == null)
            {
                return HttpNotFound();
            }
            return View(currentSheetRevaluationTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public ActionResult DoMakeSheetRevaluation(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            SheetRevaluationTDES currentSheetRevaluationTDES = GetCurrentSheetRevaluationTDES(id, searchEntGuid, searchEntBranchGuid);
            if (currentSheetRevaluationTDES == null)
            {
                return HttpNotFound();
            }
            string key = currentSheetRevaluationTDES.SheetRevaluationTDESGuid.ToString() + "SHEETREVAL";
            MakeSheetRevaluationData makeSheetRevaluationData = System.Web.HttpContext.Current.Cache[key] as MakeSheetRevaluationData;
            if (makeSheetRevaluationData != null)
            {
                if (makeSheetRevaluationData.isDone)
                {
                    base.ModelState.AddModelError("", "Для данной ведомости переоценки был ранее запущен процесс, который успешно завершился.");
                    System.Web.HttpContext.Current.Cache.Remove(key);
                }
                else if (makeSheetRevaluationData.hasError)
                {
                    base.ModelState.AddModelError("", "Для данной ведомости переоценки был ранее запущен процесс, который завершился С ошибкой: " + makeSheetRevaluationData.ErrorText);
                    System.Web.HttpContext.Current.Cache.Remove(key);
                }
                else
                {
                    base.ModelState.AddModelError("", "Для данной ведомости  переоценки был ранее запущен процесс, который еще не закончен.");
                }
            }
            else
            {
                base.ModelState.AddModelError("", "Для данной ведомости нет запущенного процесса.");
            }
            return View(currentSheetRevaluationTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("DoMakeSheetRevaluation")]
        public ActionResult DoReMakeSheetRevaluation(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            SheetRevaluationTDES currentSheetRevaluationTDES = GetCurrentSheetRevaluationTDES(id, searchEntGuid, searchEntBranchGuid);
            if (currentSheetRevaluationTDES == null)
            {
                return HttpNotFound();
            }
            string key = currentSheetRevaluationTDES.SheetRevaluationTDESGuid.ToString() + "SHEETREVAL";
            MakeSheetRevaluationData makeSheetRevaluationData = System.Web.HttpContext.Current.Cache[key] as MakeSheetRevaluationData;
            if (makeSheetRevaluationData == null)
            {
                MakeSheetRevaluationData makeSheetRevaluationData2 = new MakeSheetRevaluationData();
                makeSheetRevaluationData2.SheetRevaluationTDESGuid = currentSheetRevaluationTDES.SheetRevaluationTDESGuid;
                makeSheetRevaluationData2.hasError = false;
                makeSheetRevaluationData2.isDone = false;
                makeSheetRevaluationData2.ErrorText = null;
                makeSheetRevaluationData = makeSheetRevaluationData2;
                try
                {
                    System.Web.HttpContext.Current.Cache.Add(key, makeSheetRevaluationData, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10.0), CacheItemPriority.Normal, null);
                }
                catch
                {
                    base.ModelState.AddModelError("", "Не могу добавить данные в КЭШ. Сервер перегружен. попробуйте позже");
                }
                if (base.ModelState.IsValid)
                {
                    makeSheetRevaluationData.dbIncome = new CarShopIncomeContext(CarShopIncomeContextCatalog);
                    makeSheetRevaluationData.dbRest = new CarShopRestContext(CarShopRestContextCatalog);
                    Thread thread = new Thread(MakeSheetRevaluation.DoMakeSheetRevaluation);
                    thread.Start(makeSheetRevaluationData);
                    base.ModelState.AddModelError("", "Для данной приходной ведомости запущен процесс обработки. Для просмотра результатов вернитесь на данную страницу в течение 20 мин.");
                }
            }
            else if (makeSheetRevaluationData.isDone)
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который успешно завершился.");
                System.Web.HttpContext.Current.Cache.Remove(key);
            }
            else if (makeSheetRevaluationData.hasError)
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который завершился С ошибкой: " + makeSheetRevaluationData.ErrorText);
                System.Web.HttpContext.Current.Cache.Remove(key);
            }
            else
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который еще не закончен.");
            }
            return View(currentSheetRevaluationTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public ActionResult ReverseSheetRevaluation(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            SheetRevaluationTDES currentSheetRevaluationTDES = GetCurrentSheetRevaluationTDES(id, searchEntGuid, searchEntBranchGuid);
            if (currentSheetRevaluationTDES == null)
            {
                return HttpNotFound();
            }
            string key = currentSheetRevaluationTDES.SheetRevaluationTDESGuid.ToString() + "REVREVAL";
            MakeSheetRevaluationData makeSheetRevaluationData = System.Web.HttpContext.Current.Cache[key] as MakeSheetRevaluationData;
            if (makeSheetRevaluationData != null)
            {
                if (makeSheetRevaluationData.isDone)
                {
                    base.ModelState.AddModelError("", "Для данной ведомости переоценки был ранее запущен процесс, который успешно завершился.");
                    System.Web.HttpContext.Current.Cache.Remove(key);
                }
                else if (makeSheetRevaluationData.hasError)
                {
                    base.ModelState.AddModelError("", "Для данной ведомости переоценки был ранее запущен процесс, который завершился С ошибкой: " + makeSheetRevaluationData.ErrorText);
                    System.Web.HttpContext.Current.Cache.Remove(key);
                }
                else
                {
                    base.ModelState.AddModelError("", "Для данной ведомости  переоценки был ранее запущен процесс, который еще не закончен.");
                }
            }
            else
            {
                base.ModelState.AddModelError("", "Для данной ведомости нет запущенного процесса.");
            }
            return View(currentSheetRevaluationTDES);
        }

        [HttpPost]
        [ActionName("ReverseSheetRevaluation")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public ActionResult DoReverseSheetRevaluation(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            SheetRevaluationTDES currentSheetRevaluationTDES = GetCurrentSheetRevaluationTDES(id, searchEntGuid, searchEntBranchGuid);
            if (currentSheetRevaluationTDES == null)
            {
                return HttpNotFound();
            }
            string key = currentSheetRevaluationTDES.SheetRevaluationTDESGuid.ToString() + "REVREVAL";
            MakeSheetRevaluationData makeSheetRevaluationData = System.Web.HttpContext.Current.Cache[key] as MakeSheetRevaluationData;
            if (makeSheetRevaluationData == null)
            {
                MakeSheetRevaluationData makeSheetRevaluationData2 = new MakeSheetRevaluationData();
                makeSheetRevaluationData2.SheetRevaluationTDESGuid = currentSheetRevaluationTDES.SheetRevaluationTDESGuid;
                makeSheetRevaluationData2.hasError = false;
                makeSheetRevaluationData2.isDone = false;
                makeSheetRevaluationData2.ErrorText = null;
                makeSheetRevaluationData = makeSheetRevaluationData2;
                try
                {
                    System.Web.HttpContext.Current.Cache.Add(key, makeSheetRevaluationData, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10.0), CacheItemPriority.Normal, null);
                }
                catch
                {
                    base.ModelState.AddModelError("", "Не могу добавить данные в КЭШ. Сервер перегружен. попробуйте позже");
                }
                if (base.ModelState.IsValid)
                {
                    makeSheetRevaluationData.dbIncome = new CarShopIncomeContext(CarShopIncomeContextCatalog);
                    makeSheetRevaluationData.dbRest = new CarShopRestContext(CarShopRestContextCatalog);
                    Thread thread = new Thread(MakeSheetRevaluation.DoReverseSheetRevaluation);
                    thread.Start(makeSheetRevaluationData);
                    base.ModelState.AddModelError("", "Для данной приходной ведомости запущен процесс обработки. Для просмотра результатов вернитесь на данную страницу в течение 20 мин.");
                }
            }
            else if (makeSheetRevaluationData.isDone)
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который успешно завершился.");
                System.Web.HttpContext.Current.Cache.Remove(key);
            }
            else if (makeSheetRevaluationData.hasError)
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который завершился С ошибкой: " + makeSheetRevaluationData.ErrorText);
                System.Web.HttpContext.Current.Cache.Remove(key);
            }
            else
            {
                base.ModelState.AddModelError("", "Для данной приходной ведомости был ранее запущен процесс, который еще не закончен.");
            }
            return View(currentSheetRevaluationTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public ActionResult Create(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES != null)
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                    else
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                }
            }
            SheetRevaluationTDES model = null;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                DateTime now = DateTime.Now;
                now = now.AddMilliseconds(-now.Millisecond);
                SheetRevaluationTDES sheetRevaluationTDES = new SheetRevaluationTDES();
                sheetRevaluationTDES.SheetRevaluationTDESGuid = Guid.NewGuid();
                sheetRevaluationTDES.EntBranchGuid = searchEntBranchGuid.Value;
                sheetRevaluationTDES.EntGuid = searchEntGuid.Value;
                sheetRevaluationTDES.EntUserNic = base.User.Identity.Name;
                sheetRevaluationTDES.CreatedAt = now;
                model = sheetRevaluationTDES;
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(SheetRevaluationTDES sheetrevaluationtdes)
        {
            UserIsInRoles();
            Guid? guid = null;
            Guid? guid2 = null;
            string text = null;
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                guid = sheetrevaluationtdes.EntGuid;
                guid2 = sheetrevaluationtdes.EntBranchGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    guid = (from e in db.EnterpriseUserTDES
                            where e.EntUserNic == aName
                            select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES != null)
                    {
                        guid = enterpriseBranchIdsTDES.EntGuid;
                        guid2 = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                    else
                    {
                        guid = null;
                        guid2 = null;
                    }
                }
            }
            text = base.User.Identity.Name;
            if (base.ModelState.IsValid && (guid != sheetrevaluationtdes.EntGuid || guid2 != sheetrevaluationtdes.EntBranchGuid || text != sheetrevaluationtdes.EntUserNic))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            ViewBagHelper(guid, guid2);
            if (base.ModelState.IsValid)
            {
                sheetrevaluationtdes.CreatedAt = sheetrevaluationtdes.CreatedAt.AddMilliseconds(-sheetrevaluationtdes.CreatedAt.Millisecond);
                dbIncome.SheetRevaluationTDES.Add(sheetrevaluationtdes);
                dbIncome.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = sheetrevaluationtdes.EntGuid,
                    searchEntBranchGuid = sheetrevaluationtdes.EntBranchGuid
                });
            }
            return View(sheetrevaluationtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public ActionResult Edit(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                }
            }
            SheetRevaluationTDES sheetRevaluationTDES = null;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                sheetRevaluationTDES = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (from e in dbIncome.SheetRevaluationTDES
                                                                                                where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)id
                                                                                                select e).FirstOrDefault() : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (from e in dbIncome.SheetRevaluationTDES
                                                                                                                                                                                                                        where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                        select e).FirstOrDefault() : (from e in dbIncome.SheetRevaluationTDES
                                                                                                                                                                                                                                                      where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                      select e).FirstOrDefault()));
            }
            if (sheetRevaluationTDES == null)
            {
                return HttpNotFound();
            }
            return View(sheetRevaluationTDES);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public ActionResult Edit(SheetRevaluationTDES sheetrevaluationtdes)
        {
            UserIsInRoles();
            Guid? guid = null;
            Guid? guid2 = null;
            string text = null;
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                guid = sheetrevaluationtdes.EntGuid;
                guid2 = sheetrevaluationtdes.EntBranchGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    guid = (from e in db.EnterpriseUserTDES
                            where e.EntUserNic == aName
                            select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES != null)
                    {
                        guid = enterpriseBranchIdsTDES.EntGuid;
                        guid2 = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                    else
                    {
                        guid = null;
                        guid2 = null;
                    }
                }
            }
            text = base.User.Identity.Name;
            if (base.ModelState.IsValid && (guid != sheetrevaluationtdes.EntGuid || guid2 != sheetrevaluationtdes.EntBranchGuid || text != sheetrevaluationtdes.EntUserNic))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            ViewBagHelper(guid, guid2);
            if (base.ModelState.IsValid && sheetrevaluationtdes.IsProcessed)
            {
                base.ModelState.AddModelError("", Resources.SheetRevaluationTDES_CANNOTCANGE);
            }
            if (base.ModelState.IsValid)
            {
                sheetrevaluationtdes.CreatedAt = sheetrevaluationtdes.CreatedAt.AddMilliseconds(-sheetrevaluationtdes.CreatedAt.Millisecond);
                dbIncome.Entry(sheetrevaluationtdes).State = EntityState.Modified;
                dbIncome.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sheetrevaluationtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public ActionResult Delete(Guid? id = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                }
            }
            SheetRevaluationTDES sheetRevaluationTDES = null;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                sheetRevaluationTDES = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (from e in dbIncome.SheetRevaluationTDES
                                                                                                where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)id
                                                                                                select e).FirstOrDefault() : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (from e in dbIncome.SheetRevaluationTDES
                                                                                                                                                                                                                        where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                        select e).FirstOrDefault() : (from e in dbIncome.SheetRevaluationTDES
                                                                                                                                                                                                                                                      where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                      select e).FirstOrDefault()));
            }
            if (sheetRevaluationTDES == null)
            {
                return HttpNotFound();
            }
            return View(sheetRevaluationTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id, Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                }
            }
            SheetRevaluationTDES sheetRevaluationTDES = null;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopIncomeContextCatalog != null)
            {
                sheetRevaluationTDES = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (from e in dbIncome.SheetRevaluationTDES
                                                                                                where e.SheetRevaluationTDESGuid == id
                                                                                                select e).FirstOrDefault() : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (from e in dbIncome.SheetRevaluationTDES
                                                                                                                                                                                                                        where e.SheetRevaluationTDESGuid == id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                        select e).FirstOrDefault() : (from e in dbIncome.SheetRevaluationTDES
                                                                                                                                                                                                                                                      where e.SheetRevaluationTDESGuid == id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                      select e).FirstOrDefault()));
            }
            if (sheetRevaluationTDES == null)
            {
                return HttpNotFound();
            }
            if (sheetRevaluationTDES.IsProcessed)
            {
                base.ModelState.AddModelError("", Resources.SheetRevaluationTDES_CANNOTDELETE);
                return View(sheetRevaluationTDES);
            }
            dbIncome.SheetRevaluationTDES.Remove(sheetRevaluationTDES);
            dbIncome.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            dbIncome.Dispose();
            base.Dispose(disposing);
        }
    }
}
// CarShop.Controllers.SaleBasketController
using CarShop.Models;
using CarShop.Properties;
using CarShop.Utility;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class SaleBasketController : Controller
    {
        private EnterpriseBranchUserTDES enterprisebranchusertdes;

        private BranchSpellTDES branchspelltdes;

        private User2WorkPlaceTDES user2workplacetdes;

        private CarShopContext db = new CarShopContext();

        private string CarShopRestContextCatalog;

        private CarShopRestContext _dbRest;

        private string CarShopSalesContextCatalog;

        private CarShopSalesContext _dbSales;

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
            base.ViewBag.IsBranchAudit = base.User.IsInRole("BranchAudit");
            base.ViewBag.IsBranchSeller = base.User.IsInRole("BranchSeller");
            base.ViewBag.IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null, int? showIsActive = default(int?), int? showIsPaid = default(int?), int? showIsReverse = default(int?), int? currentIsActive = default(int?), int? currentIsPaid = default(int?), int? currentIsIsReverse = default(int?), int? page = default(int?))
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
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        if ((!base.ViewBag.IsBranchAudit))
                        {
                            searchEntUserNic = aName;
                        }
                    }
                }
            }
            IPagedList<SaleBasketTDES> aResult = null;
            if (!showIsActive.HasValue)
            {
                showIsActive = currentIsActive;
            }
            if (!showIsPaid.HasValue)
            {
                showIsPaid = currentIsPaid;
            }
            if (!showIsReverse.HasValue)
            {
                showIsReverse = currentIsIsReverse;
            }
            showIsActive = (showIsActive ?? 1);
            showIsPaid = (showIsPaid ?? 3);
            showIsReverse = (showIsReverse ?? 1);
            showIsActive = ((showIsActive.Value < 1 || showIsActive.Value > 3) ? 1 : showIsActive.Value);
            showIsReverse = ((showIsReverse.Value < 1 || showIsReverse.Value > 3) ? 1 : showIsReverse.Value);
            showIsPaid = ((showIsPaid.Value < 1 || showIsPaid.Value > 3) ? 3 : showIsPaid.Value);
            base.ViewBag.currentIsActive = showIsActive.Value;
            base.ViewBag.currentIsPaid = showIsPaid.Value;
            base.ViewBag.currentIsIsReverse = showIsReverse.Value;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                BranchSpellTDES branchspelltdes = await (from e in dbSales.BranchSpellTDES
                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                         select e).FirstOrDefaultAsync();
                if (branchspelltdes == null)
                {
                    base.ModelState.AddModelError("", Resources.SpellNotFound);
                }
                if (base.ModelState.IsValid)
                {
                    IQueryable<SaleBasketTDES> salebaskettdes2 = from e in dbSales.SaleBasketTDES
                                                                 where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic && e.SpellGuid == branchspelltdes.SpellGuid
                                                                 select e;
                    if (showIsPaid.Value > 1)
                    {
                        bool boolVal = showIsPaid.Value == 2;
                        salebaskettdes2 = from e in salebaskettdes2
                                          where e.IsPaid == boolVal
                                          select e;
                    }
                    if (showIsReverse.Value > 1)
                    {
                        bool boolVal2 = showIsReverse.Value == 2;
                        salebaskettdes2 = from e in salebaskettdes2
                                          where e.IsReverse == boolVal2
                                          select e;
                    }
                    if (showIsActive.Value > 1)
                    {
                        bool boolVal3 = showIsActive.Value == 2;
                        salebaskettdes2 = from e in salebaskettdes2
                                          where e.IsActive == boolVal3
                                          select e;
                    }
                    salebaskettdes2 = from e in salebaskettdes2
                                      orderby e.SpellGuid
                                      select e;
                    int pageSize = 20;
                    int pageNumber = page ?? 1;
                    aResult = await salebaskettdes2.ToPagedListAsync(pageNumber, pageSize);
                }
            }
            base.ViewBag.sliIsActive = SwitcherListUtil.SelectListHelper(showIsActive.Value);
            base.ViewBag.sliIsReverse = SwitcherListUtil.SelectListHelper(showIsReverse.Value);
            base.ViewBag.sliIsPaid = SwitcherListUtil.SelectListHelper(showIsPaid.Value);
            base.ViewBag.SearchEntUserNic = searchEntUserNic;
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            UserIsInRoles();
            string searchEntUserNic = null;
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
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        searchEntUserNic = aName;
                    }
                }
            }
            SaleBasketTDES salebaskettdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                salebaskettdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbSales.SaleBasketTDES
                                                                                                 where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid
                                                                                                 select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbSales.SaleBasketTDES
                                                                                                                                                                                                                                      where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                                                                                                                                                                                                                      select e).FirstOrDefaultAsync()) : (await (from e in dbSales.SaleBasketTDES
                                                                                                                                                                                                                                                                                 where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                 select e).FirstOrDefaultAsync())));
            }
            if (salebaskettdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.searchEntUserNic = salebaskettdes.EntUserNic;
            return View(salebaskettdes);
        }

        protected async Task DefineInfoObject(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
        {
            enterprisebranchusertdes = null;
            branchspelltdes = null;
            user2workplacetdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                branchspelltdes = await (from e in dbSales.BranchSpellTDES
                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                         select e).FirstOrDefaultAsync();
                if (branchspelltdes == null)
                {
                    base.ModelState.AddModelError("", Resources.SpellNotFound);
                }
                else if (!branchspelltdes.IsActive || branchspelltdes.IsBlocked)
                {
                    base.ModelState.AddModelError("", Resources.BranchSpellIsBlockedOrIsNotActive);
                }
                if (base.ModelState.IsValid)
                {
                    enterprisebranchusertdes = await (from e in db.EnterpriseBranchUserTDES
                                                      where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntUserNic == searchEntUserNic
                                                      select e).FirstOrDefaultAsync();
                    if (enterprisebranchusertdes == null)
                    {
                        base.ModelState.AddModelError("", Resources.BranchUserNotFound);
                    }
                    else if (!enterprisebranchusertdes.IsActive)
                    {
                        base.ModelState.AddModelError("", Resources.BranchUserNotActive);
                    }
                    else if (!enterprisebranchusertdes.IsSeller)
                    {
                        base.ModelState.AddModelError("", Resources.BranchUserNotActive);
                    }
                }
                if (base.ModelState.IsValid)
                {
                    user2workplacetdes = await (from e in dbSales.User2WorkPlaceTDES
                                                where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                                select e).FirstOrDefaultAsync();
                    if (user2workplacetdes == null)
                    {
                        base.ModelState.AddModelError("", Resources.BranchUserHasNoWorkPlace);
                    }
                }
            }
            base.ViewBag.searchEntUserNic = searchEntUserNic;
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Create(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
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
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        searchEntUserNic = aName;
                    }
                }
            }
            SaleBasketTDES salebaskettdes = null;
            await DefineInfoObject(searchEntGuid, searchEntBranchGuid, searchEntUserNic);
            if (base.ModelState.IsValid)
            {
                salebaskettdes = new SaleBasketTDES
                {
                    EntBasketGuid = Guid.NewGuid(),
                    WorkPlaceGuid = user2workplacetdes.WorkPlaceGuid,
                    EntUserNic = searchEntUserNic,
                    SetAt = user2workplacetdes.SetAt,
                    Description = user2workplacetdes.Description,
                    SpellGuid = branchspelltdes.SpellGuid,
                    EntBranchGuid = branchspelltdes.EntBranchGuid,
                    EntGuid = branchspelltdes.EntGuid,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    IsPaid = false,
                    PaidAt = DateTime.Now.AddSeconds(2.0),
                    IsReverse = false,
                    ArtAmount = 0,
                    PaymentSum = 0.0
                };
                salebaskettdes.CreatedAt = salebaskettdes.CreatedAt.AddMilliseconds(-salebaskettdes.CreatedAt.Millisecond);
                salebaskettdes.PaidAt = salebaskettdes.PaidAt.AddMilliseconds(-salebaskettdes.PaidAt.Millisecond);
            }
            return View(salebaskettdes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Create(SaleBasketTDES salebaskettdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid;
            Guid? searchEntBranchGuid;
            string searchEntUserNic;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                    searchEntBranchGuid = salebaskettdes.EntBranchGuid;
                    searchEntUserNic = salebaskettdes.EntUserNic;
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
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        searchEntUserNic = aName;
                    }
                }
            }
            else
            {
                searchEntBranchGuid = salebaskettdes.EntBranchGuid;
                searchEntUserNic = salebaskettdes.EntUserNic;
                searchEntGuid = salebaskettdes.EntGuid;
            }
            if (base.ModelState.IsValid && (searchEntGuid != salebaskettdes.EntGuid || searchEntBranchGuid != salebaskettdes.EntBranchGuid || searchEntUserNic != salebaskettdes.EntUserNic))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (salebaskettdes != null)
            {
                await DefineInfoObject(salebaskettdes.EntGuid, salebaskettdes.EntBranchGuid, salebaskettdes.EntUserNic);
            }
            else
            {
                base.ModelState.AddModelError("", Resources.BasketDefinitionIsNotDefined);
            }
            if (base.ModelState.IsValid)
            {
                salebaskettdes.CreatedAt = salebaskettdes.CreatedAt.AddMilliseconds(-salebaskettdes.CreatedAt.Millisecond);
                if (salebaskettdes.CreatedAt < branchspelltdes.OpenAt)
                {
                    base.ModelState.AddModelError("", Resources.BasketCreateDateLessThanSpellDate);
                }
            }
            if (base.ModelState.IsValid)
            {
                salebaskettdes.PaidAt = salebaskettdes.CreatedAt.AddSeconds(2.0);
            }
            if (base.ModelState.IsValid && salebaskettdes.IsActive)
            {
                foreach (SaleBasketTDES item in await (from e in dbSales.SaleBasketTDES
                                                       where e.EntUserNic == salebaskettdes.EntUserNic && e.EntGuid == salebaskettdes.EntGuid && e.EntBranchGuid == salebaskettdes.EntBranchGuid && e.SpellGuid == salebaskettdes.SpellGuid && e.IsActive == true
                                                       select e).ToListAsync())
                {
                    item.IsActive = false;
                    dbSales.Entry(item).State = EntityState.Modified;
                }
            }
            if (base.ModelState.IsValid)
            {
                salebaskettdes.ArtAmount = 0;
                salebaskettdes.PaymentSum = 0.0;
                dbSales.SaleBasketTDES.Add(salebaskettdes);
                await dbSales.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = salebaskettdes.EntGuid,
                    searchEntBranchGuid = salebaskettdes.EntBranchGuid,
                    searchEntUserNic = salebaskettdes.EntUserNic
                });
            }
            return View(salebaskettdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Edit(Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
                        string text = aName;
                    }
                }
            }
            SaleBasketTDES salebaskettdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                        where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid
                                        select e).FirstOrDefaultAsync();
            }
            if (salebaskettdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.searchEntUserNic = salebaskettdes.EntUserNic;
            return View(salebaskettdes);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SaleBasketTDES salebaskettdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid;
            Guid? searchEntBranchGuid;
            string searchEntUserNic;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                    searchEntBranchGuid = salebaskettdes.EntBranchGuid;
                    searchEntUserNic = salebaskettdes.EntUserNic;
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
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        searchEntUserNic = aName;
                    }
                }
            }
            else
            {
                searchEntBranchGuid = salebaskettdes.EntBranchGuid;
                searchEntUserNic = salebaskettdes.EntUserNic;
                searchEntGuid = salebaskettdes.EntGuid;
            }
            if (base.ModelState.IsValid && (searchEntGuid != salebaskettdes.EntGuid || searchEntBranchGuid != salebaskettdes.EntBranchGuid || searchEntUserNic != salebaskettdes.EntUserNic))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (salebaskettdes != null)
            {
                await DefineInfoObject(salebaskettdes.EntGuid, salebaskettdes.EntBranchGuid, salebaskettdes.EntUserNic);
            }
            else
            {
                base.ModelState.AddModelError("", Resources.BasketDefinitionIsNotDefined);
            }
            if (base.ModelState.IsValid)
            {
                salebaskettdes.CreatedAt = salebaskettdes.CreatedAt.AddMilliseconds(-salebaskettdes.CreatedAt.Millisecond);
                if (salebaskettdes.CreatedAt < branchspelltdes.OpenAt)
                {
                    base.ModelState.AddModelError("", Resources.BasketCreateDateLessThanSpellDate);
                }
            }
            if (base.ModelState.IsValid)
            {
                salebaskettdes.PaidAt = salebaskettdes.CreatedAt.AddSeconds(2.0);
            }
            if (base.ModelState.IsValid && salebaskettdes.IsActive)
            {
                foreach (SaleBasketTDES item in await (from e in dbSales.SaleBasketTDES
                                                       where e.EntUserNic == salebaskettdes.EntUserNic && e.EntGuid == salebaskettdes.EntGuid && e.EntBranchGuid == salebaskettdes.EntBranchGuid && e.SpellGuid == salebaskettdes.SpellGuid && e.IsActive == true
                                                       select e).ToListAsync())
                {
                    if (salebaskettdes.EntBasketGuid != item.EntBasketGuid)
                    {
                        item.IsActive = false;
                        dbSales.Entry(item).State = EntityState.Modified;
                    }
                }
            }
            if (base.ModelState.IsValid)
            {
                SaleBasketTDES loc = dbSales.SaleBasketTDES.Local.FirstOrDefault((SaleBasketTDES e) => e.EntBasketGuid == salebaskettdes.EntBasketGuid);
                if (loc != null)
                {
                    dbSales.Entry(loc).State = EntityState.Detached;
                }
                dbSales.Entry(salebaskettdes).State = EntityState.Modified;
                await dbSales.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = salebaskettdes.EntGuid,
                    searchEntBranchGuid = salebaskettdes.EntBranchGuid,
                    searchEntUserNic = salebaskettdes.EntUserNic
                });
            }
            return View(salebaskettdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Delete(Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            UserIsInRoles();
            string searchEntUserNic = null;
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
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        searchEntUserNic = aName;
                    }
                }
            }
            SaleBasketTDES salebaskettdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                salebaskettdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbSales.SaleBasketTDES
                                                                                                 where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid
                                                                                                 select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbSales.SaleBasketTDES
                                                                                                                                                                                                                                      where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                                                                                                                                                                                                                      select e).FirstOrDefaultAsync()) : (await (from e in dbSales.SaleBasketTDES
                                                                                                                                                                                                                                                                                 where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                 select e).FirstOrDefaultAsync())));
            }
            if (salebaskettdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.searchEntUserNic = salebaskettdes.EntUserNic;
            return View(salebaskettdes);
        }

        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> DeleteConfirmed(Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            UserIsInRoles();
            string searchEntUserNic = null;
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
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        searchEntUserNic = aName;
                    }
                }
            }
            SaleBasketTDES salebaskettdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                salebaskettdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbSales.SaleBasketTDES
                                                                                                 where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid
                                                                                                 select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbSales.SaleBasketTDES
                                                                                                                                                                                                                                      where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                                                                                                                                                                                                                      select e).FirstOrDefaultAsync()) : (await (from e in dbSales.SaleBasketTDES
                                                                                                                                                                                                                                                                                 where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                 select e).FirstOrDefaultAsync())));
            }
            if (salebaskettdes != null)
            {
                if (salebaskettdes.IsPaid)
                {
                    base.ModelState.AddModelError("", Resources.CanNotDeletePaidBasket);
                    base.ViewBag.searchEntUserNic = salebaskettdes.EntUserNic;
                    return View(salebaskettdes);
                }
                branchspelltdes = await (from e in dbSales.BranchSpellTDES
                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                         select e).FirstOrDefaultAsync();
                if (branchspelltdes == null)
                {
                    base.ModelState.AddModelError("", Resources.SpellNotFound);
                    base.ViewBag.searchEntUserNic = salebaskettdes.EntUserNic;
                    return View(salebaskettdes);
                }
                if (!branchspelltdes.IsActive || branchspelltdes.IsBlocked)
                {
                    base.ModelState.AddModelError("", Resources.BranchSpellIsBlockedOrIsNotActive);
                    base.ViewBag.searchEntUserNic = salebaskettdes.EntUserNic;
                    return View(salebaskettdes);
                }
                dbSales.SaleBasketTDES.Remove(salebaskettdes);
                await dbSales.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = salebaskettdes.EntGuid,
                    searchEntBranchGuid = salebaskettdes.EntBranchGuid,
                    searchEntUserNic = salebaskettdes.EntUserNic
                });
            }
            return RedirectToAction("Index", new
            {
                searchEntGuid,
                searchEntBranchGuid
            });
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> MakePayment(Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            UserIsInRoles();
            string searchEntUserNic = null;
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
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        searchEntUserNic = aName;
                    }
                }
            }
            await DefineInfoObject(searchEntGuid, searchEntBranchGuid, searchEntUserNic);
            SaleBasketTDES salebaskettdes = null;
            if (CarShopSalesContextCatalog != null)
            {
                salebaskettdes = await (from e in dbSales.SaleBasketTDES.Include("SaleBasketArticleTDES")
                                        where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid
                                        select e).FirstOrDefaultAsync();
            }
            if (salebaskettdes == null)
            {
                return HttpNotFound();
            }
            if (!base.ModelState.IsValid)
            {
                return View("Details", salebaskettdes);
            }
            if (salebaskettdes.IsPaid)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketTDES_ISPAYED);
                return View("Details", salebaskettdes);
            }
            if (salebaskettdes.WorkPlaceGuid != user2workplacetdes.WorkPlaceGuid)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketTDES_WP_ERROR);
                return View("Details", salebaskettdes);
            }
            if (salebaskettdes.SpellGuid != branchspelltdes.SpellGuid)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketTDES_SPELL_ERROR);
                return View("Details", salebaskettdes);
            }
            if (salebaskettdes.EntUserNic != enterprisebranchusertdes.EntUserNic)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketTDES_USER_ERROR);
                return View("Details", salebaskettdes);
            }
            DateTime PaymentDate = DateTime.Now;
            PaymentDate = PaymentDate.AddMilliseconds(-PaymentDate.Millisecond);
            if (PaymentDate < branchspelltdes.OpenAt)
            {
                PaymentDate = branchspelltdes.OpenAt.AddSeconds(5.0);
            }
            List<SaleBasketArticleTDES> Items = salebaskettdes.SaleBasketArticleTDES.ToList();
            foreach (SaleBasketArticleTDES Item in Items)
            {
                if (!Item.IsPaid)
                {
                    if (Item.ArtAmount < 1)
                    {
                        Item.IsPaid = true;
                        Item.PaidAt = PaymentDate;
                        try
                        {
                            dbSales.SaveChanges();
                        }
                        catch
                        {
                            base.ModelState.AddModelError("", Resources.SaleBasketTDES_PAYMENTERROR + ":" + Item.EntArticle + ":" + Item.EntBrandNic);
                            return View("Details", salebaskettdes);
                        }
                    }
                    else
                    {
                        BranchRestTDES branchresttdes = await (from e in dbRest.BranchRestTDES
                                                               where e.EntBranchGuid == salebaskettdes.EntBranchGuid && e.EntBranchArticle == Item.EntArticle && e.EntBranchSup == Item.EntBrandNic
                                                               select e).FirstOrDefaultAsync();
                        if (branchresttdes == null)
                        {
                            BranchRestArticleDescriptionTDES descr = await (from e in dbRest.BranchRestArticleDescriptionTDES
                                                                            where e.EntArticleDescription == Item.SaleArticleDescriptionTDES.EntArticleDescription
                                                                            select e).FirstOrDefaultAsync();
                            if (descr == null)
                            {
                                descr = new BranchRestArticleDescriptionTDES
                                {
                                    EntArticleDescription = Item.SaleArticleDescriptionTDES.EntArticleDescription
                                };
                                try
                                {
                                    dbRest.BranchRestArticleDescriptionTDES.Add(descr);
                                    await dbRest.SaveChangesAsync();
                                }
                                catch
                                {
                                    base.ModelState.AddModelError("", Resources.SaleBasketTDES_RESTERROR + ":" + Item.EntArticle + ":" + Item.EntBrandNic);
                                    return View("Details", salebaskettdes);
                                }
                            }
                            branchresttdes = new BranchRestTDES
                            {
                                EntBranchGuid = salebaskettdes.EntBranchGuid,
                                EntBranchArticle = Item.EntArticle,
                                EntBranchSup = Item.EntBrandNic,
                                ART_ARTICLE_NR = Item.ExternArticle,
                                SUP_TEXT = Item.ExternBrandNic,
                                ExternArticleEAN = Item.ExternArticleEAN,
                                ArtAmount = 0,
                                ArtPrice = Item.SalePrice,
                                LastUpdated = PaymentDate,
                                LastReplicated = PaymentDate,
                                EntArticleDescriptionId = descr.EntArticleDescriptionId
                            };
                            try
                            {
                                dbRest.BranchRestTDES.Add(branchresttdes);
                                await dbRest.SaveChangesAsync();
                            }
                            catch
                            {
                                base.ModelState.AddModelError("", Resources.SaleBasketTDES_RESTERROR + ":" + Item.EntArticle + ":" + Item.EntBrandNic);
                                return View("Details", salebaskettdes);
                            }
                        }
                        try
                        {
                            using (TransactionScope transactionScope = new TransactionScope())
                            {
                                branchresttdes.ArtAmount -= Item.ArtAmount;
                                branchresttdes.LastUpdated = PaymentDate;
                                dbRest.SaveChanges();
                                Item.IsPaid = true;
                                Item.PaidAt = PaymentDate;
                                dbSales.SaveChanges();
                                transactionScope.Complete();
                            }
                        }
                        catch
                        {
                            base.ModelState.AddModelError("", Resources.SaleBasketTDES_DISTRIBUTEDERROR + ":" + Item.EntArticle + ":" + Item.EntBrandNic);
                            return View("Details", salebaskettdes);
                        }
                    }
                }
            }
            salebaskettdes.IsPaid = true;
            salebaskettdes.PaidAt = PaymentDate;
            try
            {
                await dbSales.SaveChangesAsync();
                base.ModelState.AddModelError("", Resources.SaleBasketTDES_PAYMENT_OK);
            }
            catch
            {
                base.ModelState.AddModelError("", Resources.SaleBasketTDES_PAYMENTERROR_STATUS);
                return View("Details", salebaskettdes);
            }
            return View("Details", salebaskettdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MakeReverse(Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
        {
            UserIsInRoles();
            string searchEntUserNic = null;
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
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        searchEntUserNic = aName;
                    }
                }
            }
            await DefineInfoObject(searchEntGuid, searchEntBranchGuid, searchEntUserNic);
            SaleBasketTDES salebaskettdes = null;
            if (CarShopSalesContextCatalog != null)
            {
                salebaskettdes = await (from e in dbSales.SaleBasketTDES.Include("SaleBasketArticleTDES")
                                        where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid
                                        select e).FirstOrDefaultAsync();
            }
            if (salebaskettdes == null)
            {
                return HttpNotFound();
            }
            if (!base.ModelState.IsValid)
            {
                return View("Details", salebaskettdes);
            }
            if (salebaskettdes.IsReverse)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketTDES_ISREVERCED);
                return View("Details", salebaskettdes);
            }
            if (salebaskettdes.WorkPlaceGuid != user2workplacetdes.WorkPlaceGuid)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketTDES_WP_ERROR);
                return View("Details", salebaskettdes);
            }
            if (salebaskettdes.SpellGuid != branchspelltdes.SpellGuid)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketTDES_SPELL_ERROR);
                return View("Details", salebaskettdes);
            }
            if (salebaskettdes.EntUserNic != enterprisebranchusertdes.EntUserNic)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketTDES_USER_ERROR);
                return View("Details", salebaskettdes);
            }
            DateTime PaymentDate = DateTime.Now;
            PaymentDate = PaymentDate.AddMilliseconds(-PaymentDate.Millisecond);
            if (PaymentDate < branchspelltdes.OpenAt)
            {
                PaymentDate = branchspelltdes.OpenAt.AddSeconds(5.0);
            }
            List<SaleBasketArticleTDES> Items = salebaskettdes.SaleBasketArticleTDES.ToList();
            foreach (SaleBasketArticleTDES Item in Items)
            {
                if (!Item.IsPaid || Item.ArtAmount != Item.ReverseAmount)
                {
                    if (Item.ArtAmount < 1 || Item.ArtAmount == Item.ReverseAmount)
                    {
                        Item.IsPaid = true;
                        Item.PaidAt = PaymentDate;
                        try
                        {
                            await dbSales.SaveChangesAsync();
                        }
                        catch
                        {
                            base.ModelState.AddModelError("", Resources.SaleBasketTDES_PAYMENTERROR + ":" + Item.EntArticle + ":" + Item.EntBrandNic);
                            return View("Details", salebaskettdes);
                        }
                    }
                    else
                    {
                        BranchRestTDES branchresttdes = await (from e in dbRest.BranchRestTDES
                                                               where e.EntBranchGuid == salebaskettdes.EntBranchGuid && e.EntBranchArticle == Item.EntArticle && e.EntBranchSup == Item.EntBrandNic
                                                               select e).FirstOrDefaultAsync();
                        if (branchresttdes == null)
                        {
                            BranchRestArticleDescriptionTDES descr = await (from e in dbRest.BranchRestArticleDescriptionTDES
                                                                            where e.EntArticleDescription == Item.SaleArticleDescriptionTDES.EntArticleDescription
                                                                            select e).FirstOrDefaultAsync();
                            if (descr == null)
                            {
                                descr = new BranchRestArticleDescriptionTDES
                                {
                                    EntArticleDescription = Item.SaleArticleDescriptionTDES.EntArticleDescription
                                };
                                try
                                {
                                    dbRest.BranchRestArticleDescriptionTDES.Add(descr);
                                    await dbRest.SaveChangesAsync();
                                }
                                catch
                                {
                                    base.ModelState.AddModelError("", Resources.SaleBasketTDES_RESTERROR + ":" + Item.EntArticle + ":" + Item.EntBrandNic);
                                    return View("Details", salebaskettdes);
                                }
                            }
                            branchresttdes = new BranchRestTDES
                            {
                                EntBranchGuid = salebaskettdes.EntBranchGuid,
                                EntBranchArticle = Item.EntArticle,
                                EntBranchSup = Item.EntBrandNic,
                                ART_ARTICLE_NR = Item.ExternArticle,
                                SUP_TEXT = Item.ExternBrandNic,
                                ExternArticleEAN = Item.ExternArticleEAN,
                                ArtAmount = 0,
                                ArtPrice = Item.SalePrice,
                                LastUpdated = PaymentDate,
                                LastReplicated = PaymentDate,
                                EntArticleDescriptionId = descr.EntArticleDescriptionId
                            };
                            try
                            {
                                dbRest.BranchRestTDES.Add(branchresttdes);
                                await dbRest.SaveChangesAsync();
                            }
                            catch
                            {
                                base.ModelState.AddModelError("", Resources.SaleBasketTDES_RESTERROR + ":" + Item.EntArticle + ":" + Item.EntBrandNic);
                                return View("Details", salebaskettdes);
                            }
                        }
                        try
                        {
                            using (TransactionScope transactionScope = new TransactionScope())
                            {
                                branchresttdes.ArtAmount = branchresttdes.ArtAmount + Item.ArtAmount - Item.ReverseAmount;
                                branchresttdes.LastUpdated = PaymentDate;
                                Item.IsPaid = true;
                                Item.PaidAt = PaymentDate;
                                Item.ReverseAmount = Item.ArtAmount;
                                dbSales.SaveChanges();
                                dbRest.SaveChanges();
                                transactionScope.Complete();
                            }
                        }
                        catch
                        {
                            base.ModelState.AddModelError("", Resources.SaleBasketTDES_DISTRIBUTEDERROR + ":" + Item.EntArticle + ":" + Item.EntBrandNic);
                            return View("Details", salebaskettdes);
                        }
                    }
                }
            }
            if (!salebaskettdes.IsPaid)
            {
                salebaskettdes.PaidAt = PaymentDate;
            }
            salebaskettdes.IsPaid = true;
            salebaskettdes.IsReverse = true;
            try
            {
                dbSales.SaveChanges();
                base.ModelState.AddModelError("", Resources.SaleBasketTDES_PAYMENT_OK);
            }
            catch
            {
                base.ModelState.AddModelError("", Resources.SaleBasketTDES_PAYMENTERROR_STATUS);
                return View("Details", salebaskettdes);
            }
            return View("Details", salebaskettdes);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        [HttpPost]
        public async Task<ActionResult> MakePaymentOrReverse(Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string MAKEPAYMENT = null, string MAKEREVERSE = null)
        {
            if (MAKEPAYMENT != null)
            {
                return await MakePayment(searchEntBasketGuid, searchEntGuid, searchEntBranchGuid);
            }
            return await MakeReverse(searchEntBasketGuid, searchEntGuid, searchEntBranchGuid);
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbRest != null)
            {
                _dbRest.Dispose();
                _dbRest = null;
            }
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
// CarShop.Controllers.BranchOrderSearchController
using CarShop.Models;
using CarShop.Properties;
using CarShop.Utility;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class BranchOrderSearchController : Controller
    {
        private Guid? guestEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private Guid? guestEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private EnterpriseBranchUserTDES enterprisebranchusertdes;

        private BranchSpellTDES branchspelltdes;

        private User2WorkPlaceTDES user2workplacetdes;

        private CarShopContext db = new CarShopContext();

        private string CarShopOrderContextCatalog;

        private CarShopOrdersContext _dbOrders;

        private string CarShopSalesContextCatalog;

        private CarShopSalesContext _dbSales;

        private string CarShopRestContextCatalog;

        private CarShopRestContext _dbRest;

        private string CarShopGestRestContextCatalog;

        private CarShopRestContext _dbGestRest;

        private CarShopOrdersContext dbOrders
        {
            get
            {
                if (_dbOrders == null)
                {
                    _dbOrders = new CarShopOrdersContext(CarShopOrderContextCatalog);
                }
                return _dbOrders;
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

        private CarShopRestContext dbGestRest
        {
            get
            {
                if (_dbGestRest == null)
                {
                    _dbGestRest = new CarShopRestContext(CarShopGestRestContextCatalog);
                }
                return _dbGestRest;
            }
        }

        public async Task PreparedbRest()
        {
            EnterpriseBranchTDES enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)guestEntBranchGuid
                                                               select e).FirstOrDefaultAsync();
            if (enterprisebranchtdes != null)
            {
                CarShopGestRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
                return;
            }
            CarShopGestRestContextCatalog = null;
            base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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
                CarShopOrderContextCatalog = enterprisebranchtdes.OrderCatalog;
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
                base.ViewBag.SearchEntGuid = searchEntGuid;
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
        public async Task<ActionResult> Index(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), int? showIsActive = default(int?), string searchString = null, string searchString1 = null, int? currentFilterIsActive = default(int?), string currentFilter = null, string currentFilter1 = null, int? page = default(int?))
        {
            if (searchString != null || searchString1 != null || showIsActive.HasValue)
            {
                page = 1;
            }
            else
            {
                page = (page ?? 1);
                searchString = currentFilter;
                searchString1 = currentFilter1;
                showIsActive = currentFilterIsActive;
            }
            showIsActive = (showIsActive ?? 3);
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
            }
            if (!string.IsNullOrEmpty(searchString1))
            {
                searchString1 = searchString1.Trim();
            }
            base.ViewBag.sliIsActive = SwitcherListUtil.SelectListHelper(showIsActive.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.CurrentFilter1 = searchString1;
            base.ViewBag.CurrentFilterIsActive = showIsActive;
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
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View();
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
            IPagedList<GuestOrderTDES> aResult = null;
            if (CarShopOrderContextCatalog != null)
            {
                IQueryable<GuestOrderTDES> q2 = from e in dbOrders.GuestOrderTDES.Include((GuestOrderTDES g) => g.GuestProfileTDES)
                                                where e.EntBranchGuid == searchEntBranchGuid.Value
                                                select e;
                if (showIsActive.Value == 2)
                {
                    q2 = from e in q2
                         where e.IsDone == true
                         select e;
                }
                if (showIsActive.Value == 3)
                {
                    q2 = from e in q2
                         where e.IsDone == false
                         select e;
                }
                if (openAt.HasValue)
                {
                    q2 = from e in q2
                         where (DateTime)(DateTime?)e.LastUpdated >= (DateTime)openAt
                         select e;
                }
                if (closeAt.HasValue)
                {
                    q2 = from e in q2
                         where (DateTime)(DateTime?)e.LastUpdated <= (DateTime)closeAt
                         select e;
                }
                q2 = from e in q2
                     orderby e.EntBranchGuid
                     select e;
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await q2.ToPagedListAsync(pageNumber, pageSize);
            }
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? id = default(Guid?))
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
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES.Include("GuestProfileTDES")
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)id && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                return HttpNotFound();
            }
            return View(guestordertdes);
        }

        [ActionName("Details")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        [HttpPost]
        public async Task<ActionResult> CreateSaleOrResetPrice(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? id = default(Guid?), string CREATESALE = null, string RESETPRICES = null)
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
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        searchEntUserNic = aName;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES.Include("GuestProfileTDES")
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)id && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                return HttpNotFound();
            }
            if (guestordertdes.GuestOrderArticleTDESes.Count < 1)
            {
                base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOARTCOUNT);
                return View(guestordertdes);
            }
            if (CREATESALE != null)
            {
                if (string.IsNullOrEmpty(CarShopSalesContextCatalog))
                {
                    base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                    return View(guestordertdes);
                }
                await DefineInfoObject(searchEntGuid, searchEntBranchGuid, searchEntUserNic);
                if (!base.ModelState.IsValid)
                {
                    return View(guestordertdes);
                }
                SaleBasketTDES salebaskettdes = new SaleBasketTDES
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
                    IsActive = false,
                    IsPaid = false,
                    PaidAt = DateTime.Now.AddSeconds(2.0),
                    IsReverse = false,
                    ArtAmount = 0,
                    PaymentSum = 0.0,
                    Comments = Resources.SaleBasketTDES_BY_GuestOrderGuid + guestordertdes.GuestOrderGuid.ToString()
                };
                salebaskettdes.CreatedAt = salebaskettdes.CreatedAt.AddMilliseconds(-salebaskettdes.CreatedAt.Millisecond);
                salebaskettdes.PaidAt = salebaskettdes.PaidAt.AddMilliseconds(-salebaskettdes.PaidAt.Millisecond);
                dbSales.SaleBasketTDES.Add(salebaskettdes);
                await dbSales.SaveChangesAsync();
                foreach (GuestOrderArticleTDES itm in guestordertdes.GuestOrderArticleTDESes)
                {
                    SaleArticleDescriptionTDES desr = (from e in dbSales.SaleArticleDescriptionTDES
                                                       where e.EntArticleDescription == itm.EntArticleDescription
                                                       select e).FirstOrDefault();
                    if (desr == null)
                    {
                        desr = new SaleArticleDescriptionTDES
                        {
                            EntArticleDescription = itm.EntArticleDescription
                        };
                        dbSales.SaleArticleDescriptionTDES.Add(desr);
                        await dbSales.SaveChangesAsync();
                    }
                    SaleBasketArticleTDES salebasketarticletdes = new SaleBasketArticleTDES
                    {
                        EntArticle = itm.EntBranchArticle,
                        EntBrandNic = itm.EntBranchSup,
                        EntBasketGuid = salebaskettdes.EntBasketGuid,
                        ExternArticle = itm.ART_ARTICLE_NR,
                        ExternBrandNic = itm.SUP_TEXT,
                        ExternArticleEAN = itm.ExternArticleEAN,
                        EntGuid = salebaskettdes.EntGuid,
                        EntBranchGuid = salebaskettdes.EntBranchGuid,
                        EntUserNic = salebaskettdes.EntUserNic,
                        WorkPlaceGuid = salebaskettdes.WorkPlaceGuid,
                        SpellGuid = salebaskettdes.SpellGuid,
                        IsPaid = false,
                        PaidAt = DateTime.Now,
                        ArtAmount = itm.ArtAmount,
                        SalePrice = itm.ArtPrice,
                        ReverseAmount = 0,
                        IsSpellClosed = false,
                        CribFromIncome = 0
                    };
                    salebasketarticletdes.PaidAt = salebasketarticletdes.PaidAt.AddMilliseconds(-salebasketarticletdes.PaidAt.Millisecond);
                    salebasketarticletdes.EntArticleDescriptionId = desr.EntArticleDescriptionId;
                    salebaskettdes.PaymentSum += (double)salebasketarticletdes.ArtAmount * salebasketarticletdes.SalePrice;
                    salebaskettdes.ArtAmount += salebasketarticletdes.ArtAmount;
                    dbSales.SaleBasketArticleTDES.Add(salebasketarticletdes);
                    await dbSales.SaveChangesAsync();
                }
                guestordertdes.IsDone = true;
                dbOrders.SaveChanges();
                return RedirectToAction("Details", "SaleBasket", new
                {
                    searchEntBasketGuid = salebaskettdes.EntBasketGuid,
                    searchEntGuid = searchEntGuid,
                    searchEntBranchGuid = searchEntBranchGuid
                });
            }
            if (string.IsNullOrEmpty(CarShopRestContextCatalog))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View(guestordertdes);
            }
            foreach (GuestOrderArticleTDES itm2 in guestordertdes.GuestOrderArticleTDESes)
            {
                BranchRestTDES branchresttdes = await (from e in dbRest.BranchRestTDES.AsNoTracking()
                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntBranchArticle == itm2.EntBranchArticle && e.EntBranchSup == itm2.EntBranchSup
                                                       select e).FirstOrDefaultAsync();
                if (branchresttdes != null)
                {
                    itm2.ArtPrice = branchresttdes.ArtPrice;
                    await dbOrders.SaveChangesAsync();
                }
                else
                {
                    if (string.IsNullOrEmpty(CarShopGestRestContextCatalog))
                    {
                        await PreparedbRest();
                    }
                    if (!string.IsNullOrEmpty(CarShopGestRestContextCatalog))
                    {
                        var suppRest = await (from e in dbGestRest.SuppRestTDES
                                              where e.EntBranchArticle == itm2.EntBranchArticle && e.EntBranchSup == itm2.EntBranchSup
                                              join b in dbGestRest.BranchSuppTDES on e.EntSupplierId equals b.EntSupplierId
                                              where (Guid)(Guid?)b.EntBranchGuid == (Guid)searchEntBranchGuid
                                              orderby e.EntSupplierId, e.EntBranchArticle, e.EntBranchSup
                                              select new
                                              {
                                                  e.ArtPrice,
                                                  b.ExchRate,
                                                  b.Rounding,
                                                  b.Multiplexer
                                              }).FirstOrDefaultAsync();
                        if (suppRest != null)
                        {
                            itm2.ArtPrice = Math.Round(suppRest.ArtPrice * suppRest.ExchRate * suppRest.Multiplexer / suppRest.Rounding) * suppRest.Rounding;
                            await dbOrders.SaveChangesAsync();
                        }
                        else
                        {
                            base.ModelState.AddModelError("", Resources.BranchRestTDES_NOTFOUND + " : " + itm2.EntBranchArticle + " : " + itm2.EntBranchSup);
                        }
                    }
                    else
                    {
                        base.ModelState.AddModelError("", Resources.BranchRestTDES_NOTFOUND + " : " + itm2.EntBranchArticle + " : " + itm2.EntBranchSup);
                    }
                }
            }
            base.ModelState.AddModelError("", Resources.GuestOrderTDES_RESETPRICES_ISENDED);
            return View(guestordertdes);
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbOrders != null)
            {
                _dbOrders.Dispose();
                _dbOrders = null;
            }
            if (_dbGestRest != null)
            {
                _dbGestRest.Dispose();
                _dbGestRest = null;
            }
            if (_dbSales != null)
            {
                _dbSales.Dispose();
                _dbSales = null;
            }
            if (_dbRest != null)
            {
                _dbRest.Dispose();
                _dbRest = null;
            }
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
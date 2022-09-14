// CarShop.Controllers.User2WorkPlaceHstController
using CarShop.Models;
using CarShop.Properties;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{
    [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
    public class User2WorkPlaceHstController : Controller
    {
        private CarShopContext db = new CarShopContext();

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
            EnterpriseBranchTDES enterprisebranchtdes = await db.EnterpriseBranchTDES.Where((EnterpriseBranchTDES e) => (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefaultAsync();
            if (enterprisebranchtdes != null)
            {
                base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
                base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
                base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
                base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
                CarShopSalesContextCatalog = enterprisebranchtdes.SalesCatalog;
                return;
            }
            base.ViewBag.EntBranchDescription = Resources.EnterpriseBranchTDES_NOTFOUND;
            base.ViewBag.SearchEntBranchGuid = null;
            base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            EnterpriseTDES enterprises = await db.EnterpriseTDES.Where((EnterpriseTDES e) => (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid).FirstOrDefaultAsync();
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
        }

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
            IPagedList<User2WorkPlaceHstTDES> aResult = null;
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
                IQueryable<User2WorkPlaceHstTDES> q2 = dbSales.User2WorkPlaceHstTDES.Where((User2WorkPlaceHstTDES e) => e.EntBranchGuid == searchEntBranchGuid.Value);
                if (openAt.HasValue)
                {
                    q2 = q2.Where((User2WorkPlaceHstTDES e) => (DateTime)(DateTime?)e.SetAt >= (DateTime)openAt);
                }
                if (closeAt.HasValue)
                {
                    q2 = q2.Where((User2WorkPlaceHstTDES e) => (DateTime)(DateTime?)e.SetAt <= (DateTime)closeAt);
                }
                q2 = q2.OrderBy((User2WorkPlaceHstTDES e) => e.SetAt);
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await q2.ToPagedListAsync(pageNumber, pageSize);
            }
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.CurrentFilter1 = searchString1;
            return View(aResult);
        }

        public async Task<ActionResult> Details(DateTime? searchSetAt = null, Guid? searchWorkPlaceGuid = null, string searchEntUserNic = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null)
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
            User2WorkPlaceHstTDES user2workplacehsttdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                user2workplacehsttdes = await dbSales.User2WorkPlaceHstTDES.Where((User2WorkPlaceHstTDES e) => e.SetAt == searchSetAt.Value && e.WorkPlaceGuid == searchWorkPlaceGuid.Value && e.EntUserNic == searchEntUserNic).FirstOrDefaultAsync();
            }
            if (user2workplacehsttdes == null)
            {
                return HttpNotFound();
            }
            return View(user2workplacehsttdes);
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
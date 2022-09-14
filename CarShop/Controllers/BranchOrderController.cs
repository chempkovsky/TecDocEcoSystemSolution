// CarShop.Controllers.BranchOrderController
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

    public class BranchOrderController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopOrderContextCatalog;

        private CarShopOrdersContext _dbOrders;

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
        public async Task<ActionResult> Index(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchGestUserNic = null, int? page = default(int?))
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
            if (string.IsNullOrEmpty(searchGestUserNic))
            {
                base.ModelState.AddModelError("", Resources.GestUserNic_NOT_DEFINED);
                return View();
            }
            page = (page ?? 1);
            int pageSize = 20;
            int pageNumber = page.Value;
            IOrderedQueryable<GuestOrderTDES> guestordertdes = from e in dbOrders.GuestOrderTDES
                                                               where e.GestUserNic == searchGestUserNic && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               orderby e.LastUpdated descending
                                                               select e;
            IPagedList<GuestOrderTDES> aResult = await guestordertdes.ToPagedListAsync(pageNumber, pageSize);
            base.ViewBag.SearchGestUserNic = searchGestUserNic;
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchGestUserNic = null, Guid? searchGuestOrderGuid = default(Guid?))
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
            if (string.IsNullOrEmpty(searchGestUserNic))
            {
                base.ModelState.AddModelError("", Resources.GestUserNic_NOT_DEFINED);
                return View();
            }
            base.ViewBag.SearchGestUserNic = searchGestUserNic;
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                return HttpNotFound();
            }
            return View(guestordertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Edit(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchGestUserNic = null, Guid? searchGuestOrderGuid = default(Guid?))
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
            if (string.IsNullOrEmpty(searchGestUserNic))
            {
                base.ModelState.AddModelError("", Resources.GestUserNic_NOT_DEFINED);
                return View();
            }
            base.ViewBag.SearchGestUserNic = searchGestUserNic;
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                return HttpNotFound();
            }
            return View(guestordertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GuestOrderTDES guestordertdes)
        {
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = guestordertdes.EntBranchGuid;
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
                base.ViewBag.SearchGestUserNic = guestordertdes.GestUserNic;
                return View(guestordertdes);
            }
            if (guestordertdes.IsDone)
            {
                guestordertdes.IsActive = false;
            }
            guestordertdes.LastUpdated = DateTime.Now;
            guestordertdes.LastUpdated = guestordertdes.LastUpdated.AddMilliseconds(-guestordertdes.LastUpdated.Millisecond);
            if (guestordertdes.IsActive)
            {
                foreach (GuestOrderTDES item in await (from e in dbOrders.GuestOrderTDES
                                                       where e.GestUserNic == guestordertdes.GestUserNic && e.GuestOrderGuid != guestordertdes.GuestOrderGuid && e.EntBranchGuid == guestordertdes.EntBranchGuid
                                                       orderby e.LastUpdated
                                                       select e).ToListAsync())
                {
                    if (item.IsActive)
                    {
                        item.IsActive = false;
                    }
                }
            }
            dbOrders.Entry(guestordertdes).State = EntityState.Modified;
            await dbOrders.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid = searchEntGuid,
                searchEntBranchGuid = searchEntBranchGuid,
                searchGestUserNic = guestordertdes.GestUserNic
            });
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Delete(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchGestUserNic = null, Guid? searchGuestOrderGuid = default(Guid?))
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
            if (string.IsNullOrEmpty(searchGestUserNic))
            {
                base.ModelState.AddModelError("", Resources.GestUserNic_NOT_DEFINED);
                return View();
            }
            base.ViewBag.SearchGestUserNic = searchGestUserNic;
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                return HttpNotFound();
            }
            return View(guestordertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchGestUserNic = null, Guid? searchGuestOrderGuid = default(Guid?))
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
            if (string.IsNullOrEmpty(searchGestUserNic))
            {
                base.ModelState.AddModelError("", Resources.GestUserNic_NOT_DEFINED);
                return View();
            }
            base.ViewBag.SearchGestUserNic = searchGestUserNic;
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                return HttpNotFound();
            }
            string GestUserNic = guestordertdes.GestUserNic;
            dbOrders.GuestOrderTDES.Remove(guestordertdes);
            await dbOrders.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid = searchEntGuid,
                searchEntBranchGuid = searchEntBranchGuid,
                searchGestUserNic = GestUserNic
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbOrders != null)
            {
                _dbOrders.Dispose();
                _dbOrders = null;
            }
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
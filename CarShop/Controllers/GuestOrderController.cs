// CarShop.Controllers.GuestOrderController
using CarShop.Models;
using CarShop.Properties;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class GuestOrderController : Controller
    {
        private Guid guestEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private Guid guestEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

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

        public async Task ViewBagHelper()
        {
            EnterpriseBranchTDES enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                               where e.EntBranchGuid == guestEntBranchGuid && e.EntGuid == guestEntGuid
                                                               select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefaultAsync();
            if (enterprisebranchtdes != null)
            {
                if (enterprisebranchtdes.IsActive && enterprisebranchtdes.EnterpriseTDES.IsActive)
                {
                    CarShopOrderContextCatalog = enterprisebranchtdes.OrderCatalog;
                }
                else
                {
                    base.ModelState.AddModelError("", Resources.OrderBranchTDES_ISNOTACTIVE);
                }
            }
            else
            {
                base.ModelState.AddModelError("", Resources.OrderBranchTDES_NOTFOUND);
            }
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            string aName = base.User.Identity.Name;
            return View(await (from e in dbOrders.GuestOrderTDES
                               where e.GestUserNic == aName
                               orderby e.LastUpdated
                               select e).ToListAsync());
        }

        [Authorize]
        public async Task<ActionResult> Details(Guid? id = default(Guid?))
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            string aName = base.User.Identity.Name;
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)id && e.GestUserNic == aName
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                return HttpNotFound();
            }
            return View(guestordertdes);
        }

        [Authorize]
        public async Task<ActionResult> Create()
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            string aName = base.User.Identity.Name;
            GuestOrderTDES guestordertdes = new GuestOrderTDES
            {
                GuestOrderGuid = Guid.NewGuid(),
                GestUserNic = aName,
                IsActive = true,
                LastUpdated = DateTime.Now
            };
            guestordertdes.LastUpdated = guestordertdes.LastUpdated.AddMilliseconds(-guestordertdes.LastUpdated.Millisecond);
            guestordertdes.LastReplicated = guestordertdes.LastUpdated.AddSeconds(-5.0);
            return View(guestordertdes);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(GuestOrderTDES guestordertdes)
        {
            if (base.ModelState.IsValid)
            {
                await ViewBagHelper();
                if (!base.ModelState.IsValid)
                {
                    return View(guestordertdes);
                }
            }
            string aName = base.User.Identity.Name;
            if (base.ModelState.IsValid)
            {
                if (await (from e in dbOrders.GuestOrderTDES
                           where e.GestUserNic == aName
                           select e).CountAsync() > 7)
                {
                    base.ModelState.AddModelError("", Resources.GuestOrderTDES_ISMAX);
                    return View(guestordertdes);
                }
                if (guestordertdes.IsActive)
                {
                    foreach (GuestOrderTDES item in await (from e in dbOrders.GuestOrderTDES
                                                           where e.GestUserNic == aName
                                                           orderby e.LastUpdated
                                                           select e).ToListAsync())
                    {
                        if (item.IsActive)
                        {
                            item.IsActive = false;
                        }
                    }
                }
                guestordertdes.GestUserNic = aName;
                dbOrders.GuestOrderTDES.Add(guestordertdes);
                await dbOrders.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(guestordertdes);
        }

        [Authorize]
        public async Task<ActionResult> Edit(Guid? id = default(Guid?))
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            string aName = base.User.Identity.Name;
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)id && e.GestUserNic == aName
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                return HttpNotFound();
            }
            return View(guestordertdes);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Edit(GuestOrderTDES guestordertdes)
        {
            if (base.ModelState.IsValid)
            {
                await ViewBagHelper();
                if (!base.ModelState.IsValid)
                {
                    return View(guestordertdes);
                }
            }
            if (base.ModelState.IsValid)
            {
                string aName = base.User.Identity.Name;
                if (guestordertdes.IsActive)
                {
                    foreach (GuestOrderTDES item in await (from e in dbOrders.GuestOrderTDES
                                                           where e.GestUserNic == aName && e.GuestOrderGuid != guestordertdes.GuestOrderGuid
                                                           orderby e.LastUpdated
                                                           select e).ToListAsync())
                    {
                        if (item.IsActive)
                        {
                            item.IsActive = false;
                        }
                    }
                }
                guestordertdes.GestUserNic = aName;
                dbOrders.Entry(guestordertdes).State = EntityState.Modified;
                await dbOrders.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(guestordertdes);
        }

        [Authorize]
        public async Task<ActionResult> Delete(Guid? id = default(Guid?))
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            string aName = base.User.Identity.Name;
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)id && e.GestUserNic == aName
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                return HttpNotFound();
            }
            return View(guestordertdes);
        }

        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(Guid? id)
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            string aName = base.User.Identity.Name;
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)id && e.GestUserNic == aName
                                                   select e).FirstOrDefaultAsync();
            dbOrders.GuestOrderTDES.Remove(guestordertdes);
            await dbOrders.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            if (_dbOrders != null)
            {
                _dbOrders.Dispose();
                _dbOrders = null;
            }
            base.Dispose(disposing);
        }
    }

}
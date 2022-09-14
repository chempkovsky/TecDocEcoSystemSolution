// CarShop.Controllers.GuestProfileController
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

    public class GuestProfileController : Controller
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
            GuestProfileTDES usrs = await (from e in dbOrders.GuestProfileTDES
                                           where e.GestUserNic == aName
                                           select e).FirstOrDefaultAsync();
            if (usrs == null)
            {
                return RedirectToAction("Create");
            }
            return View(usrs);
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
            GuestProfileTDES guestprofiletdes = new GuestProfileTDES
            {
                GestUserNic = aName
            };
            return View(guestprofiletdes);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GuestProfileTDES guestprofiletdes)
        {
            if (base.ModelState.IsValid)
            {
                await ViewBagHelper();
                if (!base.ModelState.IsValid)
                {
                    return View(guestprofiletdes);
                }
            }
            guestprofiletdes.GestUserNic = base.User.Identity.Name;
            if (base.ModelState.IsValid)
            {
                dbOrders.GuestProfileTDES.Add(guestprofiletdes);
                await dbOrders.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(guestprofiletdes);
        }

        [Authorize]
        public async Task<ActionResult> Edit(string id = null)
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            string name = base.User.Identity.Name;
            GuestProfileTDES guestprofiletdes = await dbOrders.GuestProfileTDES.FindAsync(id);
            if (guestprofiletdes == null)
            {
                return RedirectToAction("Create");
            }
            return View(guestprofiletdes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(GuestProfileTDES guestprofiletdes)
        {
            if (base.ModelState.IsValid)
            {
                await ViewBagHelper();
                if (!base.ModelState.IsValid)
                {
                    return View(guestprofiletdes);
                }
            }
            guestprofiletdes.GestUserNic = base.User.Identity.Name;
            if (base.ModelState.IsValid)
            {
                dbOrders.Entry(guestprofiletdes).State = EntityState.Modified;
                await dbOrders.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(guestprofiletdes);
        }

        [Authorize]
        public async Task<ActionResult> Delete(string id = null)
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            id = base.User.Identity.Name;
            GuestProfileTDES guestprofiletdes = await dbOrders.GuestProfileTDES.FindAsync(id);
            if (guestprofiletdes == null)
            {
                return HttpNotFound();
            }
            return View(guestprofiletdes);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            id = base.User.Identity.Name;
            GuestProfileTDES guestprofiletdes = await dbOrders.GuestProfileTDES.FindAsync(id);
            dbOrders.GuestProfileTDES.Remove(guestprofiletdes);
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
// CarShop.Controllers.OriginalCatalogController
using CarShop.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class OriginalCatalogController : Controller
    {
        private CarShopContext db = new CarShopContext();

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index()
        {
            UserIsInRoles();
            return View(await db.OriginalCatalogTDES.ToListAsync());
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(int? id)
        {
            UserIsInRoles();
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OriginalCatalogTDES originalcatalogtdes = await db.OriginalCatalogTDES.FindAsync(id);
            if (originalcatalogtdes == null)
            {
                return HttpNotFound();
            }
            return View(originalcatalogtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Create()
        {
            UserIsInRoles();
            return View();
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OriginalCatalogId,OriginalCatalogName,OriginalCatalogURI")] OriginalCatalogTDES originalcatalogtdes)
        {
            UserIsInRoles();
            if (base.ModelState.IsValid)
            {
                db.OriginalCatalogTDES.Add(originalcatalogtdes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(originalcatalogtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(int? id)
        {
            UserIsInRoles();
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OriginalCatalogTDES originalcatalogtdes = await db.OriginalCatalogTDES.FindAsync(id);
            if (originalcatalogtdes == null)
            {
                return HttpNotFound();
            }
            return View(originalcatalogtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OriginalCatalogId,OriginalCatalogName,OriginalCatalogURI")] OriginalCatalogTDES originalcatalogtdes)
        {
            UserIsInRoles();
            if (base.ModelState.IsValid)
            {
                db.Entry(originalcatalogtdes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(originalcatalogtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Delete(int? id)
        {
            UserIsInRoles();
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OriginalCatalogTDES originalcatalogtdes = await db.OriginalCatalogTDES.FindAsync(id);
            if (originalcatalogtdes == null)
            {
                return HttpNotFound();
            }
            return View(originalcatalogtdes);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UserIsInRoles();
            OriginalCatalogTDES originalcatalogtdes = await db.OriginalCatalogTDES.FindAsync(id);
            db.OriginalCatalogTDES.Remove(originalcatalogtdes);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
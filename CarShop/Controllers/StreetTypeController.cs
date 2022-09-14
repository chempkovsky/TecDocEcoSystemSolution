// CarShop.Controllers.StreetTypeController
using CarShop.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{
    public class StreetTypeController : Controller
    {
        private CarShopContext db = new CarShopContext();

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Index()
        {
            UserIsInRoles();
            return View(db.StreetType.ToList());
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Details(int id = 0)
        {
            UserIsInRoles();
            StreetType streetType = db.StreetType.Find(id);
            if (streetType == null)
            {
                return HttpNotFound();
            }
            return View(streetType);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(StreetType streettype)
        {
            if (base.ModelState.IsValid)
            {
                db.StreetType.Add(streettype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(streettype);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Edit(int id = 0)
        {
            StreetType streetType = db.StreetType.Find(id);
            if (streetType == null)
            {
                return HttpNotFound();
            }
            return View(streetType);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StreetType streettype)
        {
            if (base.ModelState.IsValid)
            {
                db.Entry(streettype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(streettype);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Delete(int id = 0)
        {
            StreetType streetType = db.StreetType.Find(id);
            if (streetType == null)
            {
                return HttpNotFound();
            }
            return View(streetType);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            StreetType entity = db.StreetType.Find(id);
            db.StreetType.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
// CarShop.Controllers.BranchTypesController
using CarShop.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class BranchTypesController : Controller
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
            return View(db.BranchType.ToList());
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Details(int id = 0)
        {
            UserIsInRoles();
            BranchType branchType = db.BranchType.Find(id);
            if (branchType == null)
            {
                return HttpNotFound();
            }
            return View(branchType);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(BranchType branchtype)
        {
            if (base.ModelState.IsValid)
            {
                db.BranchType.Add(branchtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(branchtype);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Edit(int id = 0)
        {
            BranchType branchType = db.BranchType.Find(id);
            if (branchType == null)
            {
                return HttpNotFound();
            }
            return View(branchType);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BranchType branchtype)
        {
            if (base.ModelState.IsValid)
            {
                db.Entry(branchtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(branchtype);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Delete(int id = 0)
        {
            BranchType branchType = db.BranchType.Find(id);
            if (branchType == null)
            {
                return HttpNotFound();
            }
            return View(branchType);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BranchType entity = db.BranchType.Find(id);
            db.BranchType.Remove(entity);
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
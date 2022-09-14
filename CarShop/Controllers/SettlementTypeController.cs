// CarShop.Controllers.SettlementTypeController
using CarShop.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{
    public class SettlementTypeController : Controller
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
            return View(db.SettlementType.ToList());
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Details(int id = 0)
        {
            UserIsInRoles();
            SettlementType settlementType = db.SettlementType.Find(id);
            if (settlementType == null)
            {
                return HttpNotFound();
            }
            return View(settlementType);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SettlementType settlementtype)
        {
            if (base.ModelState.IsValid)
            {
                db.SettlementType.Add(settlementtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(settlementtype);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Edit(int id = 0)
        {
            SettlementType settlementType = db.SettlementType.Find(id);
            if (settlementType == null)
            {
                return HttpNotFound();
            }
            return View(settlementType);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SettlementType settlementtype)
        {
            if (base.ModelState.IsValid)
            {
                db.Entry(settlementtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(settlementtype);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Delete(int id = 0)
        {
            SettlementType settlementType = db.SettlementType.Find(id);
            if (settlementType == null)
            {
                return HttpNotFound();
            }
            return View(settlementType);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            SettlementType entity = db.SettlementType.Find(id);
            db.SettlementType.Remove(entity);
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
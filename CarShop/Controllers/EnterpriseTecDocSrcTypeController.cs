// CarShop.Controllers.EnterpriseTecDocSrcTypeController
using CarShop.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterpriseTecDocSrcTypeController : Controller
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
            return View(db.EnterpriseTecDocSrcTypeTDES.ToList());
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Details(int id = 0)
        {
            UserIsInRoles();
            EnterpriseTecDocSrcTypeTDES enterpriseTecDocSrcTypeTDES = db.EnterpriseTecDocSrcTypeTDES.Find(id);
            if (enterpriseTecDocSrcTypeTDES == null)
            {
                return HttpNotFound();
            }
            return View(enterpriseTecDocSrcTypeTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Create()
        {
            UserIsInRoles();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EnterpriseTecDocSrcTypeTDES enterprisetecdocsrctypetdes)
        {
            UserIsInRoles();
            if (base.ModelState.IsValid)
            {
                db.EnterpriseTecDocSrcTypeTDES.Add(enterprisetecdocsrctypetdes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(enterprisetecdocsrctypetdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Edit(int id = 0)
        {
            UserIsInRoles();
            EnterpriseTecDocSrcTypeTDES enterpriseTecDocSrcTypeTDES = db.EnterpriseTecDocSrcTypeTDES.Find(id);
            if (enterpriseTecDocSrcTypeTDES == null)
            {
                return HttpNotFound();
            }
            return View(enterpriseTecDocSrcTypeTDES);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        public ActionResult Edit(EnterpriseTecDocSrcTypeTDES enterprisetecdocsrctypetdes)
        {
            UserIsInRoles();
            if (base.ModelState.IsValid)
            {
                db.Entry(enterprisetecdocsrctypetdes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(enterprisetecdocsrctypetdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Delete(int id = 0)
        {
            UserIsInRoles();
            EnterpriseTecDocSrcTypeTDES enterpriseTecDocSrcTypeTDES = db.EnterpriseTecDocSrcTypeTDES.Find(id);
            if (enterpriseTecDocSrcTypeTDES == null)
            {
                return HttpNotFound();
            }
            return View(enterpriseTecDocSrcTypeTDES);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserIsInRoles();
            EnterpriseTecDocSrcTypeTDES entity = db.EnterpriseTecDocSrcTypeTDES.Find(id);
            db.EnterpriseTecDocSrcTypeTDES.Remove(entity);
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
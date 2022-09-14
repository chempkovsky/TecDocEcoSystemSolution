// CarShop.Controllers.SoatoController
using CarShop.Models;
using CarShop.Properties;
using PagedList;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{
    public class SoatoController : Controller
    {
        private CarShopContext db = new CarShopContext();

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Autocomplete(int? currentFilterBy, string term)
        {
            currentFilterBy = (currentFilterBy ?? 1);
            if (!string.IsNullOrEmpty(term))
            {
                term = term.Trim();
            }
            if (string.IsNullOrEmpty(term))
            {
                if (currentFilterBy == 1)
                {
                    var data = (from e in db.Soato
                                select new
                                {
                                    value = e.SoatoId
                                }).Take(10);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                var data2 = (from e in db.Soato
                             select new
                             {
                                 value = e.SoatoSettlementName
                             }).Take(10);
                return Json(data2, JsonRequestBehavior.AllowGet);
            }
            if (currentFilterBy == 1)
            {
                var data3 = (from e in db.Soato
                             where e.SoatoId.StartsWith(term)
                             select new
                             {
                                 value = e.SoatoId
                             }).Take(10);
                return Json(data3, JsonRequestBehavior.AllowGet);
            }
            var data4 = (from e in db.Soato
                         where e.SoatoSettlementName.Contains(term)
                         select new
                         {
                             value = e.SoatoSettlementName
                         }).Take(10);
            return Json(data4, JsonRequestBehavior.AllowGet);
        }

        protected SelectList SelectListHelper(int showIs)
        {
            return new SelectList(new SelectListItem[2]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.BySoatoId,
                Selected = (showIs == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.BySoatoSettlementName,
                Selected = (showIs == 2)
            }
            }, "Value", "Text", showIs);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Index(string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page)
        {
            UserIsInRoles();
            if (searchString != null)
            {
                page = 1;
                searchStringBy = (searchStringBy ?? 1);
            }
            else
            {
                page = (page ?? 1);
                searchString = currentFilter;
                searchStringBy = (currentFilterBy ?? 1);
            }
            IOrderedQueryable<Soato> orderedQueryable = from e in db.Soato
                                                        orderby e.SoatoId
                                                        select e;
            if (!string.IsNullOrEmpty(searchString))
            {
                orderedQueryable = ((searchStringBy != 1) ? (from e in db.Soato
                                                             where e.SoatoSettlementName.Contains(searchString)
                                                             orderby e.SoatoId
                                                             select e) : db.Soato.Where((Soato e) => e.SoatoId.StartsWith(searchString)).OrderBy((Soato e) => e.SoatoId));
            }
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.currentFilterBy = searchStringBy;
            orderedQueryable.Include((Soato s) => s.SettlementType);
            int pageSize = 20;
            int value = page.Value;
            return View(orderedQueryable.ToPagedList(value, pageSize));
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Details(string id = null)
        {
            UserIsInRoles();
            Soato soato = db.Soato.Find(id);
            if (soato == null)
            {
                return HttpNotFound();
            }
            return View(soato);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Create()
        {
            base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Soato soato)
        {
            if (base.ModelState.IsValid)
            {
                db.Soato.Add(soato);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", soato.SettlementTypeId);
            return View(soato);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Edit(string id = null)
        {
            Soato soato = db.Soato.Find(id);
            if (soato == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", soato.SettlementTypeId);
            return View(soato);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        public ActionResult Edit(Soato soato)
        {
            if (base.ModelState.IsValid)
            {
                db.Entry(soato).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", soato.SettlementTypeId);
            return View(soato);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Delete(string id = null)
        {
            Soato soato = db.Soato.Find(id);
            if (soato == null)
            {
                return HttpNotFound();
            }
            return View(soato);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult DeleteConfirmed(string id)
        {
            Soato entity = db.Soato.Find(id);
            db.Soato.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult LookUpForEnterpriseAddress(string redirecData, string redirectContriller, string redirectAction, string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page)
        {
            LookUpSoatoForEnterpriseAddress lookUpSoatoForEnterpriseAddress = new LookUpSoatoForEnterpriseAddress();
            lookUpSoatoForEnterpriseAddress.RedirecData = redirecData;
            lookUpSoatoForEnterpriseAddress.RedirectContriller = redirectContriller;
            lookUpSoatoForEnterpriseAddress.RedirectAction = redirectAction;
            if (searchString != null)
            {
                page = 1;
                searchStringBy = (searchStringBy ?? 1);
            }
            else
            {
                page = (page ?? 1);
                searchString = currentFilter;
                searchStringBy = (currentFilterBy ?? 1);
            }
            IOrderedQueryable<Soato> orderedQueryable = from e in db.Soato
                                                        orderby e.SoatoId
                                                        select e;
            if (!string.IsNullOrEmpty(searchString))
            {
                orderedQueryable = ((searchStringBy != 1) ? (from e in db.Soato
                                                             where e.SoatoSettlementName.Contains(searchString)
                                                             orderby e.SoatoId
                                                             select e) : db.Soato.Where((Soato e) => e.SoatoId.StartsWith(searchString)).OrderBy((Soato e) => e.SoatoId));
            }
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.currentFilterBy = searchStringBy;
            orderedQueryable.Include((Soato s) => s.SettlementType);
            int pageSize = 20;
            int value = page.Value;
            lookUpSoatoForEnterpriseAddress.SoatoList = orderedQueryable.ToPagedList(value, pageSize);
            return View(lookUpSoatoForEnterpriseAddress);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        [HttpPost]
        public ActionResult LookUpForEnterpriseAddressSelected(string redirecData, string redirectContriller, string redirectAction, string SoatoId, string DOSELECT_TITLE, string DOCANCEL_TITLE)
        {
            if (DOSELECT_TITLE != null)
            {
                return RedirectToAction(redirectAction, redirectContriller, new
                {
                    redirecData = redirecData,
                    soatoId = SoatoId
                });
            }
            return RedirectToAction(redirectAction, redirectContriller, new
            {
                redirecData
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
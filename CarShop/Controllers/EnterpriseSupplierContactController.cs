// CarShop.Controllers.EnterpriseSupplierContactController
using CarShop.Models;
using CarShop.Properties;
using CarShop.Utility;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterpriseSupplierContactController : Controller
    {
        private CarShopContext db = new CarShopContext();

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
        }

        public void ViewBagHelper(Guid? searchEntGuid, string searchEntSupplierId)
        {
            EnterpriseSupplierTDES enterpriseSupplierTDES = (from e in db.EnterpriseSupplierTDES
                                                             where e.EntSupplierId == searchEntSupplierId && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                             select e).Include((EnterpriseSupplierTDES x) => x.EnterpriseTDES).FirstOrDefault();
            if (enterpriseSupplierTDES != null)
            {
                base.ViewBag.EntDescription = enterpriseSupplierTDES.EnterpriseTDES.EntDescription;
                base.ViewBag.EntSupplierDescription = enterpriseSupplierTDES.EntSupplierDescription;
                base.ViewBag.SearchEntGuid = searchEntGuid;
                base.ViewBag.SearchEntSupplierId = searchEntSupplierId;
                return;
            }
            base.ViewBag.EntSupplierDescription = Resources.EnterpriseSupplierTDES_NOTFOUND;
            base.ViewBag.SearchEntSupplierId = null;
            base.ModelState.AddModelError("", Resources.EnterpriseSupplierTDES_NOTFOUND);
            EnterpriseTDES enterpriseTDES = (from e in db.EnterpriseTDES
                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                             select e).FirstOrDefault();
            if (enterpriseTDES != null)
            {
                base.ViewBag.EntDescription = enterpriseTDES.EntDescription;
                base.ViewBag.SearchEntGuid = searchEntGuid;
            }
            else
            {
                base.ViewBag.EntDescription = Resources.ENTERPRISE_NOT_DEFINED;
                base.ViewBag.SearchEntGuid = null;
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Index(Guid? searchEntGuid, string searchEntSupplierId, int? showIsVisible, int? showIsActive)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    searchEntGuid = (from e in db.EnterpriseBranchUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
            }
            ViewBagHelper(searchEntGuid, searchEntSupplierId);
            IQueryable<EnterpriseSupplierContactTDES> source = from e in db.EnterpriseSupplierContactTDES
                                                               where e.EntSupplierId == searchEntSupplierId && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                               select e;
            showIsActive = (showIsActive ?? 2);
            switch (showIsActive)
            {
                case 2:
                    source = from e in source
                             where e.IsActive == true
                             select e;
                    break;
                case 3:
                    source = from e in source
                             where e.IsActive == false
                             select e;
                    break;
                default:
                    showIsActive = 1;
                    break;
            }
            showIsVisible = (showIsVisible ?? 1);
            switch (showIsVisible)
            {
                case 2:
                    source = from e in source
                             where e.IsVisible == true
                             select e;
                    break;
                case 3:
                    source = from e in source
                             where e.IsVisible == false
                             select e;
                    break;
                default:
                    showIsVisible = 1;
                    break;
            }
            base.ViewBag.showIsActive = showIsActive;
            base.ViewBag.showIsVisible = showIsVisible;
            base.ViewBag.sliIsActive = SwitcherListUtil.SelectListHelper(showIsActive.Value);
            base.ViewBag.sliIsVisible = SwitcherListUtil.SelectListHelper(showIsVisible.Value);
            source.Include((EnterpriseSupplierContactTDES x) => x.ContactType);
            return View(source.ToList());
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Details(Guid? id = default(Guid?))
        {
            EnterpriseSupplierContactTDES enterpriseSupplierContactTDES = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    searchEntGuid = (from e in db.EnterpriseBranchUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                enterpriseSupplierContactTDES = (from e in db.EnterpriseSupplierContactTDES
                                                 where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                 select e).Include((EnterpriseSupplierContactTDES x) => x.ContactType).FirstOrDefault();
            }
            else
            {
                enterpriseSupplierContactTDES = (from e in db.EnterpriseSupplierContactTDES
                                                 where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                 select e).Include((EnterpriseSupplierContactTDES x) => x.ContactType).FirstOrDefault();
            }
            if (enterpriseSupplierContactTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseSupplierContactTDES.EntGuid;
            string entSupplierId = enterpriseSupplierContactTDES.EntSupplierId;
            ViewBagHelper(searchEntGuid, entSupplierId);
            return View(enterpriseSupplierContactTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Create(Guid? searchEntGuid, string searchEntSupplierId)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                    searchEntSupplierId = (from e in db.EnterpriseSupplierTDES
                                           where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntSupplierId == searchEntSupplierId
                                           select e.EntSupplierId).FirstOrDefault();
                }
            }
            ViewBagHelper(searchEntGuid, searchEntSupplierId);
            base.ViewBag.ContactTypeId = new SelectList(db.ContactType, "ContactTypeId", "ContactTypeDescription");
            EnterpriseSupplierContactTDES model = null;
            if (base.ViewBag.SearchEntSupplierId != null)
            {
                EnterpriseSupplierContactTDES enterpriseSupplierContactTDES = new EnterpriseSupplierContactTDES();
                enterpriseSupplierContactTDES.ContactGuid = Guid.NewGuid();
                enterpriseSupplierContactTDES.EntSupplierId = searchEntSupplierId;
                enterpriseSupplierContactTDES.EntGuid = searchEntGuid.Value;
                model = enterpriseSupplierContactTDES;
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Create(EnterpriseSupplierContactTDES enterprisesuppliercontacttdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                searchEntGuid = enterprisesuppliercontacttdes.EntGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
            }
            if (base.ModelState.IsValid && enterprisesuppliercontacttdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            string entSupplierId = enterprisesuppliercontacttdes.EntSupplierId;
            if (base.ModelState.IsValid)
            {
                db.EnterpriseSupplierContactTDES.Add(enterprisesuppliercontacttdes);
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = searchEntGuid,
                    searchEntSupplierId = entSupplierId
                });
            }
            ViewBagHelper(searchEntGuid, entSupplierId);
            base.ViewBag.ContactTypeId = new SelectList(db.ContactType, "ContactTypeId", "ContactTypeDescription", enterprisesuppliercontacttdes.ContactTypeId);
            return View(enterprisesuppliercontacttdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Edit(Guid? id = default(Guid?))
        {
            EnterpriseSupplierContactTDES enterpriseSupplierContactTDES = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    searchEntGuid = (from e in db.EnterpriseBranchUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                enterpriseSupplierContactTDES = (from e in db.EnterpriseSupplierContactTDES
                                                 where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                 select e).Include((EnterpriseSupplierContactTDES x) => x.ContactType).FirstOrDefault();
            }
            else
            {
                enterpriseSupplierContactTDES = (from e in db.EnterpriseSupplierContactTDES
                                                 where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                 select e).Include((EnterpriseSupplierContactTDES x) => x.ContactType).FirstOrDefault();
            }
            if (enterpriseSupplierContactTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseSupplierContactTDES.EntGuid;
            string entSupplierId = enterpriseSupplierContactTDES.EntSupplierId;
            base.ViewBag.ContactTypeId = new SelectList(db.ContactType, "ContactTypeId", "ContactTypeDescription", enterpriseSupplierContactTDES.ContactTypeId);
            ViewBagHelper(searchEntGuid, entSupplierId);
            return View(enterpriseSupplierContactTDES);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EnterpriseSupplierContactTDES enterprisesuppliercontacttdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                searchEntGuid = enterprisesuppliercontacttdes.EntGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
            }
            if (base.ModelState.IsValid && enterprisesuppliercontacttdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            string entSupplierId = enterprisesuppliercontacttdes.EntSupplierId;
            if (base.ModelState.IsValid)
            {
                db.Entry(enterprisesuppliercontacttdes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = searchEntGuid,
                    searchEntSupplierId = entSupplierId
                });
            }
            ViewBagHelper(searchEntGuid, entSupplierId);
            base.ViewBag.ContactTypeId = new SelectList(db.ContactType, "ContactTypeId", "ContactTypeDescription", enterprisesuppliercontacttdes.ContactTypeId);
            return View(enterprisesuppliercontacttdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Delete(Guid? id = default(Guid?))
        {
            EnterpriseSupplierContactTDES enterpriseSupplierContactTDES = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    searchEntGuid = (from e in db.EnterpriseBranchUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                enterpriseSupplierContactTDES = (from e in db.EnterpriseSupplierContactTDES
                                                 where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                 select e).Include((EnterpriseSupplierContactTDES x) => x.ContactType).FirstOrDefault();
            }
            else
            {
                enterpriseSupplierContactTDES = (from e in db.EnterpriseSupplierContactTDES
                                                 where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                 select e).Include((EnterpriseSupplierContactTDES x) => x.ContactType).FirstOrDefault();
            }
            if (enterpriseSupplierContactTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseSupplierContactTDES.EntGuid;
            string entSupplierId = enterpriseSupplierContactTDES.EntSupplierId;
            ViewBagHelper(searchEntGuid, entSupplierId);
            return View(enterpriseSupplierContactTDES);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid? id)
        {
            string searchEntSupplierId = null;
            EnterpriseSupplierContactTDES enterpriseSupplierContactTDES = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    searchEntGuid = (from e in db.EnterpriseBranchUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                enterpriseSupplierContactTDES = (from e in db.EnterpriseSupplierContactTDES
                                                 where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                 select e).FirstOrDefault();
            }
            else
            {
                enterpriseSupplierContactTDES = (from e in db.EnterpriseSupplierContactTDES
                                                 where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                 select e).FirstOrDefault();
            }
            if (enterpriseSupplierContactTDES != null)
            {
                searchEntGuid = enterpriseSupplierContactTDES.EntGuid;
                searchEntSupplierId = enterpriseSupplierContactTDES.EntSupplierId;
            }
            db.EnterpriseSupplierContactTDES.Remove(enterpriseSupplierContactTDES);
            db.SaveChanges();
            return RedirectToAction("Index", new
            {
                searchEntGuid,
                searchEntSupplierId
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
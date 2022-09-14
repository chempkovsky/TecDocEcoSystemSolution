// CarShop.Controllers.EnterpriseUserContactController
using CarShop.Models;
using CarShop.Properties;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterpriseUserContactController : Controller
    {
        private CarShopContext db = new CarShopContext();

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
            base.ViewBag.IsBranchAdmin = base.User.IsInRole("BranchAdmin");
            base.ViewBag.IsBranchAudit = base.User.IsInRole("BranchAudit");
            base.ViewBag.IsBranchSeller = base.User.IsInRole("BranchSeller");
            base.ViewBag.IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        public void ViewBagHelper(Guid? searchEntGuid, string searchEntUserNic)
        {
            EnterpriseUserTDES enterpriseUserTDES = (from e in db.EnterpriseUserTDES
                                                     where e.EntUserNic == searchEntUserNic && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                     select e).Include((EnterpriseUserTDES x) => x.EnterpriseTDES).FirstOrDefault();
            if (enterpriseUserTDES != null)
            {
                base.ViewBag.EntDescription = enterpriseUserTDES.EnterpriseTDES.EntDescription;
                string text = (string.IsNullOrEmpty(enterpriseUserTDES.LastName) ? "" : (enterpriseUserTDES.LastName + " ")) + (string.IsNullOrEmpty(enterpriseUserTDES.FirstName) ? "" : (enterpriseUserTDES.FirstName + " ")) + (string.IsNullOrEmpty(enterpriseUserTDES.MiddleName) ? "" : (enterpriseUserTDES.MiddleName + " "));
                base.ViewBag.EntUserDescription = enterpriseUserTDES.EntUserNic + (string.IsNullOrEmpty(text) ? "" : (" : " + text));
                base.ViewBag.SearchEntGuid = searchEntGuid;
                base.ViewBag.SearchEntUserNic = searchEntUserNic;
                return;
            }
            base.ViewBag.EntUserDescription = Resources.ENTERPRISEUSER_NOTDEFINED;
            base.ViewBag.SearchEntUserNic = null;
            base.ModelState.AddModelError("", Resources.ENTERPRISEUSER_NOTDEFINED);
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

        public void ViewBagListsHelper(object ContactTypeId = null)
        {
            base.ViewBag.ContactTypes = new SelectList(db.ContactType, "ContactTypeId", "ContactTypeDescription", ContactTypeId);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Index(Guid? searchEntGuid, string searchEntUserNic, int? showIsVisible, int? showIsActive)
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
                if (string.IsNullOrEmpty(searchEntUserNic))
                {
                    searchEntUserNic = aName;
                }
            }
            ViewBagHelper(searchEntGuid, searchEntUserNic);
            IQueryable<EnterpriseUserContactTDES> source = from e in db.EnterpriseUserContactTDES
                                                           where e.EntUserNic == searchEntUserNic && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
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
            base.ViewBag.sliIsActive = new SelectList(new SelectListItem[3]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.SHOWALL,
                Selected = (showIsActive == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.SHOWACTIVE,
                Selected = (showIsActive == 2)
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.SHOWNOTACTIVE,
                Selected = (showIsActive == 3)
            }
            }, "Value", "Text");
            base.ViewBag.sliIsVisible = new SelectList(new SelectListItem[3]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.SHOWALL,
                Selected = (showIsVisible == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.SHOWACTIVE,
                Selected = (showIsVisible == 2)
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.SHOWNOTACTIVE,
                Selected = (showIsVisible == 3)
            }
            }, "Value", "Text");
            source.Include((EnterpriseUserContactTDES x) => x.ContactType);
            return View(source.ToList());
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Details(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            EnterpriseUserContactTDES enterpriseUserContactTDES = null;
            Guid? searchEntGuid = null;
            string searchEntUserNic = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (string.IsNullOrEmpty(searchEntUserNic))
                {
                    searchEntUserNic = aName;
                }
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                    enterpriseUserContactTDES = (from e in db.EnterpriseUserContactTDES
                                                 where (Guid)(Guid?)e.ContactGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                 select e).Include((EnterpriseUserContactTDES x) => x.ContactType).FirstOrDefault();
                }
                else
                {
                    searchEntGuid = (from e in db.EnterpriseBranchUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                    enterpriseUserContactTDES = (from e in db.EnterpriseUserContactTDES
                                                 where (Guid)(Guid?)e.ContactGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (e.IsVisible == true || e.EntUserNic == searchEntUserNic)
                                                 select e).Include((EnterpriseUserContactTDES x) => x.ContactType).FirstOrDefault();
                }
            }
            else
            {
                enterpriseUserContactTDES = (from e in db.EnterpriseUserContactTDES
                                             where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                             select e).Include((EnterpriseUserContactTDES x) => x.ContactType).FirstOrDefault();
            }
            if (enterpriseUserContactTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseUserContactTDES.EntGuid;
            searchEntUserNic = enterpriseUserContactTDES.EntUserNic;
            ViewBagHelper(searchEntGuid, searchEntUserNic);
            return View(enterpriseUserContactTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Create(Guid? searchEntGuid, string searchEntUserNic)
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
                    searchEntUserNic = (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == searchEntUserNic && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                        select e.EntUserNic).FirstOrDefault();
                }
            }
            ViewBagHelper(searchEntGuid, searchEntUserNic);
            ViewBagListsHelper();
            EnterpriseUserContactTDES model = null;
            if (base.ViewBag.SearchEntUserNic != null)
            {
                EnterpriseUserContactTDES enterpriseUserContactTDES = new EnterpriseUserContactTDES();
                enterpriseUserContactTDES.ContactGuid = Guid.NewGuid();
                enterpriseUserContactTDES.EntUserNic = searchEntUserNic;
                enterpriseUserContactTDES.EntGuid = searchEntGuid.Value;
                model = enterpriseUserContactTDES;
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EnterpriseUserContactTDES enterpriseuserphonetdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            string searchEntUserNic = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName && e.EntGuid == enterpriseuserphonetdes.EntGuid
                                     select e.EntGuid).FirstOrDefault();
                    if (searchEntGuid.HasValue)
                    {
                        searchEntUserNic = enterpriseuserphonetdes.EntUserNic;
                    }
                    else
                    {
                        base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
                    }
                }
            }
            else
            {
                searchEntGuid = enterpriseuserphonetdes.EntGuid;
                searchEntUserNic = enterpriseuserphonetdes.EntUserNic;
            }
            if (base.ModelState.IsValid)
            {
                db.EnterpriseUserContactTDES.Add(enterpriseuserphonetdes);
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid,
                    searchEntUserNic
                });
            }
            ViewBagHelper(searchEntGuid, searchEntUserNic);
            ViewBagListsHelper();
            return View(enterpriseuserphonetdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Edit(Guid? id = default(Guid?))
        {
            Guid? searchEntGuid = null;
            EnterpriseUserContactTDES enterpriseUserContactTDES = null;
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                enterpriseUserContactTDES = (from e in db.EnterpriseUserContactTDES
                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                             select e).FirstOrDefault();
            }
            else
            {
                enterpriseUserContactTDES = db.EnterpriseUserContactTDES.Find(id);
            }
            if (enterpriseUserContactTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseUserContactTDES.EntGuid;
            string entUserNic = enterpriseUserContactTDES.EntUserNic;
            ViewBagHelper(searchEntGuid, entUserNic);
            ViewBagListsHelper(enterpriseUserContactTDES.ContactTypeId);
            return View(enterpriseUserContactTDES);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Edit(EnterpriseUserContactTDES enterpriseuserphonetdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            string searchEntUserNic = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName && e.EntGuid == enterpriseuserphonetdes.EntGuid
                                     select e.EntGuid).FirstOrDefault();
                    if (searchEntGuid.HasValue)
                    {
                        searchEntUserNic = enterpriseuserphonetdes.EntUserNic;
                    }
                    else
                    {
                        base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
                    }
                }
            }
            else
            {
                searchEntGuid = enterpriseuserphonetdes.EntGuid;
                searchEntUserNic = enterpriseuserphonetdes.EntUserNic;
            }
            if (base.ModelState.IsValid)
            {
                db.Entry(enterpriseuserphonetdes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid,
                    searchEntUserNic
                });
            }
            ViewBagHelper(searchEntGuid, searchEntUserNic);
            ViewBagListsHelper(enterpriseuserphonetdes.ContactTypeId);
            return View(enterpriseuserphonetdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Delete(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseUserContactTDES enterpriseUserContactTDES = null;
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                enterpriseUserContactTDES = (from e in db.EnterpriseUserContactTDES
                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                             select e).FirstOrDefault();
            }
            else
            {
                enterpriseUserContactTDES = (from e in db.EnterpriseUserContactTDES
                                             where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                             select e).Include((EnterpriseUserContactTDES x) => x.ContactType).FirstOrDefault();
            }
            if (enterpriseUserContactTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseUserContactTDES.EntGuid;
            string entUserNic = enterpriseUserContactTDES.EntUserNic;
            ViewBagHelper(searchEntGuid, entUserNic);
            return View(enterpriseUserContactTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid? id)
        {
            string searchEntUserNic = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseUserContactTDES enterpriseUserContactTDES = null;
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                enterpriseUserContactTDES = (from e in db.EnterpriseUserContactTDES
                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                             select e).FirstOrDefault();
            }
            else
            {
                enterpriseUserContactTDES = (from e in db.EnterpriseUserContactTDES
                                             where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                             select e).Include((EnterpriseUserContactTDES x) => x.ContactType).FirstOrDefault();
            }
            if (enterpriseUserContactTDES != null)
            {
                searchEntGuid = enterpriseUserContactTDES.EntGuid;
                searchEntUserNic = enterpriseUserContactTDES.EntUserNic;
            }
            db.EnterpriseUserContactTDES.Remove(enterpriseUserContactTDES);
            db.SaveChanges();
            return RedirectToAction("Index", new
            {
                searchEntGuid,
                searchEntUserNic
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
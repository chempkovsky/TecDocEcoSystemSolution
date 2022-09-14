// CarShop.Controllers.EnterpriseBranchUserContactController
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

    public class EnterpriseBranchUserContactController : Controller
    {
        private CarShopContext db = new CarShopContext();

        public void ViewBagHelper(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchEntUserNic)
        {
            EnterpriseBranchUserTDES enterpriseBranchUserTDES = (from e in db.EnterpriseBranchUserTDES
                                                                 where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntUserNic == searchEntUserNic
                                                                 select e).Include((EnterpriseBranchUserTDES x) => x.EnterpriseBranchTDES).FirstOrDefault();
            if (enterpriseBranchUserTDES != null)
            {
                base.ViewBag.SearchEntUserNic = searchEntUserNic;
                string text = (string.IsNullOrEmpty(enterpriseBranchUserTDES.LastName) ? "" : (enterpriseBranchUserTDES.LastName + " ")) + (string.IsNullOrEmpty(enterpriseBranchUserTDES.FirstName) ? "" : (enterpriseBranchUserTDES.FirstName + " ")) + (string.IsNullOrEmpty(enterpriseBranchUserTDES.MiddleName) ? "" : (enterpriseBranchUserTDES.MiddleName + " "));
                base.ViewBag.EntUserDescription = enterpriseBranchUserTDES.EntUserNic + (string.IsNullOrEmpty(text) ? "" : (" : " + text));
                base.ViewBag.EntBranchDescription = enterpriseBranchUserTDES.EnterpriseBranchTDES.EntBranchDescription;
                base.ViewBag.SearchEntBranchGuid = searchEntBranchGuid;
                ViewBagHelper(searchEntGuid);
                return;
            }
            base.ViewBag.EntUserDescription = Resources.ENTERPRISEUSER_NOTDEFINED;
            base.ViewBag.SearchEntUserNic = null;
            EnterpriseBranchTDES enterpriseBranchTDES = (from e in db.EnterpriseBranchTDES
                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                         select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefault();
            if (enterpriseBranchTDES != null)
            {
                base.ViewBag.EntDescription = enterpriseBranchTDES.EnterpriseTDES.EntDescription;
                base.ViewBag.EntBranchDescription = enterpriseBranchTDES.EntBranchDescription;
                base.ViewBag.SearchEntGuid = searchEntGuid;
                base.ViewBag.SearchEntBranchGuid = searchEntBranchGuid;
            }
            else
            {
                base.ViewBag.EntBranchDescription = Resources.EnterpriseBranchTDES_NOTFOUND;
                base.ViewBag.SearchEntBranchGuid = null;
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                ViewBagHelper(searchEntGuid);
            }
        }

        public void ViewBagHelper(Guid? searchEntGuid)
        {
            EnterpriseTDES enterpriseTDES = (from e in db.EnterpriseTDES
                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                             select e).FirstOrDefault();
            if (enterpriseTDES != null)
            {
                base.ViewBag.EntDescription = enterpriseTDES.EntDescription;
                base.ViewBag.SearchEntGuid = searchEntGuid;
                return;
            }
            base.ViewBag.EntDescription = Resources.ENTERPRISE_NOT_DEFINED;
            base.ViewBag.EntBranchDescription = Resources.EnterpriseBranchTDES_NOTFOUND;
            base.ViewBag.EntUserDescription = Resources.ENTERPRISEUSER_NOTDEFINED;
            base.ViewBag.SearchEntGuid = null;
            base.ViewBag.SearchEntUserNic = null;
            base.ViewBag.SearchEntBranchGuid = null;
            base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
        }

        public void ViewBagListsHelper(object ContactTypeId = null)
        {
            base.ViewBag.ContactTypes = new SelectList(db.ContactType, "ContactTypeId", "ContactTypeDescription", ContactTypeId);
        }

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
            base.ViewBag.IsBranchAdmin = base.User.IsInRole("BranchAdmin");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Index(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchEntUserNic, int? showIsVisible, int? showIsActive)
        {
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
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
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                }
            }
            IQueryable<EnterpriseBranchUserContactTDES> source = from e in db.EnterpriseBranchUserContactTDES
                                                                 where e.EntUserNic == searchEntUserNic && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                 select e;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid, searchEntUserNic);
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
            base.ViewBag.sliIsActive = SwitcherListUtil.SelectListHelper(showIsActive.Value);
            base.ViewBag.showIsVisible = showIsVisible;
            base.ViewBag.sliIsVisible = SwitcherListUtil.SelectListHelper(showIsVisible.Value);
            source.Include((EnterpriseBranchUserContactTDES x) => x.ContactType);
            return View(source.ToList());
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Details(Guid? id = default(Guid?))
        {
            EnterpriseBranchUserContactTDES enterpriseBranchUserContactTDES = null;
            string text = null;
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                    enterpriseBranchUserContactTDES = (from e in db.EnterpriseBranchUserContactTDES
                                                       where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                       select e).Include((EnterpriseBranchUserContactTDES x) => x.ContactType).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                        enterpriseBranchUserContactTDES = (from e in db.EnterpriseBranchUserContactTDES
                                                           where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                           select e).Include((EnterpriseBranchUserContactTDES x) => x.ContactType).FirstOrDefault();
                    }
                }
            }
            else
            {
                enterpriseBranchUserContactTDES = (from e in db.EnterpriseBranchUserContactTDES
                                                   where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                   select e).Include((EnterpriseBranchUserContactTDES x) => x.ContactType).FirstOrDefault();
            }
            if (enterpriseBranchUserContactTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseBranchUserContactTDES.EntGuid;
            text = enterpriseBranchUserContactTDES.EntUserNic;
            searchEntBranchGuid = enterpriseBranchUserContactTDES.EntBranchGuid;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid, text);
            return View(enterpriseBranchUserContactTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public ActionResult Create(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchEntUserNic)
        {
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
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
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES != null)
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                    else
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                }
            }
            ViewBagHelper(searchEntGuid, searchEntBranchGuid, searchEntUserNic);
            ViewBagListsHelper();
            base.ViewBag.EntUserNic = new SelectList(db.EnterpriseBranchUserTDES, "EntUserNic", "Password");
            base.ViewBag.ContactTypeId = new SelectList(db.ContactType, "ContactTypeId", "ContactTypeDescription");
            EnterpriseBranchUserContactTDES model = null;
            if (base.ViewBag.SearchEntUserNic != null)
            {
                EnterpriseBranchUserContactTDES enterpriseBranchUserContactTDES = new EnterpriseBranchUserContactTDES();
                enterpriseBranchUserContactTDES.EntGuid = searchEntGuid.Value;
                enterpriseBranchUserContactTDES.EntBranchGuid = searchEntBranchGuid.Value;
                enterpriseBranchUserContactTDES.EntUserNic = searchEntUserNic;
                enterpriseBranchUserContactTDES.ContactGuid = Guid.NewGuid();
                model = enterpriseBranchUserContactTDES;
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [HttpPost]
        public ActionResult Create(EnterpriseBranchUserContactTDES enterprisebranchusercontacttdes)
        {
            UserIsInRoles();
            Guid? guid = null;
            Guid? guid2 = null;
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            string text = null;
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                guid = enterprisebranchusercontacttdes.EntGuid;
                guid2 = enterprisebranchusercontacttdes.EntBranchGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    guid = (from e in db.EnterpriseUserTDES
                            where e.EntUserNic == aName
                            select e.EntGuid).FirstOrDefault();
                    guid2 = enterprisebranchusercontacttdes.EntBranchGuid;
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES != null)
                    {
                        guid = enterpriseBranchIdsTDES.EntGuid;
                        guid2 = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                    else
                    {
                        guid = null;
                        guid2 = null;
                    }
                }
            }
            text = enterprisebranchusercontacttdes.EntUserNic;
            if (base.ModelState.IsValid && (guid != enterprisebranchusercontacttdes.EntGuid || guid2 != enterprisebranchusercontacttdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                db.EnterpriseBranchUserContactTDES.Add(enterprisebranchusercontacttdes);
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = guid,
                    searchEntBranchGuid = guid2,
                    searchEntUserNic = text
                });
            }
            ViewBagHelper(guid, guid2, text);
            ViewBagListsHelper(enterprisebranchusercontacttdes.ContactTypeId);
            return View(enterprisebranchusercontacttdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public ActionResult Edit(Guid? id = default(Guid?))
        {
            EnterpriseBranchUserContactTDES enterpriseBranchUserContactTDES = null;
            string text = null;
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                    enterpriseBranchUserContactTDES = (from e in db.EnterpriseBranchUserContactTDES
                                                       where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                       select e).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                        enterpriseBranchUserContactTDES = (from e in db.EnterpriseBranchUserContactTDES
                                                           where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                           select e).FirstOrDefault();
                    }
                }
            }
            else
            {
                enterpriseBranchUserContactTDES = (from e in db.EnterpriseBranchUserContactTDES
                                                   where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                   select e).FirstOrDefault();
            }
            if (enterpriseBranchUserContactTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseBranchUserContactTDES.EntGuid;
            text = enterpriseBranchUserContactTDES.EntUserNic;
            searchEntBranchGuid = enterpriseBranchUserContactTDES.EntBranchGuid;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid, text);
            ViewBagListsHelper(enterpriseBranchUserContactTDES.ContactTypeId);
            return View(enterpriseBranchUserContactTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(EnterpriseBranchUserContactTDES enterprisebranchusercontacttdes)
        {
            UserIsInRoles();
            Guid? guid = null;
            Guid? guid2 = null;
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            string text = null;
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                guid = enterprisebranchusercontacttdes.EntGuid;
                guid2 = enterprisebranchusercontacttdes.EntBranchGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    guid = (from e in db.EnterpriseUserTDES
                            where e.EntUserNic == aName
                            select e.EntGuid).FirstOrDefault();
                    guid2 = enterprisebranchusercontacttdes.EntBranchGuid;
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES != null)
                    {
                        guid = enterpriseBranchIdsTDES.EntGuid;
                        guid2 = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                    else
                    {
                        guid = null;
                        guid2 = null;
                    }
                }
            }
            text = enterprisebranchusercontacttdes.EntUserNic;
            if (base.ModelState.IsValid && (guid != enterprisebranchusercontacttdes.EntGuid || guid2 != enterprisebranchusercontacttdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                db.Entry(enterprisebranchusercontacttdes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = guid,
                    searchEntBranchGuid = guid2,
                    searchEntUserNic = text
                });
            }
            ViewBagHelper(guid, guid2, text);
            ViewBagListsHelper(enterprisebranchusercontacttdes.ContactTypeId);
            return View(enterprisebranchusercontacttdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public ActionResult Delete(Guid? id = default(Guid?))
        {
            EnterpriseBranchUserContactTDES enterpriseBranchUserContactTDES = null;
            string text = null;
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                    enterpriseBranchUserContactTDES = (from e in db.EnterpriseBranchUserContactTDES
                                                       where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                       select e).Include((EnterpriseBranchUserContactTDES x) => x.ContactType).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                        enterpriseBranchUserContactTDES = (from e in db.EnterpriseBranchUserContactTDES
                                                           where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                           select e).Include((EnterpriseBranchUserContactTDES x) => x.ContactType).FirstOrDefault();
                    }
                }
            }
            else
            {
                enterpriseBranchUserContactTDES = (from e in db.EnterpriseBranchUserContactTDES
                                                   where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                   select e).Include((EnterpriseBranchUserContactTDES x) => x.ContactType).FirstOrDefault();
            }
            if (enterpriseBranchUserContactTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseBranchUserContactTDES.EntGuid;
            text = enterpriseBranchUserContactTDES.EntUserNic;
            searchEntBranchGuid = enterpriseBranchUserContactTDES.EntBranchGuid;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid, text);
            return View(enterpriseBranchUserContactTDES);
        }

        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public ActionResult DeleteConfirmed(Guid? id)
        {
            EnterpriseBranchUserContactTDES enterpriseBranchUserContactTDES = null;
            string text = null;
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                    enterpriseBranchUserContactTDES = (from e in db.EnterpriseBranchUserContactTDES
                                                       where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                       select e).Include((EnterpriseBranchUserContactTDES x) => x.ContactType).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                        enterpriseBranchUserContactTDES = (from e in db.EnterpriseBranchUserContactTDES
                                                           where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                           select e).Include((EnterpriseBranchUserContactTDES x) => x.ContactType).FirstOrDefault();
                    }
                }
            }
            else
            {
                enterpriseBranchUserContactTDES = (from e in db.EnterpriseBranchUserContactTDES
                                                   where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                   select e).Include((EnterpriseBranchUserContactTDES x) => x.ContactType).FirstOrDefault();
            }
            if (enterpriseBranchUserContactTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseBranchUserContactTDES.EntGuid;
            text = enterpriseBranchUserContactTDES.EntUserNic;
            searchEntBranchGuid = enterpriseBranchUserContactTDES.EntBranchGuid;
            db.EnterpriseBranchUserContactTDES.Remove(enterpriseBranchUserContactTDES);
            db.SaveChanges();
            return RedirectToAction("Index", new
            {
                searchEntGuid = searchEntGuid,
                searchEntBranchGuid = searchEntBranchGuid,
                searchEntUserNic = text
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
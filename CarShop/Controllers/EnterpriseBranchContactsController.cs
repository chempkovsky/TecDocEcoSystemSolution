// CarShop.Controllers.EnterpriseBranchContactsController
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

    public class EnterpriseBranchContactsController : Controller
    {
        private CarShopContext db = new CarShopContext();

        public void ViewBagHelper(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            EnterpriseBranchTDES enterpriseBranchTDES = (from e in db.EnterpriseBranchTDES
                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                         select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefault();
            if (enterpriseBranchTDES != null)
            {
                base.ViewBag.EntDescription = enterpriseBranchTDES.EnterpriseTDES.EntDescription;
                base.ViewBag.EntBranchDescription = enterpriseBranchTDES.EntBranchDescription;
                base.ViewBag.SearchEntGuid = searchEntGuid;
                base.ViewBag.SearchEntBranchGuid = searchEntBranchGuid;
                return;
            }
            base.ViewBag.EntBranchDescription = Resources.EnterpriseBranchTDES_NOTFOUND;
            base.ViewBag.SearchEntBranchGuid = null;
            base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
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

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
            base.ViewBag.IsBranchAdmin = base.User.IsInRole("BranchAdmin");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Index(Guid? searchEntGuid, Guid? searchEntBranchGuid, int? showIsVisible, int? showIsActive)
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
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            IQueryable<EnterpriseBranchContactsTDES> source = from e in db.EnterpriseBranchContactsTDES
                                                              where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
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
            source.Include((EnterpriseBranchContactsTDES x) => x.ContactType);
            return View(source.ToList());
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Details(Guid? id = default(Guid?))
        {
            EnterpriseBranchContactsTDES enterpriseBranchContactsTDES = null;
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
                    enterpriseBranchContactsTDES = (from e in db.EnterpriseBranchContactsTDES
                                                    where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                    select e).Include((EnterpriseBranchContactsTDES x) => x.ContactType).FirstOrDefault();
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
                        enterpriseBranchContactsTDES = (from e in db.EnterpriseBranchContactsTDES
                                                        where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                        select e).Include((EnterpriseBranchContactsTDES x) => x.ContactType).FirstOrDefault();
                    }
                }
            }
            else
            {
                enterpriseBranchContactsTDES = (from e in db.EnterpriseBranchContactsTDES
                                                where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                select e).Include((EnterpriseBranchContactsTDES x) => x.ContactType).FirstOrDefault();
            }
            if (enterpriseBranchContactsTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseBranchContactsTDES.EntGuid;
            searchEntBranchGuid = enterpriseBranchContactsTDES.EntBranchGuid;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterpriseBranchContactsTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public ActionResult Create(Guid? searchEntGuid, Guid? searchEntBranchGuid)
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
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            EnterpriseBranchContactsTDES model = null;
            if (base.ViewBag.SearchEntBranchGuid != null)
            {
                base.ViewBag.ContactTypes = new SelectList(db.ContactType, "ContactTypeId", "ContactTypeDescription");
                EnterpriseBranchContactsTDES enterpriseBranchContactsTDES = new EnterpriseBranchContactsTDES();
                enterpriseBranchContactsTDES.ContactGuid = Guid.NewGuid();
                enterpriseBranchContactsTDES.EntBranchGuid = searchEntBranchGuid.Value;
                enterpriseBranchContactsTDES.EntGuid = searchEntGuid.Value;
                model = enterpriseBranchContactsTDES;
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [HttpPost]
        public ActionResult Create(EnterpriseBranchContactsTDES enterprisebranchcontactstdes)
        {
            UserIsInRoles();
            Guid? guid = null;
            Guid? guid2 = null;
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                guid = enterprisebranchcontactstdes.EntGuid;
                guid2 = enterprisebranchcontactstdes.EntBranchGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    guid = (from e in db.EnterpriseUserTDES
                            where e.EntUserNic == aName
                            select e.EntGuid).FirstOrDefault();
                    guid2 = enterprisebranchcontactstdes.EntBranchGuid;
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
            if (base.ModelState.IsValid && (guid != enterprisebranchcontactstdes.EntGuid || guid2 != enterprisebranchcontactstdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                db.EnterpriseBranchContactsTDES.Add(enterprisebranchcontactstdes);
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = guid,
                    searchEntBranchGuid = guid2
                });
            }
            ViewBagHelper(guid, guid2);
            base.ViewBag.ContactTypes = new SelectList(db.ContactType, "ContactTypeId", "ContactTypeDescription", enterprisebranchcontactstdes.ContactTypeId);
            return View(enterprisebranchcontactstdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public ActionResult Edit(Guid? id = default(Guid?))
        {
            EnterpriseBranchContactsTDES enterpriseBranchContactsTDES = null;
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
                    enterpriseBranchContactsTDES = (from e in db.EnterpriseBranchContactsTDES
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
                        enterpriseBranchContactsTDES = (from e in db.EnterpriseBranchContactsTDES
                                                        where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                        select e).FirstOrDefault();
                    }
                }
            }
            else
            {
                enterpriseBranchContactsTDES = (from e in db.EnterpriseBranchContactsTDES
                                                where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                select e).FirstOrDefault();
            }
            if (enterpriseBranchContactsTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseBranchContactsTDES.EntGuid;
            searchEntBranchGuid = enterpriseBranchContactsTDES.EntBranchGuid;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            base.ViewBag.ContactTypes = new SelectList(db.ContactType, "ContactTypeId", "ContactTypeDescription", enterpriseBranchContactsTDES.ContactTypeId);
            return View(enterpriseBranchContactsTDES);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EnterpriseBranchContactsTDES enterprisebranchcontactstdes)
        {
            UserIsInRoles();
            Guid? guid = null;
            Guid? guid2 = null;
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                guid = enterprisebranchcontactstdes.EntGuid;
                guid2 = enterprisebranchcontactstdes.EntBranchGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    guid = (from e in db.EnterpriseUserTDES
                            where e.EntUserNic == aName
                            select e.EntGuid).FirstOrDefault();
                    guid2 = enterprisebranchcontactstdes.EntBranchGuid;
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
            if (base.ModelState.IsValid && (guid != enterprisebranchcontactstdes.EntGuid || guid2 != enterprisebranchcontactstdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                db.Entry(enterprisebranchcontactstdes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = guid,
                    searchEntBranchGuid = guid2
                });
            }
            ViewBagHelper(guid, guid2);
            base.ViewBag.ContactTypes = new SelectList(db.ContactType, "ContactTypeId", "ContactTypeDescription", enterprisebranchcontactstdes.ContactTypeId);
            return View(enterprisebranchcontactstdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public ActionResult Delete(Guid? id = default(Guid?))
        {
            EnterpriseBranchContactsTDES enterpriseBranchContactsTDES = null;
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
                    enterpriseBranchContactsTDES = (from e in db.EnterpriseBranchContactsTDES
                                                    where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                    select e).Include((EnterpriseBranchContactsTDES x) => x.ContactType).FirstOrDefault();
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
                        enterpriseBranchContactsTDES = (from e in db.EnterpriseBranchContactsTDES
                                                        where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                        select e).Include((EnterpriseBranchContactsTDES x) => x.ContactType).FirstOrDefault();
                    }
                }
            }
            else
            {
                enterpriseBranchContactsTDES = (from e in db.EnterpriseBranchContactsTDES
                                                where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                select e).Include((EnterpriseBranchContactsTDES x) => x.ContactType).FirstOrDefault();
            }
            if (enterpriseBranchContactsTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseBranchContactsTDES.EntGuid;
            searchEntBranchGuid = enterpriseBranchContactsTDES.EntBranchGuid;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterpriseBranchContactsTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid? id)
        {
            EnterpriseBranchContactsTDES enterpriseBranchContactsTDES = null;
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
                    enterpriseBranchContactsTDES = (from e in db.EnterpriseBranchContactsTDES
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
                        enterpriseBranchContactsTDES = (from e in db.EnterpriseBranchContactsTDES
                                                        where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                        select e).FirstOrDefault();
                    }
                }
            }
            else
            {
                enterpriseBranchContactsTDES = (from e in db.EnterpriseBranchContactsTDES
                                                where (Guid)(Guid?)e.ContactGuid == (Guid)id
                                                select e).FirstOrDefault();
            }
            if (enterpriseBranchContactsTDES == null)
            {
                return HttpNotFound();
            }
            if (enterpriseBranchContactsTDES != null)
            {
                searchEntGuid = enterpriseBranchContactsTDES.EntGuid;
                searchEntBranchGuid = enterpriseBranchContactsTDES.EntBranchGuid;
            }
            db.EnterpriseBranchContactsTDES.Remove(enterpriseBranchContactsTDES);
            db.SaveChanges();
            return RedirectToAction("Index", new
            {
                searchEntGuid,
                searchEntBranchGuid
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
// CarShop.Controllers.EnterpriseUsersController
using CarShop.Models;
using CarShop.Properties;
using CarShop.Utility;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterpriseUsersController : Controller
    {
        private CarShopContext db = new CarShopContext();

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Autocomplete(Guid? searchEntGuid, string term)
        {
            UserIsInRoles();
            if (!string.IsNullOrEmpty(term))
            {
                term = term.Trim();
            }
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
                else
                {
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value2;
                }
            }
            if (string.IsNullOrEmpty(term))
            {
                var enterprises2 = (from e in db.EnterpriseUserTDES
                                    where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                    select new
                                    {
                                        value = e.EntUserNic
                                    }).Take(10);
                return Json(await enterprises2.ToListAsync(), JsonRequestBehavior.AllowGet);
            }
            var enterprises = (from e in db.EnterpriseUserTDES
                               where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntUserNic.Contains(term)
                               select new
                               {
                                   value = e.EntUserNic
                               }).Take(10);
            return Json(await enterprises.ToListAsync(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid, int? showIsActive, int? showIsAdmin, int? showIsAudit, int? page, string currentFilter)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
                else
                {
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value2;
                }
            }
            EnterpriseTDES enterprises = await (from e in db.EnterpriseTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
            IQueryable<EnterpriseUserTDES> enterpriseusertdes2 = from e in db.EnterpriseUserTDES
                                                                 where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                 select e;
            showIsActive = (showIsActive ?? 2);
            int? num = showIsActive;
            int valueOrDefault = num.GetValueOrDefault();
            if (!num.HasValue)
            {
                goto IL_0902;
            }
            switch (valueOrDefault)
            {
                case 2:
                    break;
                case 3:
                    goto IL_0894;
                default:
                    goto IL_0902;
            }
            enterpriseusertdes2 = from e in enterpriseusertdes2
                                  where e.IsActive == true
                                  select e;
            goto IL_0913;
        IL_0902:
            showIsActive = 1;
            goto IL_0913;
        IL_0894:
            enterpriseusertdes2 = from e in enterpriseusertdes2
                                  where e.IsActive == false
                                  select e;
            goto IL_0913;
        IL_0b28:
            enterpriseusertdes2 = from e in enterpriseusertdes2
                                  where e.IsAudit == false
                                  select e;
            goto IL_0ba7;
        IL_0a5d:
            showIsAudit = (showIsAudit ?? 1);
            int? num2 = showIsAudit;
            int valueOrDefault2 = num2.GetValueOrDefault();
            if (!num2.HasValue)
            {
                goto IL_0b96;
            }
            switch (valueOrDefault2)
            {
                case 2:
                    break;
                case 3:
                    goto IL_0b28;
                default:
                    goto IL_0b96;
            }
            enterpriseusertdes2 = from e in enterpriseusertdes2
                                  where e.IsAudit == true
                                  select e;
            goto IL_0ba7;
        IL_0913:
            showIsAdmin = (showIsAdmin ?? 1);
            int? num3 = showIsAdmin;
            int valueOrDefault3 = num3.GetValueOrDefault();
            if (!num3.HasValue)
            {
                goto IL_0a4c;
            }
            switch (valueOrDefault3)
            {
                case 2:
                    break;
                case 3:
                    goto IL_09de;
                default:
                    goto IL_0a4c;
            }
            enterpriseusertdes2 = from e in enterpriseusertdes2
                                  where e.IsAdmin == true
                                  select e;
            goto IL_0a5d;
        IL_0a4c:
            showIsAdmin = 1;
            goto IL_0a5d;
        IL_0ba7:
            if (!string.IsNullOrEmpty(currentFilter))
            {
                enterpriseusertdes2 = from e in enterpriseusertdes2
                                      where e.EntUserNic.Contains(currentFilter)
                                      select e;
            }
            enterpriseusertdes2 = from e in enterpriseusertdes2
                                  orderby e.EntUserNic
                                  select e;
            base.ViewBag.searchEntGuid = searchEntGuid;
            base.ViewBag.showIsAudit = showIsAudit;
            base.ViewBag.showIsAdmin = showIsAdmin;
            base.ViewBag.showIsActive = showIsActive;
            base.ViewBag.currentFilter = currentFilter;
            if (enterprises != null)
            {
                base.ViewBag.EntDescription = enterprises.EntDescription;
            }
            else
            {
                base.ViewBag.EntDescription = Resources.ENTERPRISE_NOT_DEFINED;
                base.ViewBag.SearchEntGuid = null;
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            base.ViewBag.sliIsActive = SwitcherListUtil.SelectListHelper(showIsActive.Value);
            base.ViewBag.sliIsAdmin = SwitcherListUtil.SelectListHelper(showIsAdmin.Value);
            base.ViewBag.sliIsAudit = SwitcherListUtil.SelectListHelper(showIsAudit.Value);
            int pageSize = 20;
            int pageNumber = page ?? 1;
            return View(await enterpriseusertdes2.ToPagedListAsync(pageNumber, pageSize));
        IL_09de:
            enterpriseusertdes2 = from e in enterpriseusertdes2
                                  where e.IsAdmin == false
                                  select e;
            goto IL_0a5d;
        IL_0b96:
            showIsAudit = 1;
            goto IL_0ba7;
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(string userId = null)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseUserTDES enterpriseusertdes;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
                else
                {
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value2;
                }
                enterpriseusertdes = await db.EnterpriseUserTDES.Include((EnterpriseUserTDES i) => i.EnterpriseTDES).SingleOrDefaultAsync((EnterpriseUserTDES x) => x.EntUserNic == userId && (Guid)(Guid?)x.EntGuid == (Guid)searchEntGuid);
            }
            else
            {
                enterpriseusertdes = await db.EnterpriseUserTDES.Include((EnterpriseUserTDES i) => i.EnterpriseTDES).SingleOrDefaultAsync((EnterpriseUserTDES x) => x.EntUserNic == userId);
            }
            if (enterpriseusertdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.SearchEntGuid = enterpriseusertdes.EntGuid;
            base.ViewBag.EntDescription = enterpriseusertdes.EnterpriseTDES.EntDescription;
            return View(enterpriseusertdes);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        [ActionName("Details")]
        public async Task<ActionResult> DetailsConfirmed(string userId)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseUserTDES enterpriseusertdes;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
                enterpriseusertdes = await db.EnterpriseUserTDES.Include((EnterpriseUserTDES i) => i.EnterpriseTDES).SingleOrDefaultAsync((EnterpriseUserTDES x) => x.EntUserNic == userId && (Guid)(Guid?)x.EntGuid == (Guid)searchEntGuid);
            }
            else
            {
                enterpriseusertdes = await db.EnterpriseUserTDES.Include((EnterpriseUserTDES i) => i.EnterpriseTDES).SingleOrDefaultAsync((EnterpriseUserTDES x) => x.EntUserNic == userId);
            }
            if (enterpriseusertdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.SearchEntGuid = enterpriseusertdes.EntGuid;
            base.ViewBag.EntDescription = enterpriseusertdes.EnterpriseTDES.EntDescription;
            EntAdminActions.CreateEnterpriseUserAccount(enterpriseusertdes);
            base.ModelState.AddModelError("", Resources.CurrentOptionsAreApplied);
            return View(enterpriseusertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Create(Guid? searchEntGuid)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
            }
            EnterpriseTDES enterprises = await (from e in db.EnterpriseTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
            if (enterprises != null)
            {
                base.ViewBag.SearchEntGuid = enterprises.EntGuid;
                base.ViewBag.EntDescription = enterprises.EntDescription;
            }
            else
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
                base.ViewBag.EntDescription = Resources.ENTERPRISE_NOT_DEFINED;
                base.ViewBag.SearchEntGuid = null;
            }
            return View(new EnterpriseUserTDES
            {
                EntGuid = base.ViewBag.SearchEntGuid
            });
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EnterpriseUserTDES enterpriseusertdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
            }
            else
            {
                searchEntGuid = enterpriseusertdes.EntGuid;
            }
            EnterpriseTDES enterprises = await (from e in db.EnterpriseTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
            if (enterprises != null)
            {
                base.ViewBag.SearchEntGuid = enterprises.EntGuid;
                base.ViewBag.EntDescription = enterprises.EntDescription;
            }
            else
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
                base.ViewBag.EntDescription = Resources.ENTERPRISE_NOT_DEFINED;
                base.ViewBag.SearchEntGuid = null;
            }
            if (base.ModelState.IsValid && enterpriseusertdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            if (base.ModelState.IsValid)
            {
                db.EnterpriseUserTDES.Add(enterpriseusertdes);
                await db.SaveChangesAsync();
                if (enterpriseusertdes != null)
                {
                    EntAdminActions.CreateEnterpriseUserAccount(enterpriseusertdes);
                }
                return RedirectToAction("Index", new
                {
                    searchEntGuid
                });
            }
            return View(enterpriseusertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(string userId = null)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseUserTDES enterpriseusertdes;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
                enterpriseusertdes = await db.EnterpriseUserTDES.Include((EnterpriseUserTDES i) => i.EnterpriseTDES).SingleOrDefaultAsync((EnterpriseUserTDES x) => x.EntUserNic == userId && (Guid)(Guid?)x.EntGuid == (Guid)searchEntGuid);
            }
            else
            {
                enterpriseusertdes = await db.EnterpriseUserTDES.Include((EnterpriseUserTDES i) => i.EnterpriseTDES).SingleOrDefaultAsync((EnterpriseUserTDES x) => x.EntUserNic == userId);
            }
            if (enterpriseusertdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.SearchEntGuid = enterpriseusertdes.EntGuid;
            base.ViewBag.EntDescription = enterpriseusertdes.EnterpriseTDES.EntDescription;
            return View(enterpriseusertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(EnterpriseUserTDES enterpriseusertdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
            }
            else
            {
                searchEntGuid = enterpriseusertdes.EntGuid;
            }
            EnterpriseTDES enterprises = await (from e in db.EnterpriseTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
            if (enterprises != null)
            {
                base.ViewBag.SearchEntGuid = enterprises.EntGuid;
                base.ViewBag.EntDescription = enterprises.EntDescription;
            }
            else
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
                base.ViewBag.EntDescription = Resources.ENTERPRISE_NOT_DEFINED;
                base.ViewBag.SearchEntGuid = null;
            }
            if (base.ModelState.IsValid && enterpriseusertdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            if (base.ModelState.IsValid)
            {
                db.Entry(enterpriseusertdes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (enterpriseusertdes != null)
                {
                    EntAdminActions.CreateEnterpriseUserAccount(enterpriseusertdes);
                }
                return RedirectToAction("Index", new
                {
                    searchEntGuid = enterpriseusertdes.EntGuid
                });
            }
            return View(enterpriseusertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Delete(string userId = null)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseUserTDES enterpriseusertdes;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
                enterpriseusertdes = await db.EnterpriseUserTDES.Include((EnterpriseUserTDES i) => i.EnterpriseTDES).SingleOrDefaultAsync((EnterpriseUserTDES x) => x.EntUserNic == userId && (Guid)(Guid?)x.EntGuid == (Guid)searchEntGuid);
            }
            else
            {
                enterpriseusertdes = await db.EnterpriseUserTDES.Include((EnterpriseUserTDES i) => i.EnterpriseTDES).SingleOrDefaultAsync((EnterpriseUserTDES x) => x.EntUserNic == userId);
            }
            if (enterpriseusertdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.SearchEntGuid = enterpriseusertdes.EntGuid;
            base.ViewBag.EntDescription = enterpriseusertdes.EnterpriseTDES.EntDescription;
            return View(enterpriseusertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(string userId)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseUserTDES enterpriseusertdes;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
                enterpriseusertdes = await db.EnterpriseUserTDES.Include((EnterpriseUserTDES i) => i.EnterpriseTDES).SingleOrDefaultAsync((EnterpriseUserTDES x) => x.EntUserNic == userId && (Guid)(Guid?)x.EntGuid == (Guid)searchEntGuid);
            }
            else
            {
                enterpriseusertdes = await db.EnterpriseUserTDES.Include((EnterpriseUserTDES i) => i.EnterpriseTDES).SingleOrDefaultAsync((EnterpriseUserTDES x) => x.EntUserNic == userId);
            }
            if (enterpriseusertdes == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseusertdes.EntGuid;
            if (enterpriseusertdes != null)
            {
                EntAdminActions.DeleteEnterpriseUserAccount(enterpriseusertdes);
            }
            db.EnterpriseUserTDES.Remove(enterpriseusertdes);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
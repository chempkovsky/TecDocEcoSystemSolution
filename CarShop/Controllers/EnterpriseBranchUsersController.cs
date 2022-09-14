// CarShop.Controllers.EnterpriseBranchUsersController
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

    public class EnterpriseBranchUsersController : Controller
    {
        private CarShopContext db = new CarShopContext();

        public async Task ViewBagHelper(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            EnterpriseBranchTDES enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                               select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefaultAsync();
            if (enterprisebranchtdes != null)
            {
                base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
                base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
                base.ViewBag.SearchEntGuid = searchEntGuid;
                base.ViewBag.SearchEntBranchGuid = searchEntBranchGuid;
                return;
            }
            base.ViewBag.EntBranchDescription = Resources.EnterpriseBranchTDES_NOTFOUND;
            base.ViewBag.SearchEntBranchGuid = null;
            base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            EnterpriseTDES enterprises = await (from e in db.EnterpriseTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
            if (enterprises != null)
            {
                base.ViewBag.EntDescription = enterprises.EntDescription;
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
        public async Task<ActionResult> Index(Guid? searchEntGuid, Guid? searchEntBranchGuid, int? showIsVisible, int? showIsActive, int? showIsAdmin, int? showIsSeller)
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
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            IQueryable<EnterpriseBranchUserTDES> enterprisebranchusertdes = from e in db.EnterpriseBranchUserTDES
                                                                            where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                            select e;
            showIsActive = (showIsActive ?? 2);
            int? num = showIsActive;
            int valueOrDefault = num.GetValueOrDefault();
            if (!num.HasValue)
            {
                goto IL_0985;
            }
            switch (valueOrDefault)
            {
                case 2:
                    break;
                case 3:
                    goto IL_0917;
                default:
                    goto IL_0985;
            }
            enterprisebranchusertdes = from e in enterprisebranchusertdes
                                       where e.IsActive == true
                                       select e;
            goto IL_0996;
        IL_0c19:
            showIsAdmin = 1;
            goto IL_0c2a;
        IL_0d74:
            base.ViewBag.showIsActive = showIsActive;
            base.ViewBag.sliIsActive = SwitcherListUtil.SelectListHelper(showIsActive.Value);
            base.ViewBag.showIsVisible = showIsVisible;
            base.ViewBag.sliIsVisible = SwitcherListUtil.SelectListHelper(showIsVisible.Value);
            base.ViewBag.showIsAdmin = showIsAdmin;
            base.ViewBag.sliIsAdmin = SwitcherListUtil.SelectListHelper(showIsAdmin.Value);
            base.ViewBag.showIsSeller = showIsSeller;
            base.ViewBag.sliIsSeller = SwitcherListUtil.SelectListHelper(showIsSeller.Value);
            return View(await enterprisebranchusertdes.ToListAsync());
        IL_0cf5:
            enterprisebranchusertdes = from e in enterprisebranchusertdes
                                       where e.IsSeller == false
                                       select e;
            goto IL_0d74;
        IL_0bab:
            enterprisebranchusertdes = from e in enterprisebranchusertdes
                                       where e.IsAdmin == false
                                       select e;
            goto IL_0c2a;
        IL_0d63:
            showIsSeller = 1;
            goto IL_0d74;
        IL_0acf:
            showIsVisible = 1;
            goto IL_0ae0;
        IL_0a61:
            enterprisebranchusertdes = from e in enterprisebranchusertdes
                                       where e.IsVisible == false
                                       select e;
            goto IL_0ae0;
        IL_0917:
            enterprisebranchusertdes = from e in enterprisebranchusertdes
                                       where e.IsActive == false
                                       select e;
            goto IL_0996;
        IL_0985:
            showIsActive = 1;
            goto IL_0996;
        IL_0996:
            showIsVisible = (showIsVisible ?? 2);
            int? num2 = showIsVisible;
            int valueOrDefault2 = num2.GetValueOrDefault();
            if (!num2.HasValue)
            {
                goto IL_0acf;
            }
            switch (valueOrDefault2)
            {
                case 2:
                    break;
                case 3:
                    goto IL_0a61;
                default:
                    goto IL_0acf;
            }
            enterprisebranchusertdes = from e in enterprisebranchusertdes
                                       where e.IsVisible == true
                                       select e;
            goto IL_0ae0;
        IL_0c2a:
            showIsSeller = (showIsSeller ?? 2);
            int? num3 = showIsSeller;
            int valueOrDefault3 = num3.GetValueOrDefault();
            if (!num3.HasValue)
            {
                goto IL_0d63;
            }
            switch (valueOrDefault3)
            {
                case 2:
                    break;
                case 3:
                    goto IL_0cf5;
                default:
                    goto IL_0d63;
            }
            enterprisebranchusertdes = from e in enterprisebranchusertdes
                                       where e.IsSeller == true
                                       select e;
            goto IL_0d74;
        IL_0ae0:
            showIsAdmin = (showIsAdmin ?? 3);
            int? num4 = showIsAdmin;
            int valueOrDefault4 = num4.GetValueOrDefault();
            if (!num4.HasValue)
            {
                goto IL_0c19;
            }
            switch (valueOrDefault4)
            {
                case 2:
                    break;
                case 3:
                    goto IL_0bab;
                default:
                    goto IL_0c19;
            }
            enterprisebranchusertdes = from e in enterprisebranchusertdes
                                       where e.IsAdmin == true
                                       select e;
            goto IL_0c2a;
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(string userId = null)
        {
            EnterpriseBranchUserTDES enterprisebranchusertdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchusertdes = await (from e in db.EnterpriseBranchUserTDES
                                                      where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntUserNic == userId
                                                      select e).FirstOrDefaultAsync();
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        enterprisebranchusertdes = await (from e in db.EnterpriseBranchUserTDES
                                                          where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == userId
                                                          select e).FirstOrDefaultAsync();
                    }
                }
            }
            else
            {
                enterprisebranchusertdes = await db.EnterpriseBranchUserTDES.FindAsync(userId);
            }
            if (enterprisebranchusertdes == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterprisebranchusertdes.EntGuid;
            searchEntBranchGuid = enterprisebranchusertdes.EntBranchGuid;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterprisebranchusertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
        public async Task<ActionResult> DetailsConfirmed(string userId)
        {
            EnterpriseBranchUserTDES enterprisebranchusertdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchusertdes = await (from e in db.EnterpriseBranchUserTDES
                                                      where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntUserNic == userId
                                                      select e).FirstOrDefaultAsync();
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        enterprisebranchusertdes = await (from e in db.EnterpriseBranchUserTDES
                                                          where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == userId
                                                          select e).FirstOrDefaultAsync();
                    }
                }
            }
            else
            {
                enterprisebranchusertdes = await db.EnterpriseBranchUserTDES.FindAsync(userId);
            }
            if (enterprisebranchusertdes == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterprisebranchusertdes.EntGuid;
            searchEntBranchGuid = enterprisebranchusertdes.EntBranchGuid;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (enterprisebranchusertdes != null)
            {
                EntAdminActions.CreateEnterpriseBranchUserAccount(enterprisebranchusertdes);
            }
            base.ModelState.AddModelError("", Resources.CurrentOptionsAreApplied);
            return View(enterprisebranchusertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public async Task<ActionResult> Create(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            EnterpriseBranchUserTDES aUser = null;
            if (base.ViewBag.SearchEntBranchGuid != null)
            {
                aUser = new EnterpriseBranchUserTDES
                {
                    EntBranchGuid = searchEntBranchGuid.Value,
                    EntGuid = searchEntGuid.Value
                };
            }
            return View(aUser);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EnterpriseBranchUserTDES enterprisebranchusertdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid;
            Guid? searchEntBranchGuid;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                    searchEntBranchGuid = enterprisebranchusertdes.EntBranchGuid;
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            else
            {
                searchEntGuid = enterprisebranchusertdes.EntGuid;
                searchEntBranchGuid = enterprisebranchusertdes.EntBranchGuid;
            }
            if (base.ModelState.IsValid && (searchEntGuid != enterprisebranchusertdes.EntGuid || searchEntBranchGuid != enterprisebranchusertdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                db.EnterpriseBranchUserTDES.Add(enterprisebranchusertdes);
                db.SaveChanges();
                if (enterprisebranchusertdes != null)
                {
                    EntAdminActions.CreateEnterpriseBranchUserAccount(enterprisebranchusertdes);
                }
                return RedirectToAction("Index", new
                {
                    searchEntGuid,
                    searchEntBranchGuid
                });
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterprisebranchusertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public async Task<ActionResult> Edit(string userId = null)
        {
            EnterpriseBranchUserTDES enterprisebranchusertdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchusertdes = await (from e in db.EnterpriseBranchUserTDES
                                                      where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntUserNic == userId
                                                      select e).FirstOrDefaultAsync();
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        enterprisebranchusertdes = await (from e in db.EnterpriseBranchUserTDES
                                                          where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == userId
                                                          select e).FirstOrDefaultAsync();
                    }
                }
            }
            else
            {
                enterprisebranchusertdes = await db.EnterpriseBranchUserTDES.FindAsync(userId);
            }
            if (enterprisebranchusertdes == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterprisebranchusertdes.EntGuid;
            searchEntBranchGuid = enterprisebranchusertdes.EntBranchGuid;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterprisebranchusertdes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public async Task<ActionResult> Edit(EnterpriseBranchUserTDES enterprisebranchusertdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid;
            Guid? searchEntBranchGuid;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                    searchEntBranchGuid = enterprisebranchusertdes.EntBranchGuid;
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            else
            {
                searchEntGuid = enterprisebranchusertdes.EntGuid;
                searchEntBranchGuid = enterprisebranchusertdes.EntBranchGuid;
            }
            if (base.ModelState.IsValid && (searchEntGuid != enterprisebranchusertdes.EntGuid || searchEntBranchGuid != enterprisebranchusertdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                db.Entry(enterprisebranchusertdes).State = EntityState.Modified;
                db.SaveChanges();
                if (enterprisebranchusertdes != null)
                {
                    EntAdminActions.CreateEnterpriseBranchUserAccount(enterprisebranchusertdes);
                }
                return RedirectToAction("Index", new
                {
                    searchEntGuid,
                    searchEntBranchGuid
                });
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterprisebranchusertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public async Task<ActionResult> Delete(string userId = null)
        {
            EnterpriseBranchUserTDES enterprisebranchusertdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchusertdes = await (from e in db.EnterpriseBranchUserTDES
                                                      where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntUserNic == userId
                                                      select e).FirstOrDefaultAsync();
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        enterprisebranchusertdes = await (from e in db.EnterpriseBranchUserTDES
                                                          where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == userId
                                                          select e).FirstOrDefaultAsync();
                    }
                }
            }
            else
            {
                enterprisebranchusertdes = await db.EnterpriseBranchUserTDES.FindAsync(userId);
            }
            if (enterprisebranchusertdes == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterprisebranchusertdes.EntGuid;
            searchEntBranchGuid = enterprisebranchusertdes.EntBranchGuid;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterprisebranchusertdes);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public async Task<ActionResult> DeleteConfirmed(string userId)
        {
            EnterpriseBranchUserTDES enterprisebranchusertdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchusertdes = await (from e in db.EnterpriseBranchUserTDES
                                                      where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntUserNic == userId
                                                      select e).FirstOrDefaultAsync();
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        enterprisebranchusertdes = await (from e in db.EnterpriseBranchUserTDES
                                                          where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == userId
                                                          select e).FirstOrDefaultAsync();
                    }
                }
            }
            else
            {
                enterprisebranchusertdes = await db.EnterpriseBranchUserTDES.FindAsync(userId);
            }
            if (enterprisebranchusertdes == null)
            {
                return HttpNotFound();
            }
            if (enterprisebranchusertdes != null)
            {
                searchEntGuid = enterprisebranchusertdes.EntGuid;
                searchEntBranchGuid = enterprisebranchusertdes.EntBranchGuid;
            }
            if (enterprisebranchusertdes != null)
            {
                EntAdminActions.DeleteEnterpriseBranchUserAccount(enterprisebranchusertdes);
            }
            db.EnterpriseBranchUserTDES.Remove(enterprisebranchusertdes);
            await db.SaveChangesAsync();
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
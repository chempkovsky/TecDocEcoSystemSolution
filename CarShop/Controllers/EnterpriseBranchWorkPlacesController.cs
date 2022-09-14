// CarShop.Controllers.EnterpriseBranchWorkPlacesController
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

    public class EnterpriseBranchWorkPlacesController : Controller
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
        public async Task<ActionResult> Index(Guid? searchEntGuid, Guid? searchEntBranchGuid, int? showIsActive)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid? reference = searchEntGuid;
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
            IQueryable<EnterpriseBranchWorkPlaceTDES> enterprisebranchworkplacetdes = from e in db.EnterpriseBranchWorkPlaceTDES
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
            enterprisebranchworkplacetdes = from e in enterprisebranchworkplacetdes
                                            where e.IsActive == true
                                            select e;
            goto IL_0996;
        IL_0917:
            enterprisebranchworkplacetdes = from e in enterprisebranchworkplacetdes
                                            where e.IsActive == false
                                            select e;
            goto IL_0996;
        IL_0996:
            base.ViewBag.showIsActive = showIsActive;
            base.ViewBag.sliIsActive = SwitcherListUtil.SelectListHelper(showIsActive.Value);
            return View(await enterprisebranchworkplacetdes.ToListAsync());
        IL_0985:
            showIsActive = 1;
            goto IL_0996;
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? id = default(Guid?))
        {
            EnterpriseBranchWorkPlaceTDES enterprisebranchworkplacetdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid? reference = searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchworkplacetdes = await (from e in db.EnterpriseBranchWorkPlaceTDES
                                                           where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id
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
                        enterprisebranchworkplacetdes = await (from e in db.EnterpriseBranchWorkPlaceTDES
                                                               where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id
                                                               select e).FirstOrDefaultAsync();
                    }
                }
            }
            else
            {
                enterprisebranchworkplacetdes = await (from e in db.EnterpriseBranchWorkPlaceTDES
                                                       where (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id
                                                       select e).FirstOrDefaultAsync();
            }
            if (enterprisebranchworkplacetdes == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterprisebranchworkplacetdes.EntGuid;
            searchEntBranchGuid = enterprisebranchworkplacetdes.EntBranchGuid;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterprisebranchworkplacetdes);
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
            EnterpriseBranchWorkPlaceTDES enterprisebranchworkplacetdes = null;
            if (base.ViewBag.SearchEntBranchGuid != null)
            {
                enterprisebranchworkplacetdes = new EnterpriseBranchWorkPlaceTDES
                {
                    WorkPlaceGuid = Guid.NewGuid(),
                    EntBranchGuid = searchEntBranchGuid.Value,
                    EntGuid = searchEntGuid.Value
                };
            }
            return View(enterprisebranchworkplacetdes);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EnterpriseBranchWorkPlaceTDES enterprisebranchworkplacetdes)
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
                    searchEntBranchGuid = enterprisebranchworkplacetdes.EntBranchGuid;
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
                searchEntGuid = enterprisebranchworkplacetdes.EntGuid;
                searchEntBranchGuid = enterprisebranchworkplacetdes.EntBranchGuid;
            }
            if (base.ModelState.IsValid && (searchEntGuid != enterprisebranchworkplacetdes.EntGuid || searchEntBranchGuid != enterprisebranchworkplacetdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                db.EnterpriseBranchWorkPlaceTDES.Add(enterprisebranchworkplacetdes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid,
                    searchEntBranchGuid
                });
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterprisebranchworkplacetdes);
        }

        public async Task<ActionResult> Edit(Guid? id = default(Guid?))
        {
            EnterpriseBranchWorkPlaceTDES enterprisebranchworkplacetdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid? reference = searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchworkplacetdes = await (from e in db.EnterpriseBranchWorkPlaceTDES
                                                           where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id
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
                        enterprisebranchworkplacetdes = await (from e in db.EnterpriseBranchWorkPlaceTDES
                                                               where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id
                                                               select e).FirstOrDefaultAsync();
                    }
                }
            }
            else
            {
                enterprisebranchworkplacetdes = await (from e in db.EnterpriseBranchWorkPlaceTDES
                                                       where (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id
                                                       select e).FirstOrDefaultAsync();
            }
            if (enterprisebranchworkplacetdes == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterprisebranchworkplacetdes.EntGuid;
            searchEntBranchGuid = enterprisebranchworkplacetdes.EntBranchGuid;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterprisebranchworkplacetdes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EnterpriseBranchWorkPlaceTDES enterprisebranchworkplacetdes)
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
                    searchEntBranchGuid = enterprisebranchworkplacetdes.EntBranchGuid;
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
                searchEntGuid = enterprisebranchworkplacetdes.EntGuid;
                searchEntBranchGuid = enterprisebranchworkplacetdes.EntBranchGuid;
            }
            if (base.ModelState.IsValid && (searchEntGuid != enterprisebranchworkplacetdes.EntGuid || searchEntBranchGuid != enterprisebranchworkplacetdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                db.Entry(enterprisebranchworkplacetdes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid,
                    searchEntBranchGuid
                });
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterprisebranchworkplacetdes);
        }

        public async Task<ActionResult> Delete(Guid? id = default(Guid?))
        {
            EnterpriseBranchWorkPlaceTDES enterprisebranchworkplacetdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid? reference = searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchworkplacetdes = await (from e in db.EnterpriseBranchWorkPlaceTDES
                                                           where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id
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
                        enterprisebranchworkplacetdes = await (from e in db.EnterpriseBranchWorkPlaceTDES
                                                               where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id
                                                               select e).FirstOrDefaultAsync();
                    }
                }
            }
            else
            {
                enterprisebranchworkplacetdes = await (from e in db.EnterpriseBranchWorkPlaceTDES
                                                       where (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id
                                                       select e).FirstOrDefaultAsync();
            }
            if (enterprisebranchworkplacetdes == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterprisebranchworkplacetdes.EntGuid;
            searchEntBranchGuid = enterprisebranchworkplacetdes.EntBranchGuid;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterprisebranchworkplacetdes);
        }

        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(Guid? id)
        {
            EnterpriseBranchWorkPlaceTDES enterprisebranchworkplacetdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid? reference = searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchworkplacetdes = await (from e in db.EnterpriseBranchWorkPlaceTDES
                                                           where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id
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
                        enterprisebranchworkplacetdes = await (from e in db.EnterpriseBranchWorkPlaceTDES
                                                               where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id
                                                               select e).FirstOrDefaultAsync();
                    }
                }
            }
            else
            {
                enterprisebranchworkplacetdes = await (from e in db.EnterpriseBranchWorkPlaceTDES
                                                       where (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id
                                                       select e).FirstOrDefaultAsync();
            }
            if (enterprisebranchworkplacetdes == null)
            {
                return HttpNotFound();
            }
            if (enterprisebranchworkplacetdes != null)
            {
                searchEntGuid = enterprisebranchworkplacetdes.EntGuid;
                searchEntBranchGuid = enterprisebranchworkplacetdes.EntBranchGuid;
            }
            db.EnterpriseBranchWorkPlaceTDES.Remove(enterprisebranchworkplacetdes);
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
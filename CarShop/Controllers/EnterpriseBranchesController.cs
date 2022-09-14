// CarShop.Controllers.EnterpriseBranchesController
using CarShop.Models;
using CarShop.Properties;
using CarShop.Utility;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterpriseBranchesController : Controller
    {
        private readonly CarShopContext db = new CarShopContext();

        public async Task ViewBagHelper(Guid? searchEntGuid)
        {
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
            base.ViewBag.IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid, int? showIsActive, int? showIsVisible, int? showBranchTypeId)
        {
            UserIsInRoles();
            EnterpriseBranchIdsTDES searchEntBranchIds = null;
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
                    searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                where e.EntUserNic == aName
                                                select new EnterpriseBranchIdsTDES
                                                {
                                                    EntGuid = e.EntGuid,
                                                    EntBranchGuid = e.EntBranchGuid
                                                }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid);
            IQueryable<EnterpriseBranchTDES> enterprisebranchtdes2 = from e in db.EnterpriseBranchTDES
                                                                     where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                     select e;
            if (searchEntBranchIds != null)
            {
                enterprisebranchtdes2 = from e in db.EnterpriseBranchTDES
                                        where e.EntBranchGuid == searchEntBranchIds.EntBranchGuid
                                        select e;
            }
            showIsActive = (showIsActive ?? 2);
            int? num = showIsActive;
            int valueOrDefault = num.GetValueOrDefault();
            if (!num.HasValue)
            {
                goto IL_09d3;
            }
            switch (valueOrDefault)
            {
                case 2:
                    break;
                case 3:
                    goto IL_0965;
                default:
                    goto IL_09d3;
            }
            enterprisebranchtdes2 = from e in enterprisebranchtdes2
                                    where e.IsActive == true
                                    select e;
            goto IL_09e4;
        IL_0aaf:
            enterprisebranchtdes2 = from e in enterprisebranchtdes2
                                    where e.IsVisible == false
                                    select e;
            goto IL_0b2e;
        IL_0b2e:
            showBranchTypeId = (showBranchTypeId ?? (-1));
            if (showBranchTypeId > -1)
            {
                enterprisebranchtdes2 = from e in enterprisebranchtdes2
                                        where (int?)e.BranchTypeId == showBranchTypeId
                                        select e;
            }
            base.ViewBag.sliIsActive = SwitcherListUtil.SelectListHelper(showIsActive.Value);
            base.ViewBag.sliIsVisible = SwitcherListUtil.SelectListHelper(showIsVisible.Value);
            base.ViewBag.sliBranchType = new SelectList(await db.BranchType.ToListAsync(), "BranchTypeId", "BranchTypeDescription", showBranchTypeId);
            base.ViewBag.showIsActive = showIsActive;
            base.ViewBag.showIsVisible = showIsVisible;
            base.ViewBag.showBranchTypeId = showBranchTypeId;
            enterprisebranchtdes2 = enterprisebranchtdes2.Include((EnterpriseBranchTDES e) => e.BranchType);
            return View(await enterprisebranchtdes2.ToListAsync());
        IL_0b1d:
            showIsVisible = 1;
            goto IL_0b2e;
        IL_0965:
            enterprisebranchtdes2 = from e in enterprisebranchtdes2
                                    where e.IsActive == false
                                    select e;
            goto IL_09e4;
        IL_09d3:
            showIsActive = 1;
            goto IL_09e4;
        IL_09e4:
            showIsVisible = (showIsVisible ?? 1);
            int? num2 = showIsVisible;
            int valueOrDefault2 = num2.GetValueOrDefault();
            if (!num2.HasValue)
            {
                goto IL_0b1d;
            }
            switch (valueOrDefault2)
            {
                case 2:
                    break;
                case 3:
                    goto IL_0aaf;
                default:
                    goto IL_0b1d;
            }
            enterprisebranchtdes2 = from e in enterprisebranchtdes2
                                    where e.IsVisible == true
                                    select e;
            goto IL_0b2e;
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseBranchTDES enterprisebranchtdes;
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
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
                else
                {
                    Guid? reference2 = id;
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntBranchGuid).FirstOrDefaultAsync();
                    id = value2;
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
            }
            else
            {
                enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                              where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                              select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
            }
            if (enterprisebranchtdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EnterpriseTDES.EntGuid;
            return View(enterprisebranchtdes);
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
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                }
            }
            EnterpriseBranchTDES enterprisebranchtdes = null;
            await ViewBagHelper(searchEntGuid);
            if (base.ViewBag.SearchEntGuid != null)
            {
                base.ViewBag.sliBranchType = new SelectList(db.BranchType, "BranchTypeId", "BranchTypeDescription");
                enterprisebranchtdes = new EnterpriseBranchTDES
                {
                    EntBranchGuid = Guid.NewGuid(),
                    EntGuid = searchEntGuid.Value
                };
            }
            return View(enterprisebranchtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EnterpriseBranchTDES enterprisebranchtdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                }
            }
            else
            {
                searchEntGuid = enterprisebranchtdes.EntGuid;
            }
            if (base.ModelState.IsValid && enterprisebranchtdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            if (base.ModelState.IsValid)
            {
                db.EnterpriseBranchTDES.Add(enterprisebranchtdes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid
                });
            }
            await ViewBagHelper(searchEntGuid);
            if (base.ViewBag.SearchEntGuid != null)
            {
                base.ViewBag.sliBranchType = new SelectList(await db.BranchType.ToListAsync(), "BranchTypeId", "BranchTypeDescription");
                enterprisebranchtdes = new EnterpriseBranchTDES
                {
                    EntBranchGuid = Guid.NewGuid(),
                    EntGuid = searchEntGuid.Value
                };
            }
            return View(enterprisebranchtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public async Task<ActionResult> Edit(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseBranchTDES enterprisebranchtdes;
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
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
                else
                {
                    Guid? reference2 = id;
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntBranchGuid).FirstOrDefaultAsync();
                    id = value2;
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
            }
            else
            {
                enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                              where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                              select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
            }
            if (enterprisebranchtdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EnterpriseTDES.EntGuid;
            base.ViewBag.sliBranchType = new SelectList(await db.BranchType.ToListAsync(), "BranchTypeId", "BranchTypeDescription", enterprisebranchtdes.BranchTypeId);
            return View(enterprisebranchtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(EnterpriseBranchTDES enterprisebranchtdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                }
                else
                {
                    if ((Guid?)(await (from e in db.EnterpriseBranchUserTDES
                                       where e.EntUserNic == aName
                                       select e.EntBranchGuid).FirstOrDefaultAsync()) != (Guid?)enterprisebranchtdes.EntBranchGuid)
                    {
                        base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                    }
                    searchEntGuid = enterprisebranchtdes.EntGuid;
                }
            }
            else
            {
                searchEntGuid = enterprisebranchtdes.EntGuid;
            }
            if (base.ModelState.IsValid && enterprisebranchtdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            if (base.ModelState.IsValid)
            {
                db.Entry(enterprisebranchtdes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid
                });
            }
            await ViewBagHelper(searchEntGuid);
            base.ViewBag.sliBranchType = new SelectList(await db.BranchType.ToListAsync(), "BranchTypeId", "BranchTypeDescription", enterprisebranchtdes.BranchTypeId);
            return View(enterprisebranchtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Delete(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseBranchTDES enterprisebranchtdes;
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
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
                else
                {
                    Guid? reference2 = id;
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntBranchGuid).FirstOrDefaultAsync();
                    id = value2;
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
            }
            else
            {
                enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                              where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                              select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
            }
            if (enterprisebranchtdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EnterpriseTDES.EntGuid;
            return View(enterprisebranchtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid? id)
        {
            EnterpriseBranchTDES enterprisebranchtdes = null;
            UserIsInRoles();
            Guid? searchEntGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    Guid? reference = searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                  select e).FirstOrDefaultAsync();
                }
            }
            else
            {
                enterprisebranchtdes = await db.EnterpriseBranchTDES.FindAsync(id);
                searchEntGuid = enterprisebranchtdes.EntGuid;
            }
            db.EnterpriseBranchTDES.Remove(enterprisebranchtdes);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid
            });
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> DoMakeReply(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseBranchTDES enterprisebranchtdes;
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
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
                else
                {
                    Guid? reference2 = id;
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntBranchGuid).FirstOrDefaultAsync();
                    id = value2;
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
            }
            else
            {
                enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                              where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                              select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
            }
            if (enterprisebranchtdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EnterpriseTDES.EntGuid;
            string aKey = enterprisebranchtdes.EntBranchGuid.ToString() + "BRANCHREPLY";
            ReplyRestsdData aData = System.Web.HttpContext.Current.Cache[aKey] as ReplyRestsdData;
            if (aData != null)
            {
                if (aData.isDone)
                {
                    base.ModelState.AddModelError("", "Для данного подразделения был ранее запущен процесс, который успешно завершился.");
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else if (aData.hasError)
                {
                    base.ModelState.AddModelError("", "Для данного подразделения  был ранее запущен процесс, который завершился С ошибкой: " + aData.ErrorText);
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else
                {
                    base.ModelState.AddModelError("", "Для данного подразделения  был ранее запущен процесс, который еще не закончен.");
                }
            }
            else
            {
                base.ModelState.AddModelError("", "Для данного подразделения нет запущеных процессов.");
            }
            return View(enterprisebranchtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [HttpPost]
        [ActionName("DoMakeReply")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoReMakeReply(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseBranchTDES enterprisebranchtdes;
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
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
                else
                {
                    Guid? reference2 = id;
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntBranchGuid).FirstOrDefaultAsync();
                    id = value2;
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
            }
            else
            {
                enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                              where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                              select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
            }
            if (enterprisebranchtdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EnterpriseTDES.EntGuid;
            string aKey = enterprisebranchtdes.EntBranchGuid.ToString() + "BRANCHREPLY";
            ReplyRestsdData aData2 = System.Web.HttpContext.Current.Cache[aKey] as ReplyRestsdData;
            if (aData2 != null)
            {
                if (aData2.isDone)
                {
                    base.ModelState.AddModelError("", "Для данного подразделения был ранее запущен процесс, который успешно завершился.");
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else if (aData2.hasError)
                {
                    base.ModelState.AddModelError("", "Для данного подразделения был ранее запущен процесс, который завершился С ошибкой: " + aData2.ErrorText);
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else
                {
                    base.ModelState.AddModelError("", "Для данного подразделения был ранее запущен процесс, который еще не закончен.");
                }
            }
            else
            {
                EnterpriseBranchReplyTDES enterprisebranchreplytdes = await (from e in db.EnterpriseBranchReplyTDES
                                                                             where (Guid)(Guid?)e.EntBranchGuid == (Guid)id && e.ReplyType == 1
                                                                             select e).FirstOrDefaultAsync();
                if (enterprisebranchreplytdes == null)
                {
                    base.ModelState.AddModelError("", "Нет настроек для выполнения репликации данных.");
                    return View(enterprisebranchtdes);
                }
                HttpClient aHttpClient;
                try
                {
                    aHttpClient = new HttpClient
                    {
                        BaseAddress = new Uri(enterprisebranchreplytdes.BaseHttpAddress)
                    };
                }
                catch (Exception ex)
                {
                    base.ModelState.AddModelError("", "Не могу установить соединение с сервером репликации: " + ex.Message);
                    return View(enterprisebranchtdes);
                }
                if ((await aHttpClient.GetAsync(string.Format(enterprisebranchreplytdes.HttpLoginUrl + "?UserName={0}&Password={1}", enterprisebranchreplytdes.HttpUser, enterprisebranchreplytdes.HttpPassword))).StatusCode != HttpStatusCode.OK)
                {
                    base.ModelState.AddModelError("", "Не могу Пройти авторизацию");
                    return View(enterprisebranchtdes);
                }
                aData2 = new ReplyRestsdData
                {
                    dbRest = new CarShopRestContext(enterprisebranchtdes.TecDocCatalog),
                    httpClient = aHttpClient,
                    BranchGuid = enterprisebranchreplytdes.EntBranchGuid,
                    PostUriString = enterprisebranchreplytdes.HttpPostUrl,
                    hasError = false,
                    isDone = false,
                    ErrorText = null
                };
                try
                {
                    System.Web.HttpContext.Current.Cache.Add(aKey, aData2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10.0), CacheItemPriority.Normal, null);
                }
                catch
                {
                    base.ModelState.AddModelError("", "Не могу добавить данные в КЭШ. Сервер перегружен. попробуйте позже");
                }
                if (base.ModelState.IsValid)
                {
                    Thread thread = new Thread(ReplyRests.DoReplyRests);
                    thread.Start(aData2);
                    base.ModelState.AddModelError("", "Для данного подразделения запущен процесс репликации. Для просмотра результатов вернитесь на данную страницу в течение 20 мин.");
                }
            }
            return View(enterprisebranchtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> DoMakeOrdersReply(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseBranchTDES enterprisebranchtdes;
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
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
                else
                {
                    Guid? reference2 = id;
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntBranchGuid).FirstOrDefaultAsync();
                    id = value2;
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
            }
            else
            {
                enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                              where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                              select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
            }
            if (enterprisebranchtdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EnterpriseTDES.EntGuid;
            string aKey = enterprisebranchtdes.EntBranchGuid.ToString() + "BRANCHORDERREPLY";
            ReplyOrdersData aData = System.Web.HttpContext.Current.Cache[aKey] as ReplyOrdersData;
            if (aData != null)
            {
                if (aData.isDone)
                {
                    base.ModelState.AddModelError("", "Для данного подразделения был ранее запущен процесс, который успешно завершился.");
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else if (aData.hasError)
                {
                    base.ModelState.AddModelError("", "Для данного подразделения  был ранее запущен процесс, который завершился С ошибкой: " + aData.ErrorText);
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else
                {
                    base.ModelState.AddModelError("", "Для данного подразделения  был ранее запущен процесс, который еще не закончен.");
                }
            }
            else
            {
                base.ModelState.AddModelError("", "Для данного подразделения нет запущеных процессов.");
            }
            return View(enterprisebranchtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [HttpPost]
        [ActionName("DoMakeOrdersReply")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoReMakeOrdersReply(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseBranchTDES enterprisebranchtdes;
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
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
                else
                {
                    Guid? reference2 = id;
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntBranchGuid).FirstOrDefaultAsync();
                    id = value2;
                    enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                                  select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
                }
            }
            else
            {
                enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                              where (Guid)(Guid?)e.EntBranchGuid == (Guid)id
                                              select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).Include((EnterpriseBranchTDES x) => x.BranchType).FirstOrDefaultAsync();
            }
            if (enterprisebranchtdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EnterpriseTDES.EntGuid;
            string aKey = enterprisebranchtdes.EntBranchGuid.ToString() + "BRANCHORDERREPLY";
            ReplyOrdersData aData2 = System.Web.HttpContext.Current.Cache[aKey] as ReplyOrdersData;
            if (aData2 != null)
            {
                if (aData2.isDone)
                {
                    base.ModelState.AddModelError("", "Для данного подразделения был ранее запущен процесс, который успешно завершился.");
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else if (aData2.hasError)
                {
                    base.ModelState.AddModelError("", "Для данного подразделения был ранее запущен процесс, который завершился С ошибкой: " + aData2.ErrorText);
                    System.Web.HttpContext.Current.Cache.Remove(aKey);
                }
                else
                {
                    base.ModelState.AddModelError("", "Для данного подразделения был ранее запущен процесс, который еще не закончен.");
                }
            }
            else
            {
                string aBaseHttpAddress = null;
                string aHttpLoginUrl = null;
                string aProfileUriString = null;
                string aOrderUriString = null;
                string aArticleUriString = null;
                string aHttpUser = null;
                string aHttpPassword = null;
                Guid aBranchGuid = Guid.Empty;
                foreach (EnterpriseBranchReplyTDES item in await (from e in db.EnterpriseBranchReplyTDES
                                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)id && e.ReplyType > 1 && e.ReplyType < 5
                                                                  select e).ToListAsync())
                {
                    switch (item.ReplyType)
                    {
                        case 2:
                            aProfileUriString = item.HttpGetUrl;
                            aBaseHttpAddress = item.BaseHttpAddress;
                            aHttpLoginUrl = item.HttpLoginUrl;
                            aHttpUser = item.HttpUser;
                            aHttpPassword = item.HttpPassword;
                            aBranchGuid = item.EntBranchGuid;
                            break;
                        case 3:
                            aOrderUriString = item.HttpGetUrl;
                            break;
                        case 4:
                            aArticleUriString = item.HttpGetUrl;
                            break;
                    }
                }
                if (string.IsNullOrEmpty(aBaseHttpAddress) || string.IsNullOrEmpty(aHttpLoginUrl) || string.IsNullOrEmpty(aProfileUriString) || string.IsNullOrEmpty(aOrderUriString) || string.IsNullOrEmpty(aArticleUriString) || string.IsNullOrEmpty(aHttpUser) || string.IsNullOrEmpty(aHttpPassword))
                {
                    base.ModelState.AddModelError("", "Нет настроек для выполнения репликации данных.");
                    return View(enterprisebranchtdes);
                }
                HttpClient aHttpClient = new HttpClient
                {
                    BaseAddress = new Uri(aBaseHttpAddress)
                };
                if ((await aHttpClient.GetAsync(string.Format(aHttpLoginUrl + "?UserName={0}&Password={1}", aHttpUser, aHttpPassword))).StatusCode != HttpStatusCode.OK)
                {
                    base.ModelState.AddModelError("", "Не могу Пройти авторизацию");
                    return View(enterprisebranchtdes);
                }
                aData2 = new ReplyOrdersData
                {
                    dbOrders = new CarShopOrdersContext(enterprisebranchtdes.OrderCatalog),
                    httpClient = aHttpClient,
                    BranchGuid = aBranchGuid,
                    OrderUriString = aOrderUriString,
                    ArticleUriString = aArticleUriString,
                    ProfileUriString = aProfileUriString,
                    hasError = false,
                    isDone = false,
                    ErrorText = null
                };
                try
                {
                    System.Web.HttpContext.Current.Cache.Add(aKey, aData2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10.0), CacheItemPriority.Normal, null);
                }
                catch
                {
                    base.ModelState.AddModelError("", "Не могу добавить данные в КЭШ. Сервер перегружен. попробуйте позже");
                }
                if (base.ModelState.IsValid)
                {
                    Thread thread = new Thread(ReplyOrders.DoReplyOrders);
                    thread.Start(aData2);
                    base.ModelState.AddModelError("", "Для данного подразделения запущен процесс репликации Заказов. Для просмотра результатов вернитесь на данную страницу в течение 20 мин.");
                }
            }
            return View(enterprisebranchtdes);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
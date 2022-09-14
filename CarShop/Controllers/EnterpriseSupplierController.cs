// CarShop.Controllers.EnterpriseSupplierController
using CarShop.Models;
using CarShop.Properties;
using CarShop.Utility;
using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterpriseSupplierController : Controller
    {
        private CarShopContext db = new CarShopContext();

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
        }

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

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Autocomplete(Guid? searchEntGuid, int? currentFilterBy, int? currentIsActive, string term)
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
            currentFilterBy = (currentFilterBy ?? 1);
            currentIsActive = (currentIsActive ?? 1);
            if (!string.IsNullOrEmpty(term))
            {
                term = term.Trim();
            }
            IQueryable<EnterpriseSupplierTDES> enterprisesuppliertdes = from e in db.EnterpriseSupplierTDES
                                                                        where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                        select e;
            int? num = currentIsActive;
            int valueOrDefault = num.GetValueOrDefault();
            if (num.HasValue)
            {
                switch (valueOrDefault)
                {
                    case 2:
                        enterprisesuppliertdes = from e in enterprisesuppliertdes
                                                 where e.IsActive == true
                                                 select e;
                        break;
                    case 3:
                        enterprisesuppliertdes = from e in enterprisesuppliertdes
                                                 where e.IsActive == false
                                                 select e;
                        break;
                }
            }
            if (!string.IsNullOrEmpty(term))
            {
                enterprisesuppliertdes = ((currentFilterBy != 1) ? (from e in enterprisesuppliertdes
                                                                    where e.EntSupplierDescription.Contains(term)
                                                                    select e) : enterprisesuppliertdes.Where((EnterpriseSupplierTDES e) => e.EntSupplierId.StartsWith(term)));
            }
            if (currentFilterBy == 1)
            {
                return Json(await (from e in enterprisesuppliertdes
                                   select new
                                   {
                                       value = e.EntSupplierId
                                   }).Take(10).ToListAsync(), JsonRequestBehavior.AllowGet);
            }
            return Json(await (from e in enterprisesuppliertdes
                               select new
                               {
                                   value = e.EntSupplierDescription
                               }).Take(10).ToListAsync(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid, string searchString, int? searchStringBy, int? showIsActive, string currentFilter, int? currentFilterBy, int? currentIsActive, int? page)
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
            if (searchString != null || showIsActive.HasValue)
            {
                page = 1;
                searchStringBy = (searchStringBy ?? 1);
                showIsActive = (showIsActive ?? 1);
            }
            else
            {
                page = (page ?? 1);
                searchString = currentFilter;
                searchStringBy = (currentFilterBy ?? 1);
                showIsActive = (currentIsActive ?? 1);
            }
            IQueryable<EnterpriseSupplierTDES> enterprisesuppliertdes2 = from e in db.EnterpriseSupplierTDES
                                                                         where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                         select e;
            if (!string.IsNullOrEmpty(searchString))
            {
                if (searchStringBy == 1)
                {
                    enterprisesuppliertdes2 = from e in db.EnterpriseSupplierTDES
                                              where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntSupplierId.StartsWith(searchString)
                                              select e;
                }
                else
                {
                    searchStringBy = 2;
                    enterprisesuppliertdes2 = from e in db.EnterpriseSupplierTDES
                                              where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntSupplierDescription.Contains(searchString)
                                              select e;
                }
            }
            int? num = showIsActive;
            int valueOrDefault = num.GetValueOrDefault();
            if (!num.HasValue)
            {
                goto IL_0b18;
            }
            switch (valueOrDefault)
            {
                case 2:
                    break;
                case 3:
                    goto IL_0aaa;
                default:
                    goto IL_0b18;
            }
            enterprisesuppliertdes2 = from e in enterprisesuppliertdes2
                                      where e.IsActive == true
                                      select e;
            goto IL_0b29;
        IL_0aaa:
            enterprisesuppliertdes2 = from e in enterprisesuppliertdes2
                                      where e.IsActive == false
                                      select e;
            goto IL_0b29;
        IL_0b18:
            showIsActive = 1;
            goto IL_0b29;
        IL_0b29:
            base.ViewBag.sliSearchStringBy = SwitcherListUtil.SelectColumnListHelper(searchStringBy.Value);
            base.ViewBag.sliIsActive = SwitcherListUtil.SelectListHelper(showIsActive.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.СurrentFilterBy = searchStringBy;
            base.ViewBag.CurrentIsActive = showIsActive;
            enterprisesuppliertdes2 = from e in enterprisesuppliertdes2
                                      orderby e.EntSupplierId
                                      select e;
            await ViewBagHelper(searchEntGuid);
            int pageSize = 20;
            int pageNumber = page.Value;
            return View(await enterprisesuppliertdes2.ToPagedListAsync(pageNumber, pageSize));
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(string id = null, Guid? searchEntGuid = default(Guid?))
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
            EnterpriseSupplierTDES enterprisesuppliertdes = await (from e in db.EnterpriseSupplierTDES
                                                                   where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntSupplierId == id
                                                                   select e).FirstOrDefaultAsync();
            if (enterprisesuppliertdes == null)
            {
                return HttpNotFound();
            }
            await ViewBagHelper(searchEntGuid);
            return View(enterprisesuppliertdes);
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
            EnterpriseSupplierTDES enterprisesuppliertdes = null;
            await ViewBagHelper(searchEntGuid);
            if (base.ViewBag.SearchEntGuid != null)
            {
                enterprisesuppliertdes = new EnterpriseSupplierTDES
                {
                    EntSupplierId = "Nic" + Guid.NewGuid().ToString(),
                    EntGuid = searchEntGuid.Value
                };
            }
            return View(enterprisesuppliertdes);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EnterpriseSupplierTDES enterprisesuppliertdes)
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
                searchEntGuid = enterprisesuppliertdes.EntGuid;
            }
            if (base.ModelState.IsValid && enterprisesuppliertdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            if (base.ModelState.IsValid)
            {
                db.EnterpriseSupplierTDES.Add(enterprisesuppliertdes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid
                });
            }
            await ViewBagHelper(searchEntGuid);
            return View(enterprisesuppliertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(string id = null, Guid? searchEntGuid = default(Guid?))
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
            EnterpriseSupplierTDES enterprisesuppliertdes = await (from e in db.EnterpriseSupplierTDES
                                                                   where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntSupplierId == id
                                                                   select e).FirstOrDefaultAsync();
            if (enterprisesuppliertdes == null)
            {
                return HttpNotFound();
            }
            await ViewBagHelper(searchEntGuid);
            return View(enterprisesuppliertdes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(EnterpriseSupplierTDES enterprisesuppliertdes)
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
                searchEntGuid = enterprisesuppliertdes.EntGuid;
            }
            if (base.ModelState.IsValid && enterprisesuppliertdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            if (base.ModelState.IsValid)
            {
                db.Entry(enterprisesuppliertdes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid
                });
            }
            await ViewBagHelper(searchEntGuid);
            return View(enterprisesuppliertdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Delete(string id = null, Guid? searchEntGuid = default(Guid?))
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
            EnterpriseSupplierTDES enterprisesuppliertdes = await (from e in db.EnterpriseSupplierTDES
                                                                   where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntSupplierId == id
                                                                   select e).FirstOrDefaultAsync();
            if (enterprisesuppliertdes == null)
            {
                return HttpNotFound();
            }
            await ViewBagHelper(searchEntGuid);
            return View(enterprisesuppliertdes);
        }

        [ActionName("Delete")]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(string id = null, Guid? searchEntGuid = default(Guid?))
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
            EnterpriseSupplierTDES enterprisesuppliertdes = await (from e in db.EnterpriseSupplierTDES
                                                                   where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntSupplierId == id
                                                                   select e).FirstOrDefaultAsync();
            db.EnterpriseSupplierTDES.Remove(enterprisesuppliertdes);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid
            });
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> LookUpSupplierByID(string redirecData, string redirectContriller, string redirectAction, Guid? searchEntGuid, string searchString, int? searchStringBy, int? showIsActive, string currentFilter, int? currentFilterBy, int? currentIsActive, int? page)
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
            base.ViewBag.RedirecData = redirecData;
            base.ViewBag.RedirectContriller = redirectContriller;
            base.ViewBag.RedirectAction = redirectAction;
            if (searchString != null || showIsActive.HasValue)
            {
                page = 1;
                searchStringBy = (searchStringBy ?? 1);
                showIsActive = (showIsActive ?? 1);
            }
            else
            {
                page = (page ?? 1);
                searchString = currentFilter;
                searchStringBy = (currentFilterBy ?? 1);
                showIsActive = (currentIsActive ?? 1);
            }
            IQueryable<EnterpriseSupplierTDES> enterprisesuppliertdes2 = from e in db.EnterpriseSupplierTDES
                                                                         where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                         select e;
            if (!string.IsNullOrEmpty(searchString))
            {
                if (searchStringBy == 1)
                {
                    enterprisesuppliertdes2 = from e in db.EnterpriseSupplierTDES
                                              where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntSupplierId.StartsWith(searchString)
                                              select e;
                }
                else
                {
                    searchStringBy = 2;
                    enterprisesuppliertdes2 = from e in db.EnterpriseSupplierTDES
                                              where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntSupplierDescription.Contains(searchString)
                                              select e;
                }
            }
            int? num = showIsActive;
            int valueOrDefault = num.GetValueOrDefault();
            if (!num.HasValue)
            {
                goto IL_0c5c;
            }
            switch (valueOrDefault)
            {
                case 2:
                    break;
                case 3:
                    goto IL_0bee;
                default:
                    goto IL_0c5c;
            }
            enterprisesuppliertdes2 = from e in enterprisesuppliertdes2
                                      where e.IsActive == true
                                      select e;
            goto IL_0c6d;
        IL_0bee:
            enterprisesuppliertdes2 = from e in enterprisesuppliertdes2
                                      where e.IsActive == false
                                      select e;
            goto IL_0c6d;
        IL_0c5c:
            showIsActive = 1;
            goto IL_0c6d;
        IL_0c6d:
            base.ViewBag.sliSearchStringBy = SwitcherListUtil.SelectColumnListHelper(searchStringBy.Value);
            base.ViewBag.sliIsActive = SwitcherListUtil.SelectListHelper(showIsActive.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.СurrentFilterBy = searchStringBy;
            base.ViewBag.CurrentIsActive = showIsActive;
            enterprisesuppliertdes2 = from e in enterprisesuppliertdes2
                                      orderby e.EntSupplierId
                                      select e;
            await ViewBagHelper(searchEntGuid);
            int pageSize = 20;
            int pageNumber = page.Value;
            return View(await enterprisesuppliertdes2.ToPagedListAsync(pageNumber, pageSize));
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        [HttpPost]
        public ActionResult LookUpSupplierByIDSelected(string redirecData, string redirectContriller, string redirectAction, string SimpleSupplier, string DOSELECT_TITLE, string DOCANCEL_TITLE)
        {
            if (DOSELECT_TITLE != null)
            {
                string searchEntSupplierId = null;
                string searchEntSupplierDescription = null;
                if (SimpleSupplier != null)
                {
                    EnterpriseSupplierTDES enterpriseSupplierTDES = JsonConvert.DeserializeObject<EnterpriseSupplierTDES>(SimpleSupplier);
                    if (enterpriseSupplierTDES != null)
                    {
                        searchEntSupplierId = enterpriseSupplierTDES.EntSupplierId;
                        searchEntSupplierDescription = enterpriseSupplierTDES.EntSupplierDescription;
                    }
                }
                return RedirectToAction(redirectAction, redirectContriller, new
                {
                    redirecData,
                    searchEntSupplierId,
                    searchEntSupplierDescription
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
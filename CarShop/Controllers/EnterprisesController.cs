// CarShop.Controllers.EnterprisesController
using CarShop.Models;
using CarShop.Properties;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterprisesController : Controller
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

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Autocomplete(string term)
        {
            UserIsInRoles();
            if (base.ViewBag.IsEcoSystemAdmin)
            {
                if (!string.IsNullOrEmpty(term))
                {
                    term = term.Trim();
                }
                if (string.IsNullOrEmpty(term))
                {
                    var enterprises2 = (from e in db.EnterpriseTDES
                                        select new
                                        {
                                            value = e.EntDescription
                                        }).Take(10);
                    return Json(await enterprises2.ToListAsync(), JsonRequestBehavior.AllowGet);
                }
                var enterprises3 = (from e in db.EnterpriseTDES
                                    where e.EntDescription.Contains(term)
                                    select new
                                    {
                                        value = e.EntDescription
                                    }).Take(10);
                return Json(await enterprises3.ToListAsync(), JsonRequestBehavior.AllowGet);
            }
            string aName = base.User.Identity.Name;
            Guid? aGuid = null;
            if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
            {
                Guid value = await (from e in db.EnterpriseUserTDES
                                    where e.EntUserNic == aName
                                    select e.EntGuid).FirstOrDefaultAsync();
                aGuid = value;
            }
            else
            {
                Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefaultAsync();
                aGuid = value2;
            }
            var enterprises = (from e in db.EnterpriseTDES
                               where (Guid)(Guid?)e.EntGuid == (Guid)aGuid
                               select new
                               {
                                   value = e.EntDescription
                               }).Take(10);
            return Json(await enterprises.ToListAsync(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(string searchString, string currentFilter, int? page)
        {
            UserIsInRoles();
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            base.ViewBag.CurrentFilter = searchString;
            if (base.ViewBag.IsEcoSystemAdmin)
            {
                IOrderedQueryable<EnterpriseTDES> enterprises = from e in db.EnterpriseTDES.Include("EnterpriseTecDocSrcTypeTDES")
                                                                orderby e.EntDescription
                                                                select e;
                if (!string.IsNullOrEmpty(searchString))
                {
                    enterprises = from e in db.EnterpriseTDES.Include("EnterpriseTecDocSrcTypeTDES")
                                  where e.EntDescription.Contains(searchString)
                                  orderby e.EntDescription
                                  select e;
                }
                int pageNumber2 = page ?? 1;
                return View(await enterprises.ToPagedListAsync(pageNumber2, 20));
            }
            string aName = base.User.Identity.Name;
            Guid? aGuid = null;
            if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
            {
                Guid value = await (from e in db.EnterpriseUserTDES
                                    where e.EntUserNic == aName
                                    select e.EntGuid).FirstOrDefaultAsync();
                aGuid = value;
            }
            else
            {
                Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefaultAsync();
                aGuid = value2;
            }
            IOrderedQueryable<EnterpriseTDES> enterprises2 = from e in db.EnterpriseTDES.Include("EnterpriseTecDocSrcTypeTDES")
                                                             where (Guid)(Guid?)e.EntGuid == (Guid)aGuid
                                                             orderby e.EntDescription
                                                             select e;
            int pageNumber = page ?? 1;
            return View(await enterprises2.ToPagedListAsync(pageNumber, 20));
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                id = ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? new Guid?(await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                where e.EntUserNic == aName
                                                                                                                                select e.EntGuid).FirstOrDefaultAsync()) : new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                                                                                            where e.EntUserNic == aName
                                                                                                                                                                                            select e.EntGuid).FirstOrDefaultAsync()));
            }
            EnterpriseTDES enterprisetdes = await db.EnterpriseTDES.FindAsync(id);
            if (enterprisetdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisetdes);
        }

        [Authorize(Roles = "EcoSystemAdmin")]
        public async Task<ActionResult> Create()
        {
            EnterpriseTDES enterprisetdes = new EnterpriseTDES
            {
                EntGuid = Guid.NewGuid()
            };
            base.ViewBag.TecDocSrcTypeId = new SelectList(await db.EnterpriseTecDocSrcTypeTDES.ToListAsync(), "TecDocSrcTypeId", "TecDocSrcTypeDescr");
            return View(enterprisetdes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin")]
        public async Task<ActionResult> Create(EnterpriseTDES enterprisetdes)
        {
            if (base.ModelState.IsValid)
            {
                db.EnterpriseTDES.Add(enterprisetdes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            base.ViewBag.TecDocSrcTypeId = new SelectList(await db.EnterpriseTecDocSrcTypeTDES.ToListAsync(), "TecDocSrcTypeId", "TecDocSrcTypeDescr", enterprisetdes.TecDocSrcTypeId);
            return View(enterprisetdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                id = ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? new Guid?(await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                where e.EntUserNic == aName
                                                                                                                                select e.EntGuid).FirstOrDefaultAsync()) : new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                                                                                            where e.EntUserNic == aName
                                                                                                                                                                                            select e.EntGuid).FirstOrDefaultAsync()));
            }
            EnterpriseTDES enterprisetdes = await db.EnterpriseTDES.FindAsync(id);
            if (enterprisetdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.TecDocSrcTypeId = new SelectList(await db.EnterpriseTecDocSrcTypeTDES.ToListAsync(), "TecDocSrcTypeId", "TecDocSrcTypeDescr", enterprisetdes.TecDocSrcTypeId);
            return View(enterprisetdes);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(EnterpriseTDES enterprisetdes)
        {
            Guid? id = null;
            if (base.ModelState.IsValid)
            {
                UserIsInRoles();
                if ((!base.ViewBag.IsEcoSystemAdmin))
                {
                    string aName = base.User.Identity.Name;
                    id = ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? new Guid?(await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                    where e.EntUserNic == aName
                                                                                                                                    select e.EntGuid).FirstOrDefaultAsync()) : new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                                                                                                where e.EntUserNic == aName
                                                                                                                                                                                                select e.EntGuid).FirstOrDefaultAsync()));
                    if (!id.HasValue)
                    {
                        base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
                    }
                }
            }
            if (base.ModelState.IsValid)
            {
                if (base.ViewBag.IsEcoSystemAdmin)
                {
                    db.Entry(enterprisetdes).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                EnterpriseTDES enterprisetdesOld = await db.EnterpriseTDES.FindAsync(id);
                enterprisetdesOld.IsActive = enterprisetdes.IsActive;
                enterprisetdesOld.EntDescription = enterprisetdes.EntDescription;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            base.ViewBag.TecDocSrcTypeId = new SelectList(await db.EnterpriseTecDocSrcTypeTDES.ToListAsync(), "TecDocSrcTypeId", "TecDocSrcTypeDescr", enterprisetdes.TecDocSrcTypeId);
            return View(enterprisetdes);
        }

        [Authorize(Roles = "EcoSystemAdmin")]
        public async Task<ActionResult> Delete(Guid? id = default(Guid?))
        {
            EnterpriseTDES enterprisetdes = await db.EnterpriseTDES.FindAsync(id);
            if (enterprisetdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisetdes);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EnterpriseTDES enterprisetdes = await db.EnterpriseTDES.FindAsync(id);
            db.EnterpriseTDES.Remove(enterprisetdes);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
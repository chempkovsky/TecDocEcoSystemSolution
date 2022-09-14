// CarShop.Controllers.EnterpriseProductCategoryController
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

    public class EnterpriseProductCategoryController : Controller
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

        public async Task<ActionResult> Index(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            IQueryable<EnterpriseProductCategoryTDES> enterpriseproductcategorytdes = from e in db.EnterpriseProductCategoryTDES
                                                                                      where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                      select e;
            return View(await enterpriseproductcategorytdes.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id = default(int?), Guid? searchEntBranchGuid = default(Guid?))
        {
            EnterpriseProductCategoryTDES enterpriseproductcategorytdes = null;
            if (id.HasValue)
            {
                int aPCId = id.Value;
                enterpriseproductcategorytdes = await (from e in db.EnterpriseProductCategoryTDES
                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.PCId == aPCId
                                                       select e).FirstOrDefaultAsync();
            }
            if (enterpriseproductcategorytdes == null)
            {
                return HttpNotFound();
            }
            Guid searchEntGuid = enterpriseproductcategorytdes.EntGuid;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterpriseproductcategorytdes);
        }

        public async Task<ActionResult> Create(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            EnterpriseProductCategoryTDES aCategory = null;
            if (base.ViewBag.SearchEntBranchGuid != null)
            {
                aCategory = new EnterpriseProductCategoryTDES
                {
                    EntBranchGuid = searchEntBranchGuid.Value,
                    EntGuid = searchEntGuid.Value
                };
            }
            return View(aCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EnterpriseProductCategoryTDES enterpriseproductcategorytdes)
        {
            Guid searchEntGuid = enterpriseproductcategorytdes.EntGuid;
            Guid searchEntBranchGuid = enterpriseproductcategorytdes.EntBranchGuid;
            if (base.ModelState.IsValid)
            {
                db.EnterpriseProductCategoryTDES.Add(enterpriseproductcategorytdes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid,
                    searchEntBranchGuid
                });
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterpriseproductcategorytdes);
        }

        public async Task<ActionResult> Edit(int? id = default(int?), Guid? searchEntBranchGuid = default(Guid?))
        {
            EnterpriseProductCategoryTDES enterpriseproductcategorytdes = null;
            if (id.HasValue)
            {
                int aPCId = id.Value;
                enterpriseproductcategorytdes = await (from e in db.EnterpriseProductCategoryTDES
                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.PCId == aPCId
                                                       select e).FirstOrDefaultAsync();
            }
            if (enterpriseproductcategorytdes == null)
            {
                return HttpNotFound();
            }
            Guid searchEntGuid = enterpriseproductcategorytdes.EntGuid;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterpriseproductcategorytdes);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(EnterpriseProductCategoryTDES enterpriseproductcategorytdes)
        {
            Guid searchEntGuid = enterpriseproductcategorytdes.EntGuid;
            Guid searchEntBranchGuid = enterpriseproductcategorytdes.EntBranchGuid;
            if (base.ModelState.IsValid)
            {
                db.Entry(enterpriseproductcategorytdes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid,
                    searchEntBranchGuid
                });
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterpriseproductcategorytdes);
        }

        public async Task<ActionResult> Delete(int? id = default(int?), Guid? searchEntBranchGuid = default(Guid?))
        {
            EnterpriseProductCategoryTDES enterpriseproductcategorytdes = null;
            if (id.HasValue)
            {
                int aPCId = id.Value;
                enterpriseproductcategorytdes = await (from e in db.EnterpriseProductCategoryTDES
                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.PCId == aPCId
                                                       select e).FirstOrDefaultAsync();
            }
            if (enterpriseproductcategorytdes == null)
            {
                return HttpNotFound();
            }
            Guid searchEntGuid = enterpriseproductcategorytdes.EntGuid;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterpriseproductcategorytdes);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id = default(int?), Guid? searchEntBranchGuid = default(Guid?))
        {
            EnterpriseProductCategoryTDES enterpriseproductcategorytdes = null;
            if (id.HasValue)
            {
                int aPCId = id.Value;
                enterpriseproductcategorytdes = await (from e in db.EnterpriseProductCategoryTDES
                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.PCId == aPCId
                                                       select e).FirstOrDefaultAsync();
            }
            Guid? searchEntGuid = null;
            if (enterpriseproductcategorytdes != null)
            {
                searchEntGuid = enterpriseproductcategorytdes.EntGuid;
                db.EnterpriseProductCategoryTDES.Remove(enterpriseproductcategorytdes);
                await db.SaveChangesAsync();
            }
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
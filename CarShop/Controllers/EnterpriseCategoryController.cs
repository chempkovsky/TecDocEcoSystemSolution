// CarShop.Controllers.EnterpriseCategoryController
using CarShop.Models;
using CarShop.Properties;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterpriseCategoryController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopArticleContextCatalog;

        private CarShopArticleContext _dbBrand = new CarShopArticleContext();

        private CarShopArticleContext dbBrand
        {
            get
            {
                if (_dbBrand == null)
                {
                    _dbBrand = new CarShopArticleContext(CarShopArticleContextCatalog);
                }
                return _dbBrand;
            }
        }

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
        }

        protected async Task ViewBagHelper(Guid? searchEntGuid)
        {
            EnterpriseTDES enterprises = await (from e in db.EnterpriseTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
            if (enterprises != null)
            {
                base.ViewBag.EntDescription = enterprises.EntDescription;
                base.ViewBag.SearchEntGuid = searchEntGuid;
                CarShopArticleContextCatalog = enterprises.ArticleCatalog;
            }
            else
            {
                base.ViewBag.EntDescription = Resources.ENTERPRISE_NOT_DEFINED;
                base.ViewBag.SearchEntGuid = null;
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
        }

        protected SelectList SelectListHelper(int showIs)
        {
            return new SelectList(new SelectListItem[3]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.CategorySearchById,
                Selected = (showIs == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.CategorySearchByParent,
                Selected = (showIs == 2)
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.CategorySearchByDescr,
                Selected = (showIs == 3)
            }
            }, "Value", "Text", showIs);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid, string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page)
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
            await ViewBagHelper(searchEntGuid);
            IPagedList<EnterpriseCategoryTDES> aResult = null;
            if (CarShopArticleContextCatalog != null)
            {
                if (searchString != null)
                {
                    page = 1;
                    searchStringBy = (searchStringBy ?? 1);
                }
                else
                {
                    page = (page ?? 1);
                    searchString = currentFilter;
                    searchStringBy = (currentFilterBy ?? 1);
                }
                IOrderedQueryable<EnterpriseCategoryTDES> enterprisecategorytdes = from e in dbBrand.EnterpriseCategoryTDES
                                                                                   where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                   orderby e.CategoryId, e.EntGuid
                                                                                   select e;
                if (!string.IsNullOrEmpty(searchString))
                {
                    int? num = searchStringBy;
                    int valueOrDefault = num.GetValueOrDefault();
                    if (num.HasValue)
                    {
                        switch (valueOrDefault)
                        {
                            case 1:
                                {
                                    int aFilter = 0;
                                    if (!int.TryParse(searchString, out aFilter))
                                    {
                                        aFilter = 0;
                                    }
                                    enterprisecategorytdes = from e in dbBrand.EnterpriseCategoryTDES
                                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.CategoryId == aFilter
                                                             orderby e.CategoryId, e.EntGuid
                                                             select e;
                                    break;
                                }
                            case 2:
                                {
                                    int aFilter2 = 0;
                                    if (!int.TryParse(searchString, out aFilter2))
                                    {
                                        aFilter2 = 0;
                                    }
                                    enterprisecategorytdes = from e in dbBrand.EnterpriseCategoryTDES
                                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.CategoryParent == aFilter2
                                                             orderby e.CategoryId, e.EntGuid
                                                             select e;
                                    break;
                                }
                            case 3:
                                enterprisecategorytdes = from e in dbBrand.EnterpriseCategoryTDES
                                                         where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.CategoryDescription.StartsWith(searchString)
                                                         orderby e.CategoryId, e.EntGuid
                                                         select e;
                                break;
                        }
                    }
                }
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await enterprisecategorytdes.ToPagedListAsync(pageNumber, pageSize);
            }
            else
            {
                base.ModelState.AddModelError("", Resources.EnterpriseTDES_NOTDEFINED);
                searchStringBy = (searchStringBy ?? 1);
            }
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.СurrentFilterBy = searchStringBy;
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? searchEntGuid = default(Guid?), int? сategoryId = default(int?))
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
            }
            await ViewBagHelper(searchEntGuid);
            EnterpriseCategoryTDES enterprisecategorytdes = null;
            if (base.ModelState.IsValid)
            {
                enterprisecategorytdes = await (from e in dbBrand.EnterpriseCategoryTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (int?)e.CategoryId == сategoryId
                                                select e).FirstOrDefaultAsync();
            }
            if (enterprisecategorytdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisecategorytdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Create(Guid? searchEntGuid = default(Guid?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                searchEntGuid = ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? new Guid?((from e in db.EnterpriseBranchUserTDES
                                                                                                                                     where e.EntUserNic == aName
                                                                                                                                     select e.EntGuid).FirstOrDefault()) : new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                                                                                            where e.EntUserNic == aName
                                                                                                                                                                                            select e.EntGuid).FirstOrDefaultAsync()));
            }
            await ViewBagHelper(searchEntGuid);
            EnterpriseCategoryTDES enterprisecategorytdes = null;
            if (base.ModelState.IsValid)
            {
                enterprisecategorytdes = new EnterpriseCategoryTDES
                {
                    EntGuid = searchEntGuid.Value
                };
            }
            return View(enterprisecategorytdes);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EnterpriseCategoryTDES enterprisecategorytdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                searchEntGuid = ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? new Guid?(await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                           where e.EntUserNic == aName
                                                                                                                                           select e.EntGuid).FirstOrDefaultAsync()) : new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                                                                                                       where e.EntUserNic == aName
                                                                                                                                                                                                       select e.EntGuid).FirstOrDefaultAsync()));
            }
            else
            {
                searchEntGuid = enterprisecategorytdes.EntGuid;
            }
            await ViewBagHelper(searchEntGuid);
            if (base.ModelState.IsValid)
            {
                dbBrand.EnterpriseCategoryTDES.Add(enterprisecategorytdes);
                await dbBrand.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid
                });
            }
            return View(enterprisecategorytdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(Guid? searchEntGuid = default(Guid?), int? сategoryId = default(int?))
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
            EnterpriseCategoryTDES enterprisecategorytdes = null;
            await ViewBagHelper(searchEntGuid);
            if (base.ModelState.IsValid)
            {
                enterprisecategorytdes = await (from e in dbBrand.EnterpriseCategoryTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (int?)e.CategoryId == сategoryId
                                                select e).FirstOrDefaultAsync();
            }
            if (enterprisecategorytdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisecategorytdes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(EnterpriseCategoryTDES enterprisecategorytdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                searchEntGuid = ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? new Guid?(await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                           where e.EntUserNic == aName
                                                                                                                                           select e.EntGuid).FirstOrDefaultAsync()) : new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                                                                                                       where e.EntUserNic == aName
                                                                                                                                                                                                       select e.EntGuid).FirstOrDefaultAsync()));
            }
            else
            {
                searchEntGuid = enterprisecategorytdes.EntGuid;
            }
            await ViewBagHelper(searchEntGuid);
            if (base.ModelState.IsValid)
            {
                dbBrand.Entry(enterprisecategorytdes).State = EntityState.Modified;
                await dbBrand.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid
                });
            }
            return View(enterprisecategorytdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Delete(Guid? searchEntGuid = default(Guid?), int? сategoryId = default(int?))
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
            EnterpriseCategoryTDES enterprisecategorytdes = null;
            await ViewBagHelper(searchEntGuid);
            if (base.ModelState.IsValid)
            {
                enterprisecategorytdes = await (from e in dbBrand.EnterpriseCategoryTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (int?)e.CategoryId == сategoryId
                                                select e).FirstOrDefaultAsync();
            }
            if (enterprisecategorytdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisecategorytdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(Guid? searchEntGuid = default(Guid?), int? сategoryId = default(int?))
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
            EnterpriseCategoryTDES enterprisecategorytdes = null;
            await ViewBagHelper(searchEntGuid);
            if (base.ModelState.IsValid)
            {
                enterprisecategorytdes = await (from e in dbBrand.EnterpriseCategoryTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (int?)e.CategoryId == сategoryId
                                                select e).FirstOrDefaultAsync();
            }
            if (enterprisecategorytdes == null)
            {
                return HttpNotFound();
            }
            dbBrand.EnterpriseCategoryTDES.Remove(enterprisecategorytdes);
            await dbBrand.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbBrand != null)
            {
                _dbBrand.Dispose();
                _dbBrand = null;
            }
            db.Dispose();
            dbBrand.Dispose();
            base.Dispose(disposing);
        }
    }

}
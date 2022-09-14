// CarShop.Controllers.EnterpriseCategoryItemController
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

    public class EnterpriseCategoryItemController : Controller
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

        protected async Task ViewBagHelper(Guid? searchEntGuid, int? categoryId)
        {
            await ViewBagHelper(searchEntGuid);
            EnterpriseCategoryTDES enterprisecategorytdes = null;
            if (CarShopArticleContextCatalog != null)
            {
                enterprisecategorytdes = await (from e in dbBrand.EnterpriseCategoryTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (int?)e.CategoryId == categoryId
                                                select e).FirstOrDefaultAsync();
            }
            if (enterprisecategorytdes == null)
            {
                base.ViewBag.EntCategoryDescription = Resources.EnterpriseCategoryTDES_NOT_DEFINED;
                base.ViewBag.CategoryId = null;
                base.ModelState.AddModelError("", Resources.EnterpriseCategoryTDES_NOT_DEFINED);
            }
            else
            {
                base.ViewBag.EntCategoryDescription = enterprisecategorytdes.CategoryDescription;
                base.ViewBag.CategoryId = enterprisecategorytdes.CategoryId;
            }
        }

        protected SelectList SelectListHelper(int showIs)
        {
            return new SelectList(new SelectListItem[3]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.CategoryItemSearchById,
                Selected = (showIs == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.CategoryItemSearchByDescr,
                Selected = (showIs == 2)
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.CategoryItemSearchByGroup,
                Selected = (showIs == 3)
            }
            }, "Value", "Text", showIs);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid, int? categoryId, string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page)
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
            await ViewBagHelper(searchEntGuid, categoryId);
            IPagedList<EnterpriseCategoryItemTDES> aResult = null;
            if (CarShopArticleContextCatalog != null && base.ModelState.IsValid)
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
                IOrderedQueryable<EnterpriseCategoryItemTDES> enterprisecategoryitemtdes = from e in dbBrand.EnterpriseCategoryItemTDES.Include("EnterpriseCategoryItemDescription").Include("EnterpriseCategoryDescription")
                                                                                           where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (int?)e.CategoryId == categoryId
                                                                                           orderby e.CategoryItemId, e.CategoryId, e.EntGuid
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
                                    enterprisecategoryitemtdes = from e in dbBrand.EnterpriseCategoryItemTDES.Include("EnterpriseCategoryItemDescription").Include("EnterpriseCategoryDescription")
                                                                 where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (int?)e.CategoryId == categoryId && e.CategoryItemId == aFilter
                                                                 orderby e.CategoryItemId, e.CategoryId, e.EntGuid
                                                                 select e;
                                    break;
                                }
                            case 2:
                                enterprisecategoryitemtdes = from e in dbBrand.EnterpriseCategoryItemTDES.Include("EnterpriseCategoryItemDescription").Include("EnterpriseCategoryDescription")
                                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (int?)e.CategoryId == categoryId && e.EnterpriseCategoryItemDescription.EntCategoryItemDescription.StartsWith(searchString)
                                                             orderby e.CategoryItemId, e.CategoryId, e.EntGuid
                                                             select e;
                                break;
                            case 3:
                                enterprisecategoryitemtdes = from e in dbBrand.EnterpriseCategoryItemTDES.Include("EnterpriseCategoryItemDescription").Include("EnterpriseCategoryDescription")
                                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (int?)e.CategoryId == categoryId && e.EnterpriseCategoryDescription.EntCategoryDescription.StartsWith(searchString)
                                                             orderby e.CategoryItemId, e.CategoryId, e.EntGuid
                                                             select e;
                                break;
                        }
                    }
                }
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await enterprisecategoryitemtdes.ToPagedListAsync(pageNumber, pageSize);
            }
            else
            {
                base.ModelState.AddModelError("", Resources.EnterpriseTDES_NOTDEFINED);
                searchStringBy = (searchStringBy ?? 1);
            }
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.СurrentFilterBy = searchStringBy;
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? searchEntGuid = default(Guid?), int? categoryId = default(int?), int? categoryItemId = default(int?))
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
            await ViewBagHelper(searchEntGuid, categoryId);
            EnterpriseCategoryItemTDES enterprisecategoryitemtdes = null;
            if (base.ModelState.IsValid)
            {
                enterprisecategoryitemtdes = await (from e in dbBrand.EnterpriseCategoryItemTDES.Include("EnterpriseCategoryItemDescription").Include("EnterpriseCategoryDescription")
                                                    where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (int?)e.CategoryId == categoryId && (int?)e.CategoryItemId == categoryItemId
                                                    orderby e.CategoryItemId, e.CategoryId, e.EntGuid
                                                    select e).FirstOrDefaultAsync();
            }
            if (enterprisecategoryitemtdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisecategoryitemtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Create(Guid? searchEntGuid = default(Guid?), int? categoryId = default(int?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                searchEntGuid = ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? new Guid?(await (from e in db.EnterpriseBranchUserTDES
                                                                                                                                           where e.EntUserNic == aName
                                                                                                                                           select e.EntGuid).FirstOrDefaultAsync()) : new Guid?(await (from e in db.EnterpriseUserTDES
                                                                                                                                                                                                       where e.EntUserNic == aName
                                                                                                                                                                                                       select e.EntGuid).FirstOrDefaultAsync()));
            }
            await ViewBagHelper(searchEntGuid, categoryId);
            EnterpriseCategoryItemTmp enterprisecategoryitemtmp = null;
            if (base.ModelState.IsValid)
            {
                enterprisecategoryitemtmp = new EnterpriseCategoryItemTmp
                {
                    EntGuid = (searchEntGuid.HasValue ? searchEntGuid.Value : Guid.Empty),
                    CategoryId = (categoryId.HasValue ? categoryId.Value : 0)
                };
            }
            return View(enterprisecategoryitemtmp);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EnterpriseCategoryItemTmp enterprisecategoryitemtmp)
        {
            UserIsInRoles();
            int? categoryId = enterprisecategoryitemtmp.CategoryId;
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
                searchEntGuid = enterprisecategoryitemtmp.EntGuid;
            }
            await ViewBagHelper(searchEntGuid, categoryId);
            EnterpriseCategoryItemDescriptionTDES enterprisecategoryitemdescriptiontdes = null;
            EnterpriseCategoryDescriptionTDES enterprisecategorydescriptiontdes = null;
            if (base.ModelState.IsValid)
            {
                enterprisecategoryitemdescriptiontdes = await (from e in dbBrand.EnterpriseCategoryItemDescriptionTDES
                                                               where e.EntCategoryItemDescription == enterprisecategoryitemtmp.EntCategoryItemDescription
                                                               select e).FirstOrDefaultAsync();
                if (enterprisecategoryitemdescriptiontdes == null)
                {
                    enterprisecategoryitemdescriptiontdes = new EnterpriseCategoryItemDescriptionTDES
                    {
                        EntCategoryItemDescription = enterprisecategoryitemtmp.EntCategoryItemDescription
                    };
                    dbBrand.EnterpriseCategoryItemDescriptionTDES.Add(enterprisecategoryitemdescriptiontdes);
                    await dbBrand.SaveChangesAsync();
                }
                enterprisecategorydescriptiontdes = await (from e in dbBrand.EnterpriseCategoryDescriptionTDES
                                                           where e.EntCategoryDescription == enterprisecategoryitemtmp.EntCategoryDescription
                                                           select e).FirstOrDefaultAsync();
                if (enterprisecategorydescriptiontdes == null)
                {
                    enterprisecategorydescriptiontdes = new EnterpriseCategoryDescriptionTDES
                    {
                        EntCategoryDescription = enterprisecategoryitemtmp.EntCategoryDescription
                    };
                    dbBrand.EnterpriseCategoryDescriptionTDES.Add(enterprisecategorydescriptiontdes);
                    await dbBrand.SaveChangesAsync();
                }
            }
            if (base.ModelState.IsValid)
            {
                EnterpriseCategoryItemTDES enterprisecategoryitemtdes = new EnterpriseCategoryItemTDES
                {
                    EntGuid = searchEntGuid.Value,
                    CategoryItemId = enterprisecategoryitemtmp.CategoryItemId,
                    CategoryId = enterprisecategoryitemtmp.CategoryId,
                    EntCategoryItemDescriptionId = enterprisecategoryitemdescriptiontdes.EntCategoryItemDescriptionId,
                    EntCategoryDescriptionId = enterprisecategorydescriptiontdes.EntCategoryDescriptionId
                };
                dbBrand.EnterpriseCategoryItemTDES.Add(enterprisecategoryitemtdes);
                await dbBrand.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid,
                    categoryId
                });
            }
            return View(enterprisecategoryitemtmp);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(Guid? searchEntGuid = default(Guid?), int? categoryId = default(int?), int? categoryItemId = default(int?))
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
            await ViewBagHelper(searchEntGuid, categoryId);
            EnterpriseCategoryItemTDES enterprisecategoryitemtdes = null;
            if (base.ModelState.IsValid)
            {
                enterprisecategoryitemtdes = await (from e in dbBrand.EnterpriseCategoryItemTDES.Include("EnterpriseCategoryItemDescription").Include("EnterpriseCategoryDescription")
                                                    where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (int?)e.CategoryId == categoryId && (int?)e.CategoryItemId == categoryItemId
                                                    orderby e.CategoryItemId, e.CategoryId, e.EntGuid
                                                    select e).FirstOrDefaultAsync();
            }
            if (enterprisecategoryitemtdes == null)
            {
                return HttpNotFound();
            }
            EnterpriseCategoryItemTmp enterprisecategoryitemtmp = new EnterpriseCategoryItemTmp
            {
                EntGuid = enterprisecategoryitemtdes.EntGuid,
                CategoryId = enterprisecategoryitemtdes.CategoryId,
                CategoryItemId = enterprisecategoryitemtdes.CategoryItemId,
                EntCategoryItemDescription = enterprisecategoryitemtdes.EnterpriseCategoryItemDescription.EntCategoryItemDescription,
                EntCategoryDescription = enterprisecategoryitemtdes.EnterpriseCategoryDescription.EntCategoryDescription
            };
            return View(enterprisecategoryitemtmp);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(EnterpriseCategoryItemTmp enterprisecategoryitemtmp)
        {
            UserIsInRoles();
            int? categoryId = enterprisecategoryitemtmp.CategoryId;
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
                searchEntGuid = enterprisecategoryitemtmp.EntGuid;
            }
            await ViewBagHelper(searchEntGuid, categoryId);
            EnterpriseCategoryItemDescriptionTDES enterprisecategoryitemdescriptiontdes = null;
            EnterpriseCategoryDescriptionTDES enterprisecategorydescriptiontdes = null;
            if (base.ModelState.IsValid)
            {
                enterprisecategoryitemdescriptiontdes = await (from e in dbBrand.EnterpriseCategoryItemDescriptionTDES
                                                               where e.EntCategoryItemDescription == enterprisecategoryitemtmp.EntCategoryItemDescription
                                                               select e).FirstOrDefaultAsync();
                if (enterprisecategoryitemdescriptiontdes == null)
                {
                    enterprisecategoryitemdescriptiontdes = new EnterpriseCategoryItemDescriptionTDES
                    {
                        EntCategoryItemDescription = enterprisecategoryitemtmp.EntCategoryItemDescription
                    };
                    dbBrand.EnterpriseCategoryItemDescriptionTDES.Add(enterprisecategoryitemdescriptiontdes);
                    await dbBrand.SaveChangesAsync();
                }
                enterprisecategorydescriptiontdes = await (from e in dbBrand.EnterpriseCategoryDescriptionTDES
                                                           where e.EntCategoryDescription == enterprisecategoryitemtmp.EntCategoryDescription
                                                           select e).FirstOrDefaultAsync();
                if (enterprisecategorydescriptiontdes == null)
                {
                    enterprisecategorydescriptiontdes = new EnterpriseCategoryDescriptionTDES
                    {
                        EntCategoryDescription = enterprisecategoryitemtmp.EntCategoryDescription
                    };
                    dbBrand.EnterpriseCategoryDescriptionTDES.Add(enterprisecategorydescriptiontdes);
                    await dbBrand.SaveChangesAsync();
                }
            }
            if (base.ModelState.IsValid)
            {
                EnterpriseCategoryItemTDES enterprisecategoryitemtdes = new EnterpriseCategoryItemTDES
                {
                    EntGuid = searchEntGuid.Value,
                    CategoryItemId = enterprisecategoryitemtmp.CategoryItemId,
                    CategoryId = enterprisecategoryitemtmp.CategoryId,
                    EntCategoryItemDescriptionId = enterprisecategoryitemdescriptiontdes.EntCategoryItemDescriptionId,
                    EntCategoryDescriptionId = enterprisecategorydescriptiontdes.EntCategoryDescriptionId
                };
                dbBrand.Entry(enterprisecategoryitemtdes).State = EntityState.Modified;
                await dbBrand.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid,
                    categoryId
                });
            }
            return View(enterprisecategoryitemtmp);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Delete(Guid? searchEntGuid = default(Guid?), int? categoryId = default(int?), int? categoryItemId = default(int?))
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
            await ViewBagHelper(searchEntGuid, categoryId);
            EnterpriseCategoryItemTDES enterprisecategoryitemtdes = null;
            if (base.ModelState.IsValid)
            {
                enterprisecategoryitemtdes = await (from e in dbBrand.EnterpriseCategoryItemTDES.Include("EnterpriseCategoryItemDescription").Include("EnterpriseCategoryDescription")
                                                    where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (int?)e.CategoryId == categoryId && (int?)e.CategoryItemId == categoryItemId
                                                    orderby e.CategoryItemId, e.CategoryId, e.EntGuid
                                                    select e).FirstOrDefaultAsync();
            }
            if (enterprisecategoryitemtdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisecategoryitemtdes);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> DeleteConfirmed(Guid? searchEntGuid = default(Guid?), int? categoryId = default(int?), int? categoryItemId = default(int?))
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
            await ViewBagHelper(searchEntGuid, categoryId);
            EnterpriseCategoryItemTDES enterprisecategoryitemtdes = null;
            if (base.ModelState.IsValid)
            {
                enterprisecategoryitemtdes = await (from e in dbBrand.EnterpriseCategoryItemTDES.Include("EnterpriseCategoryItemDescription").Include("EnterpriseCategoryDescription")
                                                    where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (int?)e.CategoryId == categoryId && (int?)e.CategoryItemId == categoryItemId
                                                    orderby e.CategoryItemId, e.CategoryId, e.EntGuid
                                                    select e).FirstOrDefaultAsync();
            }
            if (enterprisecategoryitemtdes == null)
            {
                return HttpNotFound();
            }
            dbBrand.EnterpriseCategoryItemTDES.Remove(enterprisecategoryitemtdes);
            await dbBrand.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid,
                categoryId
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
            base.Dispose(disposing);
        }
    }

}
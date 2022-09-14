// CarShop.Controllers.EnterpriseBrandController
using CarShop.Models;
using CarShop.Properties;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterpriseBrandController : Controller
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

        protected SelectList SelectListHelper(int showIs)
        {
            return new SelectList(new SelectListItem[2]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.BrandSearchByNic,
                Selected = (showIs == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.BrandSearchByDesc,
                Selected = (showIs == 2)
            }
            }, "Value", "Text", showIs);
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

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Autocomplete(Guid? searchEntGuid, int? currentFilterBy, string term)
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
                    Guid? reference2 = searchEntGuid;
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value2;
                }
            }
            currentFilterBy = (currentFilterBy ?? 1);
            await ViewBagHelper(searchEntGuid);
            if (CarShopArticleContextCatalog == null)
            {
                List<string> source = new List<string>();
                var data = from e in source
                           select new
                           {
                               value = e
                           };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrEmpty(term))
            {
                term = term.Trim();
            }
            IQueryable<EnterpriseBrandTDES> enterprisebrandtdes = from e in dbBrand.EnterpriseBrandTDES
                                                                  where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                  select e;
            if (!string.IsNullOrEmpty(term))
            {
                enterprisebrandtdes = ((currentFilterBy != 1) ? (from e in enterprisebrandtdes
                                                                 where e.EntBrandDescription.StartsWith(term)
                                                                 select e) : enterprisebrandtdes.Where((EnterpriseBrandTDES e) => e.EntBrandNic.StartsWith(term)));
            }
            if (currentFilterBy == 1)
            {
                var ret2 = (from e in enterprisebrandtdes
                            select new
                            {
                                value = e.EntBrandNic
                            }).Take(10);
                return Json(await ret2.ToListAsync(), JsonRequestBehavior.AllowGet);
            }
            var ret = (from e in enterprisebrandtdes
                       select new
                       {
                           value = e.EntBrandDescription
                       }).Take(10);
            return Json(await ret.ToListAsync(), JsonRequestBehavior.AllowGet);
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
                    Guid? reference = searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
                else
                {
                    Guid? reference2 = searchEntGuid;
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value2;
                }
            }
            IPagedList<EnterpriseBrandTDES> aResult = null;
            await ViewBagHelper(searchEntGuid);
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
                IOrderedQueryable<EnterpriseBrandTDES> enterprisebrandtdes = from e in dbBrand.EnterpriseBrandTDES
                                                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                             orderby e.EntGuid, e.EntBrandNic
                                                                             select e;
                if (!string.IsNullOrEmpty(searchString))
                {
                    enterprisebrandtdes = ((searchStringBy != 1) ? (from e in dbBrand.EnterpriseBrandTDES
                                                                    where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandDescription.StartsWith(searchString)
                                                                    orderby e.EntGuid, e.EntBrandDescription
                                                                    select e) : dbBrand.EnterpriseBrandTDES.Where((EnterpriseBrandTDES e) => (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic.StartsWith(searchString)).OrderBy((EnterpriseBrandTDES e) => e.EntGuid).ThenBy((EnterpriseBrandTDES e) => e.EntBrandNic));
                }
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await enterprisebrandtdes.ToPagedListAsync(pageNumber, pageSize);
            }
            else
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                searchStringBy = (searchStringBy ?? 1);
            }
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.currentFilterBy = searchStringBy;
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(string idEntBrandNic = null, Guid? searchEntGuid = default(Guid?))
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
                    Guid? reference2 = searchEntGuid;
                    Guid value2 = await (from e in db.EnterpriseBranchUserTDES
                                         where e.EntUserNic == aName
                                         select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value2;
                }
            }
            EnterpriseBrandTDES enterprisebrandtdes = null;
            await ViewBagHelper(searchEntGuid);
            if (CarShopArticleContextCatalog != null)
            {
                enterprisebrandtdes = await (from e in dbBrand.EnterpriseBrandTDES
                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == idEntBrandNic
                                             select e).FirstOrDefaultAsync();
            }
            if (enterprisebrandtdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisebrandtdes);
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
            EnterpriseBrandTDES enterprisebrandtdes = null;
            await ViewBagHelper(searchEntGuid);
            if (CarShopArticleContextCatalog != null)
            {
                enterprisebrandtdes = new EnterpriseBrandTDES
                {
                    EntGuid = searchEntGuid.Value,
                    EntBrandNic = "Nic" + Guid.NewGuid().ToString()
                };
            }
            return View(enterprisebrandtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(EnterpriseBrandTDES enterprisebrandtdes)
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
                searchEntGuid = enterprisebrandtdes.EntGuid;
            }
            if (base.ModelState.IsValid && enterprisebrandtdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            await ViewBagHelper(searchEntGuid);
            if (base.ModelState.IsValid && CarShopArticleContextCatalog != null)
            {
                dbBrand.EnterpriseBrandTDES.Add(enterprisebrandtdes);
                await dbBrand.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = enterprisebrandtdes.EntGuid
                });
            }
            return View(enterprisebrandtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(string idEntBrandNic = null, Guid? searchEntGuid = default(Guid?))
        {
            UserIsInRoles();
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
                }
            }
            EnterpriseBrandTDES enterprisebrandtdes = null;
            await ViewBagHelper(searchEntGuid);
            if (CarShopArticleContextCatalog != null)
            {
                enterprisebrandtdes = await (from e in dbBrand.EnterpriseBrandTDES
                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == idEntBrandNic
                                             select e).FirstOrDefaultAsync();
            }
            if (enterprisebrandtdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisebrandtdes);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Edit(EnterpriseBrandTDES enterprisebrandtdes)
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
                searchEntGuid = enterprisebrandtdes.EntGuid;
            }
            if (base.ModelState.IsValid && enterprisebrandtdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            await ViewBagHelper(searchEntGuid);
            if (base.ModelState.IsValid)
            {
                dbBrand.Entry(enterprisebrandtdes).State = EntityState.Modified;
                await dbBrand.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = enterprisebrandtdes.EntGuid
                });
            }
            return View(enterprisebrandtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public async Task<ActionResult> Delete(string idEntBrandNic = null, Guid? searchEntGuid = default(Guid?))
        {
            UserIsInRoles();
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
                }
            }
            EnterpriseBrandTDES enterprisebrandtdes = null;
            await ViewBagHelper(searchEntGuid);
            if (CarShopArticleContextCatalog != null)
            {
                enterprisebrandtdes = await (from e in dbBrand.EnterpriseBrandTDES
                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == idEntBrandNic
                                             select e).FirstOrDefaultAsync();
            }
            if (enterprisebrandtdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisebrandtdes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string idEntBrandNic, Guid? searchEntGuid = default(Guid?))
        {
            UserIsInRoles();
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
                }
            }
            await ViewBagHelper(searchEntGuid);
            if (CarShopArticleContextCatalog != null)
            {
                EnterpriseBrandTDES enterprisebrandtdes = await (from e in dbBrand.EnterpriseBrandTDES
                                                                 where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBrandNic == idEntBrandNic
                                                                 select e).FirstOrDefaultAsync();
                dbBrand.EnterpriseBrandTDES.Remove(enterprisebrandtdes);
                await dbBrand.SaveChangesAsync();
            }
            return RedirectToAction("Index", new
            {
                searchEntGuid
            });
        }

        public async Task<ActionResult> LookUpEnterpriseBrand(string redirecData, string redirectContriller, string redirectAction, Guid? searchEntGuid, string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page)
        {
            base.ViewBag.RedirecData = redirecData;
            base.ViewBag.RedirectContriller = redirectContriller;
            base.ViewBag.RedirectAction = redirectAction;
            return await Index(searchEntGuid, searchString, searchStringBy, currentFilter, currentFilterBy, page);
        }

        [HttpPost]
        public ActionResult LookUpEnterpriseBrandSelected(string redirecData, string redirectContriller, string redirectAction, string SimpleArticle, string DOSELECT_TITLE, string DOCANCEL_TITLE)
        {
            if (DOSELECT_TITLE != null)
            {
                string entBrandNic = null;
                string entBrandDescription = null;
                if (SimpleArticle != null)
                {
                    EnterpriseBrandTDES enterpriseBrandTDES = JsonConvert.DeserializeObject<EnterpriseBrandTDES>(SimpleArticle);
                    entBrandNic = enterpriseBrandTDES.EntBrandNic;
                    entBrandDescription = enterpriseBrandTDES.EntBrandDescription;
                }
                return RedirectToAction(redirectAction, redirectContriller, new
                {
                    redirecData = redirecData,
                    EntBrandNic = entBrandNic,
                    EntBrandDescription = entBrandDescription
                });
            }
            return RedirectToAction(redirectAction, redirectContriller, new
            {
                redirecData
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
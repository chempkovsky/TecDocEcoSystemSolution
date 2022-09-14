// CarShop.Controllers.BranchSuppController
using CarShop.Models;
using CarShop.Properties;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class BranchSuppController : Controller
    {
        private Guid? guestEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private CarShopContext db = new CarShopContext();

        private string CarShopRestContextCatalog;

        private CarShopRestContext _dbRest;

        private CarShopRestContext dbRest
        {
            get
            {
                if (_dbRest == null)
                {
                    _dbRest = new CarShopRestContext(CarShopRestContextCatalog);
                }
                return _dbRest;
            }
        }

        public async Task<Guid?> ViewBagHelper(Guid? searchEntBranchGuid)
        {
            EnterpriseBranchTDES enterprisebranchtdes2 = await (from e in db.EnterpriseBranchTDES
                                                                where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefaultAsync();
            if (enterprisebranchtdes2 != null)
            {
                base.ViewBag.EntDescription = enterprisebranchtdes2.EnterpriseTDES.EntDescription;
                base.ViewBag.EntBranchDescription = enterprisebranchtdes2.EntBranchDescription;
                base.ViewBag.SearchEntGuid = enterprisebranchtdes2.EntGuid;
                base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes2.EntBranchGuid;
                Guid aResult = enterprisebranchtdes2.EntGuid;
                enterprisebranchtdes2 = await (from e in db.EnterpriseBranchTDES
                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)guestEntBranchGuid
                                               select e).FirstOrDefaultAsync();
                if (enterprisebranchtdes2 != null)
                {
                    CarShopRestContextCatalog = enterprisebranchtdes2.TecDocCatalog;
                }
                return aResult;
            }
            base.ViewBag.EntBranchDescription = Resources.EnterpriseBranchTDES_NOTFOUND;
            base.ViewBag.EntDescription = Resources.ENTERPRISE_NOT_DEFINED;
            base.ViewBag.SearchEntBranchGuid = null;
            base.ViewBag.SearchEntGuid = null;
            base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
            base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            return null;
        }

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
        }

        protected SelectList CurrencyListHelper(int PriceCurrencyIso)
        {
            return new SelectList(new SelectListItem[5]
            {
            new SelectListItem
            {
                Value = "974",
                Text = "белорусский рубль",
                Selected = (PriceCurrencyIso == 974)
            },
            new SelectListItem
            {
                Value = "978",
                Text = "евро",
                Selected = (PriceCurrencyIso == 978)
            },
            new SelectListItem
            {
                Value = "840",
                Text = "доллар США",
                Selected = (PriceCurrencyIso == 840)
            },
            new SelectListItem
            {
                Value = "643",
                Text = "российский рубль",
                Selected = (PriceCurrencyIso == 643)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.BrandSearchByDesc,
                Selected = (PriceCurrencyIso == 2)
            }
            }, "Value", "Text", PriceCurrencyIso);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntBranchGuid, int? page = default(int?))
        {
            IPagedList<BranchSuppTDES> aResult = null;
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefaultAsync());
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
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntBranchGuid);
            if (CarShopRestContextCatalog == null)
            {
                return View(aResult);
            }
            page = (page ?? 1);
            IOrderedQueryable<BranchSuppTDES> branchsupptdes = from e in dbRest.BranchSuppTDES
                                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               orderby e.EntSupplierId
                                                               select e;
            int pageSize = 20;
            int pageNumber = page.Value;
            return View(await branchsupptdes.ToPagedListAsync(pageNumber, pageSize));
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? searchEntBranchGuid = default(Guid?), string searchEntSupplierId = null)
        {
            if (!searchEntBranchGuid.HasValue || string.IsNullOrEmpty(searchEntSupplierId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefaultAsync());
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
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntBranchGuid);
            if (CarShopRestContextCatalog == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchSuppTDES branchsupptdes = await (from e in dbRest.BranchSuppTDES
                                                   where e.EntSupplierId == searchEntSupplierId && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (branchsupptdes == null)
            {
                return HttpNotFound();
            }
            return View(branchsupptdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public async Task<ActionResult> Create(Guid? searchEntBranchGuid)
        {
            Guid? searchEntGuid = null;
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
            searchEntGuid = await ViewBagHelper(searchEntBranchGuid);
            base.ViewBag.CurrencyList = CurrencyListHelper(974);
            base.ViewBag.SupplierList = new SelectList(await (from e in db.EnterpriseSupplierTDES
                                                              where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                              select e).ToListAsync(), "EntSupplierId", "EntSupplierDescription");
            BranchSuppTDES branchsupptdes = new BranchSuppTDES();
            if (searchEntBranchGuid.HasValue)
            {
                branchsupptdes.EntBranchGuid = searchEntBranchGuid.Value;
            }
            return View(branchsupptdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "EntBranchGuid,EntSupplierId,PriceCurrencyIso,ExchRate,Rounding,Multiplexer,SuppTime")] BranchSuppTDES branchsupptdes)
        {
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if (branchsupptdes != null)
            {
                searchEntBranchGuid = branchsupptdes.EntBranchGuid;
            }
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
            searchEntGuid = await ViewBagHelper(searchEntBranchGuid);
            if (CarShopRestContextCatalog == null)
            {
                return View(branchsupptdes);
            }
            if (base.ModelState.IsValid)
            {
                dbRest.BranchSuppTDES.Add(branchsupptdes);
                await dbRest.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntBranchGuid
                });
            }
            base.ViewBag.EntBranchGuid = new SelectList(await dbRest.GuestBranchTDES.ToListAsync(), "EntBranchGuid", "EntBranchDescription", branchsupptdes.EntBranchGuid);
            base.ViewBag.CurrencyList = CurrencyListHelper(branchsupptdes.PriceCurrencyIso);
            base.ViewBag.SupplierList = new SelectList(await (from e in db.EnterpriseSupplierTDES
                                                              where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                              select e).ToListAsync(), "EntSupplierId", "EntSupplierDescription", branchsupptdes.EntSupplierId);
            if (searchEntBranchGuid.HasValue)
            {
                branchsupptdes.EntBranchGuid = searchEntBranchGuid.Value;
            }
            return View(branchsupptdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public async Task<ActionResult> Edit(Guid? searchEntBranchGuid = default(Guid?), string searchEntSupplierId = null)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefaultAsync());
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
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntBranchGuid);
            if (CarShopRestContextCatalog == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchSuppTDES branchsupptdes = await (from e in dbRest.BranchSuppTDES
                                                   where e.EntSupplierId == searchEntSupplierId && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (branchsupptdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.CurrencyList = CurrencyListHelper(branchsupptdes.PriceCurrencyIso);
            return View(branchsupptdes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public async Task<ActionResult> Edit([Bind(Include = "EntBranchGuid,EntSupplierId,PriceCurrencyIso,ExchRate,Rounding,Multiplexer,SuppTime")] BranchSuppTDES branchsupptdes)
        {
            Guid? searchEntBranchGuid = null;
            if (branchsupptdes != null)
            {
                searchEntBranchGuid = branchsupptdes.EntBranchGuid;
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefaultAsync());
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
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntBranchGuid);
            if (CarShopRestContextCatalog == null)
            {
                return View(branchsupptdes);
            }
            if (base.ModelState.IsValid)
            {
                dbRest.Entry(branchsupptdes).State = EntityState.Modified;
                await dbRest.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntBranchGuid
                });
            }
            base.ViewBag.CurrencyList = CurrencyListHelper(branchsupptdes.PriceCurrencyIso);
            if (searchEntBranchGuid.HasValue)
            {
                branchsupptdes.EntBranchGuid = searchEntBranchGuid.Value;
            }
            return View(branchsupptdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public async Task<ActionResult> Delete(Guid? searchEntBranchGuid = default(Guid?), string searchEntSupplierId = null)
        {
            if (!searchEntBranchGuid.HasValue || string.IsNullOrEmpty(searchEntSupplierId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefaultAsync());
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
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntBranchGuid);
            if (CarShopRestContextCatalog == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchSuppTDES branchsupptdes = await (from e in dbRest.BranchSuppTDES
                                                   where e.EntSupplierId == searchEntSupplierId && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (branchsupptdes == null)
            {
                return HttpNotFound();
            }
            return View(branchsupptdes);
        }

        [ActionName("Delete")]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid? searchEntBranchGuid = default(Guid?), string searchEntSupplierId = null)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefaultAsync());
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
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntBranchGuid);
            if (CarShopRestContextCatalog == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchSuppTDES branchsupptdes = await (from e in dbRest.BranchSuppTDES
                                                   where e.EntSupplierId == searchEntSupplierId && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            dbRest.BranchSuppTDES.Remove(branchsupptdes);
            await dbRest.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntBranchGuid
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            if (_dbRest != null)
            {
                _dbRest.Dispose();
                _dbRest = null;
            }
            base.Dispose(disposing);
        }
    }

}
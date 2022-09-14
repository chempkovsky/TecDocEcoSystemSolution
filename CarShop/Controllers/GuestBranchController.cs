// CarShop.Controllers.GuestBranchController
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

    public class GuestBranchController : Controller
    {
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

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
        }

        public async Task ViewBagHelper(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            EnterpriseBranchTDES enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                               select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefaultAsync();
            if (enterprisebranchtdes != null)
            {
                base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
                base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
                base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
                base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
                CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
                return;
            }
            base.ViewBag.EntBranchDescription = Resources.EnterpriseBranchTDES_NOTFOUND;
            base.ViewBag.SearchEntBranchGuid = null;
            base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
            }
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
        public async Task<ActionResult> Index(int? page = default(int?))
        {
            IPagedList<GuestBranchTDES> aResult = null;
            Guid? searchEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            Guid? userEntBranchGuid = null;
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if ((!(base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)))
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds != null)
                    {
                        userEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            if (CarShopRestContextCatalog != null)
            {
                page = (page ?? 1);
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = ((!userEntBranchGuid.HasValue) ? (await (from e in dbRest.GuestBranchTDES
                                                                   orderby e.EntBranchGuid
                                                                   select e).ToPagedListAsync(pageNumber, pageSize)) : (await (from e in dbRest.GuestBranchTDES
                                                                                                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)userEntBranchGuid
                                                                                                                               orderby e.EntBranchGuid
                                                                                                                               select e).ToPagedListAsync(pageNumber, pageSize)));
            }
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? id = default(Guid?))
        {
            Guid? searchEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            UserIsInRoles();
            string aName = base.User.Identity.Name;
            if (((!base.ViewBag.IsEcoSystemAdmin)) && ((!(base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)) ? true : false))
            {
                EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                    where e.EntUserNic == aName
                                                                    select new EnterpriseBranchIdsTDES
                                                                    {
                                                                        EntGuid = e.EntGuid,
                                                                        EntBranchGuid = e.EntBranchGuid
                                                                    }).FirstOrDefaultAsync();
                if (searchEntBranchIds != null)
                {
                    id = searchEntBranchIds.EntBranchGuid;
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopRestContextCatalog != null)
            {
                GuestBranchTDES guestbranchtdes = await dbRest.GuestBranchTDES.FindAsync(id);
                if (guestbranchtdes == null)
                {
                    return HttpNotFound();
                }
                return View(guestbranchtdes);
            }
            return View();
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Create()
        {
            UserIsInRoles();
            GuestBranchTDES guestbranchtdes = new GuestBranchTDES();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false))
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds != null)
                    {
                        guestbranchtdes.EntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            base.ViewBag.CurrencyList = CurrencyListHelper(974);
            guestbranchtdes.PriceCurrencyIso = 974;
            return View(guestbranchtdes);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [HttpPost]
        public async Task<ActionResult> Create(GuestBranchTDES guestbranchtdes)
        {
            Guid? searchEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            if (base.ModelState.IsValid)
            {
                await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            }
            if (base.ModelState.IsValid)
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
                        searchEntBranchGuid = guestbranchtdes.EntBranchGuid;
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
                    EnterpriseBranchTDES x = await (from e in db.EnterpriseBranchTDES
                                                    where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBranchGuid == guestbranchtdes.EntBranchGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                    select e).FirstOrDefaultAsync();
                    if (x == null)
                    {
                        base.ModelState.AddModelError("", Resources.GuestBranchTDES_NOTFOUND);
                    }
                    else
                    {
                        guestbranchtdes.EntBranchGuid = x.EntBranchGuid;
                    }
                }
            }
            if (base.ModelState.IsValid)
            {
                dbRest.GuestBranchTDES.Add(guestbranchtdes);
                await dbRest.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            base.ViewBag.CurrencyList = CurrencyListHelper(guestbranchtdes.PriceCurrencyIso);
            return View(guestbranchtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Edit(Guid? id = default(Guid?))
        {
            Guid? searchEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            UserIsInRoles();
            string aName = base.User.Identity.Name;
            if (((!base.ViewBag.IsEcoSystemAdmin)) && ((!(base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)) ? true : false))
            {
                EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                    where e.EntUserNic == aName
                                                                    select new EnterpriseBranchIdsTDES
                                                                    {
                                                                        EntGuid = e.EntGuid,
                                                                        EntBranchGuid = e.EntBranchGuid
                                                                    }).FirstOrDefaultAsync();
                if (searchEntBranchIds != null)
                {
                    id = searchEntBranchIds.EntBranchGuid;
                }
            }
            if (CarShopRestContextCatalog != null)
            {
                GuestBranchTDES guestbranchtdes = await dbRest.GuestBranchTDES.FindAsync(id);
                if (guestbranchtdes == null)
                {
                    return HttpNotFound();
                }
                base.ViewBag.CurrencyList = CurrencyListHelper(guestbranchtdes.PriceCurrencyIso);
                return View(guestbranchtdes);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Edit(GuestBranchTDES guestbranchtdes)
        {
            Guid? searchEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            if (base.ModelState.IsValid)
            {
                await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            }
            if (base.ModelState.IsValid)
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
                        searchEntBranchGuid = guestbranchtdes.EntBranchGuid;
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
                    EnterpriseBranchTDES x = await (from e in db.EnterpriseBranchTDES
                                                    where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBranchGuid == guestbranchtdes.EntBranchGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                    select e).FirstOrDefaultAsync();
                    if (x == null)
                    {
                        base.ModelState.AddModelError("", Resources.GuestBranchTDES_NOTFOUND);
                    }
                    else
                    {
                        guestbranchtdes.EntBranchGuid = x.EntBranchGuid;
                    }
                }
            }
            if (base.ModelState.IsValid)
            {
                dbRest.Entry(guestbranchtdes).State = EntityState.Modified;
                await dbRest.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            base.ViewBag.CurrencyList = CurrencyListHelper(guestbranchtdes.PriceCurrencyIso);
            return View(guestbranchtdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Delete(Guid? id = default(Guid?))
        {
            Guid? searchEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            UserIsInRoles();
            string aName = base.User.Identity.Name;
            if (((!base.ViewBag.IsEcoSystemAdmin)) && ((!(base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)) ? true : false))
            {
                EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                    where e.EntUserNic == aName
                                                                    select new EnterpriseBranchIdsTDES
                                                                    {
                                                                        EntGuid = e.EntGuid,
                                                                        EntBranchGuid = e.EntBranchGuid
                                                                    }).FirstOrDefaultAsync();
                if (searchEntBranchIds != null)
                {
                    id = searchEntBranchIds.EntBranchGuid;
                }
            }
            if (CarShopRestContextCatalog != null)
            {
                GuestBranchTDES guestbranchtdes = await dbRest.GuestBranchTDES.FindAsync(id);
                if (guestbranchtdes == null)
                {
                    return HttpNotFound();
                }
                return View(guestbranchtdes);
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(Guid EntBranchGuid)
        {
            Guid? searchEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            if (base.ModelState.IsValid)
            {
                await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            }
            if (base.ModelState.IsValid)
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
                        searchEntBranchGuid = EntBranchGuid;
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
                    EnterpriseBranchTDES x = await (from e in db.EnterpriseBranchTDES
                                                    where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntBranchGuid == EntBranchGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                    select e).FirstOrDefaultAsync();
                    if (x == null)
                    {
                        base.ModelState.AddModelError("", Resources.GuestBranchTDES_NOTFOUND);
                    }
                    else
                    {
                        EntBranchGuid = x.EntBranchGuid;
                    }
                }
            }
            GuestBranchTDES guestbranchtdes2 = null;
            if (base.ModelState.IsValid)
            {
                guestbranchtdes2 = await dbRest.GuestBranchTDES.FindAsync(EntBranchGuid);
                dbRest.GuestBranchTDES.Remove(guestbranchtdes2);
                await dbRest.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(guestbranchtdes2);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            if (_dbRest != null)
            {
                _dbRest.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
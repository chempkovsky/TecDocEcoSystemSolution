// CarShop.Controllers.BranchGuestProfileController
using CarShop.Models;
using CarShop.Properties;
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

    public class BranchGuestProfileController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopOrderContextCatalog;

        private CarShopOrdersContext _dbOrders;

        private CarShopOrdersContext dbOrders
        {
            get
            {
                if (_dbOrders == null)
                {
                    _dbOrders = new CarShopOrdersContext(CarShopOrderContextCatalog);
                }
                return _dbOrders;
            }
        }

        public async Task ViewBagHelper(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            EnterpriseBranchTDES enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefaultAsync();
            if (enterprisebranchtdes != null)
            {
                base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
                base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
                base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
                base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
                CarShopOrderContextCatalog = enterprisebranchtdes.OrderCatalog;
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
            base.ViewBag.IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        protected SelectList SelectListHelper(int showIs)
        {
            return new SelectList(new SelectListItem[2]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.ProfileSearchByNic,
                Selected = (showIs == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.ProfileSearchByLastName,
                Selected = (showIs == 2)
            }
            }, "Value", "Text", showIs);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Autocomplete(Guid? searchEntGuid, Guid? searchEntBranchGuid, int? currentFilterBy, string term)
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
            currentFilterBy = (currentFilterBy ?? 1);
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopOrderContextCatalog == null)
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
            IQueryable<GuestProfileTDES> branchresttdes = from e in dbOrders.GuestProfileTDES
                                                          select (e);
            if (!string.IsNullOrEmpty(term))
            {
                branchresttdes = ((currentFilterBy != 1) ? (from e in branchresttdes
                                                            where e.LastName.StartsWith(term)
                                                            select e) : branchresttdes.Where((GuestProfileTDES e) => e.GestUserNic.StartsWith(term)));
            }
            if (currentFilterBy == 1)
            {
                return Json(await (from e in branchresttdes
                                   select new
                                   {
                                       value = e.GestUserNic
                                   }).Take(10).ToListAsync(), JsonRequestBehavior.AllowGet);
            }
            return Json(await (from e in branchresttdes
                               select new
                               {
                                   value = e.LastName
                               }).Take(10).ToListAsync(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid, Guid? searchEntBranchGuid, string searchString, int? searchStringBy, string currentFilter, int? currentFilterBy, int? page)
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
            if (!base.ModelState.IsValid)
            {
                return View();
            }
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
            IOrderedQueryable<GuestProfileTDES> guestprofiletdes = from e in dbOrders.GuestProfileTDES
                                                                   orderby e.GestUserNic
                                                                   select e;
            if (!string.IsNullOrEmpty(searchString))
            {
                guestprofiletdes = ((searchStringBy != 1) ? (from e in dbOrders.GuestProfileTDES
                                                             where e.LastName.StartsWith(searchString)
                                                             orderby e.LastName
                                                             select e) : dbOrders.GuestProfileTDES.Where((GuestProfileTDES e) => e.GestUserNic.StartsWith(searchString)).OrderBy((GuestProfileTDES e) => e.GestUserNic));
            }
            int pageSize = 20;
            int pageNumber = page.Value;
            IPagedList<GuestProfileTDES> aResult = await guestprofiletdes.ToPagedListAsync(pageNumber, pageSize);
            base.ViewBag.sliSearchStringBy = SelectListHelper(searchStringBy.Value);
            base.ViewBag.CurrentFilter = searchString;
            base.ViewBag.currentFilterBy = searchStringBy;
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchGestUserNic = null)
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
            await ViewBagHelper(null, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            GuestProfileTDES guestprofiletdes = await (from e in dbOrders.GuestProfileTDES
                                                       where e.GestUserNic == searchGestUserNic
                                                       select e).FirstOrDefaultAsync();
            if (guestprofiletdes == null)
            {
                return HttpNotFound();
            }
            return View(guestprofiletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Create(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
            await ViewBagHelper(null, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            GuestProfileTDES guestprofiletdes = new GuestProfileTDES();
            return View(guestprofiletdes);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        [HttpPost]
        public async Task<ActionResult> Create(Guid? searchEntGuid, Guid? searchEntBranchGuid, GuestProfileTDES guestprofiletdes)
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
            await ViewBagHelper(null, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View(guestprofiletdes);
            }
            if (await (from e in dbOrders.GuestProfileTDES
                       where e.GestUserNic == guestprofiletdes.GestUserNic
                       select e).CountAsync() > 0)
            {
                base.ModelState.AddModelError("", Resources.GestUserNic_EXISTS);
                return View(guestprofiletdes);
            }
            dbOrders.GuestProfileTDES.Add(guestprofiletdes);
            await dbOrders.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid = searchEntGuid,
                searchEntBranchGuid = searchEntBranchGuid,
                searchString = guestprofiletdes.GestUserNic,
                searchStringBy = 1
            });
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Edit(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchGestUserNic = null)
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
            await ViewBagHelper(null, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            GuestProfileTDES guestprofiletdes = await (from e in dbOrders.GuestProfileTDES
                                                       where e.GestUserNic == searchGestUserNic
                                                       select e).FirstOrDefaultAsync();
            if (guestprofiletdes == null)
            {
                return HttpNotFound();
            }
            return View(guestprofiletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(Guid? searchEntGuid, Guid? searchEntBranchGuid, GuestProfileTDES guestprofiletdes)
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
            await ViewBagHelper(null, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View(guestprofiletdes);
            }
            dbOrders.Entry(guestprofiletdes).State = EntityState.Modified;
            await dbOrders.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid = searchEntGuid,
                searchEntBranchGuid = searchEntBranchGuid,
                searchString = guestprofiletdes.GestUserNic,
                searchStringBy = 1
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbOrders != null)
            {
                _dbOrders.Dispose();
                _dbOrders = null;
            }
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
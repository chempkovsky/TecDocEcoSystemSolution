// CarShop.Controllers.User2WorkPlaceController
using CarShop.Models;
using CarShop.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{
    public class User2WorkPlaceController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopSalesContextCatalog;

        private CarShopSalesContext _dbSales;

        private CarShopSalesContext dbSales
        {
            get
            {
                if (_dbSales == null)
                {
                    _dbSales = new CarShopSalesContext(CarShopSalesContextCatalog);
                }
                return _dbSales;
            }
        }

        public async Task ViewBagHelper(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            EnterpriseBranchTDES enterprisebranchtdes = await db.EnterpriseBranchTDES.Where((EnterpriseBranchTDES e) => (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefaultAsync();
            if (enterprisebranchtdes != null)
            {
                base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
                base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
                base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
                base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
                CarShopSalesContextCatalog = enterprisebranchtdes.SalesCatalog;
                return;
            }
            base.ViewBag.EntBranchDescription = Resources.EnterpriseBranchTDES_NOTFOUND;
            base.ViewBag.SearchEntBranchGuid = null;
            base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            EnterpriseTDES enterprises = await db.EnterpriseTDES.Where((EnterpriseTDES e) => (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid).FirstOrDefaultAsync();
            if (enterprises != null)
            {
                base.ViewBag.EntDescription = enterprises.EntDescription;
                base.ViewBag.SearchEntGuid = enterprises.EntGuid;
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
        public async Task<ActionResult> Index(Guid? searchEntGuid, Guid? searchEntBranchGuid)
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
            List<User2WorkPlaceTDES> user2workplacetdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                user2workplacetdes = await dbSales.User2WorkPlaceTDES.Where((User2WorkPlaceTDES e) => (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid).ToListAsync();
            }
            return View(user2workplacetdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? id = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null)
        {
            EnterpriseBranchWorkPlaceTDES enterprisebranchworkplacetdes = null;
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    _ = ref searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchworkplacetdes = await db.EnterpriseBranchWorkPlaceTDES.Where((EnterpriseBranchWorkPlaceTDES e) => (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id).FirstOrDefaultAsync();
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
                        enterprisebranchworkplacetdes = await db.EnterpriseBranchWorkPlaceTDES.Where((EnterpriseBranchWorkPlaceTDES e) => (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id).FirstOrDefaultAsync();
                    }
                }
            }
            else
            {
                enterprisebranchworkplacetdes = await db.EnterpriseBranchWorkPlaceTDES.Where((EnterpriseBranchWorkPlaceTDES e) => (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id).FirstOrDefaultAsync();
            }
            if (enterprisebranchworkplacetdes == null)
            {
                return HttpNotFound();
            }
            User2WorkPlaceTDES user2workplacetdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                user2workplacetdes = await dbSales.User2WorkPlaceTDES.FindAsync(id);
            }
            if (user2workplacetdes == null)
            {
                return HttpNotFound();
            }
            return View(user2workplacetdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Create(Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    _ = ref searchEntGuid;
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
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                var wps = from e in db.EnterpriseBranchWorkPlaceTDES
                          where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.IsActive == true
                          select new
                          {
                              e.WorkPlaceGuid,
                              e.Description
                          };
                base.ViewBag.sliWorkPlaces = new SelectList(await wps.ToListAsync(), "WorkPlaceGuid", "Description");
                var usrs = from e in db.EnterpriseBranchUserTDES
                           where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.IsActive == true && e.IsSeller == true
                           select new
                           {
                               EntUserNic = e.EntUserNic,
                               EntUserName = e.LastName + " " + e.FirstName + " " + e.EntUserNic
                           };
                base.ViewBag.sliUsers = new SelectList(await usrs.ToListAsync(), "EntUserNic", "EntUserName");
            }
            return View();
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        protected async Task ResetHst(User2WorkPlaceTDES u2w)
        {
            Guid? id = u2w.WorkPlaceGuid;
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            EnterpriseBranchWorkPlaceTDES enterprisebranchworkplacetdes = null;
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    _ = ref searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchworkplacetdes = await db.EnterpriseBranchWorkPlaceTDES.Where((EnterpriseBranchWorkPlaceTDES e) => (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id).FirstOrDefaultAsync();
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
                        enterprisebranchworkplacetdes = await db.EnterpriseBranchWorkPlaceTDES.Where((EnterpriseBranchWorkPlaceTDES e) => (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id).FirstOrDefaultAsync();
                    }
                }
            }
            else
            {
                enterprisebranchworkplacetdes = await db.EnterpriseBranchWorkPlaceTDES.Where((EnterpriseBranchWorkPlaceTDES e) => (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id).FirstOrDefaultAsync();
            }
            if (enterprisebranchworkplacetdes != null)
            {
                User2WorkPlaceHstTDES tmp = await dbSales.User2WorkPlaceHstTDES.Where((User2WorkPlaceHstTDES e) => e.SetAt == u2w.SetAt && e.WorkPlaceGuid == u2w.WorkPlaceGuid && e.EntUserNic == u2w.EntUserNic).FirstOrDefaultAsync();
                if (tmp == null)
                {
                    tmp = new User2WorkPlaceHstTDES
                    {
                        SetAt = u2w.SetAt,
                        WorkPlaceGuid = u2w.WorkPlaceGuid,
                        EntUserNic = u2w.EntUserNic
                    };
                    dbSales.User2WorkPlaceHstTDES.Add(tmp);
                }
                else
                {
                    dbSales.Entry(tmp).State = EntityState.Modified;
                }
                tmp.ReSetAt = DateTime.Now.AddSeconds(2.0);
                tmp.ReSetAt = tmp.ReSetAt.AddMilliseconds(-tmp.ReSetAt.Millisecond);
                tmp.FirstName = u2w.FirstName;
                tmp.LastName = u2w.LastName;
                tmp.EntBranchGuid = u2w.EntBranchGuid;
                tmp.EntGuid = u2w.EntGuid;
                tmp.Description = u2w.Description;
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Create(Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null, Guid? WorkPlaceGuid = null, string EntUserNic = null)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    _ = ref searchEntGuid;
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
            EnterpriseBranchWorkPlaceTDES wp = null;
            EnterpriseBranchUserTDES usrs = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (base.ModelState.IsValid && !WorkPlaceGuid.HasValue)
            {
                base.ModelState.AddModelError("", Resources.WorkPlace_IS_NOT_DEFINED);
            }
            if (base.ModelState.IsValid && string.IsNullOrEmpty(EntUserNic))
            {
                base.ModelState.AddModelError("", Resources.User_IS_NOT_DEFINED);
            }
            if (base.ModelState.IsValid)
            {
                wp = await db.EnterpriseBranchWorkPlaceTDES.Where((EnterpriseBranchWorkPlaceTDES e) => (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.IsActive == true && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)WorkPlaceGuid).FirstOrDefaultAsync();
                if (wp == null)
                {
                    base.ModelState.AddModelError("", Resources.WorkPlace_IS_NOT_DEFINED);
                }
            }
            if (base.ModelState.IsValid)
            {
                usrs = await db.EnterpriseBranchUserTDES.Where((EnterpriseBranchUserTDES e) => (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.IsActive == true && e.IsSeller == true && e.EntUserNic == EntUserNic).FirstOrDefaultAsync();
                if (usrs == null)
                {
                    base.ModelState.AddModelError("", Resources.User_IS_NOT_DEFINED);
                }
            }
            if (base.ModelState.IsValid)
            {
                foreach (User2WorkPlaceTDES wpItem in await dbSales.User2WorkPlaceTDES.Where((User2WorkPlaceTDES e) => (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == EntUserNic).ToListAsync())
                {
                    await ResetHst(wpItem);
                    dbSales.User2WorkPlaceTDES.Remove(wpItem);
                }
                User2WorkPlaceTDES newWps = await dbSales.User2WorkPlaceTDES.Where((User2WorkPlaceTDES e) => (Guid)(Guid?)e.WorkPlaceGuid == (Guid)WorkPlaceGuid).FirstOrDefaultAsync();
                if (newWps == null)
                {
                    newWps = new User2WorkPlaceTDES
                    {
                        WorkPlaceGuid = wp.WorkPlaceGuid
                    };
                    dbSales.User2WorkPlaceTDES.Add(newWps);
                }
                newWps.Description = wp.Description;
                newWps.EntUserNic = usrs.EntUserNic;
                newWps.FirstName = usrs.FirstName;
                newWps.LastName = usrs.LastName;
                newWps.SetAt = DateTime.Now;
                newWps.SetAt = newWps.SetAt.AddMilliseconds(-newWps.SetAt.Millisecond);
                newWps.EntBranchGuid = wp.EntBranchGuid;
                newWps.EntGuid = wp.EntGuid;
                await ResetHst(newWps);
                await dbSales.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = newWps.EntGuid,
                    searchEntBranchGuid = newWps.EntBranchGuid
                });
            }
            if (CarShopSalesContextCatalog != null)
            {
                var wpss = from e in db.EnterpriseBranchWorkPlaceTDES
                           where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.IsActive == true
                           select new
                           {
                               e.WorkPlaceGuid,
                               e.Description
                           };
                base.ViewBag.sliWorkPlaces = new SelectList(await wpss.ToListAsync(), "WorkPlaceGuid", "Description", WorkPlaceGuid);
                var usrss = from e in db.EnterpriseBranchUserTDES
                            where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.IsActive == true && e.IsSeller == true
                            select new
                            {
                                EntUserNic = e.EntUserNic,
                                EntUserName = e.LastName + " " + e.FirstName + " " + e.EntUserNic
                            };
                base.ViewBag.sliUsers = new SelectList(await usrss.ToListAsync(), "EntUserNic", "EntUserName", EntUserNic);
            }
            return View();
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Delete(Guid? id = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null)
        {
            EnterpriseBranchWorkPlaceTDES enterprisebranchworkplacetdes = null;
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    _ = ref searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchworkplacetdes = await db.EnterpriseBranchWorkPlaceTDES.Where((EnterpriseBranchWorkPlaceTDES e) => (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id).FirstOrDefaultAsync();
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
                        enterprisebranchworkplacetdes = await db.EnterpriseBranchWorkPlaceTDES.Where((EnterpriseBranchWorkPlaceTDES e) => (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id).FirstOrDefaultAsync();
                    }
                }
            }
            else
            {
                enterprisebranchworkplacetdes = await db.EnterpriseBranchWorkPlaceTDES.Where((EnterpriseBranchWorkPlaceTDES e) => (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id).FirstOrDefaultAsync();
            }
            if (enterprisebranchworkplacetdes == null)
            {
                return HttpNotFound();
            }
            User2WorkPlaceTDES user2workplacetdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                user2workplacetdes = await dbSales.User2WorkPlaceTDES.FindAsync(id);
            }
            if (user2workplacetdes == null)
            {
                return HttpNotFound();
            }
            return View(user2workplacetdes);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> DeleteConfirmed(Guid? id, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null)
        {
            EnterpriseBranchWorkPlaceTDES enterprisebranchworkplacetdes = null;
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    _ = ref searchEntGuid;
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    enterprisebranchworkplacetdes = await db.EnterpriseBranchWorkPlaceTDES.Where((EnterpriseBranchWorkPlaceTDES e) => (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id).FirstOrDefaultAsync();
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
                        enterprisebranchworkplacetdes = await db.EnterpriseBranchWorkPlaceTDES.Where((EnterpriseBranchWorkPlaceTDES e) => (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id).FirstOrDefaultAsync();
                    }
                }
            }
            else
            {
                enterprisebranchworkplacetdes = await db.EnterpriseBranchWorkPlaceTDES.Where((EnterpriseBranchWorkPlaceTDES e) => (Guid)(Guid?)e.WorkPlaceGuid == (Guid)id).FirstOrDefaultAsync();
            }
            if (enterprisebranchworkplacetdes == null)
            {
                return HttpNotFound();
            }
            User2WorkPlaceTDES user2workplacetdes2 = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                user2workplacetdes2 = await dbSales.User2WorkPlaceTDES.FindAsync(id);
                if (user2workplacetdes2 != null)
                {
                    await ResetHst(user2workplacetdes2);
                }
                dbSales.User2WorkPlaceTDES.Remove(user2workplacetdes2);
                await dbSales.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid,
                    searchEntBranchGuid
                });
            }
            return View(user2workplacetdes2);
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbSales != null)
            {
                _dbSales.Dispose();
                _dbSales = null;
            }
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
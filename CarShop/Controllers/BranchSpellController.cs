// CarShop.Controllers.BranchSpellController
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

    public class BranchSpellController : Controller
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefaultAsync();
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
            EnterpriseTDES enterprises = await (from e in db.EnterpriseTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
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
            List<BranchSpellTDES> branchspelltdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                branchspelltdes = await (from e in dbSales.BranchSpellTDES
                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                         select e).ToListAsync();
            }
            return View(branchspelltdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
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
            await ViewBagHelper(null, id);
            BranchSpellTDES branchspelltdes = null;
            if (CarShopSalesContextCatalog != null)
            {
                branchspelltdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbSales.BranchSpellTDES
                                                                                                  where e.EntBranchGuid == id.Value
                                                                                                  select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbSales.BranchSpellTDES
                                                                                                                                                                                                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                       select e).FirstOrDefaultAsync()) : (await (from e in dbSales.BranchSpellTDES
                                                                                                                                                                                                                                                                                  where e.EntBranchGuid == id.Value && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                  select e).FirstOrDefaultAsync())));
            }
            if (branchspelltdes == null)
            {
                base.ModelState.AddModelError("", Resources.SpellNotFound);
            }
            return View(branchspelltdes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> ChangeState(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string BranchSpellTDES_UNBLOCK = null, string BranchSpellTDES_BLOCK = null, string BranchSpellTDES_CLOSE = null)
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
            BranchSpellTDES branchspelltdes = null;
            if (CarShopSalesContextCatalog != null)
            {
                branchspelltdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbSales.BranchSpellTDES
                                                                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                  select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbSales.BranchSpellTDES
                                                                                                                                                                                                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                       select e).FirstOrDefaultAsync()) : (await (from e in dbSales.BranchSpellTDES
                                                                                                                                                                                                                                                                                  where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                  select e).FirstOrDefaultAsync())));
            }
            if (branchspelltdes == null)
            {
                base.ModelState.AddModelError("", Resources.SpellNotFound);
                return View(branchspelltdes);
            }
            if (BranchSpellTDES_UNBLOCK != null)
            {
                if (!branchspelltdes.IsActive)
                {
                    base.ModelState.AddModelError("", Resources.BranchSpellTDES_CLOSED_CANNOTWORK);
                    return View(branchspelltdes);
                }
                branchspelltdes.IsBlocked = false;
                await ResetHst(branchspelltdes);
                await dbSales.SaveChangesAsync();
            }
            if (BranchSpellTDES_BLOCK != null)
            {
                if (!branchspelltdes.IsActive)
                {
                    base.ModelState.AddModelError("", Resources.BranchSpellTDES_CLOSED_CANNOTWORK);
                    return View(branchspelltdes);
                }
                branchspelltdes.IsBlocked = true;
                await ResetHst(branchspelltdes);
                await dbSales.SaveChangesAsync();
            }
            if (BranchSpellTDES_CLOSE != null)
            {
                if (!branchspelltdes.IsBlocked)
                {
                    base.ModelState.AddModelError("", Resources.BranchSpellTDES_UNBLOCKED_CANNOTCLOSE);
                    return View(branchspelltdes);
                }
                DateTime aCloseAt2 = DateTime.Now;
                aCloseAt2 = aCloseAt2.AddMilliseconds(-aCloseAt2.Millisecond);
                if (branchspelltdes.OpenAt > aCloseAt2)
                {
                    base.ModelState.AddModelError("", Resources.SpellCloseDateLargeThanOpenDate);
                    return View(branchspelltdes);
                }
                branchspelltdes.CloseAt = aCloseAt2;
                branchspelltdes.IsActive = false;
                await ResetHst(branchspelltdes);
                await dbSales.SaveChangesAsync();
            }
            return View(branchspelltdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Create(Guid? searchEntGuid, Guid? searchEntBranchGuid)
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
            BranchSpellTDES branchspelltdes = null;
            if (CarShopSalesContextCatalog != null)
            {
                string text = null;
                if (base.HttpContext.User != null && base.HttpContext.User.Identity != null)
                {
                    text = base.HttpContext.User.Identity.Name;
                }
                if (string.IsNullOrEmpty(text))
                {
                    text = "Noname";
                }
                branchspelltdes = new BranchSpellTDES
                {
                    EntBranchGuid = searchEntBranchGuid.Value,
                    EntBranchDescription = base.ViewBag.EntBranchDescription,
                    SpellGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsBlocked = true,
                    OpenAt = DateTime.Now,
                    CloseAt = DateTime.Now.AddSeconds(5.0),
                    OpenedBy = text,
                    EntGuid = base.ViewBag.SearchEntGuid
                };
                branchspelltdes.OpenAt = branchspelltdes.OpenAt.AddMilliseconds(-branchspelltdes.OpenAt.Millisecond);
                branchspelltdes.CloseAt = branchspelltdes.CloseAt.AddMilliseconds(-branchspelltdes.CloseAt.Millisecond);
            }
            return View(branchspelltdes);
        }

        protected async Task ResetHst(BranchSpellTDES branchspelltdes)
        {
            string userName = null;
            if (base.HttpContext.User != null && base.HttpContext.User.Identity != null)
            {
                userName = base.HttpContext.User.Identity.Name;
            }
            if (string.IsNullOrEmpty(userName))
            {
                userName = "Noname";
            }
            BranchSpellHstTDES tmp2 = await (from e in dbSales.BranchSpellHstTDES
                                             where e.EntBranchGuid == branchspelltdes.EntBranchGuid && e.SpellGuid == branchspelltdes.SpellGuid
                                             select e).FirstOrDefaultAsync();
            if (tmp2 == null)
            {
                tmp2 = new BranchSpellHstTDES
                {
                    SpellGuid = branchspelltdes.SpellGuid,
                    EntBranchGuid = branchspelltdes.EntBranchGuid,
                    EntGuid = branchspelltdes.EntGuid,
                    OpenAt = branchspelltdes.OpenAt,
                    CloseAt = branchspelltdes.CloseAt,
                    OpenedBy = branchspelltdes.OpenedBy,
                    IsActive = branchspelltdes.IsActive,
                    IsBlocked = branchspelltdes.IsBlocked,
                    ClosedBy = userName,
                    IsCribFromIncome = false
                };
                dbSales.BranchSpellHstTDES.Add(tmp2);
                return;
            }
            tmp2.OpenAt = branchspelltdes.OpenAt;
            tmp2.CloseAt = branchspelltdes.CloseAt;
            tmp2.OpenedBy = branchspelltdes.OpenedBy;
            tmp2.IsActive = branchspelltdes.IsActive;
            tmp2.IsBlocked = branchspelltdes.IsBlocked;
            tmp2.ClosedBy = userName;
            if (branchspelltdes.IsActive)
            {
                tmp2.IsCribFromIncome = false;
            }
            dbSales.Entry(tmp2).State = EntityState.Modified;
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [HttpPost]
        public async Task<ActionResult> Create(BranchSpellTDES branchspelltdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = branchspelltdes.EntBranchGuid;
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
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchTDES
                                                                        where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
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
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
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
            else
            {
                searchEntBranchGuid = branchspelltdes.EntBranchGuid;
                searchEntGuid = branchspelltdes.EntGuid;
            }
            if (base.ModelState.IsValid && (searchEntBranchGuid != branchspelltdes.EntBranchGuid || searchEntGuid != branchspelltdes.EntGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            }
            if (CarShopSalesContextCatalog != null)
            {
                if (base.ModelState.IsValid)
                {
                    BranchSpellTDES tmp3 = await (from e in dbSales.BranchSpellTDES
                                                  where e.EntBranchGuid == branchspelltdes.EntBranchGuid
                                                  select e).FirstOrDefaultAsync();
                    if (tmp3 != null)
                    {
                        if (tmp3.IsActive)
                        {
                            base.ModelState.AddModelError("", Resources.CreateNewSpellIfCurrentClose);
                        }
                        if (base.ModelState.IsValid && tmp3.CloseAt > branchspelltdes.OpenAt)
                        {
                            base.ModelState.AddModelError("", Resources.SpellCloseDateLargeThanOpenDate);
                        }
                    }
                }
                if (base.ModelState.IsValid)
                {
                    IQueryable<BranchSpellHstTDES> tmp2 = from e in dbSales.BranchSpellHstTDES
                                                          where e.EntBranchGuid == branchspelltdes.EntBranchGuid && e.CloseAt > branchspelltdes.OpenAt
                                                          select e;
                    if (await tmp2.CountAsync() > 0)
                    {
                        base.ModelState.AddModelError("", Resources.SpellCloseDateLargeThanOpenDate);
                    }
                }
                if (base.ModelState.IsValid)
                {
                    BranchSpellTDES tmp = await (from e in dbSales.BranchSpellTDES
                                                 where e.EntBranchGuid == branchspelltdes.EntBranchGuid
                                                 select e).FirstOrDefaultAsync();
                    if (tmp == null)
                    {
                        dbSales.BranchSpellTDES.Add(branchspelltdes);
                        await ResetHst(branchspelltdes);
                        await dbSales.SaveChangesAsync();
                        return RedirectToAction("Index", new
                        {
                            searchEntGuid = branchspelltdes.EntGuid,
                            searchEntBranchGuid = branchspelltdes.EntBranchGuid
                        });
                    }
                    if (!tmp.IsActive)
                    {
                        dbSales.Entry(tmp).State = EntityState.Modified;
                        tmp.SpellGuid = branchspelltdes.SpellGuid;
                        tmp.EntBranchGuid = branchspelltdes.EntBranchGuid;
                        tmp.EntBranchDescription = branchspelltdes.EntBranchDescription;
                        tmp.EntGuid = branchspelltdes.EntGuid;
                        tmp.OpenAt = branchspelltdes.OpenAt;
                        tmp.CloseAt = branchspelltdes.CloseAt;
                        tmp.OpenedBy = branchspelltdes.OpenedBy;
                        tmp.IsActive = branchspelltdes.IsActive;
                        tmp.IsBlocked = branchspelltdes.IsBlocked;
                        await ResetHst(tmp);
                        await dbSales.SaveChangesAsync();
                        return RedirectToAction("Index", new
                        {
                            searchEntGuid = branchspelltdes.EntGuid,
                            searchEntBranchGuid = branchspelltdes.EntBranchGuid
                        });
                    }
                    base.ModelState.AddModelError("", Resources.CloseCurrentSpellFirst);
                }
            }
            return View(branchspelltdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Edit(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
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
            await ViewBagHelper(null, id);
            BranchSpellTDES branchspelltdes = null;
            if (CarShopSalesContextCatalog != null)
            {
                branchspelltdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbSales.BranchSpellTDES
                                                                                                  where e.EntBranchGuid == id.Value
                                                                                                  select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbSales.BranchSpellTDES
                                                                                                                                                                                                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                       select e).FirstOrDefaultAsync()) : (await (from e in dbSales.BranchSpellTDES
                                                                                                                                                                                                                                                                                  where e.EntBranchGuid == id.Value && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                  select e).FirstOrDefaultAsync())));
                branchspelltdes.CloseAt = DateTime.Now;
                branchspelltdes.CloseAt = branchspelltdes.CloseAt.AddMilliseconds(-branchspelltdes.CloseAt.Millisecond);
            }
            if (branchspelltdes == null)
            {
                base.ModelState.AddModelError("", Resources.SpellNotFound);
            }
            return View(branchspelltdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BranchSpellTDES branchspelltdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = branchspelltdes.EntBranchGuid;
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
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchTDES
                                                                        where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
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
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
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
            else
            {
                searchEntBranchGuid = branchspelltdes.EntBranchGuid;
                searchEntGuid = branchspelltdes.EntGuid;
            }
            if (base.ModelState.IsValid && (searchEntBranchGuid != branchspelltdes.EntBranchGuid || searchEntGuid != branchspelltdes.EntGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            }
            if (CarShopSalesContextCatalog != null)
            {
                if (base.ModelState.IsValid && branchspelltdes.OpenAt > branchspelltdes.CloseAt)
                {
                    base.ModelState.AddModelError("", Resources.SpellCloseDateLessThanOpenDate);
                }
                bool isValid = base.ModelState.IsValid;
                if (base.ModelState.IsValid)
                {
                    dbSales.Entry(branchspelltdes).State = EntityState.Modified;
                    await ResetHst(branchspelltdes);
                    await dbSales.SaveChangesAsync();
                    return RedirectToAction("Index", new
                    {
                        searchEntGuid = branchspelltdes.EntGuid,
                        searchEntBranchGuid = branchspelltdes.EntBranchGuid
                    });
                }
            }
            return View(branchspelltdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Delete(Guid? id = default(Guid?))
        {
            await ViewBagHelper(null, id);
            BranchSpellTDES branchspelltdes = null;
            if (CarShopSalesContextCatalog != null)
            {
                branchspelltdes = await (from e in dbSales.BranchSpellTDES
                                         where e.EntBranchGuid == id.Value
                                         select e).FirstOrDefaultAsync();
            }
            if (branchspelltdes == null)
            {
                base.ModelState.AddModelError("", Resources.SpellNotFound);
            }
            return View(branchspelltdes);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> DeleteConfirmed(Guid? id)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
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
            await ViewBagHelper(null, id);
            BranchSpellTDES branchspelltdes = null;
            if (CarShopSalesContextCatalog != null)
            {
                branchspelltdes = ((!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbSales.BranchSpellTDES
                                                                                                  where e.EntBranchGuid == id.Value
                                                                                                  select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbSales.BranchSpellTDES
                                                                                                                                                                                                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                                                                                                                                                                                       select e).FirstOrDefaultAsync()) : (await (from e in dbSales.BranchSpellTDES
                                                                                                                                                                                                                                                                                  where e.EntBranchGuid == id.Value && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                  select e).FirstOrDefaultAsync())));
            }
            if (branchspelltdes != null && branchspelltdes.IsActive)
            {
                base.ModelState.AddModelError("", Resources.ActiveSpellCannotBeDeleted);
                return View(branchspelltdes);
            }
            dbSales.BranchSpellTDES.Remove(branchspelltdes);
            await dbSales.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                searchEntBranchGuid = (object)base.ViewBag.SearchEntBranchGuid
            });
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
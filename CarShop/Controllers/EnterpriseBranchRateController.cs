// CarShop.Controllers.EnterpriseBranchRateController
using CarShop.Models;
using CarShop.Properties;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterpriseBranchRateController : Controller
    {
        private Guid? guestEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private string CarShopRestContextCatalog;

        private CarShopRestContext _dbRest;

        private CarShopContext db = new CarShopContext();

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

        private async Task PreparedbRest()
        {
            EnterpriseBranchTDES enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)guestEntBranchGuid
                                                               select e).FirstOrDefaultAsync();
            if (enterprisebranchtdes != null)
            {
                CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            }
        }

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
            base.ViewBag.IsBranchAdmin = base.User.IsInRole("BranchAdmin");
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

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid, Guid? searchEntBranchGuid)
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
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            IQueryable<EnterpriseBranchRateTDES> enterprisebranchratetdes = from e in db.EnterpriseBranchRateTDES
                                                                            where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                            select e;
            return View(await enterprisebranchratetdes.ToListAsync());
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(Guid? searchEntGuid, Guid? searchEntBranchGuid, int? searchCurrencyIso)
        {
            if (!searchCurrencyIso.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
            EnterpriseBranchRateTDES enterprisebranchratetdes = await (from e in db.EnterpriseBranchRateTDES
                                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (int?)e.CurrencyIso == searchCurrencyIso
                                                                       select e).FirstOrDefaultAsync();
            if (enterprisebranchratetdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisebranchratetdes);
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
            base.ViewBag.sliCurrencyIso = new SelectList(await (from e in db.Currency
                                                                orderby e.CurrencyName
                                                                select e).ToListAsync(), "CurrencyIso", "CurrencyName");
            return View();
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EntBranchGuid,CurrencyIso,ExchRate,EntGuid")] EnterpriseBranchRateTDES enterprisebranchratetdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid;
            Guid? searchEntBranchGuid;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                    searchEntBranchGuid = enterprisebranchratetdes.EntBranchGuid;
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
                searchEntGuid = enterprisebranchratetdes.EntGuid;
                searchEntBranchGuid = enterprisebranchratetdes.EntBranchGuid;
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (base.ModelState.IsValid)
            {
                db.EnterpriseBranchRateTDES.Add(enterprisebranchratetdes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid,
                    searchEntBranchGuid
                });
            }
            base.ViewBag.sliCurrencyIso = new SelectList(await (from e in db.Currency
                                                                orderby e.CurrencyName
                                                                select e).ToListAsync(), "CurrencyIso", "CurrencyName", enterprisebranchratetdes.CurrencyIso);
            return View(enterprisebranchratetdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Edit(Guid? searchEntGuid, Guid? searchEntBranchGuid, int? searchCurrencyIso)
        {
            if (!searchCurrencyIso.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
            EnterpriseBranchRateTDES enterprisebranchratetdes = await (from e in db.EnterpriseBranchRateTDES
                                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (int?)e.CurrencyIso == searchCurrencyIso
                                                                       select e).FirstOrDefaultAsync();
            if (enterprisebranchratetdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.sliCurrencyIso = new SelectList(await (from e in db.Currency
                                                                orderby e.CurrencyName
                                                                select e).ToListAsync(), "CurrencyIso", "CurrencyName", enterprisebranchratetdes.CurrencyIso);
            return View(enterprisebranchratetdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EntBranchGuid,CurrencyIso,ExchRate,EntGuid")] EnterpriseBranchRateTDES enterprisebranchratetdes)
        {
            UserIsInRoles();
            Guid? searchEntGuid;
            Guid? searchEntBranchGuid;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                    searchEntBranchGuid = enterprisebranchratetdes.EntBranchGuid;
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
                searchEntGuid = enterprisebranchratetdes.EntGuid;
                searchEntBranchGuid = enterprisebranchratetdes.EntBranchGuid;
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (base.ModelState.IsValid)
            {
                db.Entry(enterprisebranchratetdes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid,
                    searchEntBranchGuid
                });
            }
            base.ViewBag.sliCurrencyIso = new SelectList(await (from e in db.Currency
                                                                orderby e.CurrencyName
                                                                select e).ToListAsync(), "CurrencyIso", "CurrencyName", enterprisebranchratetdes.CurrencyIso);
            return View(enterprisebranchratetdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Delete(Guid? searchEntGuid, Guid? searchEntBranchGuid, int? searchCurrencyIso)
        {
            if (!searchCurrencyIso.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
            EnterpriseBranchRateTDES enterprisebranchratetdes = await (from e in db.EnterpriseBranchRateTDES
                                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (int?)e.CurrencyIso == searchCurrencyIso
                                                                       select e).FirstOrDefaultAsync();
            if (enterprisebranchratetdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisebranchratetdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(Guid? searchEntGuid, Guid? searchEntBranchGuid, int? searchCurrencyIso)
        {
            EnterpriseBranchRateTDES enterprisebranchratetdes = await (from e in db.EnterpriseBranchRateTDES
                                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (int?)e.CurrencyIso == searchCurrencyIso
                                                                       select e).FirstOrDefaultAsync();
            db.EnterpriseBranchRateTDES.Remove(enterprisebranchratetdes);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid,
                searchEntBranchGuid
            });
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> DistribureRates(Guid? searchEntGuid, Guid? searchEntBranchGuid)
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
            return View();
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [HttpPost]
        [ActionName("DistribureRates")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DistribureRatesConfirmed(Guid? searchEntGuid, Guid? searchEntBranchGuid)
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
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            await PreparedbRest();
            if (string.IsNullOrEmpty(CarShopRestContextCatalog))
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            foreach (EnterpriseBranchRateTDES ebr in await (from e in db.EnterpriseBranchRateTDES
                                                            where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                            select e).ToListAsync())
            {
                Guid EntBranchGuid = ebr.EntBranchGuid;
                int PriceCurrencyIso = ebr.CurrencyIso;
                double ExchRate = ebr.ExchRate;
                SqlParameter EntBranchGuidPrm = new SqlParameter
                {
                    ParameterName = "@EntBranchGuidP",
                    Value = EntBranchGuid,
                    Direction = ParameterDirection.Input
                };
                SqlParameter PriceCurrencyIsoPrm = new SqlParameter
                {
                    ParameterName = "@PriceCurrencyIsoP",
                    Value = PriceCurrencyIso,
                    Direction = ParameterDirection.Input
                };
                SqlParameter ExchRatePrm = new SqlParameter
                {
                    ParameterName = "@ExchRateP",
                    Value = ExchRate,
                    Direction = ParameterDirection.Input
                };
                try
                {
                    await dbRest.Database.ExecuteSqlCommandAsync("UPDATE [dbo].[BranchSuppTDES] set ExchRate = @ExchRateP WHERE EntBranchGuid  = @EntBranchGuidP AND PriceCurrencyIso = @PriceCurrencyIsoP", ExchRatePrm, EntBranchGuidPrm, PriceCurrencyIsoPrm);
                }
                catch (Exception ex)
                {
                    base.ModelState.AddModelError("", "Ошибка БД:" + ex.Message);
                    return View();
                }
            }
            base.ViewBag.TheJobIsDone = "Курсы валют разнесены успешно";
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbRest != null)
                {
                    _dbRest.Dispose();
                    _dbRest = null;
                }
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
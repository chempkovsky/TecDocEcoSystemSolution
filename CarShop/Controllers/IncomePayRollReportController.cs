// CarShop.Controllers.IncomePayRollReportController
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

    public class IncomePayRollReportController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopIncomeContextCatalog;

        private CarShopIncomeContext _dbIncome;

        private CarShopIncomeContext dbIncome
        {
            get
            {
                if (_dbIncome == null)
                {
                    _dbIncome = new CarShopIncomeContext(CarShopIncomeContextCatalog);
                }
                return _dbIncome;
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
                CarShopIncomeContextCatalog = enterprisebranchtdes.IncomeCatalog;
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
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? searchIncomePayRollTDESGuid = default(Guid?))
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
            List<IncomeArticleTDES> incomearticletdes = new List<IncomeArticleTDES>();
            if (CarShopIncomeContextCatalog != null)
            {
                IncomePayRollTDES incomepayrolltdes = await (from e in dbIncome.IncomePayRollTDES
                                                             where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid
                                                             select e).FirstOrDefaultAsync();
                if (incomepayrolltdes == null)
                {
                    base.ModelState.AddModelError("", Resources.SpellGivenNotFound);
                    return View(incomearticletdes);
                }
                base.ViewBag.searchIncomePayRollTDESGuid = incomepayrolltdes.IncomePayRollTDESGuid;
                foreach (var item in await (from e in dbIncome.IncomeArticleTDES
                                            where (Guid)(Guid?)e.IncomePayRollTDESGuid == (Guid)searchIncomePayRollTDESGuid
                                            group e by new
                                            {
                                                e.IsProcessed,
                                                e.IsReversed
                                            } into g
                                            select new
                                            {
                                                IsProcessed = g.Key.IsProcessed,
                                                IsReversed = g.Key.IsReversed,
                                                ArtAmount = g.Sum((IncomeArticleTDES e) => e.ArtAmount),
                                                ArtAmountRest = g.Sum((IncomeArticleTDES e) => e.ArtAmountRest),
                                                PurchasePrice = g.Sum((IncomeArticleTDES e) => e.PurchasePrice * (double)e.ArtAmount),
                                                ArtPrice = g.Sum((IncomeArticleTDES e) => e.ArtPrice * (double)e.ArtAmount),
                                                CurrArtPrice = g.Sum((IncomeArticleTDES e) => e.CurrArtPrice * (double)e.ArtAmountRest)
                                            }).ToListAsync())
                {
                    incomearticletdes.Add(new IncomeArticleTDES
                    {
                        IsProcessed = item.IsProcessed,
                        IsReversed = item.IsReversed,
                        ArtAmount = item.ArtAmount,
                        ArtAmountRest = item.ArtAmountRest,
                        PurchasePrice = item.PurchasePrice,
                        ArtPrice = item.ArtPrice,
                        CurrArtPrice = item.CurrArtPrice
                    });
                }
            }
            return View(incomearticletdes);
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbIncome != null)
            {
                _dbIncome.Dispose();
                _dbIncome = null;
            }
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}
// CarShop.Controllers.RevaluationReportController
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

    public class RevaluationReportController : Controller
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

        protected SelectList SelectListHelper(int showIs)
        {
            return new SelectList(new SelectListItem[3]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.SHOWALL,
                Selected = (showIs == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.ShowMargin,
                Selected = (showIs == 2)
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.ShowMarkdown,
                Selected = (showIs == 3)
            }
            }, "Value", "Text", showIs);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? searchSheetRevaluationTDESGuid = default(Guid?), int? reportType = default(int?))
        {
            reportType = (reportType ?? 1);
            base.ViewBag.ReportType = reportType;
            base.ViewBag.sliReportType = SelectListHelper(reportType.Value);
            base.ViewBag.SearchSheetRevaluationTDESGuid = searchSheetRevaluationTDESGuid;
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
            List<RevaluationArticleTDES> aResult = new List<RevaluationArticleTDES>();
            if (CarShopIncomeContextCatalog != null)
            {
                if (await (from e in dbIncome.SheetRevaluationTDES
                           where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)searchSheetRevaluationTDESGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                           select e).FirstOrDefaultAsync() == null)
                {
                    base.ModelState.AddModelError("", Resources.SpellGivenNotFound);
                    return View(aResult);
                }
                if (reportType.Value == 1)
                {
                    foreach (var item in await (from e in dbIncome.RevaluationArticleTDES
                                                where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)searchSheetRevaluationTDESGuid
                                                group e by new
                                                {
                                                    e.IsProcessed,
                                                    e.IsReversed
                                                } into g
                                                select new
                                                {
                                                    IsProcessed = g.Key.IsProcessed,
                                                    IsReversed = g.Key.IsReversed,
                                                    ArtAmountRest = g.Sum((RevaluationArticleTDES e) => e.ArtAmountRest),
                                                    CurrArtPrice = g.Sum((RevaluationArticleTDES e) => e.CurrArtPrice * (double)e.ArtAmountRest),
                                                    NewArtPrice = g.Sum((RevaluationArticleTDES e) => e.NewArtPrice * (double)e.ArtAmountRest),
                                                    OperSum = g.Sum((RevaluationArticleTDES e) => e.OperSum)
                                                }).ToListAsync())
                    {
                        aResult.Add(new RevaluationArticleTDES
                        {
                            IsProcessed = item.IsProcessed,
                            IsReversed = item.IsReversed,
                            ArtAmountRest = item.ArtAmountRest,
                            CurrArtPrice = item.CurrArtPrice,
                            NewArtPrice = item.NewArtPrice,
                            OperSum = item.OperSum
                        });
                    }
                }
                if (reportType.Value == 2)
                {
                    foreach (var item2 in await (from e in dbIncome.RevaluationArticleTDES
                                                 where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)searchSheetRevaluationTDESGuid && e.OperSum >= 0.0
                                                 group e by new
                                                 {
                                                     e.IsProcessed,
                                                     e.IsReversed
                                                 } into g
                                                 select new
                                                 {
                                                     IsProcessed = g.Key.IsProcessed,
                                                     IsReversed = g.Key.IsReversed,
                                                     ArtAmountRest = g.Sum((RevaluationArticleTDES e) => e.ArtAmountRest),
                                                     CurrArtPrice = g.Sum((RevaluationArticleTDES e) => e.CurrArtPrice * (double)e.ArtAmountRest),
                                                     NewArtPrice = g.Sum((RevaluationArticleTDES e) => e.NewArtPrice * (double)e.ArtAmountRest),
                                                     OperSum = g.Sum((RevaluationArticleTDES e) => e.OperSum)
                                                 }).ToListAsync())
                    {
                        aResult.Add(new RevaluationArticleTDES
                        {
                            IsProcessed = item2.IsProcessed,
                            IsReversed = item2.IsReversed,
                            ArtAmountRest = item2.ArtAmountRest,
                            CurrArtPrice = item2.CurrArtPrice,
                            NewArtPrice = item2.NewArtPrice,
                            OperSum = item2.OperSum
                        });
                    }
                }
                if (reportType.Value == 3)
                {
                    foreach (var item3 in await (from e in dbIncome.RevaluationArticleTDES
                                                 where (Guid)(Guid?)e.SheetRevaluationTDESGuid == (Guid)searchSheetRevaluationTDESGuid && e.OperSum < 0.0
                                                 group e by new
                                                 {
                                                     e.IsProcessed,
                                                     e.IsReversed
                                                 } into g
                                                 select new
                                                 {
                                                     IsProcessed = g.Key.IsProcessed,
                                                     IsReversed = g.Key.IsReversed,
                                                     ArtAmountRest = g.Sum((RevaluationArticleTDES e) => e.ArtAmountRest),
                                                     CurrArtPrice = g.Sum((RevaluationArticleTDES e) => e.CurrArtPrice * (double)e.ArtAmountRest),
                                                     NewArtPrice = g.Sum((RevaluationArticleTDES e) => e.NewArtPrice * (double)e.ArtAmountRest),
                                                     OperSum = g.Sum((RevaluationArticleTDES e) => e.OperSum)
                                                 }).ToListAsync())
                    {
                        aResult.Add(new RevaluationArticleTDES
                        {
                            IsProcessed = item3.IsProcessed,
                            IsReversed = item3.IsReversed,
                            ArtAmountRest = item3.ArtAmountRest,
                            CurrArtPrice = item3.CurrArtPrice,
                            NewArtPrice = item3.NewArtPrice,
                            OperSum = item3.OperSum
                        });
                    }
                }
            }
            return View(aResult);
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
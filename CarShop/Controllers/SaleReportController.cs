// CarShop.Controllers.SaleReportController
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

    public class SaleReportController : Controller
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
        }

        protected SelectList SelectListHelper(int showIs)
        {
            return new SelectList(new SelectListItem[4]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.ReportTypePayed,
                Selected = (showIs == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.ReportTypeNotPayed,
                Selected = (showIs == 2)
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.ReportTypeStorned,
                Selected = (showIs == 3)
            },
            new SelectListItem
            {
                Value = "4",
                Text = Resources.SaleBasketReportIncomeTDES_INDEX,
                Selected = (showIs == 4)
            }
            }, "Value", "Text", showIs);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? SpellGuid = default(Guid?), int? reportType = default(int?))
        {
            reportType = (reportType ?? 1);
            base.ViewBag.ReportType = reportType;
            base.ViewBag.sliReportType = SelectListHelper(reportType.Value);
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
            List<SaleBasketReportTDES> aResult = null;
            if (CarShopSalesContextCatalog == null)
            {
                base.ViewBag.SpellGuid = null;
            }
            else
            {
                if (await (from e in dbSales.BranchSpellHstTDES
                           where (Guid)(Guid?)e.SpellGuid == (Guid)SpellGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                           select e).FirstOrDefaultAsync() == null)
                {
                    base.ModelState.AddModelError("", Resources.SpellGivenNotFound);
                    return View();
                }
                base.ViewBag.SpellGuid = SpellGuid;
                switch (reportType.Value)
                {
                    case 1:
                        aResult = await (from e in dbSales.SaleBasketTDES
                                         where (Guid)(Guid?)e.SpellGuid == (Guid)SpellGuid && e.IsPaid == true && e.IsReverse == false
                                         group e by new
                                         {
                                             e.SpellGuid,
                                             e.EntUserNic,
                                             e.WorkPlaceGuid,
                                             e.Description
                                         } into grpSpell
                                         select new SaleBasketReportTDES
                                         {
                                             SpellGuid = grpSpell.Key.SpellGuid,
                                             WorkPlaceGuid = grpSpell.Key.WorkPlaceGuid,
                                             Description = grpSpell.Key.Description,
                                             EntUserNic = grpSpell.Key.EntUserNic,
                                             PaymentSum = grpSpell.Sum((SaleBasketTDES ei) => ei.PaymentSum),
                                             ArtAmount = grpSpell.Sum((SaleBasketTDES ei) => ei.ArtAmount)
                                         }).ToListAsync();
                        break;
                    case 2:
                        aResult = await (from e in dbSales.SaleBasketTDES
                                         where (Guid)(Guid?)e.SpellGuid == (Guid)SpellGuid && e.IsPaid == false
                                         group e by new
                                         {
                                             e.SpellGuid,
                                             e.EntUserNic,
                                             e.WorkPlaceGuid,
                                             e.Description
                                         } into grpSpell
                                         select new SaleBasketReportTDES
                                         {
                                             SpellGuid = grpSpell.Key.SpellGuid,
                                             WorkPlaceGuid = grpSpell.Key.WorkPlaceGuid,
                                             Description = grpSpell.Key.Description,
                                             EntUserNic = grpSpell.Key.EntUserNic,
                                             PaymentSum = grpSpell.Sum((SaleBasketTDES ei) => ei.PaymentSum),
                                             ArtAmount = grpSpell.Sum((SaleBasketTDES ei) => ei.ArtAmount)
                                         }).ToListAsync();
                        break;
                    case 3:
                        aResult = await (from e in dbSales.SaleBasketTDES
                                         where (Guid)(Guid?)e.SpellGuid == (Guid)SpellGuid && e.IsPaid == true && e.IsReverse == true
                                         group e by new
                                         {
                                             e.SpellGuid,
                                             e.EntUserNic,
                                             e.WorkPlaceGuid,
                                             e.Description
                                         } into grpSpell
                                         select new SaleBasketReportTDES
                                         {
                                             SpellGuid = grpSpell.Key.SpellGuid,
                                             WorkPlaceGuid = grpSpell.Key.WorkPlaceGuid,
                                             Description = grpSpell.Key.Description,
                                             EntUserNic = grpSpell.Key.EntUserNic,
                                             PaymentSum = grpSpell.Sum((SaleBasketTDES ei) => ei.PaymentSum),
                                             ArtAmount = grpSpell.Sum((SaleBasketTDES ei) => ei.ArtAmount)
                                         }).ToListAsync();
                        break;
                    default:
                        aResult = await (from e in dbSales.SaleBasketArticleTDES
                                         where (Guid)(Guid?)e.SpellGuid == (Guid)SpellGuid && e.IsPaid == true && e.IsSpellClosed == true && e.ArtAmount <= e.ReverseAmount + e.CribFromIncome
                                         group e by new
                                         {
                                             e.SpellGuid,
                                             e.EntUserNic,
                                             e.WorkPlaceGuid
                                         } into grpSpell
                                         select new SaleBasketReportTDES
                                         {
                                             SpellGuid = grpSpell.Key.SpellGuid,
                                             WorkPlaceGuid = grpSpell.Key.WorkPlaceGuid,
                                             EntUserNic = grpSpell.Key.EntUserNic,
                                             PaymentSum = grpSpell.Sum((SaleBasketArticleTDES ei) => ei.SalePrice * (double)(ei.ArtAmount - ei.ReverseAmount)),
                                             ArtAmount = grpSpell.Sum((SaleBasketArticleTDES ei) => ei.ArtAmount - ei.ReverseAmount)
                                         }).ToListAsync();
                        break;
                }
            }
            return View(aResult);
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
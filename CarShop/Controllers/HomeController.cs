// CarShop.Controllers.HomeController
using CarShop.Models;
using CarShop.Properties;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CarShop.Controllers
{

    public class HomeController : Controller
    {
        private CarShopContext _db;

        private CarShopContext db
        {
            get
            {
                if (_db == null)
                {
                    _db = new CarShopContext();
                }
                return _db;
            }
        }

        protected void UserIsInRoles()
        {
            if (base.User != null)
            {
                base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
                base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
                base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
                base.ViewBag.IsBranchAdmin = base.User.IsInRole("BranchAdmin");
                base.ViewBag.IsBranchAudit = base.User.IsInRole("BranchAudit");
                base.ViewBag.IsBranchSeller = base.User.IsInRole("BranchSeller");
                base.ViewBag.IsBranchBooker = base.User.IsInRole("BranchBooker");
            }
            else
            {
                base.ViewBag.IsEcoSystemAdmin = false;
                base.ViewBag.IsEnterpriseAdmin = false;
                base.ViewBag.IsEnterpriseAudit = false;
                base.ViewBag.IsBranchAdmin = false;
                base.ViewBag.IsBranchAudit = false;
                base.ViewBag.IsBranchSeller = false;
                base.ViewBag.IsBranchBooker = false;
            }
            base.ViewBag.IsAutorise = (base.ViewBag.IsEcoSystemAdmin || base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit || base.ViewBag.IsBranchAdmin || base.ViewBag.IsBranchAudit || base.ViewBag.IsBranchSeller || base.ViewBag.IsBranchBooker);
        }

        public ActionResult Index(int? SearchKind = default(int?))
        {
            SearchKind = (SearchKind ?? 1);
            base.ViewBag.SearchKind = (SearchKind.Value == 1);
            base.ViewBag.slisrchTp = new SelectList(new SelectListItem[4]
            {
            new SelectListItem
            {
                Value = "0",
                Text = Resources.GUEST_SEARCH_SUPPID,
                Selected = true
            },
            new SelectListItem
            {
                Value = "1",
                Text = Resources.GUEST_SEARCH_TECDOCID,
                Selected = false
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.GUEST_MANUF_IDS_INDEX,
                Selected = false
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.GUEST_SEARCH_EAN,
                Selected = false
            }
            }, "Value", "Text", 0);
            UserIsInRoles();
            return View();
        }

        public ActionResult ToBusinessMan()
        {
            base.ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult ToExternals()
        {
            base.ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult IndexByRoles()
        {
            return View();
        }

        public ActionResult IndexByBooker()
        {
            return View();
        }

        public ActionResult IndexByBranchAdmin()
        {
            return View();
        }

        public ActionResult IndexByBranchAudit()
        {
            return View();
        }

        public ActionResult IndexByEnterpriseAdmin()
        {
            return View();
        }

        public ActionResult IndexByEnterpriseAudit()
        {
            return View();
        }

        public ActionResult IndexByEcoSystemAdmin()
        {
            return View();
        }

        public ActionResult WhyItsFree()
        {
            return View();
        }

        public ActionResult About()
        {
            base.ViewBag.Message = "Данное приложение призвано помочь как автолюбителям в поиске автозапчастей так и продавцам, желающим опубликовать в интернет свои товары и цены.";
            return View();
        }

        public ActionResult Contact()
        {
            base.ViewBag.Message = "Контактные данные.";
            return View();
        }

        public async Task<ActionResult> ByOriginalCatalogs()
        {
            return View(await db.OriginalCatalogTDES.ToListAsync());
        }

        public ActionResult OnLineCatalogs()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _db != null)
            {
                _db.Dispose();
                _db = null;
            }
            base.Dispose(disposing);
        }
    }
}
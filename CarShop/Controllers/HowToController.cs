// CarShop.Controllers.HowToController
using System.Web.Mvc;

namespace CarShop.Controllers
{

    public class HowToController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HowToMSSQL()
        {
            return View();
        }

        public ActionResult HowToMSSQLManagementStudio()
        {
            return View();
        }

        public ActionResult HowToWpfCarShop()
        {
            return View();
        }

        public FileResult WpfCarShop()
        {
            return File("~/App_Data/WpfCarShop.zip", "application/octet-stream", "WpfCarShop.zip");
        }

        public ActionResult WpfCarShopDict()
        {
            return View();
        }

        public ActionResult ODBCTecDoc()
        {
            return View();
        }

        public ActionResult Enterprise()
        {
            return View();
        }

        public ActionResult CarShopArticleContext()
        {
            return View();
        }

        public ActionResult InternetServer()
        {
            return View();
        }

        public ActionResult AspNetSecurity()
        {
            return View();
        }

        public ActionResult MsDts()
        {
            return View();
        }

        public ActionResult InstallonIIS()
        {
            return View();
        }

        public FileResult InstallCarShopZip()
        {
            return File("~/App_Data/CarShop.zip", "application/octet-stream", "CarShop.zip");
        }

        public ActionResult TecDocMsSql()
        {
            return View();
        }

        public FileResult TecDocMsSqlZip()
        {
            return File("~/App_Data/CarShopMsTecDoc.zip", "application/octet-stream", "CarShopMsTecDoc.zip");
        }

        public ActionResult FirewallSettings()
        {
            return View();
        }

        public ActionResult FirewallSettingsHttp()
        {
            return View();
        }

        public ActionResult FirstStartApp()
        {
            return View();
        }

        public ActionResult RegAdvanceOrders()
        {
            return View();
        }

        public ActionResult MinStepToGetWork()
        {
            return View();
        }

        public ActionResult IESettings()
        {
            return View();
        }

        public ActionResult CarShopRestToWeb()
        {
            return View();
        }

        public FileResult MySqlZip()
        {
            return File("~/App_Data/MySqlZip.zip", "application/octet-stream", "MySqlZip.zip");
        }

        public FileResult CarShopRestToWebZip()
        {
            return File("~/App_Data/CarShopRestToWeb.zip", "application/octet-stream", "CarShopRestToWeb.zip");
        }

        public FileResult MySqlZipDB()
        {
            return File("~/App_Data/MySqlZipDB.zip", "application/octet-stream", "MySqlZipDB.zip");
        }

        public FileResult CarShopArticle()
        {
            return File("~/App_Data/CarShopArticle.zip", "application/octet-stream", "CarShopArticle.zip");
        }

        public FileResult InstallCarShopAdminZip()
        {
            return File("~/App_Data/InstallCarShopAdminZip.zip", "application/octet-stream", "InstallCarShopAdminZip.zip");
        }

        public ActionResult Dounloads()
        {
            return View();
        }
    }

}
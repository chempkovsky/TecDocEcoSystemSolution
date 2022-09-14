// mvcForum.Web.Areas.Forum.Controllers.NoAccessController
using MVCThemes;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.Forum.Controllers
{

    [Themed]
    public class NoAccessController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

}
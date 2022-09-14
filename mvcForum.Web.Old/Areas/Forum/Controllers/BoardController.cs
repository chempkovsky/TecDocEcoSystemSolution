// mvcForum.Web.Areas.Forum.Controllers.BoardController
using MVCBootstrap.Web.Mvc;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Web.Helpers;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.Forum.Controllers
{

    public class BoardController : Controller
    {
        private readonly IConfiguration config;

        public BoardController(IConfiguration config)
        {
            this.config = config;
        }

        public ActionResult GimmeVersion()
        {
            return new JsonPResult(new
            {
                Version = ForumHelper.GetVersion()
            });
        }
    }

}
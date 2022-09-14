// mvcForum.Web.Areas.ForumAdmin.Controllers.TextsController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Web.Controllers;
using mvcForum.Web.Interfaces;
using SimpleLocalisation;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAdmin.Controllers
{

    public class TextsController : ForumBaseController
    {
        public TextsController(IWebUserProvider userProvider, IContext context)
            : base(userProvider, context)
        {
        }

        [OutputCache(Duration = 3600)]
        public ActionResult GetJSON()
        {
            string language = textManager.GetCurrentLanguage().Culture.Name;
            string str = $"\r\n// language = {language}\r\nfunction GetLocalizedText(key) {{ if (texts[key]) {{ return texts[key]; }} return key; }}\r\n";
            return Content(string.Format("var texts = {{ {0} }};", string.Join(",", from t in textManager.Texts
                                                                                    where t.Language == language
                                                                                    select $"{System.Web.Helpers.Json.Encode($"{t.Namespace}.{t.Key}")}: {System.Web.Helpers.Json.Encode(t.Pattern)}")) + str);
        }
    }

}
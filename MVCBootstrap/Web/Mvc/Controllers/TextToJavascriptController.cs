using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Text;

namespace MVCBootstrap.Web.Mvc.Controllers {

	public class TextToJavascriptController : LocalizedBaseController {

		[OutputCache(Duration = 3600)]
		public ActionResult Read() {
			String language = this.textManager.GetCurrentLanguage().Culture.Name;
			String output = String.Format(@"
// language = {0}
function GetLocalizedText(key) {{ if (texts[key]) {{ return texts[key]; }} return key; }}
", language);
			return Content(String.Format("var texts = {{ {0} }};", String.Join(",", this.textManager.Texts.Where(t => t.Language == language).Select(t => String.Format("{0}: {1}", System.Web.Helpers.Json.Encode(String.Format("{0}.{1}", t.Namespace, t.Key)), System.Web.Helpers.Json.Encode(t.Pattern)))))
				+ output
				);
		}
	}
}
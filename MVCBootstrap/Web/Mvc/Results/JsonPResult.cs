using System;
using System.Web.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MVCBootstrap.Web.Mvc {

	/// <summary>
	/// A Mvc ActionResult class that will return the result as JSONP.
	/// </summary>
	public class JsonPResult : ActionResult {
		private Object data { get; set; }

		public JsonPResult(Object data) {
			this.data = data;
		}

		public override void ExecuteResult(ControllerContext context) {
			context.HttpContext.Response.Write(String.Format("{0}({1});",
					context.HttpContext.Request["callback"],
					JsonConvert.SerializeObject(this.data, new IsoDateTimeConverter(), new StringEnumConverter())
				));
		}
	}
}
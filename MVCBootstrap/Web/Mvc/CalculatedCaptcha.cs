using System;
using System.Web;

namespace MVCBootstrap.Web.Mvc {

	public class CalculatedCaptcha {
		private String prefix;

		public CalculatedCaptcha() : this(String.Empty) { }

		public CalculatedCaptcha(String prefix) {
			this.prefix = prefix;
		}

		private String Key {
			get {
				return String.Format("{0}Captcha", prefix);
			}
		}

		public Boolean ValidResult(ViewModels.AlgebraCaptcha result) {
			if (!result.Result.HasValue || String.IsNullOrWhiteSpace(result.Prefix)) {
				// TODO: Fail somehow?
				return false;
			}
			this.prefix = result.Prefix;
			return this.ValidResult(result.Result.Value);
		}

		public Boolean ValidResult(Int32 result) {
			return (
					!String.IsNullOrWhiteSpace(this.Key) &&
					HttpContext.Current != null &&
					HttpContext.Current.Session != null &&
					HttpContext.Current.Session[this.Key] != null &&
					HttpContext.Current.Session[this.Key] is Int32 &&
					(Int32)HttpContext.Current.Session[this.Key] == result
				);
		}

		public void StoreResult(Int32 result) {
			HttpContext.Current.Session[this.Key] = result;
		}
	}
}
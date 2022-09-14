using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Helpers {

	public static class CultureInfoHelper {

		public static IEnumerable<SelectListItem> GetCultures(string selectedCulture) {
			List<SelectListItem> items = new List<SelectListItem>();
			IEnumerable<CultureInfo> cults = CultureInfo.GetCultures(CultureTypes.SpecificCultures).OrderBy(c => c.EnglishName);
			foreach (var cu in cults) {
				items.Add(new SelectListItem { Text = cu.DisplayName, Value = cu.Name, Selected = (cu.Name == selectedCulture) });
			}
			return items;
		}

		public static CultureInfo GetCultureInfo(String cultureInfoId) {
			return new CultureInfo(cultureInfoId);
		}
	}
}
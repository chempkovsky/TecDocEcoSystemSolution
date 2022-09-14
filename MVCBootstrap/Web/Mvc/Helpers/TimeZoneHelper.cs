using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Helpers {

	public static class TimeZoneHelper {

		public static IEnumerable<SelectListItem> GetTimeZones(String selectedTimeZone) {
			List<SelectListItem> items = new List<SelectListItem>();
			IEnumerable<TimeZoneInfo> infos = TimeZoneInfo.GetSystemTimeZones().OrderByDescending(t => t.BaseUtcOffset.TotalMinutes);
			foreach (var tzi in infos) {
				items.Add(new SelectListItem { Text = tzi.DisplayName, Value = tzi.Id, Selected = (tzi.Id == selectedTimeZone) });
			}
			return items;
		}
	}
}

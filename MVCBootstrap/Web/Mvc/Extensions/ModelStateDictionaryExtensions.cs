using System;
using System.Linq;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Extensions {

	public static class ModelStateDictionaryExtensions {

		/// <summary>
		/// Method for getting a complete string of all errors.
		/// </summary>
		/// <param name="state">The ModelState</param>
		/// <returns>A joint string of all errors found in the ModelState.</returns>
		public static String ErrorString(this ModelStateDictionary state) {
			return String.Join(", ", state.Values.SelectMany(v => v.Errors.Select(y => y.ErrorMessage)));
		}
	}
}
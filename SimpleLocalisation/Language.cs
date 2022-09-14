using System;
using System.Globalization;
using System.Collections.Generic;

namespace SimpleLocalisation {

	public class Language {
		private String Key { get; set; }
		private CultureInfo culture;

		public List<Language> Fallbacks { get; set; }

		public Language()
			: this(CultureInfo.CurrentUICulture.Name) {
		}

		public Language(String key) {
			try {
				this.culture = new CultureInfo(key);
			}
			catch { }
		}

		public CultureInfo Culture {
			get {
				return this.culture;
			}
		}
	}
}
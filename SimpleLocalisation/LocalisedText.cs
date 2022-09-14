using System;

namespace SimpleLocalisation {

	public class LocalisedText {
		public String Key { get; set; }
		public String Language { get; set; }
		public String Namespace { get; set; }
		public String PatternDialect { get; set; }
		public String Pattern { get; set; }

		public String UniqueKey { get { return this.Namespace + "__" + this.Key + "_" + this.Language; } }

		public LocalisedText() {
			PatternDialect = "Default";
		}
	}
}
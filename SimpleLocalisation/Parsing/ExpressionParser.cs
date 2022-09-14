using System;
using System.Globalization;
using System.IO;

namespace SimpleLocalisation.Parsing {

	public abstract class ExpressionParser {

		protected ExpressionParser() { }

		public CultureInfo PatternCulture = CultureInfo.GetCultureInfo("en-US");

		public virtual Expression Parse(String pattern/*, TextManager manager*/) {
			return Parse(new StringReader(pattern)/*, manager*/);
		}

		public abstract Expression Parse(TextReader reader/*, TextManager manager*/);
	}
}

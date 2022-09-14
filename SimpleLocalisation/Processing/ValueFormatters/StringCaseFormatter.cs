using System;
using System.Globalization;
using System.Linq;

namespace SimpleLocalisation.Processing.ValueFormatters {

	public class StringCaseFormatter : IValueFormatter {
		public StringCaseTransformationType TransformationType { get; set; }

		public string FormatValue(ParameterValue value, EvaluationContext context) {
			var s = "" + value.Value;
			if (!String.IsNullOrEmpty(s)) {
				switch (TransformationType) {
					case StringCaseTransformationType.Lowercase: return s.ToLower(context.Language.Culture);
					case StringCaseTransformationType.Uppercase: return s.ToUpper(context.Language.Culture);
					case StringCaseTransformationType.CapitalizeFirst: return Capitalize(s, context.Language.Culture);
					case StringCaseTransformationType.CapitalizeAll:
						return string.Join(" ",
							s.Split(' ').Select(w => Capitalize(w, context.Language.Culture)));
				}
			}

			return s;
		}

		String Capitalize(String s, CultureInfo culture) {
			return String.IsNullOrEmpty(s) ? s : Char.ToUpper(s[0], culture) + s.Substring(1).ToLower(culture);
		}
	}
}
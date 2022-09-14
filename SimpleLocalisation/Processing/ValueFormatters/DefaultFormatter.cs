using System;

namespace SimpleLocalisation.Processing.ValueFormatters {

	public class DefaultFormatter : IValueFormatter {
		public String FormatValue(ParameterValue value, EvaluationContext context) {
			if (value.DefaultFormat != null) {
				return value.DefaultFormat.FormatValue(value, context);
			}
			else {
				return String.Format(context.Language.Culture, "{0}", value.Value);
			}
		}
	}
}
using System;

namespace SimpleLocalisation.Processing.ValueFormatters {

	public class StringFormatFormatter : IValueFormatter {
		public string FormatExpression { get; private set; }

		public StringFormatFormatter(String formatExpression) {
			FormatExpression = formatExpression;
		}

		public string FormatValue(ParameterValue value, EvaluationContext context) {
			return String.Format(context.Language.Culture, "{0:" + FormatExpression + "}", value.Value);
		}
	}
}
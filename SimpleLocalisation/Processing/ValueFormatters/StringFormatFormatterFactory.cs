using System;

namespace SimpleLocalisation.Processing.ValueFormatters {

	public class StringFormatFormatterFactory : IValueFormatterFactory {

		public IValueFormatter GetFor(String formatExpression, PatternDialect dialect/*, TextManager manager*/) {
			if (!String.IsNullOrEmpty(formatExpression)) {
				return new StringFormatFormatter(formatExpression);
			}
			return null;
		}
	}
}
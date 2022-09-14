using System;

namespace SimpleLocalisation.Processing.ValueFormatters {

	public class RomanNumberFormatterFactory : IValueFormatterFactory {

		public IValueFormatter GetFor(String formatExpression, PatternDialect dialect/*, TextManager manager*/) {
			if (!String.IsNullOrEmpty(formatExpression) && formatExpression.Equals("roman", StringComparison.InvariantCulture)) {
				return new RomanNumberFormatter();
			}

			return null;
		}
	}
}
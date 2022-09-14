using System;

namespace SimpleLocalisation.Processing.ValueFormatters {

	public class DefaultFormatterFactory : IValueFormatterFactory {

		public IValueFormatter GetFor(String rep, PatternDialect dialect/*, TextManager manager*/) {
			return new DefaultFormatter();
		}
	}
}
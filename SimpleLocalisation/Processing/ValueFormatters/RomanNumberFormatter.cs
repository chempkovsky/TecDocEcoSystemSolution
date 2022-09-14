using System;

namespace SimpleLocalisation.Processing.ValueFormatters {

	public class RomanNumberFormatter : IValueFormatter {
		private NumberToRomanConvertor _converter;
		public RomanNumberFormatter() {
			_converter = new NumberToRomanConvertor();
		}

		public String FormatValue(ParameterValue value, EvaluationContext context) {
			return _converter.NumberToRoman((Int32)Convert.ChangeType(value.Value, typeof(Int32)));
		}
	}
}
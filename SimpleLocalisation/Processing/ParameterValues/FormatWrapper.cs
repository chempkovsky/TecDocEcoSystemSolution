using System;

namespace SimpleLocalisation.Processing.ParameterValues {

	public class FormatWrapper : FormatWrapper<Object> {

		public FormatWrapper(Object value, String formatExpression)
			: base(value, formatExpression) {
		}
	}

	public class FormatWrapper<TValue> : ParameterValue<TValue> {
		public string FormatExpression { get; set; }

		/// <summary>
		/// Creates a new FormatWrapper for the value specified using the formatExpression
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="formatExpression">The format expression. Wraps format around the value. {#} will be replaced with the actual value</param>
		public FormatWrapper(TValue value, String formatExpression)
			: base(value) {
			FormatExpression = formatExpression;
		}

		public override string Format(Func<String, String> stringEncoder, String formattedValue) {
			return FormatExpression.Replace("{#}", stringEncoder(formattedValue));
		}

		public static implicit operator TValue(FormatWrapper<TValue> wrapper) {
			return (TValue)wrapper.Value;
		}
	}
}
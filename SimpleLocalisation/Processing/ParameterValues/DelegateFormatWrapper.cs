using System;

namespace SimpleLocalisation.Processing.ParameterValues {

	public class DelegateFormatWrapper : DelegateFormatWrapper<Object, Object> {
	
		public DelegateFormatWrapper(Object value, Func<DelegateValueFormatArgs<Object, Object>, Object> formatter)
			: base(value, value, formatter) {
		}
	}

	public class DelegateFormatWrapper<TValue, TReference> : ParameterValue<TValue> {

		/// <summary>
		/// Gets or sets the object reference that will be passed to the formatting delegate.
		/// </summary>
		/// <value>
		/// The delegate reference.
		/// </value>
		public TReference DelegateReference { get; set; }

		public Func<DelegateValueFormatArgs<TValue, TReference>, Object> FormatDelegate { get; set; }

		public DelegateFormatWrapper(TValue value, TReference reference, Func<DelegateValueFormatArgs<TValue, TReference>, Object> formatter)
			: base(value) {
			FormatDelegate = formatter;
			DelegateReference = reference;
		}

		public override string Format(Func<String, String> stringEncoder, String formattedValue) {
			return "" + FormatDelegate(new DelegateValueFormatArgs<TValue, TReference> {
				Value = TypedValue,
				Reference = DelegateReference,
				FormattedValue = formattedValue,
				Encoder = stringEncoder
			});
		}
	}

	public class DelegateValueFormatArgs<TValue, TReference> {
		public TValue Value { get; set; }
		public TReference Reference { get; set; }
		public String FormattedValue { get; set; }
		public Func<String, String> Encoder { get; set; }
	}
}
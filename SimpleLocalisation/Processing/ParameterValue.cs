using System;

namespace SimpleLocalisation.Processing {

	public class ParameterValue {
		/// <summary>
		/// The actual value.
		/// </summary>
		public Object Value { get; set; }

		/// <summary>
		/// Default format if nothing is specified
		/// </summary>
		public IValueFormatter DefaultFormat { get; set; }

		public ParameterValue(Object value) {
			var other = value as ParameterValue;
			if (other != null) {
				Value = other.Value;
				DefaultFormat = other.DefaultFormat;
			}
			else {
				Value = value;
			}
		}

		public virtual string Format(Func<String, String> stringEncoder, String formattedValue) {
			return stringEncoder(formattedValue);
		}

		public virtual ParameterValue Clone() {
			return (ParameterValue)this.MemberwiseClone();
		}

		public override String ToString() {
			return Format(x => x, "" + Value);
		}

		/// <summary>
		/// Wraps the specified value if its not a <see cref="ParameterValue"/> already.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>        
		public static ParameterValue Wrap(Object value) {
			var pv = value as ParameterValue;
			return pv ?? new ParameterValue(value);
		}
	}

	public class ParameterValue<TValue> : ParameterValue {

		public TValue TypedValue {
			get { return (TValue)Value; }
			set { Value = value; }
		}

		public ParameterValue(TValue value)
			: base(value) { }

		public static implicit operator TValue(ParameterValue<TValue> pv) {
			return pv.TypedValue;
		}
	}
}
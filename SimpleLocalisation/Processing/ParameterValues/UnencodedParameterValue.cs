using System;

namespace SimpleLocalisation.Processing.ParameterValues {

	public class UnencodedParameterValue : ParameterValue {
		public Object Value { get; set; }

		public UnencodedParameterValue(Object value)
			: base(value) {
		}

		public override String Format(Func<String, String> stringEncoder, String formattedValue) {
			return formattedValue;
		}
	}
}
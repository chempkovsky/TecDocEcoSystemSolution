using System;
using System.Collections.Generic;
using SimpleLocalisation.Processing;

namespace SimpleLocalisation {

	public class DicitionaryParameterSet : ParameterSet {
		private IDictionary<String, ParameterValue> _values;

		public DicitionaryParameterSet() {
			_values = new Dictionary<String, ParameterValue>();
		}

		public DicitionaryParameterSet(IDictionary<String, Object> values)
			: this() {
			foreach (var val in values) {
				SetObject(val.Key, val.Value);
			}
		}

		public override IEnumerable<String> Keys {
			get { return _values.Keys; }
		}

		public override Boolean Contains(String key) {
			return _values.ContainsKey(key);
		}

		protected override ParameterValue GetInternal(String key) {
			ParameterValue val;
			return _values.TryGetValue(key, out val) ? val : null;
		}

		protected override void SetInternal(String key, ParameterValue value) {
			_values[key] = value;
		}

		public static implicit operator DicitionaryParameterSet(Dictionary<String, Object> dict) {
			return new DicitionaryParameterSet(dict);
		}
	}
}
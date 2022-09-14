using System;
using System.Collections.Generic;
using SimpleLocalisation.Processing;

namespace SimpleLocalisation {

	public abstract class ParameterSet {

		public ParameterValue this[String key] {
			get {
				key = key.ToLowerInvariant();
				var val = GetInternal(key);
				return val ?? ParameterValue.Wrap(null);
			}
			set {
				key = key.ToLowerInvariant();
				SetInternal(key, value);
			}
		}

		public Object GetObject(String key) {
			var val = this[key];
			return val != null ? val.Value : null;
		}

		public void SetObject(String key, Object value) {
			this[key] = ParameterValue.Wrap(value);
		}

		public abstract IEnumerable<String> Keys { get; }

		public abstract Boolean Contains(String key);
		protected abstract ParameterValue GetInternal(String key);
		protected abstract void SetInternal(String key, ParameterValue value);
	}
}
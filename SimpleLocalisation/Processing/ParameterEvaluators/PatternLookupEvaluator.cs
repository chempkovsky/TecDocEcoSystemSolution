using System;
using System.Collections.Generic;
using SimpleLocalisation.Processing.ParameterValues;

namespace SimpleLocalisation.Processing.ParameterEvaluators {

	public class PatternLookupEvaluator : IParameterEvaluator {
		public String PatternKey { get; set; }

		/*public TextManager Manager { get; set; }*/

		public KeyValuePair<String, IValueFormatter>[] Parameters { get; set; }

		public String NamespaceQualifier { get; set; }


		public PatternLookupEvaluator() {
			NamespaceQualifier = "__";
		}

		//If another namespace is needed for the pattern lookup it can be written as Namespace__Key, i.e. namespace and key seperated by two underscores '_'
		public ParameterValue GetValue(EvaluationContext context) {
			var values = context.Parameters;


			string actualPatternKey;
			if (PatternKey.StartsWith("@")) {
				var parts = PatternKey.Substring(1).Split('+');
				actualPatternKey = (string)values.GetObject(parts[0]);
				if (parts.Length > 1) {
					actualPatternKey += parts[1];
				}
			}
			else {
				actualPatternKey = PatternKey;
			}

			string ns = context.Namespace;

			int ix = actualPatternKey.IndexOf(NamespaceQualifier);
			if (ix != -1) {
				ns = actualPatternKey.Substring(0, ix);
				actualPatternKey = actualPatternKey.Substring(ix + NamespaceQualifier.Length);
			}

			if (actualPatternKey != null) {
				int i = 0;
				var callValues = new DicitionaryParameterSet();
				foreach (var p in Parameters) {

					if ((p.Key.StartsWith("\"") || p.Key.StartsWith("'")) && (p.Key.EndsWith("\"") || p.Key.EndsWith("'"))) {
						callValues["" + i] = ParameterValue.Wrap(p.Key.Substring(1, p.Key.Length - 2));
					}
					else {
						ParameterValue v = null;
						if (p.Value != null) {
							v = values[p.Key].Clone();
							v.DefaultFormat = p.Value;
						}
						else {
							v = values[p.Key];
						}

						callValues[p.Key] = v;
						callValues["" + i] = v;
					}
					++i;
				}

				return new UnencodedParameterValue("tododododod");

				//return new UnencodedParameterValue(Manager.Get(actualPatternKey, callValues,
				//    ns: ns, language: context.Language, returnNullOnMissing: true));
			}
			else {
				//The pattern key was to be looked up and wasn't in the provided values. Return null
				return null;
			}
		}
	}
}
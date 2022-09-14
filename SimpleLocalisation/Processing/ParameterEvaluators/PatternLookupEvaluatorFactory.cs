using System;
using System.Collections.Generic;
using System.Linq;
using SimpleLocalisation.Parsing;

namespace SimpleLocalisation.Processing.ParameterEvaluators {

	public class PatternLookupEvaluatorFactory : IParameterEvaluatorFactory {

		public IParameterEvaluator GetFor(ParameterSpec spec, PatternDialect dialect/*, TextManager manager*/) {

			if (spec.ParameterName.StartsWith("@")) {
				return new PatternLookupEvaluator {
					//Manager = manager,
					PatternKey = spec.ParameterName.Substring(1),
					Parameters = spec.Arguments != null ? spec.Arguments.Split(',')
						.Select(x => {
							var parts = x.Trim().Split(':');
							if (parts.Length > 1) {
								return new KeyValuePair<string, IValueFormatter>(parts[0], dialect.GetValueFormatter(parts[1]/*, manager*/));
							}
							else {
								return new KeyValuePair<String, IValueFormatter>(parts[0], null);
							}
						}).ToArray() : new KeyValuePair<String, IValueFormatter>[0]
				};
			}

			return null;
		}
	}
}
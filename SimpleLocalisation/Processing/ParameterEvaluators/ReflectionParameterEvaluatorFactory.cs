using System;
using System.Linq;
using SimpleLocalisation.Parsing;

namespace SimpleLocalisation.Processing.ParameterEvaluators {

	public class ReflectionParameterEvaluatorFactory : IParameterEvaluatorFactory {

		public IParameterEvaluator GetFor(ParameterSpec spec, PatternDialect dialect/*, TextManager manager*/) {
			var parts = spec.ParameterName.Split('.');
			if (parts.Length > 1) {
				return new ReflectionParameterEvaluator(parts[0], parts.Skip(1).ToArray());
			}

			return null;
		}
	}
}
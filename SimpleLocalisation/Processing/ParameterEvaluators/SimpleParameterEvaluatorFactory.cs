using System;
using SimpleLocalisation.Parsing;

namespace SimpleLocalisation.Processing.ParameterEvaluators {

	public class SimpleParameterEvaluatorFactory : IParameterEvaluatorFactory {

		public IParameterEvaluator GetFor(ParameterSpec spelling, PatternDialect dialect/*, TextManager manager*/) {
			return new SimpleParameterEvaluator(spelling.ParameterName);
		}
	}
}
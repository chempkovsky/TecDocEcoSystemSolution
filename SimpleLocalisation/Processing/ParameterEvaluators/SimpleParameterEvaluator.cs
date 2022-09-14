using System;

namespace SimpleLocalisation.Processing.ParameterEvaluators {

	public class SimpleParameterEvaluator : IParameterEvaluator {
		public String ParameterName { get; private set; }

		public SimpleParameterEvaluator(String parameterName) {
			ParameterName = parameterName;
		}

		public ParameterValue GetValue(EvaluationContext context) {
			return context.Parameters[ParameterName];
		}
	}
}
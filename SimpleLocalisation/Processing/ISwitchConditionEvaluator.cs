using System;

namespace SimpleLocalisation.Processing {

	public interface ISwitchConditionEvaluator {
		Boolean Evaluate(ParameterValue o, EvaluationContext context);
	}
}
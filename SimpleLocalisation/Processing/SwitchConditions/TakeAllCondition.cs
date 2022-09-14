using System;

namespace SimpleLocalisation.Processing.SwitchConditions {

	public class TakeAllCondition : ISwitchConditionEvaluator {

		public Boolean Evaluate(ParameterValue val, EvaluationContext context) {
			return true;
		}
	}
}

using System;

namespace SimpleLocalisation.Processing.SwitchConditions {

	public class BooleanExpressionCondition : ISwitchConditionEvaluator {
		public ISwitchConditionEvaluator Left { get; set; }
		public ISwitchConditionEvaluator Right { get; set; }

		public bool Disjunction = false;

		public bool Evaluate(ParameterValue o, EvaluationContext context) {
			if (Disjunction) {
				return Left.Evaluate(o, context) || Right.Evaluate(o, context);
			}
			else {
				return Left.Evaluate(o, context) && Right.Evaluate(o, context);
			}
		}
	}
}
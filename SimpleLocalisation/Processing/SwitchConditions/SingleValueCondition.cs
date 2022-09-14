using System;

namespace SimpleLocalisation.Processing.SwitchConditions {

	public class SingleValueCondition<TValue> : ISwitchConditionEvaluator {
		public Boolean NotEquals { get; set; }

		public TValue Value { get; private set; }

		public SingleValueCondition(TValue value, Boolean notEquals) {
			Value = value;
			NotEquals = notEquals;
		}

		public Boolean Evaluate(ParameterValue val, EvaluationContext context) {
			Boolean success = Value.Equals(Convert.ChangeType(val.Value, typeof(TValue)));
			return NotEquals ? !success : success;
		}
	}
}
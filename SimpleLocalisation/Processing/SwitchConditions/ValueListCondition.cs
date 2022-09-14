using System;
using System.Collections.Generic;

namespace SimpleLocalisation.Processing.SwitchConditions {

	public class ValueListCondition<TValues> : ISwitchConditionEvaluator {
		public Boolean NotEquals { get; set; }

		public HashSet<TValues> Values { get; private set; }

		public ValueListCondition(IEnumerable<TValues> values, Boolean notEquals) {
			Values = new HashSet<TValues>(values);
			NotEquals = notEquals;
		}

		public Boolean Evaluate(ParameterValue val, EvaluationContext context) {
			try {
				var success = Values.Contains((TValues)Convert.ChangeType(val.Value, typeof(TValues)));
				if (NotEquals) success = !success;
				return success;
			}
			catch {
				return false;
			}
		}
	}
}
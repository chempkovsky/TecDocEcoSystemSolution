using System;

namespace SimpleLocalisation.Processing.SwitchConditions {

	public class TakeAllConditionFactory : StringBasedSwitchConditionEvaluatorFactory {

		public override ISwitchConditionEvaluator GetFor(String spelling, PatternDialect dialect/*, TextManager manager*/) {
			if (String.IsNullOrEmpty(spelling) || spelling.Equals("true", StringComparison.InvariantCultureIgnoreCase)) {
				return new TakeAllCondition();
			}

			return null;
		}
	}
}
using System;
using System.Linq;

namespace SimpleLocalisation.Processing.SwitchConditions {

	public class ValueListConditionFactory : StringBasedSwitchConditionEvaluatorFactory {

		public override ISwitchConditionEvaluator GetFor(String spelling, PatternDialect dialect/*, TextManager manager*/) {
			if (!String.IsNullOrEmpty(spelling)) {
				Boolean notEquals;
				if (spelling.StartsWith("!=")) {
					notEquals = true;
					spelling = spelling.Substring(2).TrimStart();
				}
				else {
					notEquals = false;
					if (spelling.StartsWith("=")) {
						spelling = spelling.Substring(1).TrimStart();
					}
				}

				var labels = spelling.Split(',').Select(x => x.Trim()).Where(x => x != "").ToArray();

				//If all the labels can be converted to a number (i.e. a double) it's better to represent them as that instead of doing string conversions
				Boolean allDouble = true;
				Double d;
				Double[] dvals = labels.Select(x => (allDouble = Double.TryParse(x, out d)) ? d : 0).ToArray();
				if (allDouble) {
					return dvals.Length == 1 ? (ISwitchConditionEvaluator)new SingleValueCondition<Double>(dvals[0], notEquals)
						: new ValueListCondition<Double>(dvals, notEquals);
				}
				else {
					return dvals.Length == 1 ? (ISwitchConditionEvaluator)new SingleValueCondition<String>(labels[0], notEquals)
						: new ValueListCondition<String>(labels, notEquals);
				}
			}

			return null;
		}
	}
}
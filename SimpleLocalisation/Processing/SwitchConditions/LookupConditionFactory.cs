using System;
using SimpleLocalisation.Processing.ParameterEvaluators;
using SimpleLocalisation.Parsing;

namespace SimpleLocalisation.Processing.SwitchConditions {

	public class LookupConditionFactory : StringBasedSwitchConditionEvaluatorFactory {
		public override ISwitchConditionEvaluator GetFor(string spelling, PatternDialect dialect/*, TextManager manager*/) {
			if (spelling.StartsWith("@")) {
				var evaluator = dialect.GetParameterEvaluator(
					new ParameterSpec { ParameterName = spelling }/*, manager*/) as PatternLookupEvaluator;
				if (evaluator != null) {
					return new LookupCondition {
						Dialect = dialect,
						Evaluator = evaluator
					};
				}
			}

			return null;
		}
	}
}
using System;
using SimpleLocalisation.Processing.ParameterEvaluators;
using SimpleLocalisation.Parsing;

namespace SimpleLocalisation.Processing.SwitchConditions {

	public class LookupCondition : ISwitchConditionEvaluator {

		public PatternDialect Dialect { get; set; }

		public PatternLookupEvaluator Evaluator { get; set; }

		public bool Evaluate(ParameterValue o, EvaluationContext context) {
			var pattern = (string)Evaluator.GetValue(context).Value;

			if (!string.IsNullOrEmpty(pattern)) {
				return Dialect.GetSwitchConditionEvaluator(Expression.Text(pattern)/*, Evaluator.Manager*/).Evaluate(o, context);
			}
			else {
				throw new ApplicationException(
					"Exceptions.LookupConditionParameterNotResolved");
			}
		}
	}
}
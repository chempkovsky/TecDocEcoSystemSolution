using System;
using SimpleLocalisation.Parsing;

namespace SimpleLocalisation.Processing {

	public abstract class StringBasedSwitchConditionEvaluatorFactory : ISwitchConditionEvaluatorFactory {

		public abstract ISwitchConditionEvaluator GetFor(String spelling, PatternDialect dialect/*, TextManager manager*/);

		public ISwitchConditionEvaluator GetFor(Expression rep, PatternDialect dialect/*, TextManager manager*/) {
			if (rep == null) {
				return GetFor("", dialect/*, manager*/);
			}
			else if (rep.Parts.Count == 1) {
				var text = rep.Parts[0] as Text;
				if (text != null) {
					return GetFor(text.Spelling.Trim(), dialect/*, manager*/);
				}
			}

			//Unsupported expression
			return null;
		}
	}
}
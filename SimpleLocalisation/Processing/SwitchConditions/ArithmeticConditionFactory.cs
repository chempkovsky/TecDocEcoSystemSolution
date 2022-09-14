using System;
using System.Text.RegularExpressions;
using SimpleLocalisation.Support;
using System.Collections.Generic;

namespace SimpleLocalisation.Processing.SwitchConditions {

	public class ArithmeticConditionFactory : StringBasedSwitchConditionEvaluatorFactory {

		static Regex operation = new Regex(String.Format(@"\s* (?<Op>{0}) \s* (?<Number>\d+)", Arithmetic.ArithemticOperatorRegex),
			RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

		static Regex matcher = new Regex(String.Format(
				@"(?<Ops> (\s* {0} \s* \d+)+) \s* (?<Op>{1}) \s* (?<TargetValue>\d+)",
					Arithmetic.ArithemticOperatorRegex,
					Arithmetic.CompareOperatorRegex), RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

		public override ISwitchConditionEvaluator GetFor(String spelling, PatternDialect dialect/*, TextManager manager*/) {
			Match m;
			if ((m = matcher.Match(spelling)).Success) {
				var ops = new List<ArithmeticCondition.Operation>();
				foreach (Match op in operation.Matches(m.Groups["Ops"].Value)) {
					ops.Add(new ArithmeticCondition.Operation {
						Number = double.Parse(op.Groups["Number"].Value, dialect.Parser.PatternCulture),
						Operator = Arithmetic.GetArithmeticOperator(op.Groups["Op"].Value)
					});
				}

				return new ArithmeticCondition {
					Operations = ops,
					CompareOperator = Arithmetic.GetCompareOperator(m.Groups["Op"].Value),
					TargetValue = int.Parse(m.Groups["TargetValue"].Value)
				};
			}

			return null;
		}
	}
}
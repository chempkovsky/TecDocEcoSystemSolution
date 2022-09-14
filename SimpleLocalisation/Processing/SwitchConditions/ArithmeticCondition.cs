using System;
using System.Collections.Generic;
using SimpleLocalisation.Support;

namespace SimpleLocalisation.Processing.SwitchConditions {

	public class ArithmeticCondition : ISwitchConditionEvaluator {
		public class Operation {
			public Double Number { get; set; }
			public ArithmeticOperator Operator { get; set; }
		}

		public List<Operation> Operations { get; set; }

		public Double TargetValue { get; set; }

		public CompareOperator CompareOperator { get; set; }

		public ArithmeticOperator ArithmeticOperator { get; set; }

		public Boolean Evaluate(ParameterValue o, EvaluationContext context) {
			try {
				Double n = Convert.ToDouble(o.Value);
				foreach (var rhs in Operations) {
					n = n.Evaluate(rhs.Number, rhs.Operator);
				}

				return n.CompareTo(TargetValue, CompareOperator);
			}
			catch {
				return false;
			}
		}

		public enum Comparer { Lt, Gt, Lte, Gte, Eq, Neq };
	}
}
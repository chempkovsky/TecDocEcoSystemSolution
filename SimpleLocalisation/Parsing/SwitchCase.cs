using System;
using SimpleLocalisation.Processing;

namespace SimpleLocalisation.Parsing {

	public class SwitchCase : PatternPart {
		public Expression Condition;
		public Expression Expression;

		public ISwitchConditionEvaluator Evaluator;

		public override void Accept<T>(IPatternVisitor<T> visitor, T state) {
			visitor.Visit(this, state);
		}
	}
}
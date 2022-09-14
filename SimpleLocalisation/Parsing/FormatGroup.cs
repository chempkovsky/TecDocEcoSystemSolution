using System;
using SimpleLocalisation.Processing;

namespace SimpleLocalisation.Parsing {

	public class FormatGroup : ExpressionPart {
		public string ParameterName;
		public Expression Expression;

		public override void Accept<T>(IPatternVisitor<T> visitor, T state) {
			visitor.Visit(this, state);
		}

		public IParameterEvaluator Evaluator;

		public IFormatGroupExpander Expander;
	}
}
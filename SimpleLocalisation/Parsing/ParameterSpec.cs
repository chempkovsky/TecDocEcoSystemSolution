using System;
using SimpleLocalisation.Processing;

namespace SimpleLocalisation.Parsing {

	public class ParameterSpec : ExpressionPart {
		public string ParameterName;
		public string ParameterFormat;

		public string Arguments;

		public override void Accept<T>(IPatternVisitor<T> visitor, T state) {
			visitor.Visit(this, state);
		}

		public IValueFormatter Formatter;
		public IParameterEvaluator Evaluator;
	}
}
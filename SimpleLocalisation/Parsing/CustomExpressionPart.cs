using System;
using System.IO;
using SimpleLocalisation.Processing;

namespace SimpleLocalisation.Parsing {

	public abstract class CustomExpressionPart : ExpressionPart {

		public override void Accept<T>(IPatternVisitor<T> visitor, T state) {
			visitor.Visit(this, state);
		}

		/// <summary>
		/// Override this method to use the build in pattern decorator
		/// </summary>        
		public virtual void Decorate(PatternDialect dialect/*, TextManager manager*/) { }

		/// <summary>
		/// Override this method to use the build in pattern evaluator
		/// </summary>        
		public virtual void Evaluate(EvaluationContext context, TextWriter writer) { }

		public override string ToString() {
			return "CustomExpressionPart";
		}
	}
}
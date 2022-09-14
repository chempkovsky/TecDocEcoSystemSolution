using System;

namespace SimpleLocalisation.Parsing {

	public class Text : ExpressionPart {
		public string Spelling;

		public override void Accept<T>(IPatternVisitor<T> visitor, T state) {
			visitor.Visit(this, state);
		}
	}
}

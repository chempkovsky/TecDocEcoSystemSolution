using System;
using System.Collections.Generic;

namespace SimpleLocalisation.Parsing {

	public class Expression : PatternPart {
		public List<ExpressionPart> Parts = new List<ExpressionPart>();

		public override void Accept<T>(IPatternVisitor<T> visitor, T state) {
			visitor.Visit(this, state);
		}

		public static Expression Text(string spelling) {
			return new Expression { Parts = new List<ExpressionPart> { new Text { Spelling = spelling } } };
		}
	}
}
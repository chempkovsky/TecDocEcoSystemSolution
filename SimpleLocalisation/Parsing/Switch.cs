using System;
using System.Collections.Generic;

namespace SimpleLocalisation.Parsing {

	public class Switch : ParameterSpec {
		public string SwitchTemplateName { get; set; }

		public List<SwitchCase> Cases = new List<SwitchCase>();
		public Expression NullExpression;

		public override void Accept<T>(IPatternVisitor<T> visitor, T state) {
			visitor.Visit(this, state);
		}
	}
}
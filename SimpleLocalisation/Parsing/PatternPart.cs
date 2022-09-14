using System;

namespace SimpleLocalisation.Parsing {

	public abstract class PatternPart {
		public abstract void Accept<T>(IPatternVisitor<T> visitor, T state);

		public virtual void Accept<T>(IPatternVisitor<T> visitor) {
			Accept<T>(visitor, visitor.CreateInitialState());
		}

		public override String ToString() {
			var printer = new PatternPartPrinter();
			this.Accept(printer);

			return printer.ToString();
		}
	}
}
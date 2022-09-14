using System;
using SimpleLocalisation.Parsing;

namespace SimpleLocalisation.Processing {

	public class TextDialect : PatternDialect {

		public TextDialect() {
			Parser = new TextParser();
			Encode = false;
		}

		private Object _parseLock = new object();

		public override PatternEvaluator GetEvaluator(String pattern) {
			lock (_parseLock) {
				var expr = Parser.Parse(pattern);

				return new PatternEvaluator(expr);
			}
		}
	}
}
using System;
using SimpleLocalisation.Processing;

namespace SimpleLocalisation {

	public class LocalisedTextEntry {
		public LocalisedText Text;
		public PatternDialect PatternDialect { get; set; }
		public PatternEvaluator Evaluator;
	}
}
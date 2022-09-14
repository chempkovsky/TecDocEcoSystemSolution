using System;

namespace SimpleLocalisation.Processing {

	public class EvaluationContext {

		public Language Language { get; set; }

		public TimeZoneInfo TimeZoneInfo { get; set; }

		public String Namespace { get; set; }

		public ParameterSet Parameters { get; set; }

		public Func<String, String> StringEncoder { get; set; }

		public EvaluationContext Clone() {
			return (EvaluationContext)this.MemberwiseClone();
		}
	}
}
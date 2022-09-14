using System;
using SimpleLocalisation.Support;

namespace SimpleLocalisation.Processing.ParameterEvaluators {

	public class ReflectionParameterEvaluator : IParameterEvaluator {
		public String BaseParameterName { get; private set; }
		public String[] Properties { get; set; }

		public ReflectionParameterEvaluator(String parameterName, String[] properties) {
			BaseParameterName = parameterName;
			Properties = properties;
		}

		public ParameterValue GetValue(EvaluationContext context) {
			object val = context.Parameters.GetObject(BaseParameterName);
			if (val != null) {
				foreach (var prop in Properties) {
					val = ObjectHelper.Eval(val, prop);
				}
			}
			return ParameterValue.Wrap(val);
		}
	}
}
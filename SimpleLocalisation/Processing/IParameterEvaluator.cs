using System;

namespace SimpleLocalisation.Processing {

	public interface IParameterEvaluator {
		ParameterValue GetValue(EvaluationContext context);
	}
}
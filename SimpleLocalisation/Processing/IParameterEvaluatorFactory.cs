using System;
using SimpleLocalisation.Parsing;

namespace SimpleLocalisation.Processing {

	public interface IParameterEvaluatorFactory : IPatternProcessorFactory<IParameterEvaluator, ParameterSpec> {
	}
}
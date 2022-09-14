using System;
using SimpleLocalisation.Parsing;

namespace SimpleLocalisation.Processing {

	public interface ISwitchConditionEvaluatorFactory : IPatternProcessorFactory<ISwitchConditionEvaluator, Expression> {
	}
}
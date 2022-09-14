using System;

namespace SimpleLocalisation.Processing {

	public interface IValueFormatter {
		String FormatValue(ParameterValue value, EvaluationContext context);
	}
}
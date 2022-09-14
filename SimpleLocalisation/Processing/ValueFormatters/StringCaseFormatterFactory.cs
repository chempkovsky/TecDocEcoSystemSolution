using System;
using System.Collections.Generic;

namespace SimpleLocalisation.Processing.ValueFormatters {

	public class StringCaseFormatterFactory : IValueFormatterFactory {
		static Dictionary<String, StringCaseTransformationType> _transformationTypes = new Dictionary<String, StringCaseTransformationType>
        {
            {"lc", StringCaseTransformationType.Lowercase},
            {"lowercase", StringCaseTransformationType.Lowercase},
            {"uc", StringCaseTransformationType.Uppercase},
            {"uppercase", StringCaseTransformationType.Uppercase},
            {"cf", StringCaseTransformationType.CapitalizeFirst},
            {"capitalize-first", StringCaseTransformationType.CapitalizeFirst},
            {"ca", StringCaseTransformationType.CapitalizeAll},
            {"capitalize-all", StringCaseTransformationType.CapitalizeAll}
        };


		public IValueFormatter GetFor(String formatExpression, PatternDialect dialect/*, TextManager manager*/) {
			StringCaseTransformationType type;
			if (!string.IsNullOrEmpty(formatExpression)
				&& _transformationTypes.TryGetValue(formatExpression.ToLowerInvariant(), out type)) {
				return new StringCaseFormatter { TransformationType = type };
			}

			return null;
		}
	}
}
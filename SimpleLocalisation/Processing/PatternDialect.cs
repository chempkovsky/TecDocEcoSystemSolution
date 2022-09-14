using System;
using System.Collections.Generic;
using System.Linq;
using SimpleLocalisation.Parsing;

namespace SimpleLocalisation.Processing {

	public abstract class PatternDialect {
		public Boolean Encode { get; protected set; }

		protected PatternDialect() {
			Encode = true;
		}

		/// <summary>
		/// Gets or sets a pattern transformer to make simple transformations to the dialect's grammar. (See e.g. <see cref="HHtmlPatternTransformer"/>)
		/// </summary>
		/// <value>
		/// The pattern transformer.
		/// </value>
		public IPatternTransformer PatternTransformer { get; set; }

		public ExpressionParser Parser { get; protected set; }

		public List<IValueFormatterFactory> ValueFormatters { get; set; }
		public List<IParameterEvaluatorFactory> ParameterEvaluators { get; set; }
		public List<ISwitchConditionEvaluatorFactory> SwitchConditionEvaluators { get; set; }

		//TODO: This ought to follow the same structure as valueformatters, parameter evaluators etc.
		public IFormatGroupExpander FormatGroupExpander { get; set; }


		public virtual IParameterEvaluator GetParameterEvaluator(ParameterSpec spec/*, TextManager manager*/) {
			IParameterEvaluator evaluator = null;
			if (!ParameterEvaluators.Any(x => (evaluator = x.GetFor(spec, this/*, manager*/)) != null)) {
				throw new KeyNotFoundException("Exceptions.ParameterEvaluatorNotFound");
			}

			return evaluator;
		}

		public virtual IValueFormatter GetValueFormatter(String spelling/*, TextManager manager*/) {
			IValueFormatter formatter = null;
			if (!ValueFormatters.Any(x => (formatter = x.GetFor(spelling, this/*, manager*/)) != null)) {
				throw new KeyNotFoundException("Exceptions.ValueFormatterNotFound");
			}

			return formatter;
		}

		public virtual ISwitchConditionEvaluator GetSwitchConditionEvaluator(Expression expr/*, TextManager manager*/) {
			ISwitchConditionEvaluator sc = null;
			if (!SwitchConditionEvaluators.Any(x => (sc = x.GetFor(expr, this/*, manager*/)) != null)) {
				throw new KeyNotFoundException("Exceptions.SwitchConditionNotFound");
			}
			return sc;
		}

		public abstract PatternEvaluator GetEvaluator(String pattern/*, TextManager manager*/);
	}
}
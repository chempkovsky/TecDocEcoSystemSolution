using System;
using SimpleLocalisation.Parsing;

namespace SimpleLocalisation.Processing {

	public class DelegateSwitchConditionEvaluatorFactory : ISwitchConditionEvaluatorFactory {
		private Func<Expression, PatternDialect/*, TextManager*/, ISwitchConditionEvaluator> _factory;

		public DelegateSwitchConditionEvaluatorFactory(Func<Expression, PatternDialect/*, TextManager*/, ISwitchConditionEvaluator> factory) {
			_factory = factory;
		}

		public ISwitchConditionEvaluator GetFor(Expression rep, PatternDialect pattern/*, TextManager manager*/) {
			return _factory(rep, pattern/*, manager*/);
		}

		public static implicit operator DelegateSwitchConditionEvaluatorFactory(Func<Expression, PatternDialect/*, TextManager*/, ISwitchConditionEvaluator> factory) {
			return new DelegateSwitchConditionEvaluatorFactory(factory);
		}
	}
}
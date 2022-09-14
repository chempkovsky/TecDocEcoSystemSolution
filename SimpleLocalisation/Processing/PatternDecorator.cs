using System;
using SimpleLocalisation.Parsing;
using SimpleLocalisation.Processing.ValueFormatters;

namespace SimpleLocalisation.Processing {

	public class PatternDecorator : DescendingPatternVisitor<Object> {
		private PatternDialect _dialect;
		//private TextManager _manager;

		public PatternDecorator(/*TextManager manager,*/ PatternDialect dialect) {
			//_manager = manager;
			_dialect = dialect;
		}

		public override void Visit(ParameterSpec spec, Object state) {
			spec.Evaluator = _dialect.GetParameterEvaluator(spec/*, _manager*/);
			spec.Formatter = _dialect.GetValueFormatter(spec.ParameterFormat/*, _manager*/);


			base.Visit(spec, null);
		}

		public override void Visit(Switch sw, Object state) {
			Visit((ParameterSpec)sw, state);
			//Don't apply default format on the value evaluated in switches
			if (sw.Formatter is DefaultFormatter) {
				sw.Formatter = null;
			}

			base.Visit(sw, state);
		}

		public override void Visit(SwitchCase sc, Object state) {
			sc.Evaluator = _dialect.GetSwitchConditionEvaluator(sc.Condition/*, _manager*/);

			base.Visit(sc, null);
		}

		public override void Visit(FormatGroup group, Object state) {
			group.Evaluator = _dialect.GetParameterEvaluator(new ParameterSpec { ParameterName = group.ParameterName }/*, _manager*/);
			group.Expander = _dialect.FormatGroupExpander;

			base.Visit(group, state);
		}

		public override void Visit(CustomExpressionPart part, Object state) {
			part.Decorate(_dialect/*, _manager*/);

			base.Visit(part, state);
		}
	}
}
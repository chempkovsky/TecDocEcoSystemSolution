using System;
using System.Linq;
using SimpleLocalisation.Parsing;
using SimpleLocalisation.Processing.SwitchConditions;

namespace SimpleLocalisation.Processing {

	public class SwitchDecorator : DescendingPatternVisitor<Object> {
		PatternDialect _dialect;
		//TextManager _manager;

		public SwitchDecorator(/*TextManager manager, */PatternDialect dialect) {
			_dialect = dialect;
			//_manager = manager;
		}

		public override void Visit(Switch sw, object state) {
			if (!string.IsNullOrEmpty(sw.SwitchTemplateName)) {
				int i = 1;

				//Apply the template name as @TemplateName1, @TemplateName2, @TemplateName3 to cases without conditions
				foreach (var sc in sw.Cases.Where(x => x.Condition == null)) {
					//The last case takes everything that the first ones didn't
					if (i == sw.Cases.Count) {
						sc.Evaluator = new TakeAllCondition();
					}
					else {
						sc.Evaluator = _dialect.GetSwitchConditionEvaluator(Expression.Text(
							string.Format("@{0}{1}", sw.SwitchTemplateName, i++))/*, _manager*/);
					}
				}
			}

			//Order switches so that condition-less branches come last
			sw.Cases = sw.Cases.OrderBy(x => x.Condition == null ? 1 : 0).ToList();


			// This may be language specific and can be handled with templates or parameter lookups instead
			//else
			//{
			//    if ((sw.Cases.Count == 2 || sw.Cases.Count == 3) && !sw.Cases.Any(x => x.Condition != null))
			//    {
			//        sw.Cases[0].Evaluator = new SingleValueCondition<int>(0);
			//        if (sw.Cases.Count > 2)
			//        {
			//            sw.Cases[2].Evaluator = new SingleValueCondition<int>(-1);
			//            sw.Cases = new List<SwitchCase> { sw.Cases[0], sw.Cases[2], sw.Cases[1] };
			//        }
			//    }
			//}  
		}
	}
}
using System;

namespace SimpleLocalisation.Processing.SwitchConditions {

	public class IntervalCondition<T> : ISwitchConditionEvaluator where T : struct, IComparable<T> {
		public T? Min, Max;

		public bool MinInclusive, MaxInclusive;

		public bool Evaluate(ParameterValue threshold, EvaluationContext context) {
			var t = Convert.ChangeType(threshold.Value, typeof(T)) as IComparable<T>;

			if (t == null) {
				return false;
			}
			else {
				return (!Min.HasValue || t.CompareTo(Min.Value) > 0 || (MinInclusive && t.CompareTo(Min.Value) == 0)) &&
					(!Max.HasValue || t.CompareTo(Max.Value) < 0 || (MaxInclusive && t.CompareTo(Max.Value) == 0));
			}
		}
	}
}
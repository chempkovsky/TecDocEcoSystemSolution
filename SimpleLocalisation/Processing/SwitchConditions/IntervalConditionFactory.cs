using System;
using System.Text.RegularExpressions;
using System.Xml;

namespace SimpleLocalisation.Processing.SwitchConditions {

	public class IntervalConditionFactory : StringBasedSwitchConditionEvaluatorFactory {
		//Intervals
		static Regex intervalMatcher = new Regex(@"
            (?<MinInclusive>    [\[\]]      )
            (?<Min>             [^,]+       )?
            ,
            (?<Max>             [^\[\]]+    )?
            (?<MaxInclusive>    [\[\]]      )", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

		static Regex halfIntervalMatcher = new Regex(@"
            (?<Dir>         [\<\>]  )
            (?<Inclusive>   \=      )?
            \s*
            (?<Limit>       .*      )", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);


		public override ISwitchConditionEvaluator GetFor(String spelling, PatternDialect dialect/*, TextManager manager*/) {
			Match m;
			if ((m = intervalMatcher.Match(spelling)).Success) {
				return CreateIntervalCondition(
									 m.Groups["Min"].Success ? m.Groups["Min"].Value : null,
									 m.Groups["Max"].Success ? m.Groups["Max"].Value : null,
									 m.Groups["MinInclusive"].Value == "[",
									 m.Groups["MaxInclusive"].Value == "]",
									 dialect.Parser.PatternCulture);
			}
			else if ((m = halfIntervalMatcher.Match(spelling)).Success) {
				return CreateIntervalCondition(
									 m.Groups["Dir"].Value == ">" ? m.Groups["Limit"].Value : null,
									 m.Groups["Dir"].Value == "<" ? m.Groups["Limit"].Value : null,
									 m.Groups["Inclusive"].Success,
									 m.Groups["Inclusive"].Success,
									 dialect.Parser.PatternCulture);
			}

			return null;
		}

		ISwitchConditionEvaluator CreateIntervalCondition(string min, string max, bool minInclusive, bool maxInclusive, IFormatProvider culture) {

			try {
				TimeSpan? tmin = min == null ? null : (TimeSpan?)XmlConvert.ToTimeSpan(min);
				TimeSpan? tmax = max == null ? null : (TimeSpan?)XmlConvert.ToTimeSpan(max);


				return new IntervalCondition<TimeSpan> { Min = tmin, Max = tmax, MinInclusive = minInclusive, MaxInclusive = maxInclusive };
			}
			catch {
				double? dmin = min == null ? null : (double?)double.Parse(min, culture);
				double? dmax = max == null ? null : (double?)double.Parse(max, culture);


				return new IntervalCondition<double> { Min = dmin, Max = dmax, MinInclusive = minInclusive, MaxInclusive = maxInclusive };
			}
		}
	}
}
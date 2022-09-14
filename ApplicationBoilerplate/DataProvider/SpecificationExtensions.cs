using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ApplicationBoilerplate.DataProvider {

	public static class SpecificationExtensions {

		public static Expression<Func<TElement, Boolean>> BuildOr<TElement, TValue>(this ISpecification<TElement> spec, Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values) {
			ParameterExpression p = valueSelector.Parameters.Single();

			IEnumerable<Expression> equals = values.Select(value =>
									(Expression)Expression.Equal(
													valueSelector.Body,
													 Expression.Constant(
																			 value,
																			 typeof(TValue)
																		 )
																)
										);

			Expression body = equals.Aggregate<Expression>(
														 (accumulate, equal) => Expression.Or(accumulate, equal)
													);

			return Expression.Lambda<Func<TElement, Boolean>>(body, p);
		}
	}
}
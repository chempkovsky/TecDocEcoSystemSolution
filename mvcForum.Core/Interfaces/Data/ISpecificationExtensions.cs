// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Interfaces.Data.ISpecificationExtensions
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace mvcForum.Core.Interfaces.Data
{
  public static class ISpecificationExtensions
  {
    public static Expression<Func<TElement, bool>> BuildOr<TElement, TValue>(
      this ISpecification<TElement> spec,
      Expression<Func<TElement, TValue>> valueSelector,
      IEnumerable<TValue> values)
    {
      ParameterExpression parameterExpression = valueSelector.Parameters.Single<ParameterExpression>();
      return Expression.Lambda<Func<TElement, bool>>(values.Select<TValue, Expression>((Func<TValue, Expression>) (value => (Expression) Expression.Equal(valueSelector.Body, (Expression) Expression.Constant((object) value, typeof (TValue))))).Aggregate<Expression>((Func<Expression, Expression, Expression>) ((accumulate, equal) => (Expression) Expression.Or(accumulate, equal))), parameterExpression);
    }
  }
}

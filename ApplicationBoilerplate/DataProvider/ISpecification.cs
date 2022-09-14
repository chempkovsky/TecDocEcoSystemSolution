using System;
using System.Linq.Expressions;

namespace ApplicationBoilerplate.DataProvider {

	public interface ISpecification<TEntity> {
		Expression<Func<TEntity, Boolean>> IsSatisfied { get; }
	}
}
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ApplicationBoilerplate.DataProvider {

	public interface IRepository<TEntity> where TEntity : class {

		#region Simple CRUD
		TEntity Create(TEntity newEntity);
		TEntity Read(String id);
		TEntity Read(Int32 id);
		TEntity Read(Guid id);
		TEntity Update(TEntity entity);
		void Delete(TEntity entity);
		void Delete(String id);
		void Delete(Int32 id);
		void Delete(Guid id);
		#endregion

		#region Additional read operations
		IEnumerable<TEntity> ReadAll();
		TEntity ReadOne(ISpecification<TEntity> spec);
		TEntity ReadOne(Expression<Func<TEntity, Boolean>> criteria);
		IEnumerable<TEntity> ReadMany(ISpecification<TEntity> spec);
		IEnumerable<TEntity> ReadMany(Expression<Func<TEntity, Boolean>> criteria);
		#endregion
	}
}
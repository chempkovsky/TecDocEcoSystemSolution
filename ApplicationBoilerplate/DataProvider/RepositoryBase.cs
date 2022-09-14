using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ApplicationBoilerplate.DataProvider {

	public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class {
		public abstract TEntity Create(TEntity newEntity);
		public abstract TEntity Read(String id);
		public abstract TEntity Read(Int32 id);
		public abstract TEntity Read(Guid id);
		public abstract TEntity Update(TEntity entity);
		public abstract void Delete(TEntity entity);
		public abstract void Delete(String id);
		public abstract void Delete(Int32 id);
		public abstract void Delete(Guid id);

		public abstract IEnumerable<TEntity> ReadAll();
		public abstract TEntity ReadOne(ISpecification<TEntity> spec);
		public abstract TEntity ReadOne(Expression<Func<TEntity, Boolean>> criteria);
		public abstract IEnumerable<TEntity> ReadMany(ISpecification<TEntity> spec);
		public abstract IEnumerable<TEntity> ReadMany(Expression<Func<TEntity, Boolean>> criteria);
	}
}
// mvcForum.DataProvider.EntityFramework.Repository<TEntity>
using ApplicationBoilerplate.DataProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace mvcForum.DataProvider.EntityFramework
{

    public class Repository<TEntity> : RepositoryBase<TEntity> where TEntity : class
    {
        protected readonly DbContext context;

        protected readonly DbSet<TEntity> set;

        public Repository(DbContext context)
        {
            this.context = context;
            set = context.Set<TEntity>();
        }

        public override TEntity Create(TEntity newEntity)
        {
            set.Add(newEntity);
            return newEntity;
        }

        public override TEntity Read(string id)
        {
            return set.Find(id);
        }

        public override TEntity Read(int id)
        {
            return set.Find(id);
        }

        public override TEntity Read(Guid id)
        {
            return set.Find(id);
        }

        public override TEntity Update(TEntity entity)
        {
            set.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public override void Delete(TEntity entity)
        {
            set.Remove(entity);
        }

        public override void Delete(string id)
        {
            Delete(set.Find(id));
        }

        public override void Delete(int id)
        {
            Delete(set.Find(id));
        }

        public override void Delete(Guid id)
        {
            Delete(set.Find(id));
        }

        public override IEnumerable<TEntity> ReadAll()
        {
            return set;
        }

        public override TEntity ReadOne(ISpecification<TEntity> spec)
        {
            return set.Where(spec.IsSatisfied).FirstOrDefault();
        }

        public override TEntity ReadOne(Expression<Func<TEntity, bool>> criteria)
        {
            return set.Where(criteria).FirstOrDefault();
        }

        public override IEnumerable<TEntity> ReadMany(ISpecification<TEntity> spec)
        {
            return set.Where(spec.IsSatisfied).ToList();
        }

        public override IEnumerable<TEntity> ReadMany(Expression<Func<TEntity, bool>> criteria)
        {
            return set.Where(criteria).ToList();
        }
    }
}

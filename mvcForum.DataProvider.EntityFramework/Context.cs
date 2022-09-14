// mvcForum.DataProvider.EntityFramework.Context
using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;

namespace mvcForum.DataProvider.EntityFramework
{

    public class Context : ContextBase
    {
        private readonly DbContext context;

        private readonly Stack<DbTransaction> stack = new Stack<DbTransaction>();

        public Context(DbContext context, IDependencyContainer container)
            : base(container)
        {
            this.context = context;
        }

        public override IRepository<TEntity> GetRepository<TEntity>()
        {
            return new Repository<TEntity>(context);
        }

        public override void SaveChanges()
        {
            try
            {
                IEnumerable<DbEntityValidationResult> validationErrors = context.GetValidationErrors();
                foreach (DbEntityValidationResult item in validationErrors)
                {
                    Type baseType = item.Entry.Entity.GetType().BaseType;
                    PropertyInfo[] properties = item.Entry.Entity.GetType().GetProperties();
                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        if (baseType.GetProperty(propertyInfo.Name).GetCustomAttributes(typeof(RequiredAttribute), inherit: true).Any())
                        {
                            propertyInfo.GetValue(item.Entry.Entity, null);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            context.SaveChanges();
        }

        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
    }
}

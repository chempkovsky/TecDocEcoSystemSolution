// CarShop.MvcForumIdentity.IdentityBuilder
using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.DependencyInjection;
using mvcForum.DataProvider.EntityFramework;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;

namespace CarShop.MvcForumIdentity
{

    public class IdentityBuilder : IDependencyBuilder
    {
        public virtual void Configure(IDependencyContainer container)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<MVCForumContext>());
            container.RegisterPerRequest<IContext, Context>();
            container.RegisterPerRequest<DbContext, MVCForumContext>(new Dictionary<string, object>
        {
            {
                "nameOrConnectionString",
                ConfigurationManager.ConnectionStrings["mvcForum.DataProvider.MainDB"].ConnectionString
            }
        });
            container.RegisterGeneric(typeof(IRepository<>), typeof(Repository<>));
            new SpecificRepositoryConfiguration().Configure(container);
        }

        public void ValidateRequirements(IList<ApplicationRequirement> feedback)
        {
        }
    }
}
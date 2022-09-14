// mvcForum.DataProvider.EntityFramework.DataProviderConfiguration
using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.DependencyInjection;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;

namespace mvcForum.DataProvider.EntityFramework
{

    public class DataProviderConfiguration : IDependencyBuilder
    {
        private readonly bool create;

        public DataProviderConfiguration()
            : this(createDatabase: true)
        {
        }

        public DataProviderConfiguration(bool createDatabase)
        {
            create = createDatabase;
        }

        public virtual void Configure(IDependencyContainer container)
        {
            if (create)
            {
                Database.SetInitializer(new CreateDatabaseIfNotExists<MembershipDbContext>());
            }
            else
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<MembershipDbContext, MigConf>());
            }
            container.RegisterPerRequest<IContext, Context>();
            container.RegisterPerRequest<DbContext, MembershipDbContext>(new Dictionary<string, object>
            {
                {
                    "nameOrConnectionString",
                    ConfigurationManager.ConnectionStrings["mvcForum.DataProvider.MainDB"].ConnectionString
                }
            });
            container.RegisterGeneric(typeof(IRepository<>), typeof(Repository<>));
            new SpecificRepositoryConfiguration().Configure(container);
        }

        public virtual void ValidateRequirements(IList<ApplicationRequirement> feedback)
        {
            if (ConfigurationManager.ConnectionStrings["mvcForum.DataProvider.MainDB"] == null)
            {
                feedback.Add(new ApplicationRequirement
                {
                    Feedback = "No connection found for mvcForum, please insert this into the application's web.config file:\r\n\r\n<configuration>\r\n\t<connectionStrings>\r\n\t\t<add name=\"mvcForum.DataProvider.MainDB\"\r\n\t\t\tconnectionString=\"Data Source=SQLSERVERNAMEorSQLSERVERNAME\\INSTANCE;Initial Catalog=DATABASE;User ID=USERNAME;password=PASSWORD\"\r\n\t\t/>\r\n\t</connectionStrings>\r\n</configuration>\r\n\r\nRemember to replace the values in the connection string with values that matches your set-up.\r\n",
                    Level = RequirementLevel.Fatal
                });
            }
            else
            {
                feedback.Add(new ApplicationRequirement
                {
                    Feedback = "Connection string for MVC Forum located.",
                    Level = RequirementLevel.Info
                });
            }
        }
    }
}

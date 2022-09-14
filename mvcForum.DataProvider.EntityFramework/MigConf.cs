// mvcForum.DataProvider.EntityFramework.MigConf
using mvcForum.DataProvider.EntityFramework;
using System.Data.Entity.Migrations;

namespace mvcForum.DataProvider.EntityFramework
{

    public class MigConf : DbMigrationsConfiguration<MembershipDbContext>
    {
        public MigConf()
        {
            base.AutomaticMigrationsEnabled = true;
        }
    }

}

//using System;
//using System.Data.Entity;
//using System.Data.Entity.ModelConfiguration.Conventions;

//namespace MVCBootstrap.EntityFramework {

//    public class MembershipDbContext : DbContext {

//        public DbSet<User> Users { get; set; }
//        public DbSet<UserInRole> UsersInRoles { get; set; }
//        public DbSet<Role> Roles { get; set; }

//        public MembershipDbContext(String connectString)
//            : base(connectString) {
//        }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
//            base.OnModelCreating(modelBuilder);

//            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
//        }
//    }
//}
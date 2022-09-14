// mvcForum.DataProvider.EntityFramework.MembershipDbContext
using mvcForum.Core;
using mvcForum.DataProvider.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace mvcForum.DataProvider.EntityFramework
{

    public class MembershipDbContext : MVCForumContext
    {
        public DbSet<User> Users
        {
            get;
            set;
        }

        public DbSet<Role> Roles
        {
            get;
            set;
        }

        public MembershipDbContext()
            : base("DefaultConnection")
        {
        }

        public MembershipDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().Property((User u) => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Role>().Property((Role u) => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().Property((User u) => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<User>().Property((User u) => u.Username).IsRequired()
                .HasMaxLength(256);
            modelBuilder.Entity<User>().Property((User u) => u.EmailAddress).IsRequired()
                .HasMaxLength(200);
            modelBuilder.Entity<User>().Property((User u) => u.Password).IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<User>().Property((User u) => u.Created).IsRequired();
            modelBuilder.Entity<User>().Property((User u) => u.LastVisit).IsRequired();
            modelBuilder.Entity<User>().Property((User u) => u.Locked).IsRequired();
            modelBuilder.Entity<User>().Property((User u) => u.Approved).IsRequired();
            modelBuilder.Entity<User>().Property((User u) => u.LastPasswordFailure).IsRequired();
            modelBuilder.Entity<User>().Property((User u) => u.PasswordFailures).IsRequired();
            modelBuilder.Entity<User>().Property((User u) => u.LastLockout).IsRequired();
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Role>().Property((Role u) => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Role>().Property((Role u) => u.Name).IsRequired()
                .HasMaxLength(256);
        }
    }

}

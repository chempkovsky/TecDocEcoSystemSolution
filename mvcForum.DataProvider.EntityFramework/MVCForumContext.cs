// mvcForum.DataProvider.EntityFramework.MVCForumContext
using mvcForum.Core;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace mvcForum.DataProvider.EntityFramework
{

    public class MVCForumContext : DbContext
    {
        public DbSet<AccessMask> AccessMasks
        {
            get;
            set;
        }

        public DbSet<Board> Boards
        {
            get;
            set;
        }

        public DbSet<Category> Categories
        {
            get;
            set;
        }

        public DbSet<Forum> Forums
        {
            get;
            set;
        }

        public DbSet<Topic> Topics
        {
            get;
            set;
        }

        public DbSet<Post> Posts
        {
            get;
            set;
        }

        public DbSet<ForumUser> ForumUsers
        {
            get;
            set;
        }

        public DbSet<GroupMember> GroupMembers
        {
            get;
            set;
        }

        public DbSet<Group> Groups
        {
            get;
            set;
        }

        public DbSet<Attachment> Attachments
        {
            get;
            set;
        }

        public DbSet<ForumAccess> ForumAccesses
        {
            get;
            set;
        }

        public DbSet<ForumSettings> ForumSettings
        {
            get;
            set;
        }

        public DbSet<ForumTrack> ForumTracks
        {
            get;
            set;
        }

        public DbSet<TopicTrack> TopicTracks
        {
            get;
            set;
        }

        public DbSet<PostReport> PostReports
        {
            get;
            set;
        }

        public DbSet<BannedIP> BannedIPs
        {
            get;
            set;
        }

        public DbSet<FollowTopic> FollowTopics
        {
            get;
            set;
        }

        public DbSet<FollowForum> FollowForums
        {
            get;
            set;
        }

        public DbSet<AddOnConfiguration> AddOnConfigurations
        {
            get;
            set;
        }

        public MVCForumContext()
            : base("DefaultConnection")
        {
        }

        public MVCForumContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<AccessMask>().Ignore((AccessMask p) => p.AccessFlag);
            modelBuilder.Entity<AccessMask>().Property((AccessMask p) => p.AccessFlagValue).HasColumnName("AccessFlag");
            modelBuilder.Entity<ForumUser>().Ignore((ForumUser p) => p.UserFlag);
            modelBuilder.Entity<ForumUser>().Property((ForumUser p) => p.UserFlagValue).HasColumnName("UserFlag");
            modelBuilder.Entity<Post>().Ignore((Post p) => p.Flag);
            modelBuilder.Entity<Post>().Property((Post p) => p.FlagValue).HasColumnName("Flag");
            modelBuilder.Entity<Topic>().Ignore((Topic p) => p.Flag);
            modelBuilder.Entity<Topic>().Property((Topic p) => p.FlagValue).HasColumnName("Flag");
            modelBuilder.Entity<Topic>().Ignore((Topic p) => p.Type);
            modelBuilder.Entity<Topic>().Property((Topic p) => p.TypeValue).HasColumnName("Type");
            modelBuilder.Entity<Board>().HasMany((Board x) => x.Categories).WithRequired((Category i) => i.Board);
            modelBuilder.Entity<Category>().HasMany((Category x) => x.Forums).WithRequired((Forum i) => i.Category);
            modelBuilder.Entity<Forum>().HasMany((Forum x) => x.Topics).WithRequired((Topic i) => i.Forum);
            modelBuilder.Entity<Forum>().HasOptional((Forum x) => x.LastPost);
            modelBuilder.Entity<Forum>().HasOptional((Forum x) => x.LastTopic);
            modelBuilder.Entity<Forum>().HasOptional((Forum x) => x.LastPostUser);
            modelBuilder.Entity<Topic>().HasMany((Topic x) => x.Posts).WithRequired((Post i) => i.Topic);
            modelBuilder.Entity<Topic>().HasOptional((Topic x) => x.LastPost);
            modelBuilder.Entity<Topic>().HasOptional((Topic x) => x.LastPostAuthor);
            modelBuilder.Entity<Post>().HasMany((Post x) => x.Attachments).WithRequired((Attachment i) => i.Post);
            modelBuilder.Entity<Topic>().HasMany((Topic x) => x.Followers).WithRequired((FollowTopic i) => i.Topic);
            modelBuilder.Entity<Forum>().HasMany((Forum x) => x.Followers).WithRequired((FollowForum i) => i.Forum);
            modelBuilder.Entity<Topic>().Ignore((Topic t) => t.CustomData);
            modelBuilder.Entity<Post>().Ignore((Post p) => p.CustomData);
        }
    }
}

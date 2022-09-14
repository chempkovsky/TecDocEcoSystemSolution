using System.Data.Entity;
using TecDocEcoSystemDbClassLibrary;


namespace CarShop.Models
{ 
    public class CarShopArticleContext : DbContext
    {
        protected static string DoCreateConnStr(string ConnStringName)
        {
            if (string.IsNullOrEmpty(ConnStringName))
            {
                return "name=CarShopArticleContext";
            }
            else
            {
                return "name=" + ConnStringName;
            }
        }

        public CarShopArticleContext() : base("name=CarShopArticleContext")
        {
        }

        public CarShopArticleContext(string ConnStringName)
            : base(DoCreateConnStr(ConnStringName))
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // insert your code here
        }



        public DbSet<EnterpriseBrandTDES> EnterpriseBrandTDES { get; set; }
        public DbSet<EnterpriseArticleTDES> EnterpriseArticleTDES { get; set; }
        public DbSet<EnterpriseArticleDescriptionTDES> EnterpriseArticleDescriptionTDES { get; set; }

        public DbSet<EnterpriseCategoryTDES> EnterpriseCategoryTDES { get; set; }
        public DbSet<EnterpriseCategoryItemTDES> EnterpriseCategoryItemTDES { get; set; }
        public DbSet<EnterpriseCategoryItemDescriptionTDES> EnterpriseCategoryItemDescriptionTDES { get; set; }
        public DbSet<EnterpriseCategoryDescriptionTDES> EnterpriseCategoryDescriptionTDES { get; set; }

    } 
}
using System.Data.Entity;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Models
{
    public class CarShopRestContext : DbContext
    {

        protected static string DoCreateConnStr(string ConnStringName)
        {
            if (string.IsNullOrEmpty(ConnStringName))
            {
                return "name=CarShopRestContext";
            }
            else
            {
                return "name=" + ConnStringName;
            }
        }


        public CarShopRestContext() : base("name=CarShopRestContext")
        {

        }
        public CarShopRestContext(string ConnStringName)
            : base(DoCreateConnStr(ConnStringName))
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // insert your code here
        }


        public DbSet<BranchRestTDES> BranchRestTDES { get; set; }
        public DbSet<BranchRestArticleDescriptionTDES> BranchRestArticleDescriptionTDES { get; set; }
        public DbSet<GuestBranchTDES> GuestBranchTDES { get; set; }
    }
}
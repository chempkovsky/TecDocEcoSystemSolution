using System.Data.Entity;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Models
{
    public class CarShopSalesContext : DbContext
    {
        public CarShopSalesContext()
            : base("name=CarShopSalesContext")
        {

        }
        public CarShopSalesContext(string ConnStringName)
            : base("name=" + ConnStringName)
        {

        }

        public DbSet<BranchSpellTDES> BranchSpellTDES { get; set; }
        public DbSet<BranchSpellHstTDES> BranchSpellHstTDES { get; set; }

        public DbSet<User2WorkPlaceTDES> User2WorkPlaceTDES { get; set; }
        public DbSet<User2WorkPlaceHstTDES> User2WorkPlaceHstTDES { get; set; }

        public DbSet<SaleBasketTDES> SaleBasketTDES { get; set; }
        public DbSet<SaleBasketArticleTDES> SaleBasketArticleTDES { get; set; }
        public DbSet<SaleArticleDescriptionTDES> SaleArticleDescriptionTDES { get; set; }


        public DbSet<ReturnBasketTDES> ReturnBasketTDES { get; set; }
        public DbSet<ReturnBasketArticleTDES> ReturnBasketArticleTDES { get; set; }

    }
}
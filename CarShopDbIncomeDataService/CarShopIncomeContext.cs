using System.Data.Entity;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Models
{
    public class CarShopIncomeContext : DbContext
    {
        public CarShopIncomeContext()
            : base("name=CarShopIncomeContext")
        {

        }
        public CarShopIncomeContext(string ConnStringName)
            : base("name=" + ConnStringName)
        {

        }

        public DbSet<IncomePayRollTDES> IncomePayRollTDES { get; set; }
        public DbSet<IncomeArticleTDES> IncomeArticleTDES { get; set; }
        public DbSet<SheetRevaluationTDES> SheetRevaluationTDES { get; set; }
        public DbSet<RevaluationArticleTDES> RevaluationArticleTDES { get; set; }
    }
}
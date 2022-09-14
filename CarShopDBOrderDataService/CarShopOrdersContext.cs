using System.Data.Entity;
using TecDocEcoSystemDbClassLibrary;

namespace CarShopDBOrderDataService
{
    public class CarShopOrdersContext : DbContext
    {
        protected static string DoCreateConnStr(string ConnStringName)
        {
            if (string.IsNullOrEmpty(ConnStringName))
            {
                return "name=CarShopOrderContext";
            }
            else
            {
                return "name=" + ConnStringName;
            }
        }

        public CarShopOrdersContext() : base("name=CarShopOrderContext")
        {

        }
        public CarShopOrdersContext(string ConnStringName)
            : base(DoCreateConnStr(ConnStringName))
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // insert your code here
        }

        public DbSet<GuestProfileTDES> GuestProfileTDES { get; set; }
        public DbSet<GuestOrderTDES> GuestOrderTDES { get; set; }
        public DbSet<GuestOrderArticleTDES> GuestOrderArticleTDES { get; set; }

    }
}

using System.Data.Entity;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Models
{

    //
    // 1.
    // from e in db.AddressType.AsNoTracking() select e;
    // 2.
    // dbContext.Entry(entity).State = EntityState.Detached;
    //

    public class CarShopContext : DbContext
    {
        // В этот файл можно добавить пользовательский код. Изменения не будут перезаписаны.
        // 
        // Если требуется, чтобы платформа Entity Framework автоматически удаляла и формировала заново базу данных
        // при каждой смене схемы модели, добавьте следующий
        // код к методу Application_Start в файле Global.asax.
        // Примечание: в этом случае при каждой смене модели ваша база данных будет удалена и создана заново.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<CarShop.Models.CarShopContext>());

        protected static string DoCreateConnStr(string ConnStringName)
        {
            if (string.IsNullOrEmpty(ConnStringName))
            {
                return "name=CarShopContext";
            }
            else
            {
                return "name=" + ConnStringName;
            }
        }
        public CarShopContext(string ConnStringName)
            : base(DoCreateConnStr(ConnStringName))
        {
        }


        public CarShopContext() : base("name=CarShopContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            /*
            modelBuilder.Entity<EnterpriseTDES>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("EnterpriseTDES");
            });

            modelBuilder.Entity<CreditCard>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("CreditCards");
            });
            */
        }

        public DbSet<EnterpriseTDES> EnterpriseTDES { get; set; }
        public DbSet<EnterpriseUserTDES> EnterpriseUserTDES { get; set; }
        public DbSet<EnterpriseUserContactTDES> EnterpriseUserContactTDES { get; set; }
        public DbSet<ContactType> ContactType { get; set; }
        public DbSet<EnterpriseBranchTDES> EnterpriseBranchTDES { get; set; }
        public DbSet<BranchType> BranchType { get; set; }
        public DbSet<EnterpriseBranchContactsTDES> EnterpriseBranchContactsTDES { get; set; }
        public DbSet<EnterpriseBranchUserTDES> EnterpriseBranchUserTDES { get; set; }
        public DbSet<EnterpriseProductCategoryTDES> EnterpriseProductCategoryTDES { get; set; }
        public DbSet<EnterpriseBranchWorkPlaceTDES> EnterpriseBranchWorkPlaceTDES { get; set; }
        public DbSet<EnterpriseBranchUserContactTDES> EnterpriseBranchUserContactTDES { get; set; }
        public DbSet<AddressType> AddressType { get; set; }
        public DbSet<StreetType> StreetType { get; set; }
        public DbSet<SettlementType> SettlementType { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Soato> Soato { get; set; }

        public DbSet<EnterpriseAddressTDES> EnterpriseAddressTDES { get; set; }
        public DbSet<EnterpriseBranchAddressTDES> EnterpriseBranchAddressTDES { get; set; }

        public DbSet<EnterpriseSupplierTDES> EnterpriseSupplierTDES { get; set; }
        public DbSet<EnterpriseSupplierContactTDES> EnterpriseSupplierContactTDES { get; set; }
        public DbSet<EnterpriseSupplierAddressTDES> EnterpriseSupplierAddressTDES { get; set; }

        public DbSet<EnterpriseTecDocSrcTypeTDES> EnterpriseTecDocSrcTypeTDES { get; set; }

        public DbSet<EnterpriseBranchReplyTDES> EnterpriseBranchReplyTDES { get; set; }


    }
     
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecDocEcoSystemDbClassLibrary
{

    public class EnterpriseTDESCatalog : DbContext
    {

        public EnterpriseTDESCatalog(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<EnterpriseUserTDES>().Property(s => s.EntGuid).IsRequired();
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
  
  
    }

    // must be inserted in the model creations 
    // Database.SetInitializer<EnterpriseTDESCatalog>(new RecreateDatabaseIfModelChanges<EnterpriseTDESCatalog>());



}

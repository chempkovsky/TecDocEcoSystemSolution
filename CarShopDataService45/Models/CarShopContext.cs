// Decompiled with JetBrains decompiler
// Type: CarShop.Models.CarShopContext
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using System.Data.Entity;

namespace CarShop.Models
{
  public class CarShopContext : DbContext
  {
    protected static string DoCreateConnStr(string ConnStringName)
    {
      if (string.IsNullOrEmpty(ConnStringName))
        return "name=CarShopContext";
      return "name=" + ConnStringName;
    }

    public CarShopContext(string ConnStringName)
      : base(CarShopContext.DoCreateConnStr(ConnStringName))
    {
    }

    public CarShopContext()
      : base("name=CarShopContext")
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseTDES> EnterpriseTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseUserTDES> EnterpriseUserTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseUserContactTDES> EnterpriseUserContactTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.ContactType> ContactType { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseBranchTDES> EnterpriseBranchTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.BranchType> BranchType { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseBranchContactsTDES> EnterpriseBranchContactsTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseBranchUserTDES> EnterpriseBranchUserTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseProductCategoryTDES> EnterpriseProductCategoryTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseBranchWorkPlaceTDES> EnterpriseBranchWorkPlaceTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseBranchUserContactTDES> EnterpriseBranchUserContactTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.AddressType> AddressType { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.StreetType> StreetType { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.SettlementType> SettlementType { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.Country> Country { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.Currency> Currency { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.Soato> Soato { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseAddressTDES> EnterpriseAddressTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseBranchAddressTDES> EnterpriseBranchAddressTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseSupplierTDES> EnterpriseSupplierTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseSupplierContactTDES> EnterpriseSupplierContactTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseSupplierAddressTDES> EnterpriseSupplierAddressTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseTecDocSrcTypeTDES> EnterpriseTecDocSrcTypeTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseBranchReplyTDES> EnterpriseBranchReplyTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.OriginalCatalogTDES> OriginalCatalogTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseBranchRateTDES> EnterpriseBranchRateTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseSupplierDownLoadTDES> EnterpriseSupplierDownLoadTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseSupplierReplaceTDES> EnterpriseSupplierReplaceTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseSupplierDownLoadRepTDES> EnterpriseSupplierDownLoadRepTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseBranchDownLoadTDES> EnterpriseBranchDownLoadTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseBranchReplaceTDES> EnterpriseBranchReplaceTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseBranchDownLoadRepTDES> EnterpriseBranchDownLoadRepTDES { get; set; }

        public System.Data.Entity.DbSet<TecDocEcoSystemDbClassLibrary.BranchSuppTDES> BranchSuppTDES { get; set; }

        public System.Data.Entity.DbSet<TecDocEcoSystemDbClassLibrary.GuestBranchTDES> GuestBranchTDES { get; set; }
    }
}

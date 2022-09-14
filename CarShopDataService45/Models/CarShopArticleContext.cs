// Decompiled with JetBrains decompiler
// Type: CarShop.Models.CarShopArticleContext
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using System.Data.Entity;

namespace CarShop.Models
{
  public class CarShopArticleContext : DbContext
  {
    public CarShopArticleContext()
      : base("name=CarShopArticleContext")
    {
    }

    public CarShopArticleContext(string ConnStringName)
      : base("name=" + ConnStringName)
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseBrandTDES> EnterpriseBrandTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseArticleTDES> EnterpriseArticleTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseArticleDescriptionTDES> EnterpriseArticleDescriptionTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseCategoryTDES> EnterpriseCategoryTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseCategoryItemTDES> EnterpriseCategoryItemTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseCategoryItemDescriptionTDES> EnterpriseCategoryItemDescriptionTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseCategoryDescriptionTDES> EnterpriseCategoryDescriptionTDES { get; set; }
  }
}

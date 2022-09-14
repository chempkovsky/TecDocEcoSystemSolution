// Decompiled with JetBrains decompiler
// Type: CarShop.Models.CarShopMsTecDocContext
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using System.Data.Entity;

namespace CarShop.Models
{
  public class CarShopMsTecDocContext : DbContext
  {
    public CarShopMsTecDocContext()
      : base("name=CarShopMsTecDocContext")
    {
    }

    public CarShopMsTecDocContext(string ConnStringName)
      : base("name=" + ConnStringName)
    {
    }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseCategoryItemTecDocDescriptionTDES> EnterpriseCategoryItemTecDocDescriptionTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseArticleTecDocDescriptionTDES> EnterpriseArticleTecDocDescriptionTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseCategoryTecDocTDES> EnterpriseCategoryTecDocTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseCategoryItemTecDocTDES> EnterpriseCategoryItemTecDocTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseArticleCategoryItemTDES> EnterpriseArticleCategoryItemTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseArticleBrandTDES> EnterpriseArticleBrandTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseArticleLookUpTDES> EnterpriseArticleLookUpTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseArticleApplicTDES> EnterpriseArticleApplicTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseArticleTecDocTDES> EnterpriseArticleTecDocTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseCarModelFuelTDES> EnterpriseCarModelFuelTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseCarBrandTDES> EnterpriseCarBrandTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseCarModelTypeTDES> EnterpriseCarModelTypeTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseCarModelTDES> EnterpriseCarModelTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseCategoryItemApplicTDES> EnterpriseCategoryItemApplicTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.EnterpriseCategoryApplicTDES> EnterpriseCategoryApplicTDES { get; set; }
  }
}

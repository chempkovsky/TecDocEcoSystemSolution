// Decompiled with JetBrains decompiler
// Type: CarShop.Models.CarShopSalesContext
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using System.Data.Entity;

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

    public DbSet<TecDocEcoSystemDbClassLibrary.BranchSpellTDES> BranchSpellTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.BranchSpellHstTDES> BranchSpellHstTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.User2WorkPlaceTDES> User2WorkPlaceTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.User2WorkPlaceHstTDES> User2WorkPlaceHstTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.SaleBasketTDES> SaleBasketTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.SaleBasketArticleTDES> SaleBasketArticleTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.SaleArticleDescriptionTDES> SaleArticleDescriptionTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.ReturnBasketTDES> ReturnBasketTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.ReturnBasketArticleTDES> ReturnBasketArticleTDES { get; set; }
  }
}

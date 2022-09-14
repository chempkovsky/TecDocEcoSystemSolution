// Decompiled with JetBrains decompiler
// Type: CarShop.Models.CarShopRestContext
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using System.Data.Entity;

namespace CarShop.Models
{
  public class CarShopRestContext : DbContext
  {
    public CarShopRestContext()
      : base("name=CarShopRestContext")
    {
    }

    public CarShopRestContext(string ConnStringName)
      : base("name=" + ConnStringName)
    {
    }

    public DbSet<TecDocEcoSystemDbClassLibrary.BranchRestTDES> BranchRestTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.BranchRestArticleDescriptionTDES> BranchRestArticleDescriptionTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.GuestBranchTDES> GuestBranchTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.BranchSuppTDES> BranchSuppTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.SuppRestTDES> SuppRestTDES { get; set; }
  }
}

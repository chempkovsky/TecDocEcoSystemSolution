// Decompiled with JetBrains decompiler
// Type: CarShop.Models.CarShopIncomeContext
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using System.Data.Entity;

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

    public DbSet<TecDocEcoSystemDbClassLibrary.IncomePayRollTDES> IncomePayRollTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.IncomeArticleTDES> IncomeArticleTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.SheetRevaluationTDES> SheetRevaluationTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.RevaluationArticleTDES> RevaluationArticleTDES { get; set; }
  }
}

// Decompiled with JetBrains decompiler
// Type: CarShop.Models.CarShopOrdersContext
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using System.Data.Entity;

namespace CarShop.Models
{
  public class CarShopOrdersContext : DbContext
  {
    public CarShopOrdersContext()
      : base("name=CarShopOrdersContext")
    {
    }

    public CarShopOrdersContext(string ConnStringName)
      : base("name=" + ConnStringName)
    {
    }

    public DbSet<TecDocEcoSystemDbClassLibrary.GuestProfileTDES> GuestProfileTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.GuestOrderTDES> GuestOrderTDES { get; set; }

    public DbSet<TecDocEcoSystemDbClassLibrary.GuestOrderArticleTDES> GuestOrderArticleTDES { get; set; }
  }
}

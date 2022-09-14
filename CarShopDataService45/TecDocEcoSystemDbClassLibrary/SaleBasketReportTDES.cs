// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.SaleBasketReportTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class SaleBasketReportTDES
  {
    [Display(Name = "BranchSpellTDES_SpellGuid", ResourceType = typeof (Resources))]
    public Guid SpellGuid { get; set; }

    [Display(Name = "WorkPlaceGuid", ResourceType = typeof (Resources))]
    public Guid WorkPlaceGuid { get; set; }

    [StringLength(60)]
    [Display(Name = "SaleBasketTDES_Description", ResourceType = typeof (Resources))]
    public string Description { get; set; }

    [Display(Name = "EnterpriseUserTDES_EntUserNic", ResourceType = typeof (Resources))]
    [StringLength(40)]
    public string EntUserNic { get; set; }

    [Display(Name = "SaleBasketTDES_PaymentSum", ResourceType = typeof (Resources))]
    public double PaymentSum { get; set; }

    [Display(Name = "SaleBasketTDES_ArtAmount", ResourceType = typeof (Resources))]
    public int ArtAmount { get; set; }
  }
}

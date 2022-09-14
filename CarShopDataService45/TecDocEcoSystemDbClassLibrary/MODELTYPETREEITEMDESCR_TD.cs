// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.MODELTYPETREEITEMDESCR_TD
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class MODELTYPETREEITEMDESCR_TD
  {
    [Display(Name = "TECDOC_TEX_TEXT", ResourceType = typeof (Resources))]
    public string TEX_TEXT { get; set; }

    [Display(Name = "TECDOC_TEX_VALUE", ResourceType = typeof (Resources))]
    public string TEX_VALUE { get; set; }

    [Display(Name = "TECDOC_TEX_UNIT", ResourceType = typeof (Resources))]
    public string TEX_UNIT { get; set; }
  }
}

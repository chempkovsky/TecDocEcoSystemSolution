// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.MODELTYPETREEITEMMANID_TD
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class MODELTYPETREEITEMMANID_TD
  {
    [Display(Name = "TECDOC_SUP_TEXT", ResourceType = typeof (Resources))]
    public string SUP_TEXT { get; set; }

    [Display(Name = "TECDOC_TEX_VALUE", ResourceType = typeof (Resources))]
    public string TEX_VALUE { get; set; }
  }
}

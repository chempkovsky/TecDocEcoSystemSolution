// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.MODEL_TD
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class MODEL_TD
  {
    [Key]
    [Display(Name = "TECDOC_MOD_ID", ResourceType = typeof (Resources))]
    public int MOD_ID { get; set; }

    [Display(Name = "TECDOC_TEX_TEXT", ResourceType = typeof (Resources))]
    public string TEX_TEXT { get; set; }
  }
}

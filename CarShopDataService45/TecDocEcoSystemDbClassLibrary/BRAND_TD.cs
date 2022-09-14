// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.BRAND_TD
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class BRAND_TD
  {
    [Key]
    [Display(Name = "TECDOC_MFA_ID", ResourceType = typeof (Resources))]
    public int MFA_ID { get; set; }

    [Display(Name = "TECDOC_MFA_BRAND", ResourceType = typeof (Resources))]
    public string MFA_BRAND { get; set; }
  }
}

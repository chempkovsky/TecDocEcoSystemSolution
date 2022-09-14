// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.OriginalCatalogTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class OriginalCatalogTDES
  {
    [Display(Name = "OriginalCatalogId", ResourceType = typeof (Resources))]
    [Key]
    public int OriginalCatalogId { get; set; }

    [Display(Name = "OriginalCatalogName", ResourceType = typeof (Resources))]
    [StringLength(120, MinimumLength = 2)]
    public string OriginalCatalogName { get; set; }

    [Display(Name = "OriginalCatalogURI", ResourceType = typeof (Resources))]
    [StringLength(255, MinimumLength = 5)]
    public string OriginalCatalogURI { get; set; }
  }
}

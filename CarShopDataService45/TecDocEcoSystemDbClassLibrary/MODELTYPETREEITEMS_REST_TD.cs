// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.MODELTYPETREEITEMS_REST_TD
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class MODELTYPETREEITEMS_REST_TD : MODELTYPETREEITEMS_TD
  {
    [Required]
    [Display(Name = "BranchRestTDES_ArtAmount", ResourceType = typeof (Resources))]
    public int ArtAmount { get; set; }

    [Required]
    [Display(Name = "BranchRestTDES_ArtPrice", ResourceType = typeof (Resources))]
    public double ArtPrice { get; set; }

    [Required]
    [Display(Name = "BranchRestTDES_MinArtPrice", ResourceType = typeof (Resources))]
    public double MinArtPrice { get; set; }
  }
}

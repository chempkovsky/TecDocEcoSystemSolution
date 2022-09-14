// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.ANALOGOUS_REST_TD
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class ANALOGOUS_REST_TD : ANALOGOUS_TD
  {
    [Required]
    [Display(Name = "BranchRestTDES_ArtAmount", ResourceType = typeof (Resources))]
    public int ArtAmount { get; set; }

    [Display(Name = "BranchRestTDES_ArtPrice", ResourceType = typeof (Resources))]
    [Required]
    public double ArtPrice { get; set; }

    [Display(Name = "BranchRestTDES_MinArtPrice", ResourceType = typeof (Resources))]
    [Required]
    public double MinArtPrice { get; set; }
  }
}

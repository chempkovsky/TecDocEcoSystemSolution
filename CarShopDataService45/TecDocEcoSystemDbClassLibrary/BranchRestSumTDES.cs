// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.BranchRestSumTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class BranchRestSumTDES
  {
    [Display(Name = "BranchRestTDES_ART_ARTICLE_NR", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    public string ART_ARTICLE_NR { get; set; }

    [Display(Name = "BranchRestTDES_SUP_TEXT", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    public string SUP_TEXT { get; set; }

    [Display(Name = "BranchRestTDES_ArtAmount", ResourceType = typeof (Resources))]
    [Required]
    public int ArtAmount { get; set; }

    [Display(Name = "BranchRestTDES_MinArtPrice", ResourceType = typeof (Resources))]
    [Required]
    public double MinArtPrice { get; set; }

    [Required]
    [Display(Name = "BranchRestTDES_ArtPrice", ResourceType = typeof (Resources))]
    public double ArtPrice { get; set; }
  }
}

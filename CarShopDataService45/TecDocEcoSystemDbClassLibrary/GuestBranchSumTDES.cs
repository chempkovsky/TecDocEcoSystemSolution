// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.GuestBranchSumTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class GuestBranchSumTDES
  {
    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    [Key]
    public Guid EntBranchGuid { get; set; }

    [Display(Name = "EntBranchDescription", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(80)]
    public string EntBranchDescription { get; set; }

    [Display(Name = "AddressPostCode", ResourceType = typeof (Resources))]
    [StringLength(15)]
    public string AddressPostCode { get; set; }

    [Display(Name = "AddressRegion", ResourceType = typeof (Resources))]
    [StringLength(80)]
    public string AddressRegion { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "AddressSettlementName", ResourceType = typeof (Resources))]
    public string AddressSettlementName { get; set; }

    [StringLength(80)]
    [Required]
    [Display(Name = "AddressStreetName", ResourceType = typeof (Resources))]
    public string AddressStreetName { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
    [Required]
    [Display(Name = "AddressLongitude", ResourceType = typeof (Resources))]
    public double AddressLongitude { get; set; }

    [Required]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
    [Display(Name = "AddressLatitude", ResourceType = typeof (Resources))]
    public double AddressLatitude { get; set; }

    [StringLength(80)]
    [Display(Name = "WorkingDays", ResourceType = typeof (Resources))]
    [Required]
    public string WorkingDays { get; set; }

    [Display(Name = "WorkingTime", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(80)]
    public string WorkingTime { get; set; }

    [Required]
    [Display(Name = "BranchRestTDES_ArtAmount", ResourceType = typeof (Resources))]
    public int ArtAmount { get; set; }

    [Display(Name = "BranchRestTDES_ArtPrice", ResourceType = typeof (Resources))]
    [Required]
    public double ArtPrice { get; set; }

    [StringLength(80)]
    [Display(Name = "SuppTime", ResourceType = typeof (Resources))]
    [Required]
    public string SuppTime { get; set; }

    [Display(Name = "Phones", ResourceType = typeof (Resources))]
    [StringLength(120)]
    public string Phones { get; set; }

    [Display(Name = "SiteUrl", ResourceType = typeof (Resources))]
    [StringLength(250)]
    public string SiteUrl { get; set; }

    [StringLength(80)]
    [Display(Name = "ShopLicense", ResourceType = typeof (Resources))]
    public string ShopLicense { get; set; }

    [Display(Name = "ShopSupply", ResourceType = typeof (Resources))]
    [StringLength(80)]
    public string ShopSupply { get; set; }
  }
}

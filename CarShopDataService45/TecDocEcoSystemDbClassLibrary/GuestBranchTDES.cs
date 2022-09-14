// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.GuestBranchTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class GuestBranchTDES
  {
    [Key]
    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "EntBranchDescription", ResourceType = typeof (Resources))]
    public string EntBranchDescription { get; set; }

    [StringLength(15)]
    [Display(Name = "AddressPostCode", ResourceType = typeof (Resources))]
    public string AddressPostCode { get; set; }

    [Display(Name = "AddressRegion", ResourceType = typeof (Resources))]
    [StringLength(80)]
    public string AddressRegion { get; set; }

    [Display(Name = "AddressSettlementName", ResourceType = typeof (Resources))]
    [StringLength(80)]
    [Required]
    public string AddressSettlementName { get; set; }

    [Display(Name = "AddressStreetName", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(80)]
    public string AddressStreetName { get; set; }

    [Display(Name = "AddressLongitude", ResourceType = typeof (Resources))]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
    [Required]
    public double AddressLongitude { get; set; }

    [Required]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
    [Display(Name = "AddressLatitude", ResourceType = typeof (Resources))]
    public double AddressLatitude { get; set; }

    [StringLength(80)]
    [Required]
    [Display(Name = "WorkingDays", ResourceType = typeof (Resources))]
    public string WorkingDays { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "WorkingTime", ResourceType = typeof (Resources))]
    public string WorkingTime { get; set; }

    [Range(1, 2147483647)]
    [Display(Name = "PriceCurrencyIso", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int PriceCurrencyIso { get; set; }

    [Display(Name = "ExchRate", ResourceType = typeof (Resources))]
    [Required]
    [Range(0.0001, double.MaxValue)]
    public double ExchRate { get; set; }

    [Required]
    [Range(0.0001, double.MaxValue)]
    [Display(Name = "Rounding", ResourceType = typeof (Resources))]
    public double Rounding { get; set; }

    [Required]
    [Range(0.0001, double.MaxValue)]
    [Display(Name = "Multiplexer", ResourceType = typeof (Resources))]
    public double Multiplexer { get; set; }

    [StringLength(120)]
    [Display(Name = "Phones", ResourceType = typeof (Resources))]
    public string Phones { get; set; }

    [Display(Name = "SiteUrl", ResourceType = typeof (Resources))]
    [StringLength(250)]
    public string SiteUrl { get; set; }

    [Display(Name = "ShopLicense", ResourceType = typeof (Resources))]
    [StringLength(80)]
    public string ShopLicense { get; set; }

    [Display(Name = "ShopSupply", ResourceType = typeof (Resources))]
    [StringLength(80)]
    public string ShopSupply { get; set; }

    public virtual ICollection<BranchSuppTDES> BranchSuppTDESes { get; set; }
  }
}

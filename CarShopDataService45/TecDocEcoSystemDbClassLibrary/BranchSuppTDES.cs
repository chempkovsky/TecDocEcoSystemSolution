// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.BranchSuppTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class BranchSuppTDES
  {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 0)]
    [Key]
    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [StringLength(40)]
    [Column(Order = 1)]
    [Display(Name = "EnterpriseSupplierTDES_EntSupplierId", ResourceType = typeof (Resources))]
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string EntSupplierId { get; set; }

    [Range(1, 2147483647)]
    [Required]
    [Display(Name = "PriceCurrencyIso", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int PriceCurrencyIso { get; set; }

    [Range(0.0001, double.MaxValue)]
    [Required]
    [Display(Name = "ExchRate", ResourceType = typeof (Resources))]
    public double ExchRate { get; set; }

    [Display(Name = "Rounding", ResourceType = typeof (Resources))]
    [Required]
    [Range(0.0001, double.MaxValue)]
    public double Rounding { get; set; }

    [Range(0.0001, double.MaxValue)]
    [Display(Name = "Multiplexer", ResourceType = typeof (Resources))]
    [Required]
    public double Multiplexer { get; set; }

    [Display(Name = "SuppTime", ResourceType = typeof (Resources))]
    [StringLength(80)]
    [Required]
    public string SuppTime { get; set; }

    public virtual GuestBranchTDES GuestBranchTDES { get; set; }
  }
}

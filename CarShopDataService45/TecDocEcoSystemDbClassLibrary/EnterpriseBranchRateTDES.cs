// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseBranchRateTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseBranchRateTDES
  {
    [Key]
    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    [Column(Order = 0)]
    public Guid EntBranchGuid { get; set; }

    [Key]
    [Required]
    [Range(1, 2147483647)]
    [Display(Name = "CurrencyIso", ResourceType = typeof (Resources))]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CurrencyIso { get; set; }

    public virtual EnterpriseBranchTDES EnterpriseBranchTDES { get; set; }

    [Required]
    [Display(Name = "ExchRate", ResourceType = typeof (Resources))]
    [Range(0.0001, double.MaxValue)]
    public double ExchRate { get; set; }

    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }
  }
}

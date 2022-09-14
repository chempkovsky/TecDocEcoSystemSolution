// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.IncomePayRollTDES
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
  public class IncomePayRollTDES
  {
    [Display(Name = "IncomePayRollTDES_IncomePayRollTDES", ResourceType = typeof (Resources))]
    [Key]
    [Required]
    public Guid IncomePayRollTDESGuid { get; set; }

    [Display(Name = "IncomePayRollTDES_Description", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(60)]
    public string Description { get; set; }

    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    [Display(Name = "EnterpriseUserTDES_EntUserNic", ResourceType = typeof (Resources))]
    [StringLength(40)]
    [Required]
    public string EntUserNic { get; set; }

    [Display(Name = "IncomePayRollTDES_CreatedAt", ResourceType = typeof (Resources))]
    [Required]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "IsProcessed", ResourceType = typeof (Resources))]
    [Required]
    public bool IsProcessed { get; set; }

    [Display(Name = "IsReversed", ResourceType = typeof (Resources))]
    [Required]
    public bool IsReversed { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [StringLength(40)]
    [Display(Name = "IncomePayRollTDES_EntSupplierId", ResourceType = typeof (Resources))]
    public string EntSupplierId { get; set; }

    [Display(Name = "IncomePayRollTDES_EntSupplierDescription", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(80)]
    public string EntSupplierDescription { get; set; }

    public virtual ICollection<TecDocEcoSystemDbClassLibrary.IncomeArticleTDES> IncomeArticleTDES { get; set; }
  }
}

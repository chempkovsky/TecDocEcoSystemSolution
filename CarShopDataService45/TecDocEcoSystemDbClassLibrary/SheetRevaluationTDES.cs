// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.SheetRevaluationTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class SheetRevaluationTDES
  {
    [Required]
    [Display(Name = "SheetRevaluationTDES_SheetRevaluationTDESGuid", ResourceType = typeof (Resources))]
    [Key]
    public Guid SheetRevaluationTDESGuid { get; set; }

    [Display(Name = "IncomePayRollTDES_Description", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(60)]
    public string Description { get; set; }

    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntBranchGuid { get; set; }

    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntGuid { get; set; }

    [Required]
    [Display(Name = "EnterpriseUserTDES_EntUserNic", ResourceType = typeof (Resources))]
    [StringLength(40)]
    public string EntUserNic { get; set; }

    [Display(Name = "IncomePayRollTDES_CreatedAt", ResourceType = typeof (Resources))]
    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    [Display(Name = "IsProcessed", ResourceType = typeof (Resources))]
    public bool IsProcessed { get; set; }

    [Required]
    [Display(Name = "IsReversed", ResourceType = typeof (Resources))]
    public bool IsReversed { get; set; }

    public virtual ICollection<TecDocEcoSystemDbClassLibrary.RevaluationArticleTDES> RevaluationArticleTDES { get; set; }
  }
}

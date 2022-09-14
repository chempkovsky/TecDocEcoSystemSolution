// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.RevaluationArticleTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class RevaluationArticleTDES
  {
    [StringLength(40)]
    [Column(Order = 0)]
    [Required]
    [Key]
    [Display(Name = "EnterpriseArticleTDES_EntArticle", ResourceType = typeof (Resources))]
    public string EntArticle { get; set; }

    [StringLength(40)]
    [Required]
    [Key]
    [Column(Order = 1)]
    [Display(Name = "EnterpriseArticleTDES_EntBrandNic", ResourceType = typeof (Resources))]
    public string EntBrandNic { get; set; }

    [Display(Name = "RevaluationArticleTDES_IncomePayRollTDES", ResourceType = typeof (Resources))]
    [Key]
    [Column(Order = 2)]
    [Required]
    public Guid IncomePayRollTDESGuid { get; set; }

    [StringLength(120)]
    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    public string EntArticleDescription { get; set; }

    [Key]
    [Display(Name = "SheetRevaluationTDES_SheetRevaluationTDESGuid", ResourceType = typeof (Resources))]
    [Required]
    [Column(Order = 3)]
    public Guid SheetRevaluationTDESGuid { get; set; }

    public virtual SheetRevaluationTDES SheetRevaluationTDES { get; set; }

    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntGuid { get; set; }

    [Required]
    [Display(Name = "IsProcessed", ResourceType = typeof (Resources))]
    public bool IsProcessed { get; set; }

    [Required]
    [Display(Name = "IsReversed", ResourceType = typeof (Resources))]
    public bool IsReversed { get; set; }

    [Display(Name = "IncomeArticleTDES_CurrArtPrice", ResourceType = typeof (Resources))]
    [Required]
    [Range(0.0, double.MaxValue)]
    public double CurrArtPrice { get; set; }

    [Required]
    [Display(Name = "RevaluationArticleTDES_NewArtPrice", ResourceType = typeof (Resources))]
    [Range(0.0, double.MaxValue)]
    public double NewArtPrice { get; set; }

    [Required]
    [Range(0, 2147483647)]
    [Display(Name = "IncomeArticleTDES_ArtAmountRest", ResourceType = typeof (Resources))]
    public int ArtAmountRest { get; set; }

    [Display(Name = "RevaluationArticleTDES_OperSum", ResourceType = typeof (Resources))]
    [Required]
    public double OperSum { get; set; }

    [Required]
    public int ProcessedState { get; set; }

    [Display(Name = "RevaluationArticleTDES_Comments", ResourceType = typeof (Resources))]
    [StringLength(150)]
    public string Comments { get; set; }
  }
}

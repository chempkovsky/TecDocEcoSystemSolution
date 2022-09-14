// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.IncomeArticleTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class IncomeArticleTDES
  {
    [Required]
    [Display(Name = "IncomeArticleTDES_SupArticle", ResourceType = typeof (Resources))]
    [Column(Order = 0)]
    [Key]
    [StringLength(40)]
    public string SupArticle { get; set; }

    [Key]
    [Required]
    [StringLength(40)]
    [Display(Name = "IncomeArticleTDES_SupBrandNic", ResourceType = typeof (Resources))]
    [Column(Order = 1)]
    public string SupBrandNic { get; set; }

    [Display(Name = "IncomePayRollTDES_IncomePayRollTDES", ResourceType = typeof (Resources))]
    [Required]
    [Key]
    [Column(Order = 2)]
    public Guid IncomePayRollTDESGuid { get; set; }

    public virtual IncomePayRollTDES IncomePayRollTDES { get; set; }

    [StringLength(40)]
    [Display(Name = "EnterpriseArticleTDES_EntArticle", ResourceType = typeof (Resources))]
    [Required]
    public string EntArticle { get; set; }

    [Display(Name = "EnterpriseArticleTDES_EntBrandNic", ResourceType = typeof (Resources))]
    [StringLength(40)]
    [Required]
    public string EntBrandNic { get; set; }

    [StringLength(120)]
    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    public string EntArticleDescription { get; set; }

    [Display(Name = "IsProcessed", ResourceType = typeof (Resources))]
    [Required]
    public bool IsProcessed { get; set; }

    [Display(Name = "IsReversed", ResourceType = typeof (Resources))]
    [Required]
    public bool IsReversed { get; set; }

    [Required]
    [Range(1, 2147483647)]
    [Display(Name = "IncomeArticleTDES_ArtAmount", ResourceType = typeof (Resources))]
    public int ArtAmount { get; set; }

    [Range(0, 2147483647)]
    [Required]
    [Display(Name = "IncomeArticleTDES_ArtAmountRest", ResourceType = typeof (Resources))]
    public int ArtAmountRest { get; set; }

    [Display(Name = "IncomeArticleTDES_PurchasePrice", ResourceType = typeof (Resources))]
    [Required]
    [Range(0.0, double.MaxValue)]
    public double PurchasePrice { get; set; }

    [Display(Name = "IncomeArticleTDES_ArtPrice", ResourceType = typeof (Resources))]
    [Required]
    [Range(0.0, double.MaxValue)]
    public double ArtPrice { get; set; }

    [Display(Name = "IncomeArticleTDES_CurrArtPrice", ResourceType = typeof (Resources))]
    [Required]
    [Range(0.0, double.MaxValue)]
    public double CurrArtPrice { get; set; }

    [Required]
    [Display(Name = "IncomeArticleTDES_IsRevaluate", ResourceType = typeof (Resources))]
    public bool IsRevaluate { get; set; }

    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntBranchGuid { get; set; }

    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntGuid { get; set; }

    [Display(Name = "RevaluationArticleTDES_Comments", ResourceType = typeof (Resources))]
    [StringLength(150)]
    public string Comments { get; set; }

    [Required]
    public int ProcessedState { get; set; }
  }
}

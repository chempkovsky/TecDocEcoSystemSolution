// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.SaleBasketArticleTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class SaleBasketArticleTDES
  {
    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntArticle", ResourceType = typeof (Resources))]
    [Key]
    [Column(Order = 0)]
    [StringLength(40)]
    public string EntArticle { get; set; }

    [StringLength(40)]
    [Column(Order = 1)]
    [Display(Name = "EnterpriseArticleTDES_EntBrandNic", ResourceType = typeof (Resources))]
    [Required]
    [Key]
    public string EntBrandNic { get; set; }

    [Column(Order = 2)]
    [Key]
    [Required]
    [Display(Name = "SaleBasketTDES_EntBasketGuid", ResourceType = typeof (Resources))]
    public Guid EntBasketGuid { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    public int EntArticleDescriptionId { get; set; }

    public virtual SaleArticleDescriptionTDES SaleArticleDescriptionTDES { get; set; }

    [StringLength(40)]
    [Display(Name = "EnterpriseArticleTDES_ExternArticle", ResourceType = typeof (Resources))]
    [Required]
    public string ExternArticle { get; set; }

    [StringLength(40)]
    [Required]
    [Display(Name = "EnterpriseArticleTDES_ExternBrandNic", ResourceType = typeof (Resources))]
    public string ExternBrandNic { get; set; }

    [StringLength(20)]
    [Display(Name = "EnterpriseArticleTDES_ExternArticleEAN", ResourceType = typeof (Resources))]
    public string ExternArticleEAN { get; set; }

    [Display(Name = "EnterpriseArticleTDES_EntGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntGuid { get; set; }

    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntBranchGuid { get; set; }

    [StringLength(40)]
    [Required]
    [Display(Name = "EnterpriseUserTDES_EntUserNic", ResourceType = typeof (Resources))]
    public string EntUserNic { get; set; }

    [Display(Name = "SaleBasketArticleTDES_WorkPlaceGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid WorkPlaceGuid { get; set; }

    [Display(Name = "BranchSpellTDES_SpellGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid SpellGuid { get; set; }

    [Display(Name = "IsPaid", ResourceType = typeof (Resources))]
    [Required]
    public bool IsPaid { get; set; }

    [Display(Name = "SaleBasketTDES_PaidAt", ResourceType = typeof (Resources))]
    [Required]
    public DateTime PaidAt { get; set; }

    [Required]
    [Display(Name = "SaleBasketArticle_ArtAmount", ResourceType = typeof (Resources))]
    public int ArtAmount { get; set; }

    [Display(Name = "SaleBasketArticleTDES_SalePrice", ResourceType = typeof (Resources))]
    [Required]
    public double SalePrice { get; set; }

    [Required]
    [Display(Name = "ReverseAmount", ResourceType = typeof (Resources))]
    public int ReverseAmount { get; set; }

    [Required]
    public bool IsSpellClosed { get; set; }

    [Required]
    [Display(Name = "CribFromIncome", ResourceType = typeof (Resources))]
    public int CribFromIncome { get; set; }
  }
}

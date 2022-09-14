// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.ReturnBasketArticleTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class ReturnBasketArticleTDES
  {
    [StringLength(40)]
    [Key]
    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntArticle", ResourceType = typeof (Resources))]
    [Column(Order = 0)]
    public string EntArticle { get; set; }

    [Display(Name = "EnterpriseArticleTDES_EntBrandNic", ResourceType = typeof (Resources))]
    [Key]
    [Column(Order = 1)]
    [Required]
    [StringLength(40)]
    public string EntBrandNic { get; set; }

    [Column(Order = 2)]
    [Required]
    [Key]
    [Display(Name = "SaleBasketTDES_EntBasketGuid", ResourceType = typeof (Resources))]
    public Guid EntBasketGuid { get; set; }

    [Key]
    [Display(Name = "ReturnBasketTDES_EntBasketGuid", ResourceType = typeof (Resources))]
    [Column(Order = 3)]
    [Required]
    public Guid RetBasketGuid { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    public int EntArticleDescriptionId { get; set; }

    public virtual SaleArticleDescriptionTDES SaleArticleDescriptionTDES { get; set; }

    [Display(Name = "EnterpriseArticleTDES_EntGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntGuid { get; set; }

    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [Display(Name = "EnterpriseUserTDES_EntUserNic", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    public string EntUserNic { get; set; }

    [Required]
    [Display(Name = "SaleBasketArticleTDES_WorkPlaceGuid", ResourceType = typeof (Resources))]
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

    [Display(Name = "ReturnBasketArticleTDES_ArtAmount", ResourceType = typeof (Resources))]
    [Required]
    public int ArtAmount { get; set; }

    [Required]
    [Display(Name = "SaleBasketArticleTDES_SalePrice", ResourceType = typeof (Resources))]
    public double SalePrice { get; set; }

    [Display(Name = "ReturnBasketArticleTDES_ReverseAmount", ResourceType = typeof (Resources))]
    [Required]
    public int ReverseAmount { get; set; }

    [Required]
    public bool IsSpellClosed { get; set; }

    [Required]
    [Display(Name = "ReCribFromIncome", ResourceType = typeof (Resources))]
    public int CribFromIncome { get; set; }
  }
}

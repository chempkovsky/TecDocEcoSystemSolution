// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.SaleBasketArticleTmp
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class SaleBasketArticleTmp
  {
    [StringLength(40)]
    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntArticle", ResourceType = typeof (Resources))]
    public string EntArticle { get; set; }

    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntBrandNic", ResourceType = typeof (Resources))]
    [StringLength(40)]
    public string EntBrandNic { get; set; }

    [Required]
    [Display(Name = "SaleBasketTDES_EntBasketGuid", ResourceType = typeof (Resources))]
    public Guid EntBasketGuid { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    public int EntArticleDescriptionId { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    public string EntArticleDescription { get; set; }

    [Required]
    [Display(Name = "EnterpriseArticleTDES_ExternArticle", ResourceType = typeof (Resources))]
    [StringLength(40)]
    public string ExternArticle { get; set; }

    [Display(Name = "EnterpriseArticleTDES_ExternBrandNic", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    public string ExternBrandNic { get; set; }

    [StringLength(20)]
    [Display(Name = "EnterpriseArticleTDES_ExternArticleEAN", ResourceType = typeof (Resources))]
    public string ExternArticleEAN { get; set; }

    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [Required]
    [StringLength(40)]
    [Display(Name = "EnterpriseUserTDES_EntUserNic", ResourceType = typeof (Resources))]
    public string EntUserNic { get; set; }

    [Required]
    [Display(Name = "SaleBasketArticleTDES_WorkPlaceGuid", ResourceType = typeof (Resources))]
    public Guid WorkPlaceGuid { get; set; }

    [Required]
    [Display(Name = "BranchSpellTDES_SpellGuid", ResourceType = typeof (Resources))]
    public Guid SpellGuid { get; set; }

    [Required]
    [Display(Name = "IsPaid", ResourceType = typeof (Resources))]
    public bool IsPaid { get; set; }

    [Display(Name = "SaleBasketTDES_PaidAt", ResourceType = typeof (Resources))]
    [Required]
    public DateTime PaidAt { get; set; }

    [Required]
    [Display(Name = "SaleBasketArticle_ArtAmount", ResourceType = typeof (Resources))]
    public int ArtAmount { get; set; }

    [Required]
    [Display(Name = "SaleBasketArticleTDES_SalePrice", ResourceType = typeof (Resources))]
    public double SalePrice { get; set; }

    [Required]
    [Display(Name = "ReverseAmount", ResourceType = typeof (Resources))]
    public int ReverseAmount { get; set; }

    public bool IsSpellClosed { get; set; }

    [Required]
    [Display(Name = "CribFromIncome", ResourceType = typeof (Resources))]
    public int CribFromIncome { get; set; }

    public void CopyFrom(SaleBasketArticleTDES src)
    {
      if (src == null)
        return;
      this.EntArticle = src.EntArticle;
      this.EntBrandNic = src.EntBrandNic;
      this.EntBasketGuid = src.EntBasketGuid;
      this.EntArticleDescriptionId = src.EntArticleDescriptionId;
      if (src.SaleArticleDescriptionTDES != null)
        this.EntArticleDescription = src.SaleArticleDescriptionTDES.EntArticleDescription;
      this.ExternArticle = src.ExternArticle;
      this.ExternBrandNic = src.ExternBrandNic;
      this.ExternArticleEAN = src.ExternArticleEAN;
      this.EntGuid = src.EntGuid;
      this.EntBranchGuid = src.EntBranchGuid;
      this.EntUserNic = src.EntUserNic;
      this.WorkPlaceGuid = src.WorkPlaceGuid;
      this.SpellGuid = src.SpellGuid;
      this.IsPaid = src.IsPaid;
      this.PaidAt = src.PaidAt;
      this.ArtAmount = src.ArtAmount;
      this.SalePrice = src.SalePrice;
      this.ReverseAmount = src.ReverseAmount;
      this.IsSpellClosed = src.IsSpellClosed;
      this.CribFromIncome = src.CribFromIncome;
    }

    public void CopyTo(SaleBasketArticleTDES dest, bool doCreateDescr)
    {
      if (dest == null)
        return;
      dest.EntArticle = this.EntArticle;
      dest.EntBrandNic = this.EntBrandNic;
      dest.EntBasketGuid = this.EntBasketGuid;
      dest.EntArticleDescriptionId = this.EntArticleDescriptionId;
      if (doCreateDescr)
      {
        if (dest.SaleArticleDescriptionTDES == null)
          dest.SaleArticleDescriptionTDES = new SaleArticleDescriptionTDES();
        dest.SaleArticleDescriptionTDES.EntArticleDescription = this.EntArticleDescription;
      }
      dest.ExternArticle = this.ExternArticle;
      dest.ExternBrandNic = this.ExternBrandNic;
      dest.ExternArticleEAN = this.ExternArticleEAN;
      dest.EntGuid = this.EntGuid;
      dest.EntBranchGuid = this.EntBranchGuid;
      dest.EntUserNic = this.EntUserNic;
      dest.WorkPlaceGuid = this.WorkPlaceGuid;
      dest.SpellGuid = this.SpellGuid;
      dest.IsPaid = this.IsPaid;
      dest.PaidAt = this.PaidAt;
      dest.ArtAmount = this.ArtAmount;
      dest.SalePrice = this.SalePrice;
      dest.ReverseAmount = this.ReverseAmount;
      dest.IsSpellClosed = this.IsSpellClosed;
      dest.CribFromIncome = this.CribFromIncome;
    }
  }
}

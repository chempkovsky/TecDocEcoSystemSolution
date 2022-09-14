// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.ReturnBasketArticleTmp
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class ReturnBasketArticleTmp
  {
    [Display(Name = "EnterpriseArticleTDES_EntArticle", ResourceType = typeof (Resources))]
    [Required]
    [Column(Order = 0)]
    [StringLength(40)]
    [Key]
    public string EntArticle { get; set; }

    [Display(Name = "EnterpriseArticleTDES_EntBrandNic", ResourceType = typeof (Resources))]
    [Key]
    [Required]
    [StringLength(40)]
    [Column(Order = 1)]
    public string EntBrandNic { get; set; }

    [Key]
    [Required]
    [Display(Name = "SaleBasketTDES_EntBasketGuid", ResourceType = typeof (Resources))]
    [Column(Order = 2)]
    public Guid EntBasketGuid { get; set; }

    [Required]
    [Display(Name = "ReturnBasketTDES_EntBasketGuid", ResourceType = typeof (Resources))]
    [Column(Order = 3)]
    [Key]
    public Guid RetBasketGuid { get; set; }

    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int EntArticleDescriptionId { get; set; }

    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    [StringLength(120)]
    public string EntArticleDescription { get; set; }

    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
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

    [Required]
    [Display(Name = "IsPaid", ResourceType = typeof (Resources))]
    public bool IsPaid { get; set; }

    [Display(Name = "SaleBasketTDES_PaidAt", ResourceType = typeof (Resources))]
    [Required]
    public DateTime PaidAt { get; set; }

    [Display(Name = "ReturnBasketArticleTDES_ArtAmount", ResourceType = typeof (Resources))]
    [Required]
    public int ArtAmount { get; set; }

    [Display(Name = "SaleBasketArticleTDES_SalePrice", ResourceType = typeof (Resources))]
    [Required]
    public double SalePrice { get; set; }

    [Display(Name = "ReturnBasketArticleTDES_ReverseAmount", ResourceType = typeof (Resources))]
    [Required]
    public int ReverseAmount { get; set; }

    [Required]
    public bool IsSpellClosed { get; set; }

    [Required]
    [Display(Name = "CribFromIncome", ResourceType = typeof (Resources))]
    public int CribFromIncome { get; set; }

    public void CopyFrom(ReturnBasketArticleTDES src)
    {
      if (src == null)
        return;
      this.EntArticle = src.EntArticle;
      this.EntBrandNic = src.EntBrandNic;
      this.EntBasketGuid = src.EntBasketGuid;
      this.RetBasketGuid = src.RetBasketGuid;
      this.EntArticleDescriptionId = src.EntArticleDescriptionId;
      if (src.SaleArticleDescriptionTDES != null)
        this.EntArticleDescription = src.SaleArticleDescriptionTDES.EntArticleDescription;
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

    public void CopyTo(ReturnBasketArticleTDES dest, bool doCreateDescr)
    {
      if (dest == null)
        return;
      dest.EntArticle = this.EntArticle;
      dest.EntBrandNic = this.EntBrandNic;
      dest.EntBasketGuid = this.EntBasketGuid;
      dest.RetBasketGuid = this.RetBasketGuid;
      dest.EntArticleDescriptionId = this.EntArticleDescriptionId;
      if (doCreateDescr)
      {
        if (dest.SaleArticleDescriptionTDES == null)
          dest.SaleArticleDescriptionTDES = new SaleArticleDescriptionTDES();
        dest.SaleArticleDescriptionTDES.EntArticleDescription = this.EntArticleDescription;
      }
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

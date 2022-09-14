// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.GuestOrderArticleTmp
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class GuestOrderArticleTmp
  {
    [Key]
    [Required]
    [Display(Name = "GuestOrderTDES_GuestOrderGuid", ResourceType = typeof (Resources))]
    [Column(Order = 0)]
    public Guid GuestOrderGuid { get; set; }

    [Key]
    [Display(Name = "GuestOrderArticleTDES_EntBranchArticle", ResourceType = typeof (Resources))]
    [Column(Order = 1)]
    [Required]
    [StringLength(40)]
    public string EntBranchArticle { get; set; }

    [StringLength(40)]
    [Display(Name = "GuestOrderArticleTDES_EntBranchSup", ResourceType = typeof (Resources))]
    [Column(Order = 2)]
    [Key]
    [Required]
    public string EntBranchSup { get; set; }

    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(120)]
    public string EntArticleDescription { get; set; }

    [Display(Name = "GuestOrderArticleTDES_ART_ARTICLE_NR", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    public string ART_ARTICLE_NR { get; set; }

    [Required]
    [StringLength(40)]
    [Display(Name = "GuestOrderArticleTDES_SUP_TEXT", ResourceType = typeof (Resources))]
    public string SUP_TEXT { get; set; }

    [StringLength(20)]
    [Display(Name = "GuestOrderArticleTDES_ExternArticleEAN", ResourceType = typeof (Resources))]
    public string ExternArticleEAN { get; set; }

    [Required]
    [Display(Name = "GuestOrderArticleTDES_ArtAmount", ResourceType = typeof (Resources))]
    public int ArtAmount { get; set; }

    [Display(Name = "GuestOrderArticleTDES_ArtPrice", ResourceType = typeof (Resources))]
    [Required]
    public double ArtPrice { get; set; }

    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [Display(Name = "GuestOrderArticleTDES_LastUpdated", ResourceType = typeof (Resources))]
    [Required]
    public DateTime LastUpdated { get; set; }

    [Display(Name = "GuestOrderArticleTDES_LastReplicated", ResourceType = typeof (Resources))]
    [Required]
    public DateTime LastReplicated { get; set; }

    public void CopyFrom(GuestOrderArticleTDES src)
    {
      if (src == null)
        return;
      this.GuestOrderGuid = src.GuestOrderGuid;
      this.EntBranchArticle = src.EntBranchArticle;
      this.EntBranchSup = src.EntBranchSup;
      this.EntArticleDescription = src.EntArticleDescription;
      this.ART_ARTICLE_NR = src.ART_ARTICLE_NR;
      this.SUP_TEXT = src.SUP_TEXT;
      this.ExternArticleEAN = src.ExternArticleEAN;
      this.ArtAmount = src.ArtAmount;
      this.ArtPrice = src.ArtPrice;
      this.EntBranchGuid = src.EntBranchGuid;
      this.LastUpdated = src.LastUpdated;
      this.LastReplicated = src.LastReplicated;
    }

    public void CopyTo(GuestOrderArticleTDES dest)
    {
      if (dest == null)
        return;
      dest.GuestOrderGuid = this.GuestOrderGuid;
      dest.EntBranchArticle = this.EntBranchArticle;
      dest.EntBranchSup = this.EntBranchSup;
      dest.EntArticleDescription = this.EntArticleDescription;
      dest.ART_ARTICLE_NR = this.ART_ARTICLE_NR;
      dest.SUP_TEXT = this.SUP_TEXT;
      dest.ExternArticleEAN = this.ExternArticleEAN;
      dest.ArtAmount = this.ArtAmount;
      dest.ArtPrice = this.ArtPrice;
      dest.EntBranchGuid = this.EntBranchGuid;
      dest.LastUpdated = this.LastUpdated;
      dest.LastReplicated = this.LastReplicated;
    }
  }
}

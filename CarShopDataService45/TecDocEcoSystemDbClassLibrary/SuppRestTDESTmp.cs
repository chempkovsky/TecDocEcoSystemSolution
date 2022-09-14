// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.SuppRestTDESTmp
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class SuppRestTDESTmp
  {
    [StringLength(40)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 0)]
    [Key]
    [Required]
    [Display(Name = "EnterpriseSupplierTDES_EntSupplierId", ResourceType = typeof (Resources))]
    public string EntSupplierId { get; set; }

    [StringLength(40)]
    [Required]
    [Key]
    [Display(Name = "BranchRestTDES_EntBranchArticle", ResourceType = typeof (Resources))]
    [Column(Order = 1)]
    public string EntBranchArticle { get; set; }

    [Column(Order = 2)]
    [Required]
    [StringLength(40)]
    [Display(Name = "BranchRestTDES_EntBranchSup", ResourceType = typeof (Resources))]
    [Key]
    public string EntBranchSup { get; set; }

    [Display(Name = "BranchRestTDES_ART_ARTICLE_NR", ResourceType = typeof (Resources))]
    [StringLength(40)]
    [Required]
    public string ART_ARTICLE_NR { get; set; }

    [Required]
    [Display(Name = "BranchRestTDES_SUP_TEXT", ResourceType = typeof (Resources))]
    [StringLength(40)]
    public string SUP_TEXT { get; set; }

    [Required]
    [Display(Name = "BranchRestTDES_ArtAmount", ResourceType = typeof (Resources))]
    public int ArtAmount { get; set; }

    [Display(Name = "BranchRestTDES_ArtPrice", ResourceType = typeof (Resources))]
    [Required]
    public double ArtPrice { get; set; }

    [Required]
    [Display(Name = "BranchRestTDES_LastUpdated", ResourceType = typeof (Resources))]
    public DateTime LastUpdated { get; set; }

    [Display(Name = "BranchRestTDES_LastReplicated", ResourceType = typeof (Resources))]
    [Required]
    public DateTime LastReplicated { get; set; }

    [Timestamp]
    [Display(Name = "BranchRestTDES_TSConcClmn", ResourceType = typeof (Resources))]
    public byte[] TSConcClmn { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    public int EntArticleDescriptionId { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    public string EntArticleDescription { get; set; }

    [StringLength(20)]
    [Display(Name = "EnterpriseArticleTDES_ExternArticleEAN", ResourceType = typeof (Resources))]
    public string ExternArticleEAN { get; set; }

    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    public void CopyFrom(SuppRestTDES src)
    {
      if (src == null)
        return;
      this.EntSupplierId = src.EntSupplierId;
      this.EntBranchArticle = src.EntBranchArticle;
      this.EntBranchSup = src.EntBranchSup;
      this.ART_ARTICLE_NR = src.ART_ARTICLE_NR;
      this.SUP_TEXT = src.SUP_TEXT;
      this.ArtAmount = src.ArtAmount;
      this.ArtPrice = src.ArtPrice;
      this.LastUpdated = src.LastUpdated;
      this.LastReplicated = src.LastReplicated;
      this.TSConcClmn = src.TSConcClmn;
      this.ExternArticleEAN = src.ExternArticleEAN;
      this.EntArticleDescriptionId = src.EntArticleDescriptionId;
      if (src.BranchRestArticleDescriptionTDES == null)
        return;
      this.EntArticleDescription = src.BranchRestArticleDescriptionTDES.EntArticleDescription;
    }

    public void CopyTo(SuppRestTDES dest, bool doCreateDescr)
    {
      if (dest == null)
        return;
      dest.EntSupplierId = this.EntSupplierId;
      dest.EntBranchArticle = this.EntBranchArticle;
      dest.EntBranchSup = this.EntBranchSup;
      dest.ART_ARTICLE_NR = this.ART_ARTICLE_NR;
      dest.SUP_TEXT = this.SUP_TEXT;
      dest.ArtAmount = this.ArtAmount;
      dest.ArtPrice = this.ArtPrice;
      dest.LastUpdated = this.LastUpdated;
      dest.LastReplicated = this.LastReplicated;
      dest.TSConcClmn = this.TSConcClmn;
      dest.ExternArticleEAN = this.ExternArticleEAN;
      dest.EntArticleDescriptionId = this.EntArticleDescriptionId;
      if (!doCreateDescr)
        return;
      if (dest.BranchRestArticleDescriptionTDES == null)
        dest.BranchRestArticleDescriptionTDES = new BranchRestArticleDescriptionTDES();
      dest.BranchRestArticleDescriptionTDES.EntArticleDescription = this.EntArticleDescription;
      dest.BranchRestArticleDescriptionTDES.EntArticleDescriptionId = this.EntArticleDescriptionId;
    }
  }
}

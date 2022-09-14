// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseArticleTmp
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseArticleTmp
  {
    [StringLength(40)]
    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntArticle", ResourceType = typeof (Resources))]
    public string EntArticle { get; set; }

    [StringLength(40)]
    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntBrandNic", ResourceType = typeof (Resources))]
    public string EntBrandNic { get; set; }

    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    [Required]
    public int EntArticleDescriptionId { get; set; }

    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    [StringLength(120)]
    public string EntArticleDescription { get; set; }

    [Required]
    [StringLength(40)]
    [Display(Name = "EnterpriseArticleTDES_ExternArticle", ResourceType = typeof (Resources))]
    public string ExternArticle { get; set; }

    [Display(Name = "EnterpriseArticleTDES_ExternBrandNic", ResourceType = typeof (Resources))]
    [StringLength(40)]
    [Required]
    public string ExternBrandNic { get; set; }

    [StringLength(20)]
    [Display(Name = "EnterpriseArticleTDES_ExternArticleEAN", ResourceType = typeof (Resources))]
    public string ExternArticleEAN { get; set; }

    public void CopyFrom(EnterpriseArticleTDES src)
    {
      if (src == null)
        return;
      this.EntArticle = src.EntArticle;
      this.EntBrandNic = src.EntBrandNic;
      this.EntGuid = src.EntGuid;
      this.EntArticleDescriptionId = src.EntArticleDescriptionId;
      if (src.EnterpriseArticleDescriptionTDES != null)
        this.EntArticleDescription = src.EnterpriseArticleDescriptionTDES.EntArticleDescription;
      this.ExternArticle = src.ExternArticle;
      this.ExternBrandNic = src.ExternBrandNic;
      this.ExternArticleEAN = src.ExternArticleEAN;
    }

    public void CopyTo(EnterpriseArticleTDES dest, bool doCreateDescr)
    {
      if (dest == null)
        return;
      dest.EntArticle = this.EntArticle;
      dest.EntBrandNic = this.EntBrandNic;
      dest.EntGuid = this.EntGuid;
      dest.EntArticleDescriptionId = this.EntArticleDescriptionId;
      if (doCreateDescr)
      {
        if (dest.EnterpriseArticleDescriptionTDES == null)
          dest.EnterpriseArticleDescriptionTDES = new EnterpriseArticleDescriptionTDES();
        dest.EnterpriseArticleDescriptionTDES.EntArticleDescription = this.EntArticleDescription;
        dest.EnterpriseArticleDescriptionTDES.EntArticleDescriptionId = this.EntArticleDescriptionId;
      }
      dest.ExternArticle = this.ExternArticle;
      dest.ExternBrandNic = this.ExternBrandNic;
      dest.ExternArticleEAN = this.ExternArticleEAN;
    }
  }
}

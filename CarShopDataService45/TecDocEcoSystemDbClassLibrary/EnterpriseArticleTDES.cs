// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseArticleTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseArticleTDES
  {
    [Key]
    [Column(Order = 0)]
    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntArticle", ResourceType = typeof (Resources))]
    [StringLength(40)]
    public string EntArticle { get; set; }

    [Required]
    [Column(Order = 1)]
    [Key]
    [StringLength(40)]
    [Display(Name = "EnterpriseArticleTDES_EntBrandNic", ResourceType = typeof (Resources))]
    public string EntBrandNic { get; set; }

    [Key]
    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntGuid", ResourceType = typeof (Resources))]
    [Column(Order = 2)]
    public Guid EntGuid { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    public int EntArticleDescriptionId { get; set; }

    public virtual EnterpriseArticleDescriptionTDES EnterpriseArticleDescriptionTDES { get; set; }

    [Required]
    [StringLength(40)]
    [Display(Name = "EnterpriseArticleTDES_ExternArticle", ResourceType = typeof (Resources))]
    public string ExternArticle { get; set; }

    [Display(Name = "EnterpriseArticleTDES_ExternBrandNic", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    public string ExternBrandNic { get; set; }

    [Display(Name = "EnterpriseArticleTDES_ExternArticleEAN", ResourceType = typeof (Resources))]
    [StringLength(20)]
    public string ExternArticleEAN { get; set; }

    public virtual EnterpriseBrandTDES EnterpriseBrand { get; set; }
  }
}

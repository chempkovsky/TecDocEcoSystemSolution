// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.SuppRestTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class SuppRestTDES
  {
    [StringLength(40)]
    [Required]
    [Display(Name = "EnterpriseSupplierTDES_EntSupplierId", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [Column(Order = 0)]
    public string EntSupplierId { get; set; }

    [StringLength(40)]
    [Key]
    [Display(Name = "BranchRestTDES_EntBranchArticle", ResourceType = typeof (Resources))]
    [Column(Order = 1)]
    [Required]
    public string EntBranchArticle { get; set; }

    [Display(Name = "BranchRestTDES_EntBranchSup", ResourceType = typeof (Resources))]
    [Key]
    [Column(Order = 2)]
    [Required]
    [StringLength(40)]
    public string EntBranchSup { get; set; }

    [StringLength(40)]
    [Display(Name = "BranchRestTDES_ART_ARTICLE_NR", ResourceType = typeof (Resources))]
    [Required]
    public string ART_ARTICLE_NR { get; set; }

    [Display(Name = "BranchRestTDES_SUP_TEXT", ResourceType = typeof (Resources))]
    [StringLength(40)]
    [Required]
    public string SUP_TEXT { get; set; }

    [Required]
    [Display(Name = "BranchRestTDES_ArtAmount", ResourceType = typeof (Resources))]
    public int ArtAmount { get; set; }

    [Required]
    [Display(Name = "BranchRestTDES_ArtPrice", ResourceType = typeof (Resources))]
    public double ArtPrice { get; set; }

    [Required]
    [Display(Name = "BranchRestTDES_LastUpdated", ResourceType = typeof (Resources))]
    public DateTime LastUpdated { get; set; }

    [Required]
    [Display(Name = "BranchRestTDES_LastReplicated", ResourceType = typeof (Resources))]
    public DateTime LastReplicated { get; set; }

    [Timestamp]
    [Display(Name = "BranchRestTDES_TSConcClmn", ResourceType = typeof (Resources))]
    public byte[] TSConcClmn { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    [Required]
    public int EntArticleDescriptionId { get; set; }

    public virtual BranchRestArticleDescriptionTDES BranchRestArticleDescriptionTDES { get; set; }

    [Display(Name = "EnterpriseArticleTDES_ExternArticleEAN", ResourceType = typeof (Resources))]
    [StringLength(20)]
    public string ExternArticleEAN { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Display(Name = "BranchRestTDES_IsReplicated", ResourceType = typeof (Resources))]
    public int IsReplicated { get; private set; }
  }
}

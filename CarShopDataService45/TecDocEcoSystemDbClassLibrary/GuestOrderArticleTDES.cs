// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.GuestOrderArticleTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class GuestOrderArticleTDES
  {
    [Key]
    [Required]
    [Display(Name = "GuestOrderTDES_GuestOrderGuid", ResourceType = typeof (Resources))]
    [Column(Order = 0)]
    public Guid GuestOrderGuid { get; set; }

    public virtual GuestOrderTDES GuestOrderTDES { get; set; }

    [Display(Name = "GuestOrderArticleTDES_EntBranchArticle", ResourceType = typeof (Resources))]
    [Key]
    [Column(Order = 1)]
    [StringLength(40)]
    [Required]
    public string EntBranchArticle { get; set; }

    [Key]
    [Display(Name = "GuestOrderArticleTDES_EntBranchSup", ResourceType = typeof (Resources))]
    [Column(Order = 2)]
    [Required]
    [StringLength(40)]
    public string EntBranchSup { get; set; }

    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(120)]
    public string EntArticleDescription { get; set; }

    [StringLength(40)]
    [Display(Name = "GuestOrderArticleTDES_ART_ARTICLE_NR", ResourceType = typeof (Resources))]
    [Required]
    public string ART_ARTICLE_NR { get; set; }

    [Required]
    [StringLength(40)]
    [Display(Name = "GuestOrderArticleTDES_SUP_TEXT", ResourceType = typeof (Resources))]
    public string SUP_TEXT { get; set; }

    [Display(Name = "GuestOrderArticleTDES_ExternArticleEAN", ResourceType = typeof (Resources))]
    [StringLength(20)]
    public string ExternArticleEAN { get; set; }

    [Required]
    [Display(Name = "GuestOrderArticleTDES_ArtAmount", ResourceType = typeof (Resources))]
    public int ArtAmount { get; set; }

    [Display(Name = "GuestOrderArticleTDES_ArtPrice", ResourceType = typeof (Resources))]
    [Required]
    public double ArtPrice { get; set; }

    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntBranchGuid { get; set; }

    [Display(Name = "GuestOrderArticleTDES_LastUpdated", ResourceType = typeof (Resources))]
    [Required]
    public DateTime LastUpdated { get; set; }

    [Required]
    [Display(Name = "GuestOrderArticleTDES_LastReplicated", ResourceType = typeof (Resources))]
    public DateTime LastReplicated { get; set; }

    [Display(Name = "GuestOrderArticleTDES_IsReplicated", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int IsReplicated { get; private set; }
  }
}

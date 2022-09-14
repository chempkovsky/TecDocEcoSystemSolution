// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseArticleLookUpTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseArticleLookUpTDES
  {
    [Display(Name = "EnterpriseArticleLookUpTDES_ArticleSearch", ResourceType = typeof (Resources))]
    [StringLength(105)]
    [Required]
    [Key]
    [Column(Order = 0)]
    public string ArticleSearch { get; set; }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 1)]
    [Required]
    [Display(Name = "EnterpriseArticleCategoryItemTDES_ArticleId", ResourceType = typeof (Resources))]
    public int ArticleId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [Key]
    [Column(Order = 2)]
    [Display(Name = "EnterpriseArticleBrandTDES_ArticleBrandId", ResourceType = typeof (Resources))]
    public int ArticleBrandId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseArticleBrandTDES_ArticleSearchKind", ResourceType = typeof (Resources))]
    [Required]
    [Key]
    [Column(Order = 3)]
    public int ArticleSearchKind { get; set; }

    [StringLength(105)]
    [Display(Name = "EnterpriseArticleLookUpTDES_ArticleDysplay", ResourceType = typeof (Resources))]
    public string ArticleDysplay { get; set; }
  }
}

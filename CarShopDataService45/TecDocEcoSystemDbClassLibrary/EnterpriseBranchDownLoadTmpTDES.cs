// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseBranchDownLoadTmpTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseBranchDownLoadTmpTDES
  {
    [Key]
    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    [Column(Order = 0)]
    public Guid EntBranchGuid { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "EntBranchDescription", ResourceType = typeof (Resources))]
    public string EntBranchDescription { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_SrcType", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int SrcType { get; set; }

    [Required]
    [StringLength(80)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_SrcFile", ResourceType = typeof (Resources))]
    public string SrcFile { get; set; }

    [StringLength(2)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_FldDelim", ResourceType = typeof (Resources))]
    public string FldDelim { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_DeleteFirstLines", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int DeleteFirstLines { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_SkipFirstLines", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int SkipFirstLines { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_Win1251ToUtf8", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public bool Win1251ToUtf8 { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_BookToRead", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int BookToRead { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_FldMapType", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int FldMapType { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_SubFldDelim", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(5)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string SubFldDelim { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_ExternArticle", ResourceType = typeof (Resources))]
    [Required]
    public int ExternArticle { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_ExternBrandNic", ResourceType = typeof (Resources))]
    public int ExternBrandNic { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_ExternArticleAmount", ResourceType = typeof (Resources))]
    public int ExternArticleAmount { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_ExternArticlePrice", ResourceType = typeof (Resources))]
    public int ExternArticlePrice { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_IgnorIfZeroPrice", ResourceType = typeof (Resources))]
    public bool IgnorIfZeroPrice { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_IgnorIfZeroAmount", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public bool IgnorIfZeroAmount { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_ZeroRestBeforeImport", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public bool ZeroRestBeforeImport { get; set; }

    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }
  }
}

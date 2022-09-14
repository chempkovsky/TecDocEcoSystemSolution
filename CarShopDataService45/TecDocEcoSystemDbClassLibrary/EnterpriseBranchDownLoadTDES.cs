// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseBranchDownLoadTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseBranchDownLoadTDES
  {
    [Column(Order = 0)]
    [Required]
    [Key]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_SrcType", ResourceType = typeof (Resources))]
    [Required]
    public int SrcType { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_SrcFile", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [StringLength(80)]
    public string SrcFile { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_FldDelim", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(2)]
    public string FldDelim { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_DeleteFirstLines", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int DeleteFirstLines { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_SkipFirstLines", ResourceType = typeof (Resources))]
    public int SkipFirstLines { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_Win1251ToUtf8", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public bool Win1251ToUtf8 { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_BookToRead", ResourceType = typeof (Resources))]
    [Required]
    public int BookToRead { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_FldMapType", ResourceType = typeof (Resources))]
    [Required]
    public int FldMapType { get; set; }

    [StringLength(5)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_SubFldDelim", ResourceType = typeof (Resources))]
    [Required]
    public string SubFldDelim { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_ExternArticle", ResourceType = typeof (Resources))]
    public int ExternArticle { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_ExternBrandNic", ResourceType = typeof (Resources))]
    [Required]
    public int ExternBrandNic { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_ExternArticleAmount", ResourceType = typeof (Resources))]
    [Required]
    public int ExternArticleAmount { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_ExternArticlePrice", ResourceType = typeof (Resources))]
    public int ExternArticlePrice { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_IgnorIfZeroPrice", ResourceType = typeof (Resources))]
    [Required]
    public bool IgnorIfZeroPrice { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_IgnorIfZeroAmount", ResourceType = typeof (Resources))]
    public bool IgnorIfZeroAmount { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_ZeroRestBeforeImport", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public bool ZeroRestBeforeImport { get; set; }

    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    public virtual ICollection<EnterpriseBranchReplaceTDES> EnterpriseBranchReplaceTDESes { get; set; }

    public virtual ICollection<EnterpriseBranchDownLoadRepTDES> EnterpriseBranchDownLoadRepTDESes { get; set; }
  }
}

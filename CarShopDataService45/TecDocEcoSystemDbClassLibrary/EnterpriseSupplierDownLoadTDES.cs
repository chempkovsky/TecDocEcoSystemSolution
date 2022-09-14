// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseSupplierDownLoadTDES
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
  public class EnterpriseSupplierDownLoadTDES
  {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    [Column(Order = 0)]
    [Key]
    public Guid EntGuid { get; set; }

    [Column(Order = 1)]
    [Required]
    [Display(Name = "EnterpriseSupplierTDES_EntSupplierId", ResourceType = typeof (Resources))]
    [Key]
    [StringLength(40)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string EntSupplierId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_SrcUrl", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(250)]
    public string SrcUrl { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_SrcType", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int SrcType { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_SrcFolder", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(80)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string SrcFolder { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_SrcFile", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(80)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string SrcFile { get; set; }

    [Required]
    [StringLength(2)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_FldDelim", ResourceType = typeof (Resources))]
    public string FldDelim { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_DeleteFirstLines", ResourceType = typeof (Resources))]
    public int DeleteFirstLines { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_SkipFirstLines", ResourceType = typeof (Resources))]
    public int SkipFirstLines { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_Win1251ToUtf8", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public bool Win1251ToUtf8 { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_BookToRead", ResourceType = typeof (Resources))]
    public int BookToRead { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_FldMapType", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int FldMapType { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [StringLength(5)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_SubFldDelim", ResourceType = typeof (Resources))]
    public string SubFldDelim { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_ExternArticle", ResourceType = typeof (Resources))]
    public int ExternArticle { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_ExternBrandNic", ResourceType = typeof (Resources))]
    [Required]
    public int ExternBrandNic { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    [Required]
    public int EntArticleDescription { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_ExternArticleEAN", ResourceType = typeof (Resources))]
    [Required]
    public int ExternArticleEAN { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_EntArticleOens", ResourceType = typeof (Resources))]
    [Required]
    public int EntArticleOens { get; set; }

    [StringLength(5)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_EntArticleOensDelim", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string EntArticleOensDelim { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_ExternArticleAmount", ResourceType = typeof (Resources))]
    public int ExternArticleAmount { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_ExternArticlePrice", ResourceType = typeof (Resources))]
    public int ExternArticlePrice { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_LookForShatem", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public bool LookForShatem { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_LookForAutoSpace", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public bool LookForAutoSpace { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_LookForMotex", ResourceType = typeof (Resources))]
    [Required]
    public bool LookForMotex { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadTDES_LookForLAuto", ResourceType = typeof (Resources))]
    public bool LookForLAuto { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_IgnorIfZeroPrice", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public bool IgnorIfZeroPrice { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadTDES_IgnorIfZeroAmount", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    public bool IgnorIfZeroAmount { get; set; }

    public virtual ICollection<EnterpriseSupplierReplaceTDES> EnterpriseSupplierReplaceTDESes { get; set; }

    public virtual ICollection<EnterpriseSupplierDownLoadRepTDES> EnterpriseSupplierDownLoadRepTDESes { get; set; }
  }
}

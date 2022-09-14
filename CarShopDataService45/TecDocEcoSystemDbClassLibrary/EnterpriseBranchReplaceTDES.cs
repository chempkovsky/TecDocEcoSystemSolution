// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseBranchReplaceTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseBranchReplaceTDES
  {
    [Key]
    [Required]
    [Column(Order = 0)]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierTDES_EntSupplierId", ResourceType = typeof (Resources))]
    [Key]
    [Column(Order = 1)]
    [Required]
    public string EntSupplierId { get; set; }

    [Column(Order = 2)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [Display(Name = "EnterpriseSupplierReplaceTDES_Order", ResourceType = typeof (Resources))]
    [Key]
    public int Order { get; set; }

    [Display(Name = "EnterpriseSupplierReplaceTDES_LookFor", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string LookFor { get; set; }

    [Display(Name = "EnterpriseSupplierReplaceTDES_ReplaceBy", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string ReplaceBy { get; set; }

    public virtual EnterpriseBranchDownLoadTDES EnterpriseBranchDownLoadTDES { get; set; }
  }
}

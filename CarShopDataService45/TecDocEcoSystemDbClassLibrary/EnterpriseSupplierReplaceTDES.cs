// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseSupplierReplaceTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseSupplierReplaceTDES
  {
    [Required]
    [Column(Order = 0)]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    [StringLength(40)]
    [Required]
    [Display(Name = "EnterpriseSupplierTDES_EntSupplierId", ResourceType = typeof (Resources))]
    [Column(Order = 1)]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string EntSupplierId { get; set; }

    [Key]
    [Display(Name = "EnterpriseSupplierReplaceTDES_Order", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 2)]
    public int Order { get; set; }

    [Required]
    [StringLength(40)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierReplaceTDES_LookFor", ResourceType = typeof (Resources))]
    public string LookFor { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierReplaceTDES_ReplaceBy", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    public string ReplaceBy { get; set; }

    public virtual EnterpriseSupplierDownLoadTDES EnterpriseSupplierDownLoadTDES { get; set; }
  }
}

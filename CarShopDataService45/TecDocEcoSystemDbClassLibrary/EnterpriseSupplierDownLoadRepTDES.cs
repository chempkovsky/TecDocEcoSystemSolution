// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseSupplierDownLoadRepTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseSupplierDownLoadRepTDES
  {
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    [Column(Order = 0)]
    public Guid EntGuid { get; set; }

    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierTDES_EntSupplierId", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    public string EntSupplierId { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadRepTDES_LastFileName", ResourceType = typeof (Resources))]
    [StringLength(120)]
    public string LastFileName { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadRepTDES_LastUpdated", ResourceType = typeof (Resources))]
    public DateTime LastUpdated { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadRepTDES_LastReplicatedStart", ResourceType = typeof (Resources))]
    public DateTime LastReplicatedStart { get; set; }

    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadRepTDES_LastReplicatedEnd", ResourceType = typeof (Resources))]
    public DateTime LastReplicatedEnd { get; set; }

    [Display(Name = "EnterpriseSupplierDownLoadRepTDES_RowPassed", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int RowPassed { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [Display(Name = "EnterpriseSupplierDownLoadRepTDES_RowUpdated", ResourceType = typeof (Resources))]
    public int RowUpdated { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadRepTDES_RowWithError", ResourceType = typeof (Resources))]
    public int RowWithError { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseSupplierDownLoadRepTDES_RowEgnored", ResourceType = typeof (Resources))]
    [Required]
    public int RowEgnored { get; set; }

    public virtual EnterpriseSupplierDownLoadTDES EnterpriseSupplierDownLoadTDES { get; set; }
  }
}

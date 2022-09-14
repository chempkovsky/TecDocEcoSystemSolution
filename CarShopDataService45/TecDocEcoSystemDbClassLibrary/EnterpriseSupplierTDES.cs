// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseSupplierTDES
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
  public class EnterpriseSupplierTDES
  {
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    [Key]
    public Guid EntGuid { get; set; }

    [Display(Name = "EnterpriseSupplierTDES_EntSupplierId", ResourceType = typeof (Resources))]
    [Column(Order = 1)]
    [Key]
    [Required]
    [StringLength(40)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string EntSupplierId { get; set; }

    [StringLength(80)]
    [Display(Name = "EntSupplierDescription", ResourceType = typeof (Resources))]
    [Required]
    public string EntSupplierDescription { get; set; }

    [Required]
    [Display(Name = "IsActive", ResourceType = typeof (Resources))]
    public bool IsActive { get; set; }

    public virtual EnterpriseTDES EnterpriseTDES { get; set; }

    public virtual ICollection<EnterpriseSupplierContactTDES> EnterpriseSupplierContactTDESes { get; set; }

    public virtual ICollection<EnterpriseSupplierAddressTDES> EnterpriseSupplierAddressTDESes { get; set; }
  }
}

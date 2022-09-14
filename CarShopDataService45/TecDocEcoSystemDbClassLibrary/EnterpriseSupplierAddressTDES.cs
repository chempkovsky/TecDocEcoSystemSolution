// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseSupplierAddressTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseSupplierAddressTDES : Address
  {
    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    [Required]
    [StringLength(40)]
    [Display(Name = "EnterpriseSupplierTDES_EntSupplierId", ResourceType = typeof (Resources))]
    public string EntSupplierId { get; set; }

    public virtual EnterpriseSupplierTDES EnterpriseSupplierTDES { get; set; }
  }
}

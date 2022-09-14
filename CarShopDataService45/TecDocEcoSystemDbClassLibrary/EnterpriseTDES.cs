// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseTDES
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
  public class EnterpriseTDES
  {
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    [Key]
    [Required]
    public Guid EntGuid { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "EnterpriseTDES_EntDescription", ResourceType = typeof (Resources))]
    public string EntDescription { get; set; }

    [Display(Name = "IsActive", ResourceType = typeof (Resources))]
    [Required]
    public bool IsActive { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "EnterpriseBranchTDES_ArticleCatalog", ResourceType = typeof (Resources))]
    public string ArticleCatalog { get; set; }

    [Display(Name = "EnterpriseTDES_TecDocSrcTypeId", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int TecDocSrcTypeId { get; set; }

    public virtual EnterpriseTecDocSrcTypeTDES EnterpriseTecDocSrcTypeTDES { get; set; }

    public virtual ICollection<EnterpriseUserTDES> EnterpriseUserTDESes { get; set; }

    public virtual ICollection<EnterpriseBranchTDES> EnterpriseBranchTDESes { get; set; }

    public virtual ICollection<EnterpriseAddressTDES> EnterpriseAddressTDESes { get; set; }

    public virtual ICollection<EnterpriseSupplierTDES> EnterpriseSupplierTDESes { get; set; }
  }
}

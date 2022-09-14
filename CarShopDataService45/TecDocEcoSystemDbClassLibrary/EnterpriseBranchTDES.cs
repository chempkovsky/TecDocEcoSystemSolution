// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseBranchTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseBranchTDES
  {
    [Required]
    [Key]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "EntBranchDescription", ResourceType = typeof (Resources))]
    public string EntBranchDescription { get; set; }

    [Required]
    [Display(Name = "IsActive", ResourceType = typeof (Resources))]
    public bool IsActive { get; set; }

    [Required]
    [Display(Name = "IsVisible", ResourceType = typeof (Resources))]
    public bool IsVisible { get; set; }

    [Required]
    [Display(Name = "BranchType", ResourceType = typeof (Resources))]
    [Range(1, 2147483647)]
    public int BranchTypeId { get; set; }

    public virtual BranchType BranchType { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "EnterpriseBranchTDES_TecDocCatalog", ResourceType = typeof (Resources))]
    public string TecDocCatalog { get; set; }

    [Required]
    [Display(Name = "EnterpriseBranchTDES_SalesCatalog", ResourceType = typeof (Resources))]
    [StringLength(120)]
    public string SalesCatalog { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "EnterpriseBranchTDES_IncomeCatalog", ResourceType = typeof (Resources))]
    public string IncomeCatalog { get; set; }

    [StringLength(120)]
    [Display(Name = "EnterpriseBranchTDES_OrderCatalog", ResourceType = typeof (Resources))]
    [Required]
    public string OrderCatalog { get; set; }

    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntGuid { get; set; }

    public virtual EnterpriseTDES EnterpriseTDES { get; set; }

    public virtual ICollection<EnterpriseBranchContactsTDES> EnterpriseBranchContactsTDESes { get; set; }

    public virtual ICollection<EnterpriseBranchUserTDES> EnterpriseBranchUserTDESes { get; set; }

    public virtual ICollection<EnterpriseBranchWorkPlaceTDES> EnterpriseBranchWorkPlaceTDESes { get; set; }

    public virtual ICollection<EnterpriseBranchAddressTDES> EnterpriseBranchAddressTDESes { get; set; }

    public virtual ICollection<TecDocEcoSystemDbClassLibrary.EnterpriseBranchReplyTDES> EnterpriseBranchReplyTDES { get; set; }
  }
}

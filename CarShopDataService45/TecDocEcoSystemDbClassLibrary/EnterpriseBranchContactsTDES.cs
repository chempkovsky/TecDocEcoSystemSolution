// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseBranchContactsTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseBranchContactsTDES
  {
    [Required]
    [Display(Name = "EnterpriseUserContactTDES_ContactGuid", ResourceType = typeof (Resources))]
    [Key]
    public Guid ContactGuid { get; set; }

    [Display(Name = "EnterpriseUserContactTDES_Contact", ResourceType = typeof (Resources))]
    [StringLength(40)]
    [Required]
    public string Contact { get; set; }

    [Required]
    [Display(Name = "IsActive", ResourceType = typeof (Resources))]
    public bool IsActive { get; set; }

    [Required]
    [Display(Name = "IsVisible", ResourceType = typeof (Resources))]
    public bool IsVisible { get; set; }

    [StringLength(120)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Description", ResourceType = typeof (Resources))]
    public string Description { get; set; }

    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    public virtual EnterpriseBranchTDES EnterpriseBranchTDES { get; set; }

    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    [Required]
    [Range(1, 2147483647)]
    [Display(Name = "EnterpriseUserContactTDES_ContactTypeId", ResourceType = typeof (Resources))]
    public int ContactTypeId { get; set; }

    public virtual ContactType ContactType { get; set; }
  }
}

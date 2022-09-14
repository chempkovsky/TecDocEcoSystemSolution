// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseUserContactTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseUserContactTDES
  {
    [Key]
    [Required]
    [Display(Name = "EnterpriseUserContactTDES_ContactGuid", ResourceType = typeof (Resources))]
    public Guid ContactGuid { get; set; }

    [StringLength(40)]
    [Display(Name = "EnterpriseUserContactTDES_Contact", ResourceType = typeof (Resources))]
    [Required]
    public string Contact { get; set; }

    [Display(Name = "IsActive", ResourceType = typeof (Resources))]
    [Required]
    public bool IsActive { get; set; }

    [Required]
    [Display(Name = "IsVisible", ResourceType = typeof (Resources))]
    public bool IsVisible { get; set; }

    [DataType(DataType.MultilineText)]
    [StringLength(120)]
    [Display(Name = "Description", ResourceType = typeof (Resources))]
    public string Description { get; set; }

    [Display(Name = "EnterpriseUserTDES_EntUserNic", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    public string EntUserNic { get; set; }

    public virtual EnterpriseUserTDES EnterpriseUserTDES { get; set; }

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

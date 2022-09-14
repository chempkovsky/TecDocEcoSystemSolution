// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseBranchUserTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseBranchUserTDES
  {
    [Key]
    [Required]
    [StringLength(40)]
    [Display(Name = "EnterpriseUserTDES_EntUserNic", ResourceType = typeof (Resources))]
    public string EntUserNic { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Password", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(30)]
    public string Password { get; set; }

    [StringLength(20)]
    [Display(Name = "FirstName", ResourceType = typeof (Resources))]
    public string FirstName { get; set; }

    [Display(Name = "LastName", ResourceType = typeof (Resources))]
    [StringLength(20)]
    public string LastName { get; set; }

    [StringLength(20)]
    [Display(Name = "MiddleName", ResourceType = typeof (Resources))]
    public string MiddleName { get; set; }

    [Display(Name = "IsActive", ResourceType = typeof (Resources))]
    [Required]
    public bool IsActive { get; set; }

    [Required]
    [Display(Name = "IsVisible", ResourceType = typeof (Resources))]
    public bool IsVisible { get; set; }

    [Display(Name = "IsAdmin", ResourceType = typeof (Resources))]
    [Required]
    public bool IsAdmin { get; set; }

    [Display(Name = "IsAudit", ResourceType = typeof (Resources))]
    [Required]
    public bool IsAudit { get; set; }

    [Required]
    [Display(Name = "IsSeller", ResourceType = typeof (Resources))]
    public bool IsSeller { get; set; }

    [Required]
    [Display(Name = "IsBooker", ResourceType = typeof (Resources))]
    public bool IsBooker { get; set; }

    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    public virtual EnterpriseBranchTDES EnterpriseBranchTDES { get; set; }

    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    public virtual ICollection<EnterpriseBranchUserContactTDES> EnterpriseBranchUserContactTDESes { get; set; }
  }
}

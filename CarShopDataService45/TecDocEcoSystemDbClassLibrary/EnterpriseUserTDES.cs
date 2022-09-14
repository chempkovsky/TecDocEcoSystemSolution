// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseUserTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseUserTDES
  {
    [Required]
    [StringLength(40)]
    [Display(Name = "EnterpriseUserTDES_EntUserNic", ResourceType = typeof (Resources))]
    [Key]
    public string EntUserNic { get; set; }

    [Display(Name = "Password", ResourceType = typeof (Resources))]
    [StringLength(30)]
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; }

    [StringLength(20)]
    [Display(Name = "FirstName", ResourceType = typeof (Resources))]
    public string FirstName { get; set; }

    [StringLength(20)]
    [Display(Name = "LastName", ResourceType = typeof (Resources))]
    public string LastName { get; set; }

    [Display(Name = "MiddleName", ResourceType = typeof (Resources))]
    [StringLength(20)]
    public string MiddleName { get; set; }

    [Required]
    [Display(Name = "IsActive", ResourceType = typeof (Resources))]
    public bool IsActive { get; set; }

    [Display(Name = "IsAdmin", ResourceType = typeof (Resources))]
    [Required]
    public bool IsAdmin { get; set; }

    [Display(Name = "IsAudit", ResourceType = typeof (Resources))]
    [Required]
    public bool IsAudit { get; set; }

    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    public virtual EnterpriseTDES EnterpriseTDES { get; set; }

    public virtual ICollection<EnterpriseUserContactTDES> EnterpriseUserContactTDESes { get; set; }
  }
}

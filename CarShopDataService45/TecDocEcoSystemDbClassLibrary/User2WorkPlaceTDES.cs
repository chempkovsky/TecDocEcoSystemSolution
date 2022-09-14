// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.User2WorkPlaceTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class User2WorkPlaceTDES
  {
    [Required]
    [Display(Name = "WorkPlaceGuid", ResourceType = typeof (Resources))]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid WorkPlaceGuid { get; set; }

    [StringLength(60)]
    [Display(Name = "Description", ResourceType = typeof (Resources))]
    public string Description { get; set; }

    [StringLength(40)]
    [Display(Name = "EnterpriseUserTDES_EntUserNic", ResourceType = typeof (Resources))]
    public string EntUserNic { get; set; }

    [Display(Name = "FirstName", ResourceType = typeof (Resources))]
    [StringLength(20)]
    public string FirstName { get; set; }

    [StringLength(20)]
    [Display(Name = "LastName", ResourceType = typeof (Resources))]
    public string LastName { get; set; }

    [Display(Name = "User2WorkPlaceTDES_SetAt", ResourceType = typeof (Resources))]
    [Required]
    public DateTime SetAt { get; set; }

    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntGuid { get; set; }
  }
}

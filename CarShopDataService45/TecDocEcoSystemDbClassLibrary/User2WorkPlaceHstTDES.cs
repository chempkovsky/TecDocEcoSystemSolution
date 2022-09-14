// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.User2WorkPlaceHstTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class User2WorkPlaceHstTDES
  {
    [Display(Name = "User2WorkPlaceTDES_SetAt", ResourceType = typeof (Resources))]
    [Column(Order = 0)]
    [Required]
    [Key]
    public DateTime SetAt { get; set; }

    [Key]
    [Display(Name = "WorkPlaceGuid", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [Column(Order = 1)]
    public Guid WorkPlaceGuid { get; set; }

    [Column(Order = 2)]
    [StringLength(40)]
    [Key]
    [Display(Name = "EnterpriseUserTDES_EntUserNic", ResourceType = typeof (Resources))]
    public string EntUserNic { get; set; }

    [StringLength(60)]
    [Display(Name = "Description", ResourceType = typeof (Resources))]
    public string Description { get; set; }

    [Required]
    [Display(Name = "User2WorkPlaceTDES_ReSetAt", ResourceType = typeof (Resources))]
    public DateTime ReSetAt { get; set; }

    [Display(Name = "FirstName", ResourceType = typeof (Resources))]
    [StringLength(20)]
    public string FirstName { get; set; }

    [StringLength(20)]
    [Display(Name = "LastName", ResourceType = typeof (Resources))]
    public string LastName { get; set; }

    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntGuid { get; set; }
  }
}

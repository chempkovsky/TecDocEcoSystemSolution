// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseBranchWorkPlaceTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseBranchWorkPlaceTDES
  {
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "WorkPlaceGuid", ResourceType = typeof (Resources))]
    public Guid WorkPlaceGuid { get; set; }

    [Required]
    [Display(Name = "IsActive", ResourceType = typeof (Resources))]
    public bool IsActive { get; set; }

    [StringLength(60)]
    [Display(Name = "Description", ResourceType = typeof (Resources))]
    public string Description { get; set; }

    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntBranchGuid { get; set; }

    public virtual EnterpriseBranchTDES EnterpriseBranchTDES { get; set; }

    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }
  }
}

// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.BranchSpellTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class BranchSpellTDES
  {
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    [Key]
    [Required]
    public Guid EntBranchGuid { get; set; }

    [Required]
    [Display(Name = "EntBranchDescription", ResourceType = typeof (Resources))]
    [StringLength(80)]
    public string EntBranchDescription { get; set; }

    [Required]
    [Display(Name = "BranchSpellTDES_SpellGuid", ResourceType = typeof (Resources))]
    public Guid SpellGuid { get; set; }

    [Required]
    [Display(Name = "BranchSpellTDES_IsActive", ResourceType = typeof (Resources))]
    public bool IsActive { get; set; }

    [Display(Name = "IsBlocked", ResourceType = typeof (Resources))]
    [Required]
    public bool IsBlocked { get; set; }

    [Required]
    [Display(Name = "BranchSpellTDES_OpenAt", ResourceType = typeof (Resources))]
    public DateTime OpenAt { get; set; }

    [Required]
    [Display(Name = "BranchSpellTDES_CloseAt", ResourceType = typeof (Resources))]
    public DateTime CloseAt { get; set; }

    [Display(Name = "BranchSpellTDES_OpenedBy", ResourceType = typeof (Resources))]
    [Required]
    public string OpenedBy { get; set; }

    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    [Display(Name = "BranchRestTDES_TSConcClmn", ResourceType = typeof (Resources))]
    [Timestamp]
    public byte[] TSConcClmn { get; set; }
  }
}

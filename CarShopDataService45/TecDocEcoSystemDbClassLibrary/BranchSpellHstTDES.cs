// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.BranchSpellHstTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class BranchSpellHstTDES
  {
    [Key]
    [Display(Name = "BranchSpellTDES_SpellGuid", ResourceType = typeof (Resources))]
    public Guid SpellGuid { get; set; }

    [Required]
    [Display(Name = "BranchSpellTDES_IsActive", ResourceType = typeof (Resources))]
    public bool IsActive { get; set; }

    [Required]
    [Display(Name = "IsBlocked", ResourceType = typeof (Resources))]
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

    [Display(Name = "BranchSpellTDES_ClosedBy", ResourceType = typeof (Resources))]
    [Required]
    public string ClosedBy { get; set; }

    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    [Required]
    [Display(Name = "CribFromIncome", ResourceType = typeof (Resources))]
    public bool IsCribFromIncome { get; set; }
  }
}

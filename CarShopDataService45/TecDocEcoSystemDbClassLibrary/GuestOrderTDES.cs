// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.GuestOrderTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class GuestOrderTDES
  {
    [Display(Name = "GuestOrderTDES_GuestOrderGuid", ResourceType = typeof (Resources))]
    [Required]
    [Key]
    public Guid GuestOrderGuid { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "GuestProfileTDES_GestUserNic", ResourceType = typeof (Resources))]
    public string GestUserNic { get; set; }

    public virtual GuestProfileTDES GuestProfileTDES { get; set; }

    [Required]
    [Display(Name = "GuestOrderTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "GuestOrderTDES_EntBranchDescription", ResourceType = typeof (Resources))]
    public string EntBranchDescription { get; set; }

    [Required]
    [Display(Name = "IsActive", ResourceType = typeof (Resources))]
    public bool IsActive { get; set; }

    [Display(Name = "GuestOrderTDES_IsDone", ResourceType = typeof (Resources))]
    [Required]
    public bool IsDone { get; set; }

    [Required]
    [Display(Name = "GuestOrderTDES_LastUpdated", ResourceType = typeof (Resources))]
    public DateTime LastUpdated { get; set; }

    [Display(Name = "GuestOrderTDES_LastReplicated", ResourceType = typeof (Resources))]
    [Required]
    public DateTime LastReplicated { get; set; }

    [Display(Name = "GuestOrderTDES_IsReplicated", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int IsReplicated { get; private set; }

    public virtual ICollection<GuestOrderArticleTDES> GuestOrderArticleTDESes { get; set; }
  }
}

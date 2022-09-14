// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.GuestOrderTmp
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class GuestOrderTmp
  {
    [Key]
    [Display(Name = "GuestOrderTDES_GuestOrderGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid GuestOrderGuid { get; set; }

    [Display(Name = "GuestProfileTDES_GestUserNic", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(80)]
    public string GestUserNic { get; set; }

    [Display(Name = "GuestOrderTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntBranchGuid { get; set; }

    [Required]
    [Display(Name = "GuestOrderTDES_EntBranchDescription", ResourceType = typeof (Resources))]
    [StringLength(80)]
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

    public void CopyFrom(GuestOrderTDES src)
    {
      if (src == null)
        return;
      this.GuestOrderGuid = src.GuestOrderGuid;
      this.GestUserNic = src.GestUserNic;
      this.EntBranchGuid = src.EntBranchGuid;
      this.EntBranchDescription = src.EntBranchDescription;
      this.IsActive = src.IsActive;
      this.IsDone = src.IsDone;
      this.LastUpdated = src.LastUpdated;
      this.LastReplicated = src.LastReplicated;
    }

    public void CopyTo(GuestOrderTDES dest)
    {
      if (dest == null)
        return;
      dest.GuestOrderGuid = this.GuestOrderGuid;
      dest.GestUserNic = this.GestUserNic;
      dest.EntBranchGuid = this.EntBranchGuid;
      dest.EntBranchDescription = this.EntBranchDescription;
      dest.IsActive = this.IsActive;
      dest.IsDone = this.IsDone;
      dest.LastUpdated = this.LastUpdated;
      dest.LastReplicated = this.LastReplicated;
    }
  }
}

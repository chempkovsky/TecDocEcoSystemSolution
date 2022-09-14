// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.SaleBasketTDES
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
  public class SaleBasketTDES
  {
    [Key]
    [Display(Name = "SaleBasketTDES_EntBasketGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntBasketGuid { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "WorkPlaceGuid", ResourceType = typeof (Resources))]
    public Guid WorkPlaceGuid { get; set; }

    [Display(Name = "EnterpriseUserTDES_EntUserNic", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    public string EntUserNic { get; set; }

    [Display(Name = "User2WorkPlaceTDES_SetAt", ResourceType = typeof (Resources))]
    [Required]
    public DateTime SetAt { get; set; }

    [Display(Name = "SaleBasketTDES_Description", ResourceType = typeof (Resources))]
    [StringLength(60)]
    public string Description { get; set; }

    [Display(Name = "BranchSpellTDES_SpellGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid SpellGuid { get; set; }

    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntBranchGuid { get; set; }

    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    [Display(Name = "SaleBasketTDES_CreatedAt", ResourceType = typeof (Resources))]
    [Required]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "IsActive", ResourceType = typeof (Resources))]
    [Required]
    public bool IsActive { get; set; }

    [Display(Name = "IsPaid", ResourceType = typeof (Resources))]
    [Required]
    public bool IsPaid { get; set; }

    [Required]
    [Display(Name = "SaleBasketTDES_PaidAt", ResourceType = typeof (Resources))]
    public DateTime PaidAt { get; set; }

    [Display(Name = "IsReverse", ResourceType = typeof (Resources))]
    [Required]
    public bool IsReverse { get; set; }

    [Display(Name = "Comments", ResourceType = typeof (Resources))]
    [StringLength(60)]
    public string Comments { get; set; }

    [Display(Name = "SaleBasketTDES_ArtAmount", ResourceType = typeof (Resources))]
    [Required]
    public int ArtAmount { get; set; }

    [Display(Name = "SaleBasketTDES_PaymentSum", ResourceType = typeof (Resources))]
    [Required]
    public double PaymentSum { get; set; }

    public virtual ICollection<TecDocEcoSystemDbClassLibrary.SaleBasketArticleTDES> SaleBasketArticleTDES { get; set; }
  }
}

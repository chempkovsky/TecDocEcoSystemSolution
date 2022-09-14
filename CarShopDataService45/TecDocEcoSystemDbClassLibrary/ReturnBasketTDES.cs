// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.ReturnBasketTDES
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
  public class ReturnBasketTDES
  {
    [Key]
    [Required]
    [Display(Name = "ReturnBasketTDES_EntBasketGuid", ResourceType = typeof (Resources))]
    public Guid RetBasketGuid { get; set; }

    [Display(Name = "ReturnBasketTDES_WorkPlaceGuid", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    public Guid WorkPlaceGuid { get; set; }

    [Display(Name = "EnterpriseUserTDES_EntUserNic", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    public string EntUserNic { get; set; }

    [Display(Name = "User2WorkPlaceTDES_SetAt", ResourceType = typeof (Resources))]
    [Required]
    public DateTime SetAt { get; set; }

    [StringLength(60)]
    [Display(Name = "ReturnBasketTDES_Description", ResourceType = typeof (Resources))]
    public string Description { get; set; }

    [Required]
    [Display(Name = "BranchSpellTDES_SpellGuid", ResourceType = typeof (Resources))]
    public Guid SpellGuid { get; set; }

    [Required]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    public Guid EntBranchGuid { get; set; }

    [Required]
    [Display(Name = "EnterpriseTDES_EntGuid", ResourceType = typeof (Resources))]
    public Guid EntGuid { get; set; }

    [Required]
    [Display(Name = "ReturnBasketTDES_CreatedAt", ResourceType = typeof (Resources))]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "IsActive", ResourceType = typeof (Resources))]
    [Required]
    public bool IsActive { get; set; }

    [Display(Name = "ReturnBasketTDES_IsPaid", ResourceType = typeof (Resources))]
    [Required]
    public bool IsPaid { get; set; }

    [Display(Name = "ReturnBasketTDES_PaidAt", ResourceType = typeof (Resources))]
    [Required]
    public DateTime PaidAt { get; set; }

    [Display(Name = "IsReverse", ResourceType = typeof (Resources))]
    [Required]
    public bool IsReverse { get; set; }

    [StringLength(60)]
    [Display(Name = "Comments", ResourceType = typeof (Resources))]
    public string Comments { get; set; }

    [Display(Name = "ReturnBasketTDES_ArtAmount", ResourceType = typeof (Resources))]
    [Required]
    public int ArtAmount { get; set; }

    [Required]
    [Display(Name = "ReturnBasketTDES_PaymentSum", ResourceType = typeof (Resources))]
    public double PaymentSum { get; set; }

    public virtual ICollection<TecDocEcoSystemDbClassLibrary.ReturnBasketArticleTDES> ReturnBasketArticleTDES { get; set; }
  }
}

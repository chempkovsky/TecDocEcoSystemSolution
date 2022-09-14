// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseBranchReplyTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseBranchReplyTDES
  {
    [Column(Order = 0)]
    [Display(Name = "EnterpriseBranchTDES_EntBranchGuid", ResourceType = typeof (Resources))]
    [Required]
    [Key]
    public Guid EntBranchGuid { get; set; }

    public virtual EnterpriseBranchTDES EnterpriseBranchTDES { get; set; }

    [Required]
    [Key]
    [Column(Order = 1)]
    [Display(Name = "EnterpriseBranchReplyTDES_ReplyType", ResourceType = typeof (Resources))]
    public int ReplyType { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "EnterpriseBranchReplyTDES_BaseHttpAddress", ResourceType = typeof (Resources))]
    public string BaseHttpAddress { get; set; }

    [Display(Name = "EnterpriseBranchReplyTDES_HttpLoginUrl", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(120)]
    public string HttpLoginUrl { get; set; }

    [StringLength(120)]
    [Display(Name = "EnterpriseBranchReplyTDES_HttpGetUrl", ResourceType = typeof (Resources))]
    [Required]
    public string HttpGetUrl { get; set; }

    [Display(Name = "EnterpriseBranchReplyTDES_HttpPostUrl", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(120)]
    public string HttpPostUrl { get; set; }

    [StringLength(120)]
    [Display(Name = "EnterpriseBranchReplyTDES_HttpPutUrl", ResourceType = typeof (Resources))]
    [Required]
    public string HttpPutUrl { get; set; }

    [StringLength(120)]
    [Display(Name = "EnterpriseBranchReplyTDES_HttpDeleteUrl", ResourceType = typeof (Resources))]
    [Required]
    public string HttpDeleteUrl { get; set; }

    [Display(Name = "EnterpriseBranchReplyTDES_HttpUser", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(30)]
    public string HttpUser { get; set; }

    [StringLength(30)]
    [Required]
    [Display(Name = "EnterpriseBranchReplyTDES_HttpPassword", ResourceType = typeof (Resources))]
    public string HttpPassword { get; set; }
  }
}

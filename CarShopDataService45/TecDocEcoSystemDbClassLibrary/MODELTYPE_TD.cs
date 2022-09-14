// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.MODELTYPE_TD
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class MODELTYPE_TD
  {
    [Display(Name = "TECDOC_TYP_ID", ResourceType = typeof (Resources))]
    [Key]
    public int TYP_ID { get; set; }

    [Display(Name = "TECDOC_TEX_TEXT", ResourceType = typeof (Resources))]
    public string TEX_TEXT { get; set; }

    [Display(Name = "TECDOC_TYP_KV_BODY", ResourceType = typeof (Resources))]
    public string TYP_KV_BODY { get; set; }

    [Display(Name = "TECDOC_TYP_PCON_START", ResourceType = typeof (Resources))]
    public string TYP_PCON_START { get; set; }

    [Display(Name = "TECDOC_TYP_PCON_END", ResourceType = typeof (Resources))]
    public string TYP_PCON_END { get; set; }

    [Display(Name = "TECDOC_TYP_KW_FROM", ResourceType = typeof (Resources))]
    public string TYP_KW_FROM { get; set; }

    [Display(Name = "TECDOC_TYP_KW_UPTO", ResourceType = typeof (Resources))]
    public string TYP_KW_UPTO { get; set; }

    [Display(Name = "TECDOC_TYP_HP_FROM", ResourceType = typeof (Resources))]
    public string TYP_HP_FROM { get; set; }

    [Display(Name = "TECDOC_TYP_HP_UPTO", ResourceType = typeof (Resources))]
    public string TYP_HP_UPTO { get; set; }

    [Display(Name = "TECDOC_TYP_CCM", ResourceType = typeof (Resources))]
    public string TYP_CCM { get; set; }

    [Display(Name = "TECDOC_TYP_VALVES", ResourceType = typeof (Resources))]
    public string TYP_VALVES { get; set; }

    [Display(Name = "TECDOC_TYP_CYLINDERS", ResourceType = typeof (Resources))]
    public string TYP_CYLINDERS { get; set; }

    [Display(Name = "TECDOC_TYP_DOORS", ResourceType = typeof (Resources))]
    public string TYP_DOORS { get; set; }

    [Display(Name = "TECDOC_TYP_KV_ABS", ResourceType = typeof (Resources))]
    public string TYP_KV_ABS { get; set; }

    [Display(Name = "TECDOC_TYP_KV_ASR", ResourceType = typeof (Resources))]
    public string TYP_KV_ASR { get; set; }

    [Display(Name = "TECDOC_TYP_KV_BRAKE_TYPE", ResourceType = typeof (Resources))]
    public string TYP_KV_BRAKE_TYPE { get; set; }

    [Display(Name = "TECDOC_TYP_KV_BRAKE_SYST", ResourceType = typeof (Resources))]
    public string TYP_KV_BRAKE_SYST { get; set; }

    [Display(Name = "TECDOC_TYP_KV_FUEL", ResourceType = typeof (Resources))]
    public string TYP_KV_FUEL { get; set; }

    [Display(Name = "TECDOC_TYP_KV_FUEL_SUPPLY", ResourceType = typeof (Resources))]
    public string TYP_KV_FUEL_SUPPLY { get; set; }

    [Display(Name = "TECDOC_TYP_KV_CATALYST", ResourceType = typeof (Resources))]
    public string TYP_KV_CATALYST { get; set; }

    [Display(Name = "TECDOC_TYP_KV_TRANS", ResourceType = typeof (Resources))]
    public string TYP_KV_TRANS { get; set; }

    [Display(Name = "TECDOC_TYP_KV_ENGINE", ResourceType = typeof (Resources))]
    public string TYP_KV_ENGINE { get; set; }

    public int FUEL_ID { get; set; }
  }
}

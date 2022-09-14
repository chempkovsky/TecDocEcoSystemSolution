// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.MODELTYPETREEITEMS_TD
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class MODELTYPETREEITEMS_TD
  {
    [Display(Name = "TECDOC_SUP_ID", ResourceType = typeof (Resources))]
    [Key]
    public int SUP_ID { get; set; }

    [Display(Name = "TECDOC_SUP_TEXT", ResourceType = typeof (Resources))]
    public string SUP_TEXT { get; set; }

    [Display(Name = "TECDOC_GA_NR", ResourceType = typeof (Resources))]
    public int GA_NR { get; set; }

    [Display(Name = "TECDOC_MASTER_BEZ", ResourceType = typeof (Resources))]
    public string MASTER_BEZ { get; set; }

    [Display(Name = "TECDOC_ART_ARTICLE_NR", ResourceType = typeof (Resources))]
    public string ART_ARTICLE_NR { get; set; }

    [Display(Name = "TECDOC_ART_ID", ResourceType = typeof (Resources))]
    public int ART_ID { get; set; }

    [Display(Name = "TECDOC_LA_ID", ResourceType = typeof (Resources))]
    public int LA_ID { get; set; }

    [Display(Name = "TECDOC_GA_TEXT", ResourceType = typeof (Resources))]
    public string GA_TEXT { get; set; }

    [Display(Name = "TECDOC_EAN_TEXT", ResourceType = typeof (Resources))]
    public string EAN_TEXT { get; set; }
  }
}

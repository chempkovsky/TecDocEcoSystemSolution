// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.SimpleArticle_TD
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class SimpleArticle_TD
  {
    [Display(Name = "TECDOC_ART_ID", ResourceType = typeof (Resources))]
    public int ART_ID { get; set; }

    [Display(Name = "TECDOC_ART_ARTICLE_NR", ResourceType = typeof (Resources))]
    public string ART_ARTICLE_NR { get; set; }

    [Display(Name = "TECDOC_SUP_TEXT", ResourceType = typeof (Resources))]
    public string SUP_TEXT { get; set; }

    [Display(Name = "TECDOC_MASTER_BEZ", ResourceType = typeof (Resources))]
    public string MASTER_BEZ { get; set; }

    [Display(Name = "TECDOC_GA_NR", ResourceType = typeof (Resources))]
    public int GA_NR { get; set; }
  }
}

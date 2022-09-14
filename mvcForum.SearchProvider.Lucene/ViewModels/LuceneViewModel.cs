// Decompiled with JetBrains decompiler
// Type: mvcForum.SearchProvider.Lucene.ViewModels.LuceneViewModel
// Assembly: mvcForum.SearchProvider.Lucene, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 13A5DCA9-908F-4689-9C6D-F584F417B850
// Assembly location: C:\Development\WebCarShop\mvcForum.SearchProvider.Lucene.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.SearchProvider.Lucene.ViewModels
{
  public class LuceneViewModel
  {
    [Required]
    [LocalizedDisplay("mvcForum.SearchProvider.Lucene", "Enabled")]
    public bool Enabled { get; set; }

    [LocalizedDisplay("mvcForum.SearchProvider.Lucene", "RunAsync")]
    [Required]
    public bool RunAsynchronously { get; set; }

    [LocalizedDisplay("mvcForum.SearchProvider.Lucene", "Delay")]
    [Required]
    public int Delay { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.SearchProvider.Lucene", "TitleWeight")]
    public int TitleWeight { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.SearchProvider.Lucene", "TopicWeight")]
    public int TopicWeight { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.SearchProvider.Lucene", "StickyWeight")]
    public int StickyWeight { get; set; }

    [LocalizedDisplay("mvcForum.SearchProvider.Lucene", "AnnouncementWeight")]
    [Required]
    public int AnnouncementWeight { get; set; }
  }
}

// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.SearchViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.ViewModels
{
  public class SearchViewModel
  {
    [LocalizedDisplay("mvcForum.Web.Search", "Forums")]
    public int[] Forums { get; set; }

    [LocalizedDisplay("mvcForum.Web.Search", "Query")]
    [Required]
    public string Query { get; set; }

    public IEnumerable<TopicViewModel> Results { get; set; }
  }
}

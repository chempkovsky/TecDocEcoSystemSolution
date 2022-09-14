// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.UnansweredTopicsViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System.Collections.Generic;

namespace mvcForum.Web.ViewModels
{
  public class UnansweredTopicsViewModel
  {
    public Dictionary<string, string> Path { get; set; }

    public IEnumerable<TopicViewModel> Topics { get; set; }
  }
}

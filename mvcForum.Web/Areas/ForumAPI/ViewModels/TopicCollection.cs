// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.ViewModels.TopicCollection
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Web.ViewModels;
using System.Collections.Generic;

namespace mvcForum.Web.Areas.ForumAPI.ViewModels
{
  public class TopicCollection
  {
    private readonly IEnumerable<TopicViewModel> topics;

    public TopicCollection(IEnumerable<TopicViewModel> topics)
    {
      this.topics = topics;
    }

    public IEnumerable<TopicViewModel> Topics
    {
      get
      {
        return this.topics;
      }
    }
  }
}

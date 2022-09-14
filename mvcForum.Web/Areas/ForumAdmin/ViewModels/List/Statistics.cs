// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAdmin.ViewModels.List.Statistics
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System;

namespace mvcForum.Web.Areas.ForumAdmin.ViewModels.List
{
  public class Statistics
  {
    public int TopicCount { get; set; }

    public int PostCount { get; set; }

    public int AttachmentCount { get; set; }

    public int UserCount { get; set; }

    public int AttachmentSize { get; set; }

    public DateTime InstallDate { get; set; }

    public int Days { get; set; }

    public string Version { get; set; }
  }
}

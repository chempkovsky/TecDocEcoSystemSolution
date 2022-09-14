// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.ForumAccessViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;

namespace mvcForum.Web.ViewModels
{
  public class ForumAccessViewModel
  {
    public ForumAccessViewModel(ForumAccess access)
    {
    }

    public GroupViewModel Group { get; set; }

    public AccessMaskViewModel AccessMask { get; set; }
  }
}

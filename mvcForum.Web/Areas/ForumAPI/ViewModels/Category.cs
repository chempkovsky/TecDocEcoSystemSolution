// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.ViewModels.Category
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

namespace mvcForum.Web.Areas.ForumAPI.ViewModels
{
  public class Category
  {
    public int Id { get; set; }

    public int SortOrder { get; set; }

    public string Name { get; set; }

    public int BoardId { get; set; }

    public string BoardUrl { get; set; }
  }
}

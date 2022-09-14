// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Extensions.GroupExtensions
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Web.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace mvcForum.Web.Extensions
{
  public static class GroupExtensions
  {
    public static IEnumerable<SelectListItem> ToSelectList(
      this IEnumerable<GroupViewModel> groups)
    {
      return groups.ToSelectList((GroupViewModel) null);
    }

    public static IEnumerable<SelectListItem> ToSelectList(
      this IEnumerable<GroupViewModel> groups,
      GroupViewModel selected)
    {
      List<SelectListItem> selectListItemList = new List<SelectListItem>();
      foreach (GroupViewModel group in groups)
        selectListItemList.Add(new SelectListItem()
        {
          Text = group.Name,
          Value = group.Id.ToString(),
          Selected = selected != null && group.Id == selected.Id
        });
      return (IEnumerable<SelectListItem>) selectListItemList;
    }
  }
}

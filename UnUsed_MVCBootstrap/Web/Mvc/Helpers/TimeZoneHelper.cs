// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Helpers.TimeZoneHelper
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Helpers
{
  public static class TimeZoneHelper
  {
    public static IEnumerable<SelectListItem> GetTimeZones(
      string selectedTimeZone)
    {
      List<SelectListItem> selectListItemList = new List<SelectListItem>();
      foreach (TimeZoneInfo timeZoneInfo in (IEnumerable<TimeZoneInfo>) TimeZoneInfo.GetSystemTimeZones().OrderByDescending<TimeZoneInfo, double>((Func<TimeZoneInfo, double>) (t => t.BaseUtcOffset.TotalMinutes)))
        selectListItemList.Add(new SelectListItem()
        {
          Text = timeZoneInfo.DisplayName,
          Value = timeZoneInfo.Id,
          Selected = timeZoneInfo.Id == selectedTimeZone
        });
      return (IEnumerable<SelectListItem>) selectListItemList;
    }
  }
}

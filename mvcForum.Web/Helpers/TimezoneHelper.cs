// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Helpers.TimezoneHelper
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace mvcForum.Web.Helpers
{
  public static class TimezoneHelper
  {
    private const string timeZoneCacheKey = "TimeZone";

    public static DateTime ToLocalDateTime(this DateTimeOffset dt)
    {
      TimeZoneInfo timeZoneInfo = TimezoneHelper.GetTimeZoneInfo();
      return TimeZoneInfo.ConvertTimeFromUtc(dt.UtcDateTime, timeZoneInfo);
    }

    public static DateTime ToLocalDateTime(this DateTime dt)
    {
      TimeZoneInfo timeZoneInfo = TimezoneHelper.GetTimeZoneInfo();
      dt = TimeZoneInfo.ConvertTimeFromUtc(dt, timeZoneInfo);
      return dt;
    }

    public static TimeZoneInfo GetTimeZoneInfo()
    {
      TimeZoneInfo timeZoneInfo = TimeZoneInfo.Local;
      try
      {
        IPerRequestCache service1 = DependencyResolver.Current.GetService<IPerRequestCache>();
        object obj = service1.Pull("TimeZone");
        if (obj != null)
        {
          timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById((string) obj);
        }
        else
        {
          IConfiguration service2 = DependencyResolver.Current.GetService<IConfiguration>();
          if (!string.IsNullOrWhiteSpace(service2.DefaultTimezone))
          {
            try
            {
              timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(service2.DefaultTimezone);
            }
            catch
            {
            }
          }
          IWebUserProvider service3 = DependencyResolver.Current.GetService<IWebUserProvider>();
          DependencyResolver.Current.GetService<IRepository<ForumUser>>();
          if (service3.Authenticated)
          {
            ForumUser activeUser = service3.ActiveUser;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(activeUser.Timezone);
            service1.Push("TimeZone", (object) activeUser.Timezone);
          }
        }
      }
      catch
      {
      }
      return timeZoneInfo;
    }

    public static DateTime ToLocalDateTime(this HtmlHelper html, DateTime dt)
    {
      return dt.ToLocalDateTime();
    }

    public static IEnumerable<SelectListItem> GetTimeZones(
      string selectedTimeZone)
    {
      List<SelectListItem> selectListItemList = new List<SelectListItem>();
      foreach (TimeZoneInfo systemTimeZone in TimeZoneInfo.GetSystemTimeZones())
        selectListItemList.Add(new SelectListItem()
        {
          Text = systemTimeZone.DisplayName,
          Value = systemTimeZone.Id,
          Selected = systemTimeZone.Id == selectedTimeZone
        });
      return (IEnumerable<SelectListItem>) selectListItemList;
    }
  }
}

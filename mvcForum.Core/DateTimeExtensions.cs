// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.DateTimeExtensions
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;

namespace mvcForum.Core
{
  public static class DateTimeExtensions
  {
    public static DateTime Handle(this DateTime dt)
    {
      if (dt.Kind == DateTimeKind.Unspecified)
        return new DateTime(dt.Ticks, DateTimeKind.Utc);
      if (dt.Kind != DateTimeKind.Utc)
        return TimeZoneInfo.ConvertTimeToUtc(dt);
      return dt;
    }

    public static DateTime? Handle(this DateTime? dt)
    {
      if (dt.HasValue)
      {
        if (dt.Value.Kind == DateTimeKind.Unspecified)
          return new DateTime?(new DateTime(dt.Value.Ticks, DateTimeKind.Utc));
        if (dt.Value.Kind != DateTimeKind.Utc)
          return new DateTime?(TimeZoneInfo.ConvertTimeToUtc(dt.Value));
      }
      return dt;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Constants
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

namespace mvcForum.Core
{
  public static class Constants
  {
    public static class FieldLengths
    {
      public const int Username = 256;
      public const int Subject = 200;
      public const int EmailAddress = 200;
      public const int FullName = 200;
      public const int ProviderName = 50;
      public const int ProviderId = 200;
      public const int BoardName = 200;
      public const int CategoryName = 200;
      public const int ForumName = 200;
      public const int AccessMaskName = 200;
      public const int AttachmentPath = 500;
      public const int AttachmentFilename = 200;
      public const int Password = 100;
      public const int IPAddress = 50;
      public const int CultureInfoId = 10;
      public const int TimezoneId = 100;
      public const int GroupName = 100;
      public const int EditReason = 500;
      public const int DeleteReason = 500;
      public const int ReportReason = 500;
      public const int Title = 200;
      public const int SettingsKey = 100;
      public const int SiteName = 200;
      public const int DefaultLanguage = 10;
      public const int DefaultTimeZone = 100;
      public const int DefaultDateTimeFormat = 50;
      public const int Theme = 200;
    }

    public static class Roles
    {
      public const string SolutionAdmin = "Solution Administrator";
      public const string BoardAdmin = "Board Administrator";
      public const string Combined = "Board Administrator,Solution Administrator";
    }
  }
}

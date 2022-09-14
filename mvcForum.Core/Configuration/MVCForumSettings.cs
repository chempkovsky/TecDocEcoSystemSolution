// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Configuration.MVCForumSettings
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.Configuration;

namespace mvcForum.Core.Configuration
{
  public class MVCForumSettings : ConfigurationSection
  {
    private static MVCForumSettings settings = ConfigurationManager.GetSection(nameof (MVCForumSettings)) as MVCForumSettings;

    private MVCForumSettings()
    {
    }

    public static MVCForumSettings Settings
    {
      get
      {
        return MVCForumSettings.settings;
      }
    }

    [ConfigurationProperty("asynchronousAntiSpam")]
    public bool AsynchronousAntiSpam
    {
      get
      {
        return (bool) this["asynchronousAntiSpam"];
      }
      set
      {
        this["asynchronousAntiSpam"] = (object) value;
      }
    }

    [ConfigurationProperty("showOldPostsOnReply", DefaultValue = true)]
    public bool ShowOldPostsOnReply
    {
      get
      {
        return (bool) this["showOldPostsOnReply"];
      }
      set
      {
        this["showOldPostsOnReply"] = (object) value;
      }
    }

    [ConfigurationProperty("showDeletedMessages", DefaultValue = true)]
    public bool ShowDeletedMessages
    {
      get
      {
        return (bool) this["showDeletedMessages"];
      }
      set
      {
        this["showDeletedMessages"] = (object) value;
      }
    }

    [ConfigurationProperty("postsOnReply", DefaultValue = 10)]
    public int PostsOnReply
    {
      get
      {
        return (int) this["postsOnReply"];
      }
      set
      {
        this["postsOnReply"] = (object) value;
      }
    }

    [ConfigurationProperty("defaultCulture", IsRequired = true)]
    public string DefaultCulture
    {
      get
      {
        return (string) this["defaultCulture"];
      }
      set
      {
        this["defaultCulture"] = (object) value;
      }
    }

    [ConfigurationProperty("defaultTimezone", IsRequired = true)]
    public string DefaultTimezone
    {
      get
      {
        return (string) this["defaultTimezone"];
      }
      set
      {
        this["defaultTimezone"] = (object) value;
      }
    }

    [ConfigurationProperty("maxFileSize", DefaultValue = 2000000, IsRequired = false)]
    [IntegerValidator]
    public int MaxFileSize
    {
      get
      {
        return (int) this["maxFileSize"];
      }
      set
      {
        this["maxFileSize"] = (object) value;
      }
    }

    [IntegerValidator]
    [ConfigurationProperty("maxAttachmentsSize", DefaultValue = 10000000, IsRequired = false)]
    public int MaxAttachmentsSize
    {
      get
      {
        return (int) this["maxAttachmentsSize"];
      }
      set
      {
        this["maxAttachmentsSize"] = (object) value;
      }
    }

    [ConfigurationProperty("topicsPerPage", DefaultValue = 15, IsRequired = false)]
    [IntegerValidator(MaxValue = 100, MinValue = 1)]
    public int TopicsPerPage
    {
      get
      {
        return (int) this["topicsPerPage"];
      }
      set
      {
        this["topicsPerPage"] = (object) value;
      }
    }

    [ConfigurationProperty("messagesPerPage", DefaultValue = 20, IsRequired = false)]
    [IntegerValidator(MaxValue = 100, MinValue = 1)]
    public int MessagesPerPage
    {
      get
      {
        return (int) this["messagesPerPage"];
      }
      set
      {
        this["messagesPerPage"] = (object) value;
      }
    }

    [ConfigurationProperty("allowedExtensions", DefaultValue = "jpg;png;gif", IsRequired = false)]
    public string AllowedExtensions
    {
      get
      {
        return (string) this["allowedExtensions"];
      }
      set
      {
        this["allowedExtensions"] = (object) value;
      }
    }

    [ConfigurationProperty("siteURL", IsRequired = true)]
    public string SiteURL
    {
      get
      {
        return (string) this["siteURL"];
      }
      set
      {
        this["siteURL"] = (object) value;
      }
    }

    [ConfigurationProperty("robotEmailAddress", IsRequired = true)]
    public string RobotEmailAddress
    {
      get
      {
        return (string) this["robotEmailAddress"];
      }
      set
      {
        this["robotEmailAddress"] = (object) value;
      }
    }

    [ConfigurationProperty("robotName", IsRequired = true)]
    public string RobotName
    {
      get
      {
        return (string) this["robotName"];
      }
      set
      {
        this["robotName"] = (object) value;
      }
    }

    [ConfigurationProperty("theme", IsRequired = false)]
    public string Theme
    {
      get
      {
        return (string) this["theme"];
      }
      set
      {
        this["theme"] = (object) value;
      }
    }

    [ConfigurationProperty("allowUserDefinedTheme", DefaultValue = false, IsRequired = false)]
    public bool AllowUserDefinedTheme
    {
      get
      {
        return (bool) this["allowUserDefinedTheme"];
      }
      set
      {
        this["allowUserDefinedTheme"] = (object) value;
      }
    }
  }
}

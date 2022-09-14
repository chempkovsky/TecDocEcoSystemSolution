// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Configuration.MVCForumDBConfig
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mvcForum.Core.Configuration
{
  public class MVCForumDBConfig : IConfiguration
  {
    private readonly IRepository<ForumSettings> settingsRepo;
    private readonly IContext context;
    private IList<ForumSettings> pvtSettings;

    public MVCForumDBConfig(IContext context)
    {
      this.context = context;
      this.settingsRepo = this.context.GetRepository<ForumSettings>();
      this.NewUserGroups = (IList<int>) new List<int>();
      this.AllowedExtensions = (IList<string>) new List<string>();
      IList<ForumSettings> settings = this.Settings;
    }

    private IList<ForumSettings> Settings
    {
      get
      {
        if (this.pvtSettings == null)
        {
          try
          {
            this.pvtSettings = (IList<ForumSettings>) this.settingsRepo.ReadAll().ToList<ForumSettings>();
            this.SetDefaults(this.pvtSettings);
          }
          catch
          {
          }
        }
        return this.pvtSettings;
      }
    }

    private void SetDefaults(IList<ForumSettings> settings)
    {
      this.TopicsPerPage = this.GetInt32(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "TopicsPerPage")), "TopicsPerPage", 30);
      this.MessagesPerPage = this.GetInt32(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "MessagesPerPage")), "MessagesPerPage", 20);
      this.DefaultCulture = this.GetString(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "DefaultCulture")), "DefaultCulture", "en-GB");
      this.DefaultTimezone = this.GetString(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "DefaultTimezone")), "DefaultTimezone", "GMT Standard Time");
      this.SiteURL = this.GetString(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "SiteURL")), "SiteURL", "http://localhost");
      this.RobotEmailAddress = this.GetString(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "RobotEmailAddress")), "RobotEmailAddress", "robot@mvcforum.org");
      this.RobotName = this.GetString(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "RobotName")), "RobotName", "mvcForum");
      this.AsynchronousAntiSpam = this.GetBoolean(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "AsynchronousAntiSpam")), "AsynchronousAntiSpam", true);
      this.MaxFileSize = this.GetInt32(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "MaxFileSize")), "MaxFileSize", 2000000);
      this.MaxAttachmentsSize = this.GetInt32(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "MaxAttachmentsSize")), "MaxAttachmentsSize", 10000000);
      this.AllowedExtensions = (IList<string>) this.GetString(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "AllowedExtensions")), "AllowedExtensions", "jpg;png;gif").Split(new char[1]
      {
        ';'
      }, StringSplitOptions.RemoveEmptyEntries);
      this.ShowOldPostsOnReply = this.GetBoolean(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "ShowOldPostsOnReply")), "ShowOldPostsOnReply", true);
      this.PostsOnReply = this.GetInt32(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "PostsOnReply")), "PostsOnReply", 10);
      this.AllowUserDefinedTheme = this.GetBoolean(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "AllowUserDefinedTheme")), "AllowUserDefinedTheme", true);
      this.Theme = this.GetString(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "Theme")), "Theme", "");
      this.SpamAverage = this.GetInt32(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "SpamAverage")), "SpamAverage", 100);
      this.NewUserGroups = this.GetInt32List(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "NewUserGroups")), "NewUserGroups");
      this.CheckForNews = this.GetBoolean(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "CheckForNews")), "CheckForNews", false);
      this.Editor = this.GetString(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "Editor")), "Editor", "");
      this.ShowDeletedMessages = this.GetBoolean(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "ShowDeletedMessages")), "ShowDeletedMessages", true);
      this.ShowOnlineUsers = this.GetBoolean(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "ShowOnlineUsers")), "ShowOnlineUsers", false);
      this.UseForumAccountController = this.GetBoolean(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "UseForumAccountController")), "UseForumAccountController", true);
      this.AllowLocalUsers = this.GetBoolean(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "AllowLocalUsers")), "AllowLocalUsers", true);
      this.AllowSignUp = this.GetBoolean(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "AllowSignUp")), "AllowSignUp", true);
      this.RequireEmailValidation = this.GetBoolean(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "RequireEmailValidation")), "RequireEmailValidation", true);
      this.AllowOpenAuthUsers = this.GetBoolean(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "AllowOpenAuthUsers")), "AllowOpenAuthUsers", false);
      this.ValidationSubject = this.GetString(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "ValidationSubject")), "ValidationSubject", "New account on MVC Forum");
      this.ValidationBody = this.GetString(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "ValidationBody")), "ValidationBody", "Welcome to your new account on MVC Forum, please follow the link to activate your account {Url}");
      this.RequireRulesAccept = this.GetBoolean(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "RequireRulesAccept")), "RequireRulesAccept", false);
      this.SignUpRules = this.GetString(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "SignUpRules")), "SignUpRules", "");
      this.InformOnQuarantine = this.GetBoolean(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "InformOnQuarantine")), "InformOnQuarantine", true);
      this.ForgottenPasswordSubject = this.GetString(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "ForgottenPasswordSubject")), "ForgottenPasswordSubject", "Your new password from MVC Forum");
      this.ForgottenPasswordBody = this.GetString(settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == "ForgottenPasswordBody")), "ForgottenPasswordBody", "You have requested a new password for your account on MVC Forum. Your new password is: {Password}");
      this.context.SaveChanges();
    }

    private int GetInt32(IEnumerable<ForumSettings> setting, string key, int defaultValue)
    {
      ForumSettings newEntity;
      if (setting == null || !setting.Any<ForumSettings>())
      {
        newEntity = new ForumSettings(key, defaultValue);
        this.settingsRepo.Create(newEntity);
      }
      else
        newEntity = setting.First<ForumSettings>();
      if (!newEntity.GetInt32().HasValue)
        newEntity.SetInt32(defaultValue);
      return newEntity.GetInt32().Value;
    }

    private string GetString(IEnumerable<ForumSettings> setting, string key, string defaultValue)
    {
      ForumSettings newEntity;
      if (setting == null || !setting.Any<ForumSettings>())
      {
        newEntity = new ForumSettings(key, defaultValue);
        this.settingsRepo.Create(newEntity);
      }
      else
        newEntity = setting.First<ForumSettings>();
      return newEntity.Value;
    }

    private IList<int> GetInt32List(IEnumerable<ForumSettings> setting, string key)
    {
      ForumSettings newEntity;
      if (setting == null || !setting.Any<ForumSettings>())
      {
        newEntity = new ForumSettings(key, "");
        this.settingsRepo.Create(newEntity);
      }
      else
        newEntity = setting.First<ForumSettings>();
      return newEntity.GetInt32List();
    }

    private bool GetBoolean(IEnumerable<ForumSettings> setting, string key, bool defaultValue)
    {
      ForumSettings newEntity;
      if (setting == null || !setting.Any<ForumSettings>())
      {
        newEntity = new ForumSettings(key, defaultValue);
        this.settingsRepo.Create(newEntity);
      }
      else
        newEntity = setting.First<ForumSettings>();
      return newEntity.GetBoolean();
    }

    private void SetString(string key, string value)
    {
      IEnumerable<ForumSettings> source = this.Settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == key));
      if (!source.Any<ForumSettings>())
        this.settingsRepo.Create(new ForumSettings(key, value));
      else
        source.First<ForumSettings>().Value = value;
    }

    private void SetInt32(string key, int value)
    {
      IEnumerable<ForumSettings> source = this.Settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == key));
      if (!source.Any<ForumSettings>())
        this.settingsRepo.Create(new ForumSettings(key, value));
      else
        source.First<ForumSettings>().SetInt32(value);
    }

    private void SetInt32List(string key, IList<int> values)
    {
      IEnumerable<ForumSettings> source = this.Settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == key));
      ForumSettings newEntity;
      if (!source.Any<ForumSettings>())
      {
        newEntity = new ForumSettings(key, "");
        this.settingsRepo.Create(newEntity);
      }
      else
        newEntity = source.First<ForumSettings>();
      newEntity.SetInt32List(values);
    }

    private void SetBoolean(string key, bool value)
    {
      IEnumerable<ForumSettings> source = this.Settings.Where<ForumSettings>((Func<ForumSettings, bool>) (x => x.Key == key));
      if (!source.Any<ForumSettings>())
        this.settingsRepo.Create(new ForumSettings(key, value));
      else
        source.First<ForumSettings>().SetBoolean(value);
    }

    public void Save()
    {
      this.SetInt32("TopicsPerPage", this.TopicsPerPage);
      this.SetInt32("MessagesPerPage", this.MessagesPerPage);
      this.SetString("DefaultCulture", this.DefaultCulture);
      this.SetString("DefaultTimezone", this.DefaultTimezone);
      this.SetString("SiteURL", this.SiteURL);
      this.SetString("RobotEmailAddress", this.RobotEmailAddress);
      this.SetString("RobotName", this.RobotName);
      this.SetBoolean("AsynchronousAntiSpam", this.AsynchronousAntiSpam);
      this.SetInt32("MaxFileSize", this.MaxFileSize);
      this.SetInt32("MaxAttachmentsSize", this.MaxAttachmentsSize);
      this.SetString("AllowedExtensions", string.Join(";", (IEnumerable<string>) this.AllowedExtensions));
      this.SetBoolean("ShowOldPostsOnReply", this.ShowOldPostsOnReply);
      this.SetInt32("PostsOnReply", this.PostsOnReply);
      this.SetBoolean("AllowUserDefinedTheme", this.AllowUserDefinedTheme);
      this.SetString("Theme", this.Theme);
      this.SetInt32("SpamAverage", this.SpamAverage);
      this.SetInt32List("NewUserGroups", this.NewUserGroups);
      this.SetBoolean("CheckForNews", this.CheckForNews);
      this.SetString("Editor", this.Editor);
      this.SetBoolean("ShowDeletedMessages", this.ShowDeletedMessages);
      this.SetBoolean("ShowOnlineUsers", this.ShowOnlineUsers);
      this.SetBoolean("UseForumAccountController", this.UseForumAccountController);
      this.SetBoolean("AllowLocalUsers", this.AllowLocalUsers);
      this.SetBoolean("AllowSignUp", this.AllowSignUp);
      this.SetBoolean("RequireEmailValidation", this.RequireEmailValidation);
      this.SetBoolean("AllowOpenAuthUsers", this.AllowOpenAuthUsers);
      this.SetString("ValidationSubject", this.ValidationSubject);
      this.SetString("ValidationBody", this.ValidationBody);
      this.SetBoolean("RequireRulesAccept", this.RequireRulesAccept);
      this.SetString("SignUpRules", this.SignUpRules);
      this.SetBoolean("InformOnQuarantine", this.InformOnQuarantine);
      this.context.SaveChanges();
    }

    public int TopicsPerPage { get; set; }

    public int MessagesPerPage { get; set; }

    public string DefaultCulture { get; set; }

    public string DefaultTimezone { get; set; }

    public string SiteURL { get; set; }

    public string RobotEmailAddress { get; set; }

    public string RobotName { get; set; }

    public bool AsynchronousAntiSpam { get; set; }

    public int MaxFileSize { get; set; }

    public int MaxAttachmentsSize { get; set; }

    public IList<string> AllowedExtensions { get; set; }

    public IList<int> NewUserGroups { get; set; }

    public bool ShowDeletedMessages { get; set; }

    public bool ShowOldPostsOnReply { get; set; }

    public int PostsOnReply { get; set; }

    public bool AllowUserDefinedTheme { get; set; }

    public string Theme { get; set; }

    public bool Editable
    {
      get
      {
        return true;
      }
    }

    public int SpamAverage { get; set; }

    public bool CheckForNews { get; set; }

    public string Editor { get; set; }

    public bool ShowOnlineUsers { get; set; }

    public bool UseForumAccountController { get; set; }

    public bool AllowSignUp { get; set; }

    public bool AllowLocalUsers { get; set; }

    public bool RequireEmailValidation { get; set; }

    public string ValidationSubject { get; set; }

    public string ValidationBody { get; set; }

    public string ForgottenPasswordSubject { get; set; }

    public string ForgottenPasswordBody { get; set; }

    public bool AllowOpenAuthUsers { get; set; }

    public bool RequireRulesAccept { get; set; }

    public string SignUpRules { get; set; }

    public bool InformOnQuarantine { get; set; }

    private static class Keys
    {
      public const string TopicsPerPage = "TopicsPerPage";
      public const string MessagesPerPage = "MessagesPerPage";
      public const string DefaultCulture = "DefaultCulture";
      public const string DefaultTimezone = "DefaultTimezone";
      public const string SiteURL = "SiteURL";
      public const string RobotEmailAddress = "RobotEmailAddress";
      public const string RobotName = "RobotName";
      public const string AsynchronousAntiSpam = "AsynchronousAntiSpam";
      public const string MaxFileSize = "MaxFileSize";
      public const string MaxAttachmentsSize = "MaxAttachmentsSize";
      public const string AllowedExtensions = "AllowedExtensions";
      public const string ShowOldPostsOnReply = "ShowOldPostsOnReply";
      public const string PostsOnReply = "PostsOnReply";
      public const string AllowUserDefinedTheme = "AllowUserDefinedTheme";
      public const string Theme = "Theme";
      public const string SpamAverage = "SpamAverage";
      public const string NewUserGroups = "NewUserGroups";
      public const string CheckForNews = "CheckForNews";
      public const string Editor = "Editor";
      public const string ShowDeletedMessages = "ShowDeletedMessages";
      public const string ShowOnlineUsers = "ShowOnlineUsers";
      public const string UseForumAccountController = "UseForumAccountController";
      public const string AllowSignUp = "AllowSignUp";
      public const string AllowLocalUsers = "AllowLocalUsers";
      public const string AllowOpenAuthUsers = "AllowOpenAuthUsers";
      public const string RequireEmailValidation = "RequireEmailValidation";
      public const string ValidationSubject = "ValidationSubject";
      public const string ValidationBody = "ValidationBody";
      public const string ForgottenPasswordSubject = "ForgottenPasswordSubject";
      public const string ForgottenPasswordBody = "ForgottenPasswordBody";
      public const string RequireRulesAccept = "RequireRulesAccept";
      public const string SignUpRules = "SignUpRules";
      public const string InformOnQuarantine = "InformOnQuarantine";
    }
  }
}

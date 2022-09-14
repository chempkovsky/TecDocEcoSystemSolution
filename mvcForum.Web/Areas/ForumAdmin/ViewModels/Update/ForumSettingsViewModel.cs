// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAdmin.ViewModels.Update.ForumSettingsViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using mvcForum.Core.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mvcForum.Web.Areas.ForumAdmin.ViewModels.Update
{
  public class ForumSettingsViewModel
  {
    public ForumSettingsViewModel()
    {
    }

    public ForumSettingsViewModel(IConfiguration config, string themeRoot)
    {
      this.Editable = config.Editable;
      this.TopicsPerPage = config.TopicsPerPage;
      this.MessagesPerPage = config.MessagesPerPage;
      this.DefaultCulture = config.DefaultCulture;
      this.DefaultTimezone = config.DefaultTimezone;
      this.SiteURL = config.SiteURL;
      this.RobotEmailAddress = config.RobotEmailAddress;
      this.RobotName = config.RobotName;
      this.AsynchronousAntiSpam = config.AsynchronousAntiSpam;
      this.MaxAttachmentsSize = config.MaxAttachmentsSize / 1000000;
      this.MaxFileSize = config.MaxFileSize / 1000000;
      this.ShowOldPostsOnReply = config.ShowOldPostsOnReply;
      this.PostsOnReply = config.PostsOnReply;
      this.AllowUserDefinedTheme = config.AllowUserDefinedTheme;
      this.Theme = config.Theme;
      this.CheckForNews = config.CheckForNews;
      this.Editor = config.Editor;
      this.NewUserGroups = config.NewUserGroups.Select<int, string>((Func<int, string>) (i => i.ToString())).ToArray<string>();
      this.AllowedExtensions = string.Join(";", (IEnumerable<string>) config.AllowedExtensions);
      this.ShowDeletedMessages = config.ShowDeletedMessages;
      this.ShowOnlineUsers = config.ShowOnlineUsers;
      this.InformOnQuarantine = config.InformOnQuarantine;
      this.Themes = new string[0];
      if (!Directory.Exists(themeRoot))
        return;
      this.Themes = ((IEnumerable<string>) Directory.GetDirectories(themeRoot)).Select<string, string>((Func<string, string>) (d => d.Substring(themeRoot.Length + 1))).ToArray<string>();
    }

    public bool Editable { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "TopicsPerPage")]
    public int TopicsPerPage { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "MessagesPerPage")]
    public int MessagesPerPage { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "ShowDeletedMessages")]
    public bool ShowDeletedMessages { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "DefaultCulture")]
    public string DefaultCulture { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "DefaultTimezone")]
    public string DefaultTimezone { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "SiteURL")]
    public string SiteURL { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "RobotEmailAddress")]
    public string RobotEmailAddress { get; set; }

    public string RobotName { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "AsynchronousAntiSpam")]
    public bool AsynchronousAntiSpam { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "MaxFileSize")]
    public int MaxFileSize { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "MaxAttachmentsSize")]
    public int MaxAttachmentsSize { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "AllowedExtensions")]
    public string AllowedExtensions { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "ShowOldPostsOnReply")]
    public bool ShowOldPostsOnReply { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "PostsOnReply")]
    public int PostsOnReply { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "AllowUserDefinedTheme")]
    public bool AllowUserDefinedTheme { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "Theme")]
    public string Theme { get; set; }

    public bool CheckForNews { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "Editor")]
    public string Editor { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "NewUserGroups")]
    public string[] NewUserGroups { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "ShowOnlineUsers")]
    public bool ShowOnlineUsers { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumSettingsViewModel", "InformOnQuarantine")]
    public bool InformOnQuarantine { get; set; }

    public string[] Themes { get; set; }
  }
}

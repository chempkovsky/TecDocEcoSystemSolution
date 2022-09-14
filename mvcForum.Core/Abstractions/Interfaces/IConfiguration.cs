// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Abstractions.Interfaces.IConfiguration
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.Collections.Generic;

namespace mvcForum.Core.Abstractions.Interfaces
{
  public interface IConfiguration
  {
    int TopicsPerPage { get; set; }

    int MessagesPerPage { get; set; }

    string DefaultCulture { get; set; }

    string DefaultTimezone { get; set; }

    bool ShowDeletedMessages { get; set; }

    string SiteURL { get; set; }

    string RobotEmailAddress { get; set; }

    string RobotName { get; set; }

    bool AsynchronousAntiSpam { get; set; }

    int MaxFileSize { get; set; }

    int MaxAttachmentsSize { get; set; }

    IList<string> AllowedExtensions { get; set; }

    IList<int> NewUserGroups { get; set; }

    bool ShowOldPostsOnReply { get; set; }

    int PostsOnReply { get; set; }

    bool AllowUserDefinedTheme { get; set; }

    string Theme { get; set; }

    int SpamAverage { get; set; }

    void Save();

    bool Editable { get; }

    bool CheckForNews { get; set; }

    string Editor { get; set; }

    bool ShowOnlineUsers { get; set; }

    bool UseForumAccountController { get; set; }

    bool AllowLocalUsers { get; set; }

    bool AllowSignUp { get; set; }

    bool RequireEmailValidation { get; set; }

    string ValidationSubject { get; set; }

    string ValidationBody { get; set; }

    string ForgottenPasswordSubject { get; set; }

    string ForgottenPasswordBody { get; set; }

    bool RequireRulesAccept { get; set; }

    string SignUpRules { get; set; }

    bool AllowOpenAuthUsers { get; set; }

    bool InformOnQuarantine { get; set; }
  }
}

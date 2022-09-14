// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Configuration.MVCForumWebConfig
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using mvcForum.Core.Abstractions.Interfaces;
using System;
using System.Collections.Generic;

namespace mvcForum.Core.Configuration
{
  public class MVCForumWebConfig : IConfiguration
  {
    public void Save()
    {
      throw new NotImplementedException();
    }

    public int TopicsPerPage
    {
      get
      {
        return MVCForumSettings.Settings.TopicsPerPage;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int MessagesPerPage
    {
      get
      {
        return MVCForumSettings.Settings.MessagesPerPage;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string DefaultCulture
    {
      get
      {
        return MVCForumSettings.Settings.DefaultCulture;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string DefaultTimezone
    {
      get
      {
        return MVCForumSettings.Settings.DefaultTimezone;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string SiteURL
    {
      get
      {
        return MVCForumSettings.Settings.SiteURL;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string RobotEmailAddress
    {
      get
      {
        return MVCForumSettings.Settings.RobotEmailAddress;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string RobotName
    {
      get
      {
        return MVCForumSettings.Settings.RobotName;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool AsynchronousAntiSpam
    {
      get
      {
        return MVCForumSettings.Settings.AsynchronousAntiSpam;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool ShowDeletedMessages
    {
      get
      {
        return MVCForumSettings.Settings.ShowDeletedMessages;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int MaxAttachmentsSize
    {
      get
      {
        return MVCForumSettings.Settings.MaxAttachmentsSize;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int MaxFileSize
    {
      get
      {
        return MVCForumSettings.Settings.MaxFileSize;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public IList<int> NewUserGroups
    {
      get
      {
        return (IList<int>) new List<int>();
      }
      set
      {
      }
    }

    public IList<string> AllowedExtensions
    {
      get
      {
        return (IList<string>) new List<string>((IEnumerable<string>) MVCForumSettings.Settings.AllowedExtensions.Split(new char[1]
        {
          ';'
        }, StringSplitOptions.RemoveEmptyEntries));
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool ShowOldPostsOnReply
    {
      get
      {
        return MVCForumSettings.Settings.ShowOldPostsOnReply;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int PostsOnReply
    {
      get
      {
        return MVCForumSettings.Settings.PostsOnReply;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public int SpamAverage
    {
      get
      {
        return 100;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool AllowUserDefinedTheme
    {
      get
      {
        return MVCForumSettings.Settings.AllowUserDefinedTheme;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string Theme
    {
      get
      {
        return MVCForumSettings.Settings.Theme;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string SubscriptionSubject
    {
      get
      {
        return "";
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string Editor
    {
      get
      {
        return "";
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool CheckForNews
    {
      get
      {
        return true;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool Editable
    {
      get
      {
        return false;
      }
    }

    public bool ShowOnlineUsers
    {
      get
      {
        return false;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool UseForumAccountController
    {
      get
      {
        return false;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool AllowSignUp
    {
      get
      {
        return false;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool AllowLocalUsers
    {
      get
      {
        return false;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool AllowOpenAuthUsers
    {
      get
      {
        return false;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool RequireEmailValidation
    {
      get
      {
        return false;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string ValidationSubject
    {
      get
      {
        return string.Empty;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string ValidationBody
    {
      get
      {
        return string.Empty;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string ForgottenPasswordSubject
    {
      get
      {
        return string.Empty;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string ForgottenPasswordBody
    {
      get
      {
        return string.Empty;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool RequireRulesAccept
    {
      get
      {
        return false;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public string SignUpRules
    {
      get
      {
        return string.Empty;
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool InformOnQuarantine
    {
      get
      {
        return false;
      }
      set
      {
        throw new NotImplementedException();
      }
    }
  }
}

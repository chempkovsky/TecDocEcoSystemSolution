// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.Update.UpdateUserViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using mvcForum.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace mvcForum.Web.ViewModels.Update
{
  public class UpdateUserViewModel
  {
    public UpdateUserViewModel()
    {
    }

    public UpdateUserViewModel(
      ForumUser user,
      bool external,
      bool allowUserTheme,
      string themeRoot)
    {
      this.Culture = user.Culture;
      this.Timezone = user.Timezone;
      this.FullName = user.FullName;
      this.Name = user.Name;
      this.Email = user.EmailAddress.EndsWith("repl@ce.this") ? string.Empty : user.EmailAddress;
      this.Id = user.Id;
      this.ExternalUser = external;
      this.AllowUserTheme = allowUserTheme;
      this.Theme = user.Theme;
      this.Themes = new string[0];
      if (!this.AllowUserTheme || !Directory.Exists(themeRoot))
        return;
      this.Themes = ((IEnumerable<string>) Directory.GetDirectories(themeRoot)).Select<string, string>((Func<string, string>) (d => d.Substring(themeRoot.Length + 1))).ToArray<string>();
    }

    [Required]
    public int Id { get; set; }

    [LocalizedDisplay(typeof (UpdateUserViewModel), "Culture")]
    [Required]
    public string Culture { get; set; }

    [LocalizedDisplay(typeof (UpdateUserViewModel), "Timezone")]
    [Required]
    public string Timezone { get; set; }

    [LocalizedDisplay(typeof (UpdateUserViewModel), "FullName")]
    public string FullName { get; set; }

    [LocalizedDisplay(typeof (UpdateUserViewModel), "Name")]
    public string Name { get; set; }

    [LocalizedDisplay(typeof (UpdateUserViewModel), "Email")]
    [Required]
    public string Email { get; set; }

    [LocalizedDisplay(typeof (UpdateUserViewModel), "OldPassword")]
    public string OldPassword { get; set; }

    [LocalizedDisplay(typeof (UpdateUserViewModel), "NewPassword")]
    public string NewPassword { get; set; }

    [LocalizedDisplay(typeof (UpdateUserViewModel), "RepeatNewPassword")]
    public string RepeatedNewPassword { get; set; }

    [LocalizedDisplay(typeof (UpdateUserViewModel), "Theme")]
    public string Theme { get; set; }

    public bool AllowUserTheme { get; set; }

    public string[] Themes { get; set; }

    public bool ExternalUser { get; private set; }
  }
}

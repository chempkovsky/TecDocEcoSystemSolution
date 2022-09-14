// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAdmin.ViewModels.Update.UserRegistrationViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using mvcForum.Core.Abstractions.Interfaces;

namespace mvcForum.Web.Areas.ForumAdmin.ViewModels.Update
{
  public class UserRegistrationViewModel
  {
    public UserRegistrationViewModel()
    {
    }

    public UserRegistrationViewModel(IConfiguration config)
    {
      this.Editable = config.Editable;
      this.UseForumAccountController = config.UseForumAccountController;
      this.AllowLocalUsers = config.AllowLocalUsers;
      this.AllowSignUp = config.AllowSignUp;
      this.RequireEmailValidation = config.RequireEmailValidation;
      this.AllowOpenAuthUsers = config.AllowOpenAuthUsers;
      this.ValidationSubject = config.ValidationSubject;
      this.ValidationBody = config.ValidationBody;
      this.RequireRulesAccept = config.RequireRulesAccept;
      this.SignUpRules = config.SignUpRules;
      this.ForgottenPasswordSubject = config.ForgottenPasswordSubject;
      this.ForgottenPasswordBody = config.ForgottenPasswordBody;
    }

    public bool Editable { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.UserRegistrationViewModel", "UseForumAccountController")]
    public bool UseForumAccountController { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.UserRegistrationViewModel", "AllowLocalUsers")]
    public bool AllowLocalUsers { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.UserRegistrationViewModel", "AllowSignUp")]
    public bool AllowSignUp { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.UserRegistrationViewModel", "RequireEmailValidation")]
    public bool RequireEmailValidation { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.UserRegistrationViewModel", "AllowOpenAuthUsers")]
    public bool AllowOpenAuthUsers { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.UserRegistrationViewModel", "ValidationSubject")]
    public string ValidationSubject { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.UserRegistrationViewModel", "ValidationBody")]
    public string ValidationBody { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.UserRegistrationViewModel", "ForgottenPasswordSubject")]
    public string ForgottenPasswordSubject { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.UserRegistrationViewModel", "ForgottenPasswordBody")]
    public string ForgottenPasswordBody { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.UserRegistrationViewModel", "RequireRulesAccept")]
    public bool RequireRulesAccept { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.UserRegistrationViewModel", "SignUpRules")]
    public string SignUpRules { get; set; }
  }
}

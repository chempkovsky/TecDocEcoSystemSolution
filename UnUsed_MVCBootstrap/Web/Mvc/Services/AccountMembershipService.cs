// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Services.AccountMembershipService
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using MVCBootstrap.Web.Mvc.Interfaces;
using SimpleLocalisation;
using System;
using System.Web.Security;

namespace MVCBootstrap.Web.Mvc.Services
{
  public class AccountMembershipService : IMembershipService
  {
    private readonly MembershipProvider membership;
    private readonly RoleProvider role;

    public AccountMembershipService()
      : this((MembershipProvider) null, (RoleProvider) null)
    {
    }

    public AccountMembershipService(MembershipProvider membership, RoleProvider role)
    {
      this.membership = membership ?? Membership.Provider;
      this.role = role ?? Roles.Provider;
    }

    public string GetUsername(string email)
    {
      return this.membership.GetUserNameByEmail(email);
    }

    public MembershipUser GetUser(string name)
    {
      return this.membership.GetUser(name, false);
    }

    public bool ValidateUser(string userName, string password)
    {
      if (string.IsNullOrEmpty(userName))
        throw new ArgumentException("Value cannot be null or empty.", nameof (userName));
      if (string.IsNullOrEmpty(password))
        throw new ArgumentException("Value cannot be null or empty.", nameof (password));
      return this.membership.ValidateUser(userName, password);
    }

    public bool UnlockUser(string username)
    {
      return this.membership.UnlockUser(username);
    }

    public MembershipCreateStatus CreateUser(
      string userName,
      string password,
      string email)
    {
      return this.CreateUser(userName, password, email, true);
    }

    public MembershipCreateStatus CreateUser(
      string userName,
      string password,
      string email,
      bool isApproved)
    {
      if (string.IsNullOrEmpty(userName))
        throw new ArgumentException("Value cannot be null or empty.", nameof (userName));
      if (string.IsNullOrEmpty(password))
        throw new ArgumentException("Value cannot be null or empty.", nameof (password));
      if (string.IsNullOrEmpty(email))
        throw new ArgumentException("Value cannot be null or empty.", nameof (email));
      MembershipCreateStatus status;
      this.membership.CreateUser(userName, password, email, (string) null, (string) null, isApproved, (object) null, out status);
      return status;
    }

    public bool ChangePassword(string userName, string oldPassword, string newPassword)
    {
      if (string.IsNullOrEmpty(userName))
        throw new ArgumentException("Value cannot be null or empty.", nameof (userName));
      if (string.IsNullOrEmpty(oldPassword))
        throw new ArgumentException("Value cannot be null or empty.", nameof (oldPassword));
      if (string.IsNullOrEmpty(newPassword))
        throw new ArgumentException("Value cannot be null or empty.", nameof (newPassword));
      try
      {
        return this.membership.GetUser(userName, true).ChangePassword(oldPassword, newPassword);
      }
      catch (ArgumentException ex)
      {
        return false;
      }
      catch (MembershipPasswordException ex)
      {
        return false;
      }
    }

    public string ErrorCodeToString(MembershipCreateStatus createStatus, TextManager texts)
    {
      switch (createStatus)
      {
        case MembershipCreateStatus.InvalidUserName:
        case MembershipCreateStatus.InvalidPassword:
        case MembershipCreateStatus.InvalidQuestion:
        case MembershipCreateStatus.InvalidAnswer:
        case MembershipCreateStatus.InvalidEmail:
        case MembershipCreateStatus.DuplicateUserName:
        case MembershipCreateStatus.DuplicateEmail:
        case MembershipCreateStatus.UserRejected:
        case MembershipCreateStatus.ProviderError:
          TextManager textManager = texts;
          string str = "mvcForum.WebUI.MembershipErrors";
          string key = createStatus.ToString();
          string ns = str;
          return textManager.Get(key, (object) null, ns);
        default:
          return texts.Get("Default", (object) null, "mvcForum.WebUI.MembershipErrors");
      }
    }

    public void Update(MembershipUser user)
    {
      this.membership.UpdateUser(user);
    }
  }
}

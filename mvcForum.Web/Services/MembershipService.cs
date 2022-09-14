// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Services.MembershipService
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core.Interfaces.Services;
using SimpleLocalisation;
using System;
using System.Collections.Generic;
using System.Web.Security;

namespace mvcForum.Web.Services
{
  public class MembershipService : IMembershipService
  {
    private TextManager text;

    public MembershipService(TextManager text)
    {
      this.text = text;
    }

    public bool CreateAccount(
      string accountName,
      string password,
      string emailAddress,
      out string errorMessage)
    {
      errorMessage = string.Empty;
      if (string.IsNullOrWhiteSpace(accountName))
        throw new ArgumentException("Value cannot be null or empty.", "userName");
      if (string.IsNullOrWhiteSpace(password))
        throw new ArgumentException("Value cannot be null or empty.", nameof (password));
      if (string.IsNullOrWhiteSpace(emailAddress))
        throw new ArgumentException("Value cannot be null or empty.", "email");
      MembershipCreateStatus status;
      Membership.CreateUser(accountName, password, emailAddress, (string) null, (string) null, true, (object) null, out status);
      if (status != MembershipCreateStatus.Success)
        errorMessage = this.ErrorCodeToString(status, this.text);
      return status == MembershipCreateStatus.Success;
    }

    public void UnlockAccount(string accountName)
    {
      Membership.Provider.UnlockUser(accountName);
    }

    private IAccount GetAccountInstance(MembershipUser user)
    {
      return (IAccount) new Account()
      {
        AccountName = user.UserName,
        CreationDate = user.CreationDate,
        EmailAddress = user.Email,
        IsApproved = user.IsApproved,
        IsLockedOut = user.IsLockedOut,
        LastActivityDate = user.LastActivityDate,
        ProviderUserKey = user.ProviderUserKey,
        LastLockoutDate = user.LastLockoutDate,
        LastLoginDate = user.LastLoginDate
      };
    }

    private MembershipUser GetMUInstance(IAccount account)
    {
      return new MembershipUser(Membership.Provider.Name, account.AccountName, account.ProviderUserKey, account.EmailAddress, (string) null, (string) null, account.IsApproved, account.IsLockedOut, account.CreationDate, account.LastLoginDate, account.LastActivityDate, DateTime.MinValue, account.LastLockoutDate);
    }

    public string GetAccountNameByEmailAddress(string emailAddress)
    {
      return Membership.GetUserNameByEmail(emailAddress);
    }

    public IAccount GetAccount(bool online)
    {
      return this.GetAccountInstance(Membership.GetUser(online));
    }

    public IAccount GetAccount(object id)
    {
      return this.GetAccountInstance(Membership.GetUser(id));
    }

    public IAccount GetAccountByName(string accountName)
    {
      return this.GetAccountInstance(Membership.GetUser(accountName, false));
    }

    public IAccount GetAccountByName(string accountName, bool online)
    {
      return this.GetAccountInstance(Membership.GetUser(accountName, online));
    }

    public IAccount GetAccountByEmailAddress(string emailAddress)
    {
      return this.GetAccountInstance(Membership.GetUser(Membership.GetUserNameByEmail(emailAddress), false));
    }

    public void UpdateAccount(IAccount account)
    {
      Membership.UpdateUser(this.GetMUInstance(account));
    }

    public void DeleteAccount(string accountName)
    {
      this.DeleteAccount(accountName, true);
    }

    public void DeleteAccount(string accountName, bool deleteAllRelatedData)
    {
      Membership.DeleteUser(accountName, deleteAllRelatedData);
    }

    public IEnumerable<IAccount> GetAllAccounts(
      int pageIndex,
      int pageSize,
      out int records)
    {
      List<IAccount> accountList = new List<IAccount>();
      foreach (MembershipUser allUser in Membership.GetAllUsers(pageIndex, pageSize, out records))
        accountList.Add(this.GetAccountInstance(allUser));
      return (IEnumerable<IAccount>) accountList;
    }

    public void CreateRole(string roleName)
    {
      Roles.CreateRole(roleName);
    }

    public string[] GetRolesForAccount(string accountName)
    {
      return Roles.GetRolesForUser(accountName);
    }

    public string[] GetAllRoles()
    {
      return Roles.GetAllRoles();
    }

    public bool IsAccountInRole(string accountName, string roleName)
    {
      return Roles.IsUserInRole(accountName, roleName);
    }

    public void RemoveAccountFromRoles(string accountName, string[] roles)
    {
      Roles.RemoveUserFromRoles(accountName, roles);
    }

    public void AddAccountToRoles(string accountName, string[] roles)
    {
      Roles.AddUserToRoles(accountName, roles);
    }

    public bool ValidateAccount(string accountName, string password)
    {
      return Membership.ValidateUser(accountName, password);
    }

    private string ErrorCodeToString(MembershipCreateStatus createStatus, TextManager texts)
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
  }
}

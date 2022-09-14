// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Interfaces.Services.IMembershipService
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.Collections.Generic;

namespace mvcForum.Core.Interfaces.Services
{
  public interface IMembershipService
  {
    bool CreateAccount(
      string accountName,
      string password,
      string emailAddress,
      out string errorMessage);

    void UnlockAccount(string accountName);

    IAccount GetAccount(object id);

    IAccount GetAccountByName(string accountName);

    IAccount GetAccountByName(string accountName, bool online);

    string GetAccountNameByEmailAddress(string emailAddress);

    IAccount GetAccountByEmailAddress(string emailAddress);

    void UpdateAccount(IAccount account);

    void DeleteAccount(string accountName);

    void DeleteAccount(string accountName, bool deleteAllRelatedData);

    bool ValidateAccount(string accountName, string password);

    IEnumerable<IAccount> GetAllAccounts(int page, int pageSize, out int total);

    void CreateRole(string roleName);

    string[] GetAllRoles();

    bool IsAccountInRole(string accountName, string roleName);

    string[] GetRolesForAccount(string accountName);

    void RemoveAccountFromRoles(string accountName, string[] roles);

    void AddAccountToRoles(string accountName, string[] roles);
  }
}

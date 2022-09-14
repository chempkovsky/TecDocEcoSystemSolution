// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Interfaces.IMembershipService
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using SimpleLocalisation;
using System.Web.Security;

namespace MVCBootstrap.Web.Mvc.Interfaces
{
  public interface IMembershipService
  {
    bool ValidateUser(string userName, string password);

    MembershipCreateStatus CreateUser(
      string userName,
      string password,
      string email);

    MembershipCreateStatus CreateUser(
      string userName,
      string password,
      string email,
      bool isApproved);

    bool ChangePassword(string userName, string oldPassword, string newPassword);

    string GetUsername(string email);

    MembershipUser GetUser(string name);

    bool UnlockUser(string username);

    string ErrorCodeToString(MembershipCreateStatus createStatus, TextManager texts);

    void Update(MembershipUser user);
  }
}

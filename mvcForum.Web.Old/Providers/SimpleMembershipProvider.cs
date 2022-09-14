// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Providers.SimpleMembershipProvider
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using MVCBootstrap.Web.Security;
using mvcForum.Core;
using mvcForum.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace mvcForum.Web.Providers
{
  public class SimpleMembershipProvider : MembershipProvider
  {
    private int passwordAttemptWindow;
    private int userIsOnlineTimeWindow;
    private int maxInvalidPasswordAttempts;
    private int minPasswordLength;

    private static string GetHashedString(string value)
    {
      byte[] hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(value));
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in hash)
        stringBuilder.Append(num.ToString("x2").ToLower());
      return stringBuilder.ToString();
    }

    private MembershipUser GetMembershipUser(User user)
    {
      return new MembershipUser(this.Name, user.Username, (object) user.Id, user.EmailAddress, (string) null, (string) null, true, false, user.Created, user.LastVisit, user.LastVisit, DateTime.MinValue, DateTime.MinValue);
    }

    protected IContext Context
    {
      get
      {
        return DependencyResolver.Current.GetService<IContext>();
      }
    }

    protected IRepository<User> UserRepository
    {
      get
      {
        return this.Context.GetRepository<User>();
      }
    }

    public override void Initialize(string name, NameValueCollection config)
    {
      if (config == null)
        throw new ArgumentNullException(nameof (config));
      base.Initialize(name, config);
      string s1 = config["passwordAttemptWindow"];
      int result;
      this.passwordAttemptWindow = string.IsNullOrWhiteSpace(s1) || !int.TryParse(s1, out result) ? 10 : result;
      string s2 = config["userIsOnlineTimeWindow"];
      this.userIsOnlineTimeWindow = string.IsNullOrWhiteSpace(s2) || !int.TryParse(s2, out result) ? 10 : result;
      string s3 = config["maxInvalidPasswordAttempts"];
      this.maxInvalidPasswordAttempts = string.IsNullOrWhiteSpace(s3) || !int.TryParse(s3, out result) ? 5 : result;
      string s4 = config["passwordAttemptLockoutDuration"];
      this.PasswordAttemptLockoutDuration = string.IsNullOrWhiteSpace(s4) || !int.TryParse(s4, out result) ? 30 : result;
      string s5 = config["minRequiredPasswordLength"];
      this.minPasswordLength = string.IsNullOrWhiteSpace(s5) || !int.TryParse(s5, out result) ? 7 : result;
      if (this.minPasswordLength < 1 || this.minPasswordLength > 128)
        throw new ArgumentOutOfRangeException("minRequiredPasswordLength");
    }

    public override string ApplicationName { get; set; }

    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    {
      if (string.IsNullOrWhiteSpace(username))
        throw new ArgumentNullException(nameof (username));
      if (string.IsNullOrWhiteSpace(oldPassword))
        throw new ArgumentNullException(nameof (oldPassword));
      if (string.IsNullOrWhiteSpace(newPassword))
        throw new ArgumentNullException(nameof (newPassword));
      if (newPassword.Length < this.MinRequiredPasswordLength)
        throw new ArgumentException("password");
      User user = this.UserRepository.ReadOne((ISpecification<User>) new UserSpecifications.SpecificUsernameAndPassword(username, SimpleMembershipProvider.GetHashedString(oldPassword)));
      if (user == null)
        return false;
      user.Password = SimpleMembershipProvider.GetHashedString(newPassword);
      this.Context.SaveChanges();
      return true;
    }

    public override bool ChangePasswordQuestionAndAnswer(
      string username,
      string password,
      string newPasswordQuestion,
      string newPasswordAnswer)
    {
      throw new NotImplementedException();
    }

    public override MembershipUser CreateUser(
      string username,
      string password,
      string email,
      string passwordQuestion,
      string passwordAnswer,
      bool isApproved,
      object providerUserKey,
      out MembershipCreateStatus status)
    {
      if (string.IsNullOrWhiteSpace(username))
      {
        status = MembershipCreateStatus.InvalidUserName;
        return (MembershipUser) null;
      }
      if (string.IsNullOrWhiteSpace(password) || password.Length < this.MinRequiredPasswordLength)
      {
        status = MembershipCreateStatus.InvalidPassword;
        return (MembershipUser) null;
      }
      if (string.IsNullOrWhiteSpace(email))
      {
        status = MembershipCreateStatus.InvalidEmail;
        return (MembershipUser) null;
      }
      status = MembershipCreateStatus.Success;
      if (this.UserRepository.ReadOne((ISpecification<User>) new UserSpecifications.SpecificEmailAddress(email)) != null)
      {
        status = MembershipCreateStatus.DuplicateEmail;
        return (MembershipUser) null;
      }
      if (this.UserRepository.ReadOne((ISpecification<User>) new UserSpecifications.SpecificUsername(username)) != null)
      {
        status = MembershipCreateStatus.DuplicateUserName;
        return (MembershipUser) null;
      }
      try
      {
        if (new MailAddress(email).Address != email)
        {
          status = MembershipCreateStatus.InvalidEmail;
          return (MembershipUser) null;
        }
      }
      catch
      {
        status = MembershipCreateStatus.InvalidEmail;
        return (MembershipUser) null;
      }
      User user = new User()
      {
        Username = username,
        Password = SimpleMembershipProvider.GetHashedString(password),
        EmailAddress = email,
        Created = DateTime.UtcNow,
        LastVisit = DateTime.UtcNow,
        Locked = false,
        LastPasswordFailure = DateTime.UtcNow,
        PasswordFailures = 0,
        LastLockout = DateTime.UtcNow.AddDays(-1.0),
        Approved = isApproved
      };
      this.UserRepository.Create(user);
      this.Context.SaveChanges();
      return this.GetMembershipUser(user);
    }

    public override bool DeleteUser(string username, bool deleteAllRelatedData)
    {
      throw new NotImplementedException();
    }

    public int PasswordAttemptLockoutDuration { private set; get; }

    public override bool EnablePasswordReset
    {
      get
      {
        return true;
      }
    }

    public override bool EnablePasswordRetrieval
    {
      get
      {
        return false;
      }
    }

    public override MembershipUserCollection FindUsersByEmail(
      string emailToMatch,
      int pageIndex,
      int pageSize,
      out int totalRecords)
    {
      throw new NotImplementedException();
    }

    public override MembershipUserCollection FindUsersByName(
      string usernameToMatch,
      int pageIndex,
      int pageSize,
      out int totalRecords)
    {
      throw new NotImplementedException();
    }

    public override MembershipUserCollection GetAllUsers(
      int pageIndex,
      int pageSize,
      out int totalRecords)
    {
      IEnumerable<User> source = this.UserRepository.ReadAll();
      totalRecords = source.Count<User>();
      IEnumerable<User> users = source.Skip<User>((pageIndex - 1) * pageSize).Take<User>(pageSize);
      MembershipUserCollection membershipUserCollection = new MembershipUserCollection();
      foreach (User user in users)
      {
        if (membershipUserCollection[user.Username] == null)
        {
          try
          {
            membershipUserCollection.Add(this.GetMembershipUser(user));
          }
          catch
          {
          }
        }
      }
      return membershipUserCollection;
    }

    public override int GetNumberOfUsersOnline()
    {
      TimeSpan timeSpan = TimeSpan.FromMinutes((double) Membership.UserIsOnlineTimeWindow);
      return this.Context.GetRepository<User>().ReadMany((Expression<Func<User, bool>>) (u => u.LastVisit > DateTime.UtcNow.Subtract(timeSpan))).Count<User>();
    }

    public override string GetPassword(string username, string answer)
    {
      throw new NotImplementedException();
    }

    public override MembershipUser GetUser(string username, bool userIsOnline)
    {
      User user = this.UserRepository.ReadOne((ISpecification<User>) new UserSpecifications.SpecificUsername(username));
      if (user == null)
        return (MembershipUser) null;
      if (userIsOnline)
      {
        user.LastVisit = DateTime.UtcNow;
        this.Context.SaveChanges();
      }
      return this.GetMembershipUser(user);
    }

    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
    {
      User user = (User) null;
      if (providerUserKey is int)
        user = this.UserRepository.Read((int) providerUserKey);
      else if (providerUserKey is Guid)
      {
        user = this.UserRepository.Read((Guid) providerUserKey);
      }
      else
      {
        Guid result;
        if (providerUserKey is string && Guid.TryParse((string) providerUserKey, out result))
          user = this.UserRepository.Read(result);
      }
      if (user == null)
        return (MembershipUser) null;
      if (userIsOnline)
      {
        user.LastVisit = DateTime.UtcNow;
        this.Context.SaveChanges();
      }
      return this.GetMembershipUser(user);
    }

    public override string GetUserNameByEmail(string email)
    {
      User user = this.UserRepository.ReadOne((ISpecification<User>) new UserSpecifications.SpecificEmailAddress(email));
      if (user != null)
        return user.Username;
      return "";
    }

    public override int PasswordAttemptWindow
    {
      get
      {
        return this.passwordAttemptWindow;
      }
    }

    public int UserIsOnlineTimeWindow
    {
      get
      {
        return this.userIsOnlineTimeWindow;
      }
    }

    public override int MaxInvalidPasswordAttempts
    {
      get
      {
        return this.maxInvalidPasswordAttempts;
      }
    }

    public override int MinRequiredPasswordLength
    {
      get
      {
        return this.minPasswordLength;
      }
    }

    public override int MinRequiredNonAlphanumericCharacters
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public override MembershipPasswordFormat PasswordFormat
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public override string PasswordStrengthRegularExpression
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public override bool RequiresQuestionAndAnswer
    {
      get
      {
        return false;
      }
    }

    public override bool RequiresUniqueEmail
    {
      get
      {
        return true;
      }
    }

    public override string ResetPassword(string username, string answer)
    {
      string empty = string.Empty;
      User user = this.UserRepository.ReadOne((ISpecification<User>) new UserSpecifications.SpecificUsername(username));
      if (user != null)
      {
        empty = PasswordGenerator.Generate(8, PasswordStrength.AlphaNumeric);
        user.Password = SimpleMembershipProvider.GetHashedString(empty);
        this.Context.SaveChanges();
      }
      return empty;
    }

    public override bool UnlockUser(string username)
    {
      User user = this.UserRepository.ReadOne((ISpecification<User>) new UserSpecifications.SpecificUsername(username));
      if (user == null)
        return false;
      user.Locked = false;
      this.Context.SaveChanges();
      return true;
    }

    public override void UpdateUser(MembershipUser user)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      User user1 = (User) null;
      if (user.ProviderUserKey is int)
        user1 = this.UserRepository.Read((int) user.ProviderUserKey);
      else if (user.ProviderUserKey is Guid)
        user1 = this.UserRepository.Read((Guid) user.ProviderUserKey);
      if (user1 == null)
        return;
      if (new MailAddress(user.Email).Address != user.Email)
        throw new FormatException("email");
      user1.EmailAddress = user.Email;
      user1.Username = user.UserName;
      this.Context.SaveChanges();
    }

    public override bool ValidateUser(string username, string password)
    {
      User user = this.UserRepository.ReadOne((ISpecification<User>) new UserSpecifications.SpecificUsername(username));
      if (user != null && (!user.Locked || user.Locked && DateTime.UtcNow.Subtract(user.LastLockout).TotalMinutes > (double) this.PasswordAttemptLockoutDuration) && user.Approved)
      {
        if (user.Password == SimpleMembershipProvider.GetHashedString(password))
        {
          user.LastVisit = DateTime.UtcNow;
          user.PasswordFailures = 0;
          user.Locked = false;
          this.Context.SaveChanges();
          return true;
        }
        if (DateTime.UtcNow.Subtract(user.LastPasswordFailure).TotalMinutes > (double) this.PasswordAttemptWindow)
          user.PasswordFailures = 1;
        else
          ++user.PasswordFailures;
        if (user.PasswordFailures > this.MaxInvalidPasswordAttempts)
        {
          user.Locked = true;
          user.LastLockout = DateTime.UtcNow;
        }
        user.LastPasswordFailure = DateTime.UtcNow;
        this.Context.SaveChanges();
      }
      return false;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Providers.MembershipProviderWrapper
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Events;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Events;
using mvcForum.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace mvcForum.Web.Providers
{
  public class MembershipProviderWrapper : MembershipProvider
  {
    private string wrappedProvider;
    private MembershipProvider mp;

    public override void Initialize(string name, NameValueCollection config)
    {
      base.Initialize(name, config);
      if (string.IsNullOrWhiteSpace(config["WrappedProvider"]))
        throw new ArgumentNullException("WrappedProvider");
      this.wrappedProvider = config["WrappedProvider"];
    }

    protected IContext Context
    {
      get
      {
        return DependencyResolver.Current.GetService<IContext>();
      }
    }

    protected IRepository<ForumUser> UserRepository
    {
      get
      {
        return DependencyResolver.Current.GetService<IRepository<ForumUser>>();
      }
    }

    protected IRepository<Group> GroupRepository
    {
      get
      {
        return DependencyResolver.Current.GetService<IRepository<Group>>();
      }
    }

    protected IRepository<GroupMember> GroupMemberRepository
    {
      get
      {
        return DependencyResolver.Current.GetService<IRepository<GroupMember>>();
      }
    }

    public MembershipProvider ActualProvider
    {
      get
      {
        if (this.mp == null)
        {
          this.mp = Membership.Providers[this.wrappedProvider];
          if (this.mp == null)
            throw new ArgumentNullException("wrappedProvider");
        }
        return this.mp;
      }
    }

    public override string ApplicationName
    {
      get
      {
        return this.ActualProvider.ApplicationName;
      }
      set
      {
        this.ActualProvider.ApplicationName = value;
      }
    }

    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    {
      return this.ActualProvider.ChangePassword(username, oldPassword, newPassword);
    }

    public override bool ChangePasswordQuestionAndAnswer(
      string username,
      string password,
      string newPasswordQuestion,
      string newPasswordAnswer)
    {
      return this.ActualProvider.ChangePasswordQuestionAndAnswer(username, password, newPasswordAnswer, newPasswordAnswer);
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
      IEventPublisher service1 = DependencyResolver.Current.GetService<IEventPublisher>();
      NewUserEvent payload = new NewUserEvent()
      {
        Username = username,
        EmailAddress = email,
        IPAddress = HttpContext.Current.Request.UserHostAddress
      };
      service1.Publish<NewUserEvent>(payload);
      if (payload.Bot)
      {
        status = MembershipCreateStatus.UserRejected;
        return (MembershipUser) null;
      }
      MembershipUser user = this.ActualProvider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
      if (status == MembershipCreateStatus.Success)
      {
        try
        {
          IConfiguration service2 = DependencyResolver.Current.GetService<IConfiguration>();
          ForumUser forumUser = new ForumUser(user.ProviderUserKey.ToString(), user.UserName, user.Email, HttpContext.Current.Request.UserHostAddress);
          forumUser.Timezone = service2.DefaultTimezone;
          forumUser.Culture = service2.DefaultCulture;
          this.UserRepository.Create(forumUser);
          this.Context.SaveChanges();
          IRepository<GroupMember> memberRepository = this.GroupMemberRepository;
          foreach (int newUserGroup in (IEnumerable<int>) DependencyResolver.Current.GetService<IConfiguration>().NewUserGroups)
          {
            if (newUserGroup > 0)
            {
              Group group = this.GroupRepository.Read(newUserGroup);
              memberRepository.Create(new GroupMember(group, forumUser));
            }
          }
          this.Context.SaveChanges();
        }
        catch
        {
        }
      }
      return user;
    }

    public override bool DeleteUser(string username, bool deleteAllRelatedData)
    {
      return this.ActualProvider.DeleteUser(username, deleteAllRelatedData);
    }

    public override bool EnablePasswordReset
    {
      get
      {
        return this.ActualProvider.EnablePasswordReset;
      }
    }

    public override bool EnablePasswordRetrieval
    {
      get
      {
        return this.ActualProvider.EnablePasswordRetrieval;
      }
    }

    public override MembershipUserCollection FindUsersByEmail(
      string emailToMatch,
      int pageIndex,
      int pageSize,
      out int totalRecords)
    {
      return this.ActualProvider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
    }

    public override MembershipUserCollection FindUsersByName(
      string usernameToMatch,
      int pageIndex,
      int pageSize,
      out int totalRecords)
    {
      return this.ActualProvider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
    }

    public override MembershipUserCollection GetAllUsers(
      int pageIndex,
      int pageSize,
      out int totalRecords)
    {
      return this.ActualProvider.GetAllUsers(pageIndex, pageSize, out totalRecords);
    }

    public override int GetNumberOfUsersOnline()
    {
      return this.ActualProvider.GetNumberOfUsersOnline();
    }

    public override string GetPassword(string username, string answer)
    {
      return this.ActualProvider.GetPassword(username, answer);
    }

    private void UpdateVisit(MembershipUser user)
    {
      this.UserRepository.ReadOne((ISpecification<ForumUser>) new ForumUserSpecifications.SpecificProviderUserKey(user.ProviderUserKey.ToString())).LastVisit = DateTime.UtcNow;
      this.Context.SaveChanges();
    }

    public override MembershipUser GetUser(string username, bool userIsOnline)
    {
      MembershipUser user = this.ActualProvider.GetUser(username, userIsOnline);
      if (user != null && userIsOnline)
        this.UpdateVisit(user);
      return user;
    }

    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
    {
      MembershipUser user = this.ActualProvider.GetUser(providerUserKey, userIsOnline);
      if (user != null && userIsOnline)
        this.UpdateVisit(user);
      return user;
    }

    public override string GetUserNameByEmail(string email)
    {
      return this.ActualProvider.GetUserNameByEmail(email);
    }

    public override int MaxInvalidPasswordAttempts
    {
      get
      {
        return this.ActualProvider.MaxInvalidPasswordAttempts;
      }
    }

    public override int MinRequiredNonAlphanumericCharacters
    {
      get
      {
        return this.ActualProvider.MinRequiredNonAlphanumericCharacters;
      }
    }

    public override int MinRequiredPasswordLength
    {
      get
      {
        return this.ActualProvider.MinRequiredPasswordLength;
      }
    }

    public override int PasswordAttemptWindow
    {
      get
      {
        return this.ActualProvider.PasswordAttemptWindow;
      }
    }

    public override MembershipPasswordFormat PasswordFormat
    {
      get
      {
        return this.ActualProvider.PasswordFormat;
      }
    }

    public override string PasswordStrengthRegularExpression
    {
      get
      {
        return this.ActualProvider.PasswordStrengthRegularExpression;
      }
    }

    public override bool RequiresQuestionAndAnswer
    {
      get
      {
        return this.ActualProvider.RequiresQuestionAndAnswer;
      }
    }

    public override bool RequiresUniqueEmail
    {
      get
      {
        return this.ActualProvider.RequiresUniqueEmail;
      }
    }

    public override string ResetPassword(string username, string answer)
    {
      return this.ActualProvider.ResetPassword(username, answer);
    }

    public override bool UnlockUser(string userName)
    {
      return this.ActualProvider.UnlockUser(userName);
    }

    public override void UpdateUser(MembershipUser user)
    {
      ForumUser forumUser = this.UserRepository.ReadOne((ISpecification<ForumUser>) new ForumUserSpecifications.SpecificProviderUserKey(user.ProviderUserKey.ToString()));
      this.GetUser(user.ProviderUserKey, false);
      this.ActualProvider.UpdateUser(user);
      if (!forumUser.ExternalAccount)
        forumUser.Name = user.UserName;
      forumUser.EmailAddress = user.Email;
      this.Context.SaveChanges();
    }

    public override bool ValidateUser(string username, string password)
    {
      if (this.ActualProvider.ValidateUser(username, password))
      {
        MembershipUser user = this.GetUser(username, false);
        if (user == null)
          throw new ArgumentException("No membership user found");
        ForumUser forumUser = this.UserRepository.ReadOne((ISpecification<ForumUser>) new ForumUserSpecifications.SpecificProviderUserKey(user.ProviderUserKey.ToString()));
        if (forumUser == null)
          throw new ArgumentException("No local user found");
        if (forumUser.Active && !forumUser.Deleted)
        {
          forumUser.LastIP = HttpContext.Current.Request.UserHostAddress;
          forumUser.LastVisit = DateTime.UtcNow;
          this.Context.SaveChanges();
          return true;
        }
      }
      return false;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Security.GenericMembershipProvider
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Hosting;
using System.Web.Security;

namespace MVCBootstrap.Web.Security
{
  public abstract class GenericMembershipProvider : MembershipProvider
  {
    private bool enablePasswordReset;
    private bool enablePasswordRetrieval;
    private int maxInvalidPasswordAttempts;
    private int minRequiredNonAlphanumericCharacters;
    private int minRequiredPasswordLength;
    private int passwordAttemptWindow;
    private MembershipPasswordFormat passwordFormat;
    private string passwordStrengthRegularExpression;
    private bool requiresQuestionAndAnswer;
    private bool requiresUniqueEmail;

    public override string ApplicationName { get; set; }

    public override bool EnablePasswordReset
    {
      get
      {
        return this.enablePasswordReset;
      }
    }

    public override bool EnablePasswordRetrieval
    {
      get
      {
        return this.enablePasswordRetrieval;
      }
    }

    public override int MaxInvalidPasswordAttempts
    {
      get
      {
        return this.maxInvalidPasswordAttempts;
      }
    }

    public override int MinRequiredNonAlphanumericCharacters
    {
      get
      {
        return this.minRequiredNonAlphanumericCharacters;
      }
    }

    public override int MinRequiredPasswordLength
    {
      get
      {
        return this.minRequiredPasswordLength;
      }
    }

    public override int PasswordAttemptWindow
    {
      get
      {
        return this.passwordAttemptWindow;
      }
    }

    public override MembershipPasswordFormat PasswordFormat
    {
      get
      {
        return this.passwordFormat;
      }
    }

    public override string PasswordStrengthRegularExpression
    {
      get
      {
        return this.passwordStrengthRegularExpression;
      }
    }

    public override bool RequiresQuestionAndAnswer
    {
      get
      {
        return this.requiresQuestionAndAnswer;
      }
    }

    public override bool RequiresUniqueEmail
    {
      get
      {
        return this.requiresUniqueEmail;
      }
    }

    protected string EncodePassword(
      string password,
      MembershipPasswordFormat membershipPasswordFormat,
      string salt)
    {
      if (string.IsNullOrWhiteSpace(password))
        return (string) null;
      if (membershipPasswordFormat == MembershipPasswordFormat.Clear)
        return password;
      byte[] bytes = Encoding.Unicode.GetBytes(password);
      byte[] numArray1 = Convert.FromBase64String(salt);
      byte[] numArray2 = new byte[numArray1.Length + bytes.Length];
      Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 0, numArray1.Length);
      Buffer.BlockCopy((Array) bytes, 0, (Array) numArray2, numArray1.Length, bytes.Length);
      if (membershipPasswordFormat == MembershipPasswordFormat.Hashed)
        return Convert.ToBase64String(HashAlgorithm.Create("SHA1").ComputeHash(numArray2));
      return Convert.ToBase64String(this.EncryptPassword(numArray2));
    }

    protected bool VerifyPassword(IUser user, string password)
    {
      return user.Password == this.EncodePassword(password, this.PasswordFormat, user.Salt);
    }

    protected bool VerifyPasswordAnswer(IUser user, string passwordAnswer)
    {
      return user.PasswordAnswer == this.EncodePassword(passwordAnswer, this.PasswordFormat, user.Salt);
    }

    protected MembershipUser ToMembershipUser(IUser user)
    {
      if (user == null)
        return (MembershipUser) null;
      string email = user.Email ?? (string) null;
      string passwordQuestion = user.PasswordQuestion ?? (string) null;
      string comment = user.Comment ?? (string) null;
      return new MembershipUser(this.Name, user.Username, (object) user.Id, email, passwordQuestion, comment, user.IsApproved, user.IsLockedOut, user.CreationDate, user.LastLoginDate, user.LastActivityDate, user.LastPasswordChangedDate, user.LastLockoutDate);
    }

    protected string DecodePassword(
      string password,
      MembershipPasswordFormat membershipPasswordFormat)
    {
      switch (this.passwordFormat)
      {
        case MembershipPasswordFormat.Clear:
          return password;
        case MembershipPasswordFormat.Hashed:
          throw new ProviderException("Hashed passwords cannot be decoded.");
        default:
          byte[] bytes = this.DecryptPassword(Convert.FromBase64String(password));
          if (bytes != null)
            return Encoding.Unicode.GetString(bytes, 16, bytes.Length - 16);
          return (string) null;
      }
    }

    protected abstract IEnumerable<IUser> GetUsersByName(string name);

    protected abstract IEnumerable<IUser> GetUsersByEmail(string email);

    protected abstract int GetOnlineUsers();

    protected abstract IEnumerable<IUser> GetAllUsersInternal();

    protected abstract IUser GetUserByEmail(string email);

    protected abstract IUser GetUserByName(string username);

    protected abstract IUser GetUserByProviderId(Guid id);

    protected abstract bool UpdateUser(IUser user);

    protected abstract void CreateUser(
      Guid id,
      DateTime creationDate,
      string emailAddress,
      bool isApproved,
      string password,
      string passwordAnswer,
      string passwordQuestion,
      string salt,
      string username);

    protected abstract bool DeleteUserInternal(string username, bool deleteAllRelatedData);

    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    {
      IUser userByName = this.GetUserByName(username);
      if (!this.VerifyPassword(userByName, oldPassword))
        return false;
      ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, newPassword, false);
      this.OnValidatingPassword(e);
      if (e.Cancel)
        throw new MembershipPasswordException(e.FailureInformation.Message);
      userByName.LastPasswordChangedDate = DateTime.UtcNow;
      userByName.Password = this.EncodePassword(newPassword, this.PasswordFormat, userByName.Salt);
      return this.UpdateUser(userByName);
    }

    public override bool ChangePasswordQuestionAndAnswer(
      string username,
      string password,
      string newPasswordQuestion,
      string newPasswordAnswer)
    {
      IUser userByName = this.GetUserByName(username);
      if (!this.VerifyPassword(userByName, password))
        return false;
      userByName.PasswordQuestion = newPasswordQuestion;
      userByName.PasswordAnswer = this.EncodePassword(newPasswordAnswer, this.PasswordFormat, userByName.Salt);
      return this.UpdateUser(userByName);
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
      if (providerUserKey != null)
      {
        if (!(providerUserKey is Guid))
        {
          status = MembershipCreateStatus.InvalidProviderUserKey;
          return (MembershipUser) null;
        }
      }
      else
        providerUserKey = (object) Guid.NewGuid();
      ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, password, true);
      this.OnValidatingPassword(e);
      if (e.Cancel)
      {
        status = MembershipCreateStatus.InvalidPassword;
        return (MembershipUser) null;
      }
      if (this.RequiresQuestionAndAnswer && !string.IsNullOrWhiteSpace(passwordQuestion))
      {
        status = MembershipCreateStatus.InvalidQuestion;
        return (MembershipUser) null;
      }
      if (this.RequiresQuestionAndAnswer && !string.IsNullOrWhiteSpace(passwordAnswer))
      {
        status = MembershipCreateStatus.InvalidAnswer;
        return (MembershipUser) null;
      }
      if (this.GetUser(username, false) != null)
      {
        status = MembershipCreateStatus.DuplicateUserName;
        return (MembershipUser) null;
      }
      if (this.GetUser(providerUserKey, false) != null)
      {
        status = MembershipCreateStatus.DuplicateProviderUserKey;
        return (MembershipUser) null;
      }
      if (this.RequiresUniqueEmail && !string.IsNullOrWhiteSpace(this.GetUserNameByEmail(email)))
      {
        status = MembershipCreateStatus.DuplicateEmail;
        return (MembershipUser) null;
      }
      byte[] numArray = new byte[16];
      new RNGCryptoServiceProvider().GetBytes(numArray);
      string base64String = Convert.ToBase64String(numArray);
      DateTime utcNow = DateTime.UtcNow;
      this.CreateUser((Guid) providerUserKey, utcNow, email, isApproved, password, passwordAnswer, passwordQuestion, base64String, username);
      status = MembershipCreateStatus.Success;
      return this.GetUser(username, false);
    }

    public override bool DeleteUser(string username, bool deleteAllRelatedData)
    {
      return this.DeleteUserInternal(username, deleteAllRelatedData);
    }

    public override MembershipUserCollection FindUsersByEmail(
      string emailToMatch,
      int pageIndex,
      int pageSize,
      out int totalRecords)
    {
      MembershipUserCollection membershipUserCollection = new MembershipUserCollection();
      IEnumerable<IUser> usersByEmail = this.GetUsersByEmail(emailToMatch);
      totalRecords = usersByEmail.Count<IUser>();
      foreach (IUser user in usersByEmail.Skip<IUser>(pageIndex * pageSize).Take<IUser>(pageSize))
        membershipUserCollection.Add(this.ToMembershipUser(user));
      return membershipUserCollection;
    }

    public override MembershipUserCollection FindUsersByName(
      string usernameToMatch,
      int pageIndex,
      int pageSize,
      out int totalRecords)
    {
      MembershipUserCollection membershipUserCollection = new MembershipUserCollection();
      IEnumerable<IUser> usersByName = this.GetUsersByName(usernameToMatch);
      totalRecords = usersByName.Count<IUser>();
      foreach (IUser user in usersByName.Skip<IUser>(pageIndex * pageSize).Take<IUser>(pageSize))
        membershipUserCollection.Add(this.ToMembershipUser(user));
      return membershipUserCollection;
    }

    public override MembershipUserCollection GetAllUsers(
      int pageIndex,
      int pageSize,
      out int totalRecords)
    {
      MembershipUserCollection membershipUserCollection = new MembershipUserCollection();
      IEnumerable<IUser> allUsersInternal = this.GetAllUsersInternal();
      totalRecords = allUsersInternal.Count<IUser>();
      foreach (IUser user in allUsersInternal.Skip<IUser>(pageIndex * pageSize).Take<IUser>(pageSize))
        membershipUserCollection.Add(this.ToMembershipUser(user));
      return membershipUserCollection;
    }

    public override int GetNumberOfUsersOnline()
    {
      return this.GetOnlineUsers();
    }

    public override string GetPassword(string username, string answer)
    {
      if (!this.EnablePasswordRetrieval)
        throw new NotSupportedException("This Membership Provider has not been configured to support password retrieval.");
      IUser userByName = this.GetUserByName(username);
      if (this.RequiresQuestionAndAnswer && !this.VerifyPasswordAnswer(userByName, answer))
        throw new MembershipPasswordException("The password-answer supplied is invalid.");
      return this.DecodePassword(userByName.PasswordQuestion, this.PasswordFormat);
    }

    public override MembershipUser GetUser(string username, bool userIsOnline)
    {
      IUser userByName = this.GetUserByName(username);
      if (userByName == null)
        return (MembershipUser) null;
      if (userIsOnline)
      {
        userByName.LastActivityDate = DateTime.UtcNow;
        this.UpdateUser(userByName);
      }
      return this.ToMembershipUser(userByName);
    }

    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
    {
      IUser userByProviderId = this.GetUserByProviderId((Guid) providerUserKey);
      if (userByProviderId == null)
        return (MembershipUser) null;
      if (userIsOnline)
      {
        userByProviderId.LastActivityDate = DateTime.UtcNow;
        this.UpdateUser(userByProviderId);
      }
      return this.ToMembershipUser(userByProviderId);
    }

    public override string GetUserNameByEmail(string email)
    {
      return this.GetUserByEmail(email)?.Username;
    }

    public override void Initialize(string name, NameValueCollection config)
    {
      this.ApplicationName = config["applicationName"] ?? HostingEnvironment.ApplicationVirtualPath ?? string.Empty;
      this.enablePasswordReset = bool.Parse(config["enablePasswordReset"] ?? "true");
      this.enablePasswordRetrieval = bool.Parse(config["enablePasswordRetrieval"] ?? "false");
      this.maxInvalidPasswordAttempts = int.Parse(config["maxInvalidPasswordAttempts"] ?? "5");
      this.minRequiredNonAlphanumericCharacters = int.Parse(config["minRequiredNonAlphanumericCharacters"] ?? "1");
      this.minRequiredPasswordLength = int.Parse(config["minRequiredPasswordLength"] ?? "7");
      this.passwordAttemptWindow = int.Parse(config["passwordAttemptWindow"] ?? "10");
      this.passwordFormat = (MembershipPasswordFormat) Enum.Parse(typeof (MembershipPasswordFormat), config["passwordFormat"] ?? "Hashed");
      this.passwordStrengthRegularExpression = config["passwordStrengthRegularExpression"] ?? string.Empty;
      this.requiresQuestionAndAnswer = bool.Parse(config["requiresQuestionAndAnswer"] ?? "false");
      this.requiresUniqueEmail = bool.Parse(config["requiresUniqueEmail"] ?? "true");
      if (this.PasswordFormat == MembershipPasswordFormat.Hashed && this.EnablePasswordRetrieval)
        throw new ProviderException("Configured settings are invalid: Hashed passwords cannot be retrieved. Either set the password format to different type, or set enablePasswordRetrieval to false.");
      base.Initialize(name, config);
    }

    public override string ResetPassword(string username, string answer)
    {
      if (!this.EnablePasswordReset)
        throw new NotSupportedException("This provider is not configured to allow password resets. To enable password reset, set enablePasswordReset to \"true\" in the configuration file.");
      IUser userByName = this.GetUserByName(username);
      if (this.RequiresQuestionAndAnswer && !this.VerifyPasswordAnswer(userByName, answer))
        throw new MembershipPasswordException("The password-answer supplied is invalid.");
      string password = Membership.GeneratePassword(this.MinRequiredPasswordLength, this.MinRequiredNonAlphanumericCharacters);
      userByName.LastPasswordChangedDate = DateTime.UtcNow;
      userByName.Password = this.EncodePassword(password, this.PasswordFormat, userByName.Salt);
      this.UpdateUser(userByName);
      return password;
    }

    public override bool UnlockUser(string username)
    {
      IUser userByName = this.GetUserByName(username);
      userByName.FailedPasswordAttemptCount = 0;
      userByName.FailedPasswordAttemptWindowStart = new DateTime(1970, 1, 1);
      userByName.FailedPasswordAnswerAttemptCount = 0;
      userByName.FailedPasswordAnswerAttemptWindowStart = new DateTime(1970, 1, 1);
      userByName.IsLockedOut = false;
      userByName.LastLockoutDate = new DateTime(1970, 1, 1);
      this.UpdateUser(userByName);
      return true;
    }

    public override void UpdateUser(MembershipUser user)
    {
      IUser userByProviderId = this.GetUserByProviderId((Guid) user.ProviderUserKey);
      if (userByProviderId == null)
        throw new ProviderException("The user was not found.");
      userByProviderId.Email = user.Email;
      userByProviderId.IsApproved = user.IsApproved;
      userByProviderId.LastActivityDate = user.LastActivityDate.ToUniversalTime();
      userByProviderId.LastLoginDate = user.LastLockoutDate.ToUniversalTime();
      this.UpdateUser(userByProviderId);
    }

    public override bool ValidateUser(string username, string password)
    {
      IUser userByName = this.GetUserByName(username);
      if (userByName == null || !userByName.IsApproved || userByName.IsLockedOut)
        return false;
      if (this.VerifyPassword(userByName, password))
      {
        userByName.LastLoginDate = userByName.LastActivityDate = DateTime.UtcNow;
        this.UpdateUser(userByName);
        return true;
      }
      int num = userByName.FailedPasswordAttemptCount + 1;
      if (DateTime.UtcNow > userByName.FailedPasswordAttemptWindowStart.AddMinutes((double) this.PasswordAttemptWindow))
      {
        userByName.FailedPasswordAttemptCount = 1;
        userByName.FailedPasswordAttemptWindowStart = DateTime.UtcNow;
      }
      else if (num >= this.MaxInvalidPasswordAttempts)
      {
        userByName.IsLockedOut = true;
        userByName.LastLockoutDate = DateTime.UtcNow;
      }
      else
        userByName.FailedPasswordAttemptCount = num;
      this.UpdateUser(userByName);
      return false;
    }
  }
}

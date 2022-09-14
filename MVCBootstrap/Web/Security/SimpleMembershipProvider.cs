using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

using ApplicationBoilerplate.DataProvider;

namespace MVCBootstrap.Web.Security {

	public class SimpleMembershipProvider : MembershipProvider {

		private static String GetHashedString(String value) {
			MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
			Byte[] bs = Encoding.UTF8.GetBytes(value);
			bs = x.ComputeHash(bs);
			StringBuilder s = new StringBuilder();
			foreach (byte b in bs) {
				s.Append(b.ToString("x2").ToLower());
			}
			return s.ToString();
		}

		private MembershipUser GetMembershipUser(User user) {
			return new MembershipUser(this.Name, user.Username, user.Id, user.EmailAddress, null, null, true, false, user.Created, user.LastVisit, user.LastVisit, DateTime.MinValue, DateTime.MinValue);
		}

		protected IContext Context {
			get {
				return DependencyResolver.Current.GetService<IContext>();
			}
		}

		protected IRepository<User> UserRepository {
			get {
				return this.Context.GetRepository<User>();
			}
		}

		/// <summary>
		/// Initializes the provider.
		/// </summary>
		/// <param name="name">The friendly name of the provider.</param>
		/// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
		/// <exception cref="System.ArgumentNullException">The name of the provider is null.</exception>
		/// <exception cref="System.ArgumentException">The name of the provider has a length of zero.</exception>
		/// <exception cref="System.InvalidOperationException">An attempt is made to call System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection) on a provider after the provider has already been initialized.</exception>
		public override void Initialize(String name, NameValueCollection config) {
			if (config == null) {
				throw new ArgumentNullException("config");
			}
			base.Initialize(name, config);

			Int32 intValue;
			String value = config["passwordAttemptWindow"];
			this.passwordAttemptWindow = (!String.IsNullOrWhiteSpace(value) && Int32.TryParse(value, out intValue)) ? intValue : 10;

			value = config["userIsOnlineTimeWindow"];
			this.userIsOnlineTimeWindow = (!String.IsNullOrWhiteSpace(value) && Int32.TryParse(value, out intValue)) ? intValue : 10;

			value = config["maxInvalidPasswordAttempts"];
			this.maxInvalidPasswordAttempts = (!String.IsNullOrWhiteSpace(value) && Int32.TryParse(value, out intValue)) ? intValue : 5;

			value = config["passwordAttemptLockoutDuration"];
			this.PasswordAttemptLockoutDuration = (!String.IsNullOrWhiteSpace(value) && Int32.TryParse(value, out intValue)) ? intValue : 30;

			value = config["minRequiredPasswordLength"];
			this.minPasswordLength = (!String.IsNullOrWhiteSpace(value) && Int32.TryParse(value, out intValue)) ? intValue : 7;
			if (this.minPasswordLength < 1 || this.minPasswordLength > 128) {
				throw new ArgumentOutOfRangeException("minRequiredPasswordLength");
			}
		}

		/// <summary>
		/// The friendly name of the provider.
		/// </summary>
		public override String ApplicationName { get; set; }

		public override Boolean ChangePassword(String username, String oldPassword, String newPassword) {
			if (String.IsNullOrWhiteSpace(username)) {
				throw new ArgumentNullException("username");
			}
			if (String.IsNullOrWhiteSpace(oldPassword)) {
				throw new ArgumentNullException("oldPassword");
			}
			if (String.IsNullOrWhiteSpace(newPassword)) {
				throw new ArgumentNullException("newPassword");
			}
			if (newPassword.Length < this.MinRequiredPasswordLength) {
				throw new ArgumentException("password");
			}
			User user = this.UserRepository.ReadOne(new UserSpecifications.SpecificUsernameAndPassword(username, GetHashedString(oldPassword)));
			if (user != null) {
				user.Password = GetHashedString(newPassword);
				this.Context.SaveChanges();
				return true;
			}

			return false;
		}

		public override Boolean ChangePasswordQuestionAndAnswer(String username, String password, String newPasswordQuestion, String newPasswordAnswer) {
			throw new NotImplementedException();
		}

		public override MembershipUser CreateUser(String username, String password, String email, String passwordQuestion, String passwordAnswer, Boolean isApproved, Object providerUserKey, out MembershipCreateStatus status) {
			if (String.IsNullOrWhiteSpace(username)) {
				status = MembershipCreateStatus.InvalidUserName;
				return null;
			}
			if (String.IsNullOrWhiteSpace(password) || password.Length < this.MinRequiredPasswordLength) {
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			if (String.IsNullOrWhiteSpace(email)) {
				status = MembershipCreateStatus.InvalidEmail;
				return null;
			}

			status = MembershipCreateStatus.Success;
			User user = this.UserRepository.ReadOne(new UserSpecifications.SpecificEmailAddress(email));
			if (user != null) {
				status = MembershipCreateStatus.DuplicateEmail;
				return null;
			}

			user = this.UserRepository.ReadOne(new UserSpecifications.SpecificUsername(username));
			if (user != null) {
				status = MembershipCreateStatus.DuplicateUserName;
				return null;
			}

			try {
				MailAddress address = new MailAddress(email);
				if (address.Address != email) {
					status = MembershipCreateStatus.InvalidEmail;
					return null;
				}
			}
			catch {
				status = MembershipCreateStatus.InvalidEmail;
				return null;
			}

			user = new User { Username = username, Password = GetHashedString(password), EmailAddress = email, Created = DateTime.UtcNow, LastVisit = DateTime.UtcNow, Locked = false, LastPasswordFailure = DateTime.UtcNow, PasswordFailures = 0, LastLockout = DateTime.UtcNow.AddDays(-1), Approved = isApproved };
			this.UserRepository.Create(user);
			this.Context.SaveChanges();

			return GetMembershipUser(user);
		}

		public override Boolean DeleteUser(String username, Boolean deleteAllRelatedData) {
			// TODO:
			throw new NotImplementedException();
		}

		/// <summary>
		/// The duration, in minutes, that a lockout due to a bad password is considered still in effect.
		/// </summary>
		public Int32 PasswordAttemptLockoutDuration { private set; get; }

		public override Boolean EnablePasswordReset { get { return true; } }

		public override Boolean EnablePasswordRetrieval { get { return false; } }

		public override MembershipUserCollection FindUsersByEmail(String emailToMatch, Int32 pageIndex, Int32 pageSize, out Int32 totalRecords) {
			throw new NotImplementedException();
		}

		public override MembershipUserCollection FindUsersByName(String usernameToMatch, Int32 pageIndex, Int32 pageSize, out Int32 totalRecords) {
			throw new NotImplementedException();
		}

		public override MembershipUserCollection GetAllUsers(Int32 pageIndex, Int32 pageSize, out Int32 totalRecords) {
			IEnumerable<User> users = this.UserRepository.ReadAll();
			totalRecords = users.Count();

			users = users.Skip((pageIndex - 1) * pageSize).Take(pageSize);

			MembershipUserCollection output = new MembershipUserCollection();
			foreach (User u in users) {
				// Unique name!
				if (output[u.Username] == null) {
					try {
						output.Add(this.GetMembershipUser(u));
					}
					catch { }
				}
			}

			return output;
		}

		public override Int32 GetNumberOfUsersOnline() {
			TimeSpan timeSpan = TimeSpan.FromMinutes(Membership.UserIsOnlineTimeWindow);
			return this.Context
					.GetRepository<User>()
					.ReadMany(u => u.LastVisit > DateTime.UtcNow.Subtract(timeSpan))
					.Count();
		}

		public override String GetPassword(String username, String answer) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Return the user with the given username, if any exist.
		/// </summary>
		/// <param name="username">Name of the user</param>
		/// <param name="userIsOnline">Is the user online, if he is, the LastVisit datestamp will be updated.</param>
		/// <returns></returns>
		public override MembershipUser GetUser(String username, Boolean userIsOnline) {
			User user = this.UserRepository.ReadOne(new UserSpecifications.SpecificUsername(username));
			if (user != null) {
				if (userIsOnline) {
					user.LastVisit = DateTime.UtcNow;
					this.Context.SaveChanges();
				}
				return GetMembershipUser(user);
			}

			return null;
		}

		public override MembershipUser GetUser(Object providerUserKey, Boolean userIsOnline) {
			User user = null;
			if (providerUserKey is Int32) {
				user = this.UserRepository.Read((Int32)providerUserKey);
			}
			else if (providerUserKey is Guid) {
				user = this.UserRepository.Read((Guid)providerUserKey);
			}
			if (user != null) {
				if (userIsOnline) {
					user.LastVisit = DateTime.UtcNow;
					this.Context.SaveChanges();
				}
				return GetMembershipUser(user);
			}

			return null;
		}

		public override String GetUserNameByEmail(String email) {
			User user = this.UserRepository.ReadOne(new UserSpecifications.SpecificEmailAddress(email));
			return (user == null ? "" : user.Username);
		}

		private Int32 passwordAttemptWindow;
		/// <summary>
		/// The time window, in minutes, during which failed password attemps are tracked.
		/// </summary>
		public override Int32 PasswordAttemptWindow {
			get {
				return this.passwordAttemptWindow;
			}
		}

		private Int32 userIsOnlineTimeWindow;
		/// <summary>
		/// 
		/// </summary>
		public Int32 UserIsOnlineTimeWindow {
			get {
				return this.userIsOnlineTimeWindow;
			}
		}

		private Int32 maxInvalidPasswordAttempts;
		/// <summary>
		/// The number of failed password attempts allowed before a user's account is locked.
		/// </summary>
		public override Int32 MaxInvalidPasswordAttempts {
			get {
				return this.maxInvalidPasswordAttempts;
			}
		}

		private Int32 minPasswordLength;
		/// <summary>
		/// The minimum number of characters required in a password. The value can be from 1 to 128.
		/// </summary>
		public override Int32 MinRequiredPasswordLength {
			get {
				return this.minPasswordLength;
			}
		}

		public override Int32 MinRequiredNonAlphanumericCharacters {
			get { throw new NotImplementedException(); }
		}

		public override MembershipPasswordFormat PasswordFormat {
			get { throw new NotImplementedException(); }
		}

		public override String PasswordStrengthRegularExpression {
			get { throw new NotImplementedException(); }
		}

		public override Boolean RequiresQuestionAndAnswer {
			get {
				return false;
			}
		}

		public override Boolean RequiresUniqueEmail {
			get {
				return true;
			}
		}

		public override String ResetPassword(String username, String answer) {
			String newPassword = String.Empty;
			User user = this.UserRepository.ReadOne(new UserSpecifications.SpecificUsername(username));
			if (user != null) {
				newPassword = PasswordGenerator.Generate(8, PasswordStrength.AlphaNumeric);
				user.Password = GetHashedString(newPassword);
				this.Context.SaveChanges();
			}

			return newPassword;
		}

		public override Boolean UnlockUser(String username) {
			User user = this.UserRepository.ReadOne(new UserSpecifications.SpecificUsername(username));
			if (user != null) {
				user.Locked = false;
				this.Context.SaveChanges();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Updates a member's properties.
		/// </summary>
		/// <param name="user">Membership user to update.</param>
		/// <exception cref="System.ArgumentNullException">The membership user is null.</exception>
		/// <exception cref="System.FormatException">The e-mail address is invalid.</exception>
		public override void UpdateUser(MembershipUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			User u = null;
			if (user.ProviderUserKey is Int32) {
				u = this.UserRepository.Read((Int32)user.ProviderUserKey);
			}
			else if (user.ProviderUserKey is Guid) {
				u = this.UserRepository.Read((Guid)user.ProviderUserKey);
			}
			if (u != null) {
				MailAddress address = new MailAddress(user.Email);
				if (address.Address != user.Email) {
					throw new FormatException("email");
				}
				u.EmailAddress = user.Email;
				u.Username = user.UserName;
				this.Context.SaveChanges();
			}
		}

		/// <summary>
		/// Validates a user's credentials.
		/// </summary>
		/// <param name="username">Username to validate.</param>
		/// <param name="password">User password to validate.</param>
		/// <returns>Returns true if the credentials are valid and false if they are not.</returns>
		public override Boolean ValidateUser(String username, String password) {
			// Try getting a user with the given name!
			User user = this.UserRepository.ReadOne(new UserSpecifications.SpecificUsername(username));
			// Any user with that username? And not locked? Or locked, but locked has timed out!
			if (user != null && (!user.Locked || (user.Locked && DateTime.UtcNow.Subtract(user.LastLockout).TotalMinutes > this.PasswordAttemptLockoutDuration)) && user.Approved) {
				// Do the password match?
				if (user.Password == GetHashedString(password)) {
					// Oh yeah, let's update last visit etc.
					user.LastVisit = DateTime.UtcNow;
					user.PasswordFailures = 0;
					user.Locked = false;
					// Save changes!
					this.Context.SaveChanges();
					// Success!!
					return true;
				}
				else {
					// Wrong password, when did the user fail last time? Within the "window"?
					if (DateTime.UtcNow.Subtract(user.LastPasswordFailure).TotalMinutes > this.PasswordAttemptWindow) {
						// Let's start over, first failure!
						user.PasswordFailures = 1;
					}
					else {
						// Another failure!
						user.PasswordFailures++;
					}
					// How many times did the user fail?
					if (user.PasswordFailures > this.MaxInvalidPasswordAttempts) {
						// Too many times, let's lock the user!
						user.Locked = true;
						user.LastLockout = DateTime.UtcNow;
					}
					// Update last attempt!
					user.LastPasswordFailure = DateTime.UtcNow;
					// Update it!
					this.Context.SaveChanges();
				}
			}

			// Sorry, somehow it didn't succeed!
			return false;
		}
	}
}
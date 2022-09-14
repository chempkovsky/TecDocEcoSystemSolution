using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Hosting;
using System.Web.Security;

namespace MVCBootstrap.Web.Security {

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Code heavily inspired by the MongoDB.Web project on GitHub
	/// Answer/question functionality is not implemented yet!
	/// </remarks>
	public abstract class GenericMembershipProvider : MembershipProvider {
		private Boolean enablePasswordReset;
		private Boolean enablePasswordRetrieval;
		private Int32 maxInvalidPasswordAttempts;
		private Int32 minRequiredNonAlphanumericCharacters;
		private Int32 minRequiredPasswordLength;
		private Int32 passwordAttemptWindow;
		private MembershipPasswordFormat passwordFormat;
		private String passwordStrengthRegularExpression;
		private Boolean requiresQuestionAndAnswer;
		private Boolean requiresUniqueEmail;

		#region Properties
		public override String ApplicationName { get; set; }

		public override Boolean EnablePasswordReset {
			get {
				return this.enablePasswordReset;
			}
		}

		public override Boolean EnablePasswordRetrieval {
			get {
				return this.enablePasswordRetrieval;
			}
		}

		public override Int32 MaxInvalidPasswordAttempts {
			get {
				return this.maxInvalidPasswordAttempts;
			}
		}

		public override Int32 MinRequiredNonAlphanumericCharacters {
			get {
				return this.minRequiredNonAlphanumericCharacters;
			}
		}

		public override Int32 MinRequiredPasswordLength {
			get {
				return this.minRequiredPasswordLength;
			}
		}

		public override Int32 PasswordAttemptWindow {
			get {
				return this.passwordAttemptWindow;
			}
		}

		public override MembershipPasswordFormat PasswordFormat {
			get {
				return this.passwordFormat;
			}
		}

		public override String PasswordStrengthRegularExpression {
			get {
				return this.passwordStrengthRegularExpression;
			}
		}

		public override Boolean RequiresQuestionAndAnswer {
			get {
				return this.requiresQuestionAndAnswer;
			}
		}

		public override Boolean RequiresUniqueEmail {
			get {
				return this.requiresUniqueEmail;
			}
		}
		#endregion

		#region Private Methods
		protected String EncodePassword(String password, MembershipPasswordFormat membershipPasswordFormat, String salt) {
			if (String.IsNullOrWhiteSpace(password)) {
				return null;
			}

			if (membershipPasswordFormat == MembershipPasswordFormat.Clear) {
				return password;
			}

			Byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
			Byte[] saltBytes = Convert.FromBase64String(salt);
			Byte[] allBytes = new Byte[saltBytes.Length + passwordBytes.Length];

			Buffer.BlockCopy(saltBytes, 0, allBytes, 0, saltBytes.Length);
			Buffer.BlockCopy(passwordBytes, 0, allBytes, saltBytes.Length, passwordBytes.Length);

			if (membershipPasswordFormat == MembershipPasswordFormat.Hashed) {
				return Convert.ToBase64String(HashAlgorithm.Create("SHA1").ComputeHash(allBytes));
			}

			return Convert.ToBase64String(this.EncryptPassword(allBytes));
		}

		protected Boolean VerifyPassword(IUser user, String password) {
			return user.Password == this.EncodePassword(password, this.PasswordFormat, user.Salt);
		}

		protected Boolean VerifyPasswordAnswer(IUser user, String passwordAnswer) {
			return user.PasswordAnswer == this.EncodePassword(passwordAnswer, this.PasswordFormat, user.Salt);
		}

		protected MembershipUser ToMembershipUser(IUser user) {
			if (user == null) {
				return null;
			}

			String email = user.Email ?? null;
			String passwordQuestion = user.PasswordQuestion ?? null;
			String comment = user.Comment ?? null;

			return new MembershipUser(this.Name, user.Username, user.Id, email, passwordQuestion, comment, user.IsApproved, user.IsLockedOut, user.CreationDate, user.LastLoginDate, user.LastActivityDate, user.LastPasswordChangedDate, user.LastLockoutDate);
		}

		protected String DecodePassword(String password, MembershipPasswordFormat membershipPasswordFormat) {
			switch (passwordFormat) {
				case MembershipPasswordFormat.Clear:
					return password;

				case MembershipPasswordFormat.Hashed:
					throw new ProviderException("Hashed passwords cannot be decoded.");

				default:
					Byte[] passwordBytes = Convert.FromBase64String(password);
					Byte[] decryptedBytes = this.DecryptPassword(passwordBytes);
					return decryptedBytes == null ? null : Encoding.Unicode.GetString(decryptedBytes, 16, decryptedBytes.Length - 16);
			}
		}

		protected abstract IEnumerable<IUser> GetUsersByName(String name);

		protected abstract IEnumerable<IUser> GetUsersByEmail(String email);

		protected abstract Int32 GetOnlineUsers();

		protected abstract IEnumerable<IUser> GetAllUsersInternal();

		protected abstract IUser GetUserByEmail(String email);

		protected abstract IUser GetUserByName(String username);

		protected abstract IUser GetUserByProviderId(Guid id);

		protected abstract Boolean UpdateUser(IUser user);

		protected abstract void CreateUser(Guid id, DateTime creationDate, String emailAddress, Boolean isApproved, String password, String passwordAnswer, String passwordQuestion, String salt, String username);

		protected abstract Boolean DeleteUserInternal(String username, Boolean deleteAllRelatedData);
		#endregion

		#region Public Methods
		public override Boolean ChangePassword(String username, String oldPassword, String newPassword) {
			IUser user = this.GetUserByName(username);

			if (!this.VerifyPassword(user, oldPassword)) {
				return false;
			}

			//if (newPassword.Length < base.MinRequiredPasswordLength) {
			//    throw new ArgumentException();
			//}
			//int num3 = 0;
			//for (int i = 0; i < newPassword.Length; i++) {
			//    if (!char.IsLetterOrDigit(newPassword, i)) {
			//        num3++;
			//    }
			//}
			//if (num3 < base.MinRequiredNonAlphanumericCharacters) {
			//    throw new ArgumentException();
			//}
			//if (base.PasswordStrengthRegularExpression.Length > 0 && !Regex.IsMatch(newPassword, base.PasswordStrengthRegularExpression)) {
			//    throw new ArgumentException();
			//}

			ValidatePasswordEventArgs validatePasswordEventArgs = new ValidatePasswordEventArgs(username, newPassword, false);
			OnValidatingPassword(validatePasswordEventArgs);

			if (validatePasswordEventArgs.Cancel) {
				throw new MembershipPasswordException(validatePasswordEventArgs.FailureInformation.Message);
			}

			user.LastPasswordChangedDate = DateTime.UtcNow;
			user.Password = this.EncodePassword(newPassword, this.PasswordFormat, user.Salt);
			return this.UpdateUser(user);
		}

		public override Boolean ChangePasswordQuestionAndAnswer(String username, String password, String newPasswordQuestion, String newPasswordAnswer) {
			IUser user = this.GetUserByName(username);

			if (!this.VerifyPassword(user, password)) {
				return false;
			}

			user.PasswordQuestion = newPasswordQuestion;
			user.PasswordAnswer = this.EncodePassword(newPasswordAnswer, this.PasswordFormat, user.Salt);
			return this.UpdateUser(user);
		}

		public override MembershipUser CreateUser(String username, String password, String email, String passwordQuestion, String passwordAnswer, Boolean isApproved, Object providerUserKey, out MembershipCreateStatus status) {
			if (providerUserKey != null) {
				if (!(providerUserKey is Guid)) {
					status = MembershipCreateStatus.InvalidProviderUserKey;
					return null;
				}
			}
			else {
				providerUserKey = Guid.NewGuid();
			}

			ValidatePasswordEventArgs validatePasswordEventArgs = new ValidatePasswordEventArgs(username, password, true);
			OnValidatingPassword(validatePasswordEventArgs);

			if (validatePasswordEventArgs.Cancel) {
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}

			if (this.RequiresQuestionAndAnswer && !String.IsNullOrWhiteSpace(passwordQuestion)) {
				status = MembershipCreateStatus.InvalidQuestion;
				return null;
			}

			if (this.RequiresQuestionAndAnswer && !String.IsNullOrWhiteSpace(passwordAnswer)) {
				status = MembershipCreateStatus.InvalidAnswer;
				return null;
			}

			if (this.GetUser(username, false) != null) {
				status = MembershipCreateStatus.DuplicateUserName;
				return null;
			}

			if (this.GetUser(providerUserKey, false) != null) {
				status = MembershipCreateStatus.DuplicateProviderUserKey;
				return null;
			}

			if (this.RequiresUniqueEmail && !String.IsNullOrWhiteSpace(this.GetUserNameByEmail(email))) {
				status = MembershipCreateStatus.DuplicateEmail;
				return null;
			}

			Byte[] buffer = new Byte[16];
			(new RNGCryptoServiceProvider()).GetBytes(buffer);
			String salt = Convert.ToBase64String(buffer);

			DateTime creationDate = DateTime.UtcNow;

			this.CreateUser(
					(Guid)providerUserKey,
					creationDate,
					email,
					isApproved,
					password,
					passwordAnswer,
					passwordQuestion,
					salt,
					username
				);

			status = MembershipCreateStatus.Success;
			return this.GetUser(username, false);
		}

		public override Boolean DeleteUser(String username, Boolean deleteAllRelatedData) {
			return this.DeleteUserInternal(username, deleteAllRelatedData);
		}

		public override MembershipUserCollection FindUsersByEmail(String emailToMatch, Int32 pageIndex, Int32 pageSize, out Int32 totalRecords) {
			MembershipUserCollection membershipUsers = new MembershipUserCollection();

			IEnumerable<IUser> users = this.GetUsersByEmail(emailToMatch);
			totalRecords = users.Count();

			foreach (IUser user in users.Skip(pageIndex * pageSize).Take(pageSize)) {
				membershipUsers.Add(this.ToMembershipUser(user));
			}

			return membershipUsers;
		}

		public override MembershipUserCollection FindUsersByName(String usernameToMatch, Int32 pageIndex, Int32 pageSize, out Int32 totalRecords) {
			MembershipUserCollection membershipUsers = new MembershipUserCollection();

			IEnumerable<IUser> users = this.GetUsersByName(usernameToMatch);
			totalRecords = users.Count();

			foreach (IUser user in users.Skip(pageIndex * pageSize).Take(pageSize)) {
				membershipUsers.Add(ToMembershipUser(user));
			}

			return membershipUsers;
		}

		public override MembershipUserCollection GetAllUsers(Int32 pageIndex, Int32 pageSize, out Int32 totalRecords) {
			MembershipUserCollection membershipUsers = new MembershipUserCollection();

			IEnumerable<IUser> users = this.GetAllUsersInternal();
			totalRecords = users.Count();

			foreach (IUser user in users.Skip(pageIndex * pageSize).Take(pageSize)) {
				membershipUsers.Add(this.ToMembershipUser(user));
			}

			return membershipUsers;
		}

		public override Int32 GetNumberOfUsersOnline() {
			return this.GetOnlineUsers();
		}

		public override String GetPassword(String username, String answer) {
			if (!this.EnablePasswordRetrieval) {
				throw new NotSupportedException("This Membership Provider has not been configured to support password retrieval.");
			}

			IUser user = this.GetUserByName(username);

			if (this.RequiresQuestionAndAnswer && !this.VerifyPasswordAnswer(user, answer)) {
				throw new MembershipPasswordException("The password-answer supplied is invalid.");
			}

			return this.DecodePassword(user.PasswordQuestion, this.PasswordFormat);
		}

		public override MembershipUser GetUser(String username, Boolean userIsOnline) {
			IUser user = this.GetUserByName(username);
			if (user == null) {
				return null;
			}

			if (userIsOnline == true) {
				user.LastActivityDate = DateTime.UtcNow;
				this.UpdateUser(user);
			}

			return this.ToMembershipUser(user);
		}

		public override MembershipUser GetUser(Object providerUserKey, Boolean userIsOnline) {
			IUser user = this.GetUserByProviderId((Guid)providerUserKey);

			if (user == null) {
				return null;
			}

			if (userIsOnline == true) {
				user.LastActivityDate = DateTime.UtcNow;
				this.UpdateUser(user);
			}

			return this.ToMembershipUser(user);
		}

		public override String GetUserNameByEmail(String email) {
			IUser user = this.GetUserByEmail(email);
			return user != null ? user.Username : null;
		}

		public override void Initialize(String name, NameValueCollection config) {
			this.ApplicationName = config["applicationName"] ?? HostingEnvironment.ApplicationVirtualPath ?? String.Empty;
			this.enablePasswordReset = Boolean.Parse(config["enablePasswordReset"] ?? "true");
			this.enablePasswordRetrieval = Boolean.Parse(config["enablePasswordRetrieval"] ?? "false");
			this.maxInvalidPasswordAttempts = Int32.Parse(config["maxInvalidPasswordAttempts"] ?? "5");
			this.minRequiredNonAlphanumericCharacters = Int32.Parse(config["minRequiredNonAlphanumericCharacters"] ?? "1");
			this.minRequiredPasswordLength = Int32.Parse(config["minRequiredPasswordLength"] ?? "7");
			this.passwordAttemptWindow = Int32.Parse(config["passwordAttemptWindow"] ?? "10");
			this.passwordFormat = (MembershipPasswordFormat)Enum.Parse(typeof(MembershipPasswordFormat), config["passwordFormat"] ?? "Hashed");
			this.passwordStrengthRegularExpression = config["passwordStrengthRegularExpression"] ?? String.Empty;
			this.requiresQuestionAndAnswer = Boolean.Parse(config["requiresQuestionAndAnswer"] ?? "false");
			this.requiresUniqueEmail = Boolean.Parse(config["requiresUniqueEmail"] ?? "true");

			if (this.PasswordFormat == MembershipPasswordFormat.Hashed && this.EnablePasswordRetrieval) {
				throw new ProviderException("Configured settings are invalid: Hashed passwords cannot be retrieved. Either set the password format to different type, or set enablePasswordRetrieval to false.");
			}

			base.Initialize(name, config);
		}

		public override String ResetPassword(String username, String answer) {
			if (!this.EnablePasswordReset) {
				throw new NotSupportedException("This provider is not configured to allow password resets. To enable password reset, set enablePasswordReset to \"true\" in the configuration file.");
			}

			IUser user = this.GetUserByName(username);

			if (this.RequiresQuestionAndAnswer && !this.VerifyPasswordAnswer(user, answer)) {
				throw new MembershipPasswordException("The password-answer supplied is invalid.");
			}

			String password = Membership.GeneratePassword(this.MinRequiredPasswordLength, this.MinRequiredNonAlphanumericCharacters);
			user.LastPasswordChangedDate = DateTime.UtcNow;
			user.Password = this.EncodePassword(password, this.PasswordFormat, user.Salt);
			this.UpdateUser(user);

			return password;
		}

		public override Boolean UnlockUser(String username) {
			IUser user = this.GetUserByName(username);

			user.FailedPasswordAttemptCount = 0;
			user.FailedPasswordAttemptWindowStart = new DateTime(1970, 1, 1);
			user.FailedPasswordAnswerAttemptCount = 0;
			user.FailedPasswordAnswerAttemptWindowStart = new DateTime(1970, 1, 1);
			user.IsLockedOut = false;
			user.LastLockoutDate = new DateTime(1970, 1, 1);
			this.UpdateUser(user);

			return true;
		}

		public override void UpdateUser(MembershipUser user) {
			IUser u = this.GetUserByProviderId((Guid)user.ProviderUserKey);

			if (u == null) {
				throw new ProviderException("The user was not found.");
			}

			u.Email = user.Email;
			u.IsApproved = user.IsApproved;
			u.LastActivityDate = user.LastActivityDate.ToUniversalTime();
			u.LastLoginDate = user.LastLockoutDate.ToUniversalTime();

			this.UpdateUser(u);
		}

		public override Boolean ValidateUser(String username, String password) {
			IUser user = this.GetUserByName(username);

			if (user == null || !user.IsApproved || user.IsLockedOut) {
				return false;
			}

			if (this.VerifyPassword(user, password)) {
				user.LastLoginDate = user.LastActivityDate = DateTime.UtcNow;
				this.UpdateUser(user);
				return true;
			}

			Int32 attemptCount = user.FailedPasswordAttemptCount + 1;
			// Outside the fail windows??
			if (DateTime.UtcNow > user.FailedPasswordAttemptWindowStart.AddMinutes(this.PasswordAttemptWindow)) {
				// Let's set the attemt count to 1 and restart the window!!
				user.FailedPasswordAttemptCount = 1;
				user.FailedPasswordAttemptWindowStart = DateTime.UtcNow;
			}
			else {
				// Too many attempts??
				if (attemptCount >= this.MaxInvalidPasswordAttempts) {
					// Let's lock-out the user!!
					user.IsLockedOut = true;
					user.LastLockoutDate = DateTime.UtcNow;
				}
				else {
					// No, let's add to the count!
					user.FailedPasswordAttemptCount = attemptCount;
				}
			}
			this.UpdateUser(user);

			return false;
		}
		#endregion
	}
}
using System;

namespace MVCBootstrap.Web.Security {

	public interface IUser {
		Guid Id { get; set; }
		String Password { get; set; }
		String PasswordAnswer { get; set; }
		String Salt { get; set; }
		String Email { get; set; }
		String Username { get; set; }
		String PasswordQuestion { get; set; }
		String Comment { get; set; }
		DateTime LastLockoutDate { get; set; }
		DateTime LastPasswordChangedDate { get; set; }
		DateTime LastActivityDate { get; set; }
		DateTime LastLoginDate { get; set; }
		DateTime CreationDate { get; set; }
		Boolean IsLockedOut { get; set; }
		Boolean IsAnonymous { get; set; }
		Boolean IsApproved { get; set; }
		Int32 FailedPasswordAnswerAttemptCount { get; set; }
		DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
		Int32 FailedPasswordAttemptCount { get; set; }
		DateTime FailedPasswordAttemptWindowStart { get; set; }
	}
}
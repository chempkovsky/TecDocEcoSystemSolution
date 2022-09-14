using System;
using System.Web.Security;

using MVCBootstrap.Web.Mvc.Interfaces;
using SimpleLocalisation;

namespace MVCBootstrap.Web.Mvc.Services {

	public class AccountMembershipService : IMembershipService {
		private readonly MembershipProvider membership;
		private readonly RoleProvider role;

		public AccountMembershipService()
			: this(null, null) {
		}

		public AccountMembershipService(MembershipProvider membership, RoleProvider role) {
			this.membership = membership ?? System.Web.Security.Membership.Provider;
			this.role = role ?? Roles.Provider;
		}

		public String GetUsername(String email) {
			return membership.GetUserNameByEmail(email);
		}

		public MembershipUser GetUser(String name) {
			return membership.GetUser(name, false);
		}

		public Boolean ValidateUser(String userName, String password) {
			if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
			if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

			return membership.ValidateUser(userName, password);
		}

		public Boolean UnlockUser(String username) {
			return membership.UnlockUser(username);
		}

		public MembershipCreateStatus CreateUser(String userName, String password, String email) {
			return this.CreateUser(userName, password, email, true);
		}

		public MembershipCreateStatus CreateUser(String userName, String password, String email, Boolean isApproved) {
			if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
			if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
			if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

			MembershipCreateStatus status;
			membership.CreateUser(userName, password, email, null, null, isApproved, null, out status);
			//if (status == MembershipCreateStatus.Success) {
			//    role.AddUsersToRoles(new String[] { email }, new String[] { "Member" });
			//}
			return status;
		}

		public Boolean ChangePassword(String userName, String oldPassword, String newPassword) {
			if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
			if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
			if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

			// The underlying ChangePassword() will throw an exception rather
			// than return false in certain failure scenarios.
			try {
				MembershipUser currentUser = membership.GetUser(userName, true /* userIsOnline */);
				return currentUser.ChangePassword(oldPassword, newPassword);
			}
			catch (ArgumentException) {
				return false;
			}
			catch (MembershipPasswordException) {
				return false;
			}
		}

		public String ErrorCodeToString(MembershipCreateStatus createStatus, TextManager texts) {
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for a full list of status codes.
			switch (createStatus) {
				case MembershipCreateStatus.DuplicateUserName:
				case MembershipCreateStatus.DuplicateEmail:
				case MembershipCreateStatus.InvalidPassword:
				case MembershipCreateStatus.InvalidEmail:
				case MembershipCreateStatus.InvalidAnswer:
				case MembershipCreateStatus.InvalidQuestion:
				case MembershipCreateStatus.InvalidUserName:
				case MembershipCreateStatus.ProviderError:
				case MembershipCreateStatus.UserRejected:
					return texts.Get(createStatus.ToString(), ns: "mvcForum.WebUI.MembershipErrors");
				default:
					return texts.Get("Default", ns: "mvcForum.WebUI.MembershipErrors");
			}
		}

		public void Update(MembershipUser user) {
			this.membership.UpdateUser(user);
		}
	}
}
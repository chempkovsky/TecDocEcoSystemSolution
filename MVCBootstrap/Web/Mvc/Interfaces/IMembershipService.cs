using System;
using System.Web.Security;
using SimpleLocalisation;

namespace MVCBootstrap.Web.Mvc.Interfaces {

	public interface IMembershipService {
		Boolean ValidateUser(String userName, String password);
		MembershipCreateStatus CreateUser(String userName, String password, String email);
		MembershipCreateStatus CreateUser(String userName, String password, String email, Boolean isApproved);
		Boolean ChangePassword(String userName, String oldPassword, String newPassword);
		String GetUsername(String email);
		MembershipUser GetUser(String name);
		Boolean UnlockUser(String username);
		String ErrorCodeToString(MembershipCreateStatus createStatus, TextManager texts);
		void Update(MembershipUser user);
	}
}
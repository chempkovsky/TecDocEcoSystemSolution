using System;
using System.Web.Security;

using MVCBootstrap.Web.Mvc.Interfaces;

namespace MVCBootstrap.Web.Mvc.Services {

	public class FormsAuthenticationService : IFormsAuthenticationService {

		/// <summary>
		/// Method for signing in the given user.
		/// </summary>
		/// <param name="username">Username of the user</param>
		/// <param name="createPersistentCookie">A boolean indicating whether or not the the user is remember for next visit</param>
		public void SignIn(String username, Boolean createPersistentCookie) {
			if (String.IsNullOrEmpty(username)) {
				throw new ArgumentException("Value cannot be null or empty.", "userName");
			}

			FormsAuthentication.SetAuthCookie(username, createPersistentCookie);
		}

		/// <summary>
		/// Sign out the current authenticated user.
		/// </summary>
		public void SignOut() {
			FormsAuthentication.SignOut();
		}
	}
}
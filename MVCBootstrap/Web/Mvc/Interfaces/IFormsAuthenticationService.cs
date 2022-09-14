using System;

namespace MVCBootstrap.Web.Mvc.Interfaces {

	public interface IFormsAuthenticationService {
		void SignIn(String userName, Boolean createPersistentCookie);
		void SignOut();
	}
}
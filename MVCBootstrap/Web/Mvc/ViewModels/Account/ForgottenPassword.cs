using System;
using System.ComponentModel;

namespace MVCBootstrap.Web.Mvc.ViewModels {

	public class ForgottenPassword {
		[DisplayName("E-mail address")]
		public String EmailAddress { get; set; }
		[DisplayName("Username")]
		public String Username { get; set; }
	}
}
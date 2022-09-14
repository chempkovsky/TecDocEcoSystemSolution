using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVCBootstrap.Web.Mvc.ViewModels {

	public class LogOn {
		[DisplayName("E-mail address")]
		[Required]
		[DataType(DataType.EmailAddress)]
		public String EmailAddress { get; set; }
		[DisplayName("Password")]
		[Required]
		[DataType(DataType.Password)]
		public String Password { get; set; }
		[DisplayName("Remember me")]
		public Boolean RememberMe { get; set; }
	}
}
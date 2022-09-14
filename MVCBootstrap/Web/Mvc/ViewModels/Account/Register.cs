using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MVCBootstrap.Web.Mvc.Attributes;

namespace MVCBootstrap.Web.Mvc.ViewModels {

	public class Register {
		[LocalizedDisplay(typeof(Register), "Username")]
		[Required]
		public String Username { get; set; }
		[LocalizedDisplay(typeof(Register), "EmailAddress")]
		[Required]
		[DataType(DataType.EmailAddress)]
		public String EmailAddress { get; set; }
		[LocalizedDisplay(typeof(Register), "Password")]
		[Required]
		[DataType(DataType.Password)]
		public String Password { get; set; }
		[LocalizedDisplay(typeof(Register), "RepeatPassword")]
		[Required]
		[DataType(DataType.Password)]
		[LocalizedCompare("Password", typeof(Register), "NotMatchingPasswords")]
		public String RepeatPassword { get; set; }
	}
}
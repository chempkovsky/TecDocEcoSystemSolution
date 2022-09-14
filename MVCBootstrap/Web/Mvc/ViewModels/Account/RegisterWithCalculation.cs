using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using MVCBootstrap.Web.Mvc.Attributes;

namespace MVCBootstrap.Web.Mvc.ViewModels {

	public class RegisterWithCalculation {
		[LocalizedDisplay(typeof(RegisterWithCalculation), "Username")]
		[Required]
		public String Username { get; set; }
		[LocalizedDisplay(typeof(RegisterWithCalculation), "EmailAddress")]
		[Required]
		[DataType(DataType.EmailAddress)]
		public String EmailAddress { get; set; }
		[LocalizedDisplay(typeof(RegisterWithCalculation), "Password")]
		[Required]
		[DataType(DataType.Password)]
		public String Password { get; set; }
		[LocalizedDisplay(typeof(RegisterWithCalculation), "RepeatPassword")]
		[Required]
		[DataType(DataType.Password)]
		[LocalizedCompare("Password", typeof(RegisterWithCalculation), "NotMatchingPasswords")]
		public String RepeatPassword { get; set; }
		[Required]
		[LocalizedDisplay(typeof(RegisterWithCalculation), "Result")]
		public AlgebraCaptcha Result { get; set; }
	}
}
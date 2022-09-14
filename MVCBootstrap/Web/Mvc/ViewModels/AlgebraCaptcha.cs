using System;
using System.ComponentModel.DataAnnotations;
using MVCBootstrap.Web.Mvc.Attributes;

namespace MVCBootstrap.Web.Mvc.ViewModels {

	public class AlgebraCaptcha {
		[Required]
		public Int32? Result { get; set; }
		[Required]
		[StringLength(Int32.MaxValue, MinimumLength = 1)]
		public String Prefix { get; set; }
	}
}
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MVCBootstrap.Web.Mvc.Attributes;

namespace MVCBootstrap.Web.Mvc.ViewModels {

	public class ContactUs {
		[LocalizedDisplay(typeof(ContactUs), "EmailAddress")]
		[Required]
		[DataType(DataType.EmailAddress)]
		public String EmailAddress { get; set; }
		[LocalizedDisplay(typeof(ContactUs), "Subject")]
		[Required]
		public String Subject { get; set; }
		[LocalizedDisplay(typeof(ContactUs), "Message")]
		[Required]
		public String Message { get; set; }
		[Required]
		[LocalizedDisplay(typeof(ContactUs), "Result")]
		public AlgebraCaptcha Result { get; set; }
	}
}
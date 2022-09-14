using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBootstrap {

	public class UserInRole {
		public String ApplicationName { get; set; }
		public String Username { get; set; }
		public String Role { get; set; }
	}
}
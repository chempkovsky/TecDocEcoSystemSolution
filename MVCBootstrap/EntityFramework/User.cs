using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBootstrap {

	public class User {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		[Required]
		[StringLength(256)]
		public String Username { get; set; }
		[Required]
		[StringLength(200)]
		public String EmailAddress { get; set; }
		[Required]
		[StringLength(100)]
		public String Password { get; set; }
		[Required]
		public DateTime Created { get; set; }
		[Required]
		public DateTime LastVisit { get; set; }
		[Required]
		public Boolean Locked { get; set; }
		[Required]
		public Boolean Approved { get; set; }
		[Required]
		public DateTime LastPasswordFailure { get; set; }
		[Required]
		public Int32 PasswordFailures { get; set; }
		[Required]
		public DateTime LastLockout { get; set; }

		public virtual ICollection<Role> Roles { get; set; }

		public User() {
			this.Roles = new List<Role>();
		}
	}
}
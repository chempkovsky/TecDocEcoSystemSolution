﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBootstrap {

	public class Role {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		[Required]
		[StringLength(256)]
		public String Name { get; set; }

		public virtual ICollection<User> Users { get; set; }

		public Role() {
			this.Users = new List<User>();
		}
	}
}
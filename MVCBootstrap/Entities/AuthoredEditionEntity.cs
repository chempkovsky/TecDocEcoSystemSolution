using System;
using System.ComponentModel.DataAnnotations;

namespace MVCBootstrap.Entities {

	public abstract class AuthoredEditionEntity : AuthoredEntity {

		protected AuthoredEditionEntity() : base() { }
		public AuthoredEditionEntity(String edition, User author)
			: base(author) {

			this.Edition = edition;
		}

		[Required]
		[StringLength(10)]
		public String Edition { get; set; }
	}
}
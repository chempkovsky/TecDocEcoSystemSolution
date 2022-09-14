using System;
using System.ComponentModel.DataAnnotations;

namespace MVCBootstrap.Entities {

	public abstract class AuthoredEntity {

		protected AuthoredEntity() { }
		protected AuthoredEntity(User author) {
			if (author == null) {
				throw new ArgumentNullException("author");
			}

			this.Author = this.LastEditor = author;
			this.Created = this.LastEdited = DateTime.UtcNow;
		}

		[Required]
		public virtual User Author { get; set; }
		[Required]
		public virtual User LastEditor { get; set; }

		private DateTime created;
		[Required]
		public DateTime Created {
			get {
				return this.created;
			}
			set {
				this.created = value.ToUtc();
			}
		}

		private DateTime lastEdited;
		[Required]
		public DateTime LastEdited {
			get {
				return this.lastEdited;
			}
			set {
				this.lastEdited = value.ToUtc();
			}
		}
	}
}
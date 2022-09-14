using System;
using System.Collections.Generic;

namespace MVCBootstrap.Entities {

	public abstract class EditionedAuthoredEntity<TEdition> : AuthoredEntity where TEdition : AuthoredEditionEntity {

		protected EditionedAuthoredEntity() : base() { }
		protected EditionedAuthoredEntity(User author) : base(author) { }

		public virtual ICollection<TEdition> Editions { get; set; }
	}
}
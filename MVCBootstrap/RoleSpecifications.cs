using System;
using System.Linq.Expressions;

using ApplicationBoilerplate.DataProvider;

namespace MVCBootstrap {

	public static class RoleSpecifications {

		public class SpecificNames : ISpecification<Role> {
			private readonly String[] names;

			public SpecificNames(String[] names) {
				this.names = names;
			}

			public Expression<Func<Role, Boolean>> IsSatisfied {
				get {
					return this.BuildOr<Role, String>(r => r.Name, names);
				}
			}
		}

		public class SpecificName : ISpecification<Role> {
			private readonly String name;

			public SpecificName(String name) {
				this.name = name;
			}

			public Expression<Func<Role, Boolean>> IsSatisfied {
				get {
					return r => r.Name == name;
				}
			}
		}
	}
}
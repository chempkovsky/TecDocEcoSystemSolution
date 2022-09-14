using System;
using System.Linq.Expressions;

using ApplicationBoilerplate.DataProvider;

namespace MVCBootstrap {

	public static class UserSpecifications {

		public class SpecificUsernames : ISpecification<User> {
			private readonly String[] names;

			public SpecificUsernames(String[] names) {
				this.names = names;
			}

			public Expression<Func<User, Boolean>> IsSatisfied {
				get {
					return this.BuildOr<User, String>(r => r.Username, names);
				}
			}
		}

		public class SpecificUsernameAndPassword : ISpecification<User> {
			private readonly String username;
			private readonly String password;

			public SpecificUsernameAndPassword(String username, String password) {
				this.username = username;
				this.password = password;
			}

			public Expression<Func<User, Boolean>> IsSatisfied {
				get {
					return u => u.Username == username && u.Password == password;
				}
			}
		}

		public class SpecificEmailAddress : ISpecification<User> {
			private readonly String email;

			public SpecificEmailAddress(String email) {
				this.email = email;
			}

			public Expression<Func<User, Boolean>> IsSatisfied {
				get {
					return u => u.EmailAddress == email;
				}
			}
		}

		public class SpecificUsername : ISpecification<User> {
			private readonly String username;

			public SpecificUsername(String username) {
				this.username = username;
			}

			public Expression<Func<User, Boolean>> IsSatisfied {
				get {
					return u => u.Username == username;
				}
			}
		}

		public class SpecificProviderUserKey : ISpecification<User> {
			private readonly Guid providerUserKey;

			public SpecificProviderUserKey(Guid providerUserKey) {
				this.providerUserKey = providerUserKey;
			}

			public Expression<Func<User, Boolean>> IsSatisfied {
				get {
					return u => u.Id == providerUserKey;
				}
			}
		}
	}
}
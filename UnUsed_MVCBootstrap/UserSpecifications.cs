// MVCBootstrap.UserSpecifications
using ApplicationBoilerplate.DataProvider;
using MVCBootstrap;
using System;
using System.Linq.Expressions;

namespace MVCBootstrap
{

    public static class UserSpecifications
    {
        public class SpecificUsernames : ISpecification<User>
        {
            private readonly string[] names;

            public Expression<Func<User, bool>> IsSatisfied => this.BuildOr((User r) => r.Username, names);

            public SpecificUsernames(string[] names)
            {
                this.names = names;
            }
        }

        public class SpecificUsernameAndPassword : ISpecification<User>
        {
            private readonly string username;

            private readonly string password;

            public Expression<Func<User, bool>> IsSatisfied => (User u) => u.Username == username && u.Password == password;

            public SpecificUsernameAndPassword(string username, string password)
            {
                this.username = username;
                this.password = password;
            }
        }

        public class SpecificEmailAddress : ISpecification<User>
        {
            private readonly string email;

            public Expression<Func<User, bool>> IsSatisfied => (User u) => u.EmailAddress == email;

            public SpecificEmailAddress(string email)
            {
                this.email = email;
            }
        }

        public class SpecificUsername : ISpecification<User>
        {
            private readonly string username;

            public Expression<Func<User, bool>> IsSatisfied => (User u) => u.Username == username;

            public SpecificUsername(string username)
            {
                this.username = username;
            }
        }

        public class SpecificProviderUserKey : ISpecification<User>
        {
            private readonly Guid providerUserKey;

            public Expression<Func<User, bool>> IsSatisfied => (User u) => u.Id == providerUserKey;

            public SpecificProviderUserKey(Guid providerUserKey)
            {
                this.providerUserKey = providerUserKey;
            }
        }
    }

}

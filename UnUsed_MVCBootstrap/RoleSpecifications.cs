// MVCBootstrap.RoleSpecifications
using ApplicationBoilerplate.DataProvider;
using MVCBootstrap;
using System;
using System.Linq.Expressions;

namespace MVCBootstrap
{

    public static class RoleSpecifications
    {
        public class SpecificNames : ISpecification<Role>
        {
            private readonly string[] names;

            public Expression<Func<Role, bool>> IsSatisfied => this.BuildOr((Role r) => r.Name, names);

            public SpecificNames(string[] names)
            {
                this.names = names;
            }
        }

        public class SpecificName : ISpecification<Role>
        {
            private readonly string name;

            public Expression<Func<Role, bool>> IsSatisfied => (Role r) => r.Name == name;

            public SpecificName(string name)
            {
                this.name = name;
            }
        }
    }

}

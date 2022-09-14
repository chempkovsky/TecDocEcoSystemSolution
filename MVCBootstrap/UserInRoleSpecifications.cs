//using System;
//using System.Linq.Expressions;

//using ApplicationBoilerplate.DataProvider;

//namespace MVCBootstrap {

//    public static class UserInRoleSpecifications {

//        public class SpecificUser : ISpecification<UserInRole> {
//            private readonly User user;

//            public SpecificUser(User user) {
//                this.user = user;
//            }

//            public Expression<Func<UserInRole, Boolean>> IsSatisfied {
//                get {
//                    return ur => ur.User.Id == user.Id;
//                }
//            }
//        }

//        public class SpecificRole : ISpecification<UserInRole> {
//            private readonly Role role;

//            public SpecificRole(Role role) {
//                this.role = role;
//            }

//            public Expression<Func<UserInRole, Boolean>> IsSatisfied {
//                get {
//                    return ur => ur.Role.Id == role.Id;
//                }
//            }
//        }

//        public class SpecificUserAndRole : ISpecification<UserInRole> {
//            private readonly User user;
//            private readonly Role role;

//            public SpecificUserAndRole(User user, Role role) {
//                this.user = user;
//                this.role = role;
//            }

//            public Expression<Func<UserInRole, Boolean>> IsSatisfied {
//                get {
//                    return ur => ur.User.Id == user.Id && ur.Role.Id == role.Id;
//                }
//            }
//        }
//    }
//}
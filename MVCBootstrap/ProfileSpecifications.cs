//using System;

//namespace MVCBootstrap {

//    public static class ProfileSpecifications {

//        public class SpecificUsername : ISpecification<Profile> {
//            private String username;

//            public SpecificUsername(String username) {
//                this.username = username;
//            }

//            public Expression<Func<Profile, Boolean>> IsSatisfied {
//                get {
//                    return p => p.Username == username;
//                }
//            }
//        }
//    }
//}
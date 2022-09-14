//using System;
//using System.Collections.Specialized;
//using System.Web.Hosting;
//using System.Web.Security;
//using System.Configuration.Provider;
//using ApplicationBoilerplate.DataProvider;
//using System.Web.Mvc;

//namespace MVCBootstrap.Web.Security {

//    public class GenericRoleProvider : RoleProvider {

//        private IContext Context {
//            get {
//                return DependencyResolver.Current.GetService<IContext>();
//            }
//        }

//        public override void Initialize(String name, NameValueCollection config) {
//            this.ApplicationName = config["applicationName"] ?? HostingEnvironment.ApplicationVirtualPath;
//            base.Initialize(name, config);
//        }

//        public override void AddUsersToRoles(String[] usernames, String[] roleNames) {
//            foreach (String roleName in roleNames) {
//                if (!this.RoleExists(roleName)) {
//                    throw new ProviderException(String.Format("The role '{0}' was not found.", roleName));
//                }
//            }

//            foreach (String username in usernames) {
//                var membershipUser = Membership.GetUser(username);

//                if (membershipUser == null) {
//                    throw new ProviderException(String.Format("The user '{0}' was not found.", username));
//                }

//                foreach (String roleName in roleNames) {
//                    if (this.IsUserInRole(username, roleName)) {
//                        throw new ProviderException(String.Format("The user '{0}' is already in role '{1}'.", username, roleName));
//                    }


//                    var bsonDocument = new BsonDocument
//                    {
//                        { "ApplicationName", this.ApplicationName },
//                        { "Role", roleName },
//                        { "Username", username }
//                    };

//                    this.usersInRolesMongoCollection.Insert(bsonDocument);
//                }
//            }
//        }

//        public override String ApplicationName { get; set; }

//        public override void CreateRole(String roleName) {
//            throw new NotImplementedException();
//        }

//        public override Boolean DeleteRole(String roleName, Boolean throwOnPopulatedRole) {
//            throw new NotImplementedException();
//        }

//        public override String[] FindUsersInRole(String roleName, String usernameToMatch) {
//            throw new NotImplementedException();
//        }

//        public override String[] GetAllRoles() {
//            throw new NotImplementedException();
//        }

//        public override String[] GetRolesForUser(String username) {
//            throw new NotImplementedException();
//        }

//        public override String[] GetUsersInRole(String roleName) {
//            throw new NotImplementedException();
//        }

//        public override Boolean IsUserInRole(String username, String roleName) {
//            throw new NotImplementedException();
//        }

//        public override void RemoveUsersFromRoles(String[] usernames, String[] roleNames) {
//            throw new NotImplementedException();
//        }

//        public override Boolean RoleExists(String roleName) {
//            throw new NotImplementedException();
//        }
//    }
//}
////    public class GenericRoleProvider : RoleProvider {

////        public override String ApplicationName { get; set; }

////        public override void AddUsersToRoles(String[] usernames, String[] roleNames) {
////            foreach (String roleName in roleNames) {
////                if (!this.RoleExists(roleName)) {
////                    throw new ProviderException(String.Format("The role '{0}' was not found.", roleName));
////                }
////            }

////            foreach (String username in usernames) {
////                MembershipUser membershipUser = Membership.GetUser(username);

////                if (membershipUser == null) {
////                    throw new ProviderException(String.Format("The user '{0}' was not found.", username));
////                }

////                foreach (String roleName in roleNames) {
////                    if (this.IsUserInRole(username, roleName)) {
////                        throw new ProviderException(String.Format("The user '{0}' is already in role '{1}'.", username, roleName));
////                    }




////                    var bsonDocument = new BsonDocument
////                    {
////                        { "ApplicationName", this.ApplicationName },
////                        { "Role", roleName },
////                        { "Username", username }
////                    };

////                    this.usersInRolesMongoCollection.Insert(bsonDocument);
////                }
////            }
////        }

////        public override void CreateRole(String roleName) {
////            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));

////            if (this.rolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0) {
////                throw new ProviderException(String.Format("The role '{0}' already exists.", roleName));
////            }

////            var bsonDocument = new BsonDocument
////            {
////                { "ApplicationName", this.ApplicationName },
////                { "Role", roleName }
////            };

////            this.rolesMongoCollection.Insert(bsonDocument);
////        }

////        public override Boolean DeleteRole(String roleName, Boolean throwOnPopulatedRole) {
////            if (!this.RoleExists(roleName)) {
////                throw new ProviderException(String.Format("The role '{0}' was not found.", roleName));
////            }

////            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));

////            if (throwOnPopulatedRole && this.usersInRolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0) {
////                throw new ProviderException("This role cannot be deleted because there are users present in it.");
////            }

////            this.usersInRolesMongoCollection.Remove(query);
////            this.rolesMongoCollection.Remove(query);

////            return true;
////        }

////        public override string[] FindUsersInRole(string roleName, string usernameToMatch) {
////            if (!RoleExists(roleName)) {
////                throw new ProviderException(String.Format("The role '{0}' was not found.", roleName));
////            }

////            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));
////            return this.usersInRolesMongoCollection.FindAs<BsonDocument>(query).ToList().Select(bsonDocument => bsonDocument["Username"].AsString).ToArray();
////        }

////        public override string[] GetAllRoles() {
////            var query = Query.EQ("ApplicationName", this.ApplicationName);
////            return this.rolesMongoCollection.FindAs<BsonDocument>(query).ToList().Select(bsonDocument => bsonDocument["Role"].AsString).ToArray();
////        }

////        public override string[] GetRolesForUser(string username) {
////            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Username", username));
////            return this.usersInRolesMongoCollection.FindAs<BsonDocument>(query).ToList().Select(bsonDocument => bsonDocument["Role"].AsString).ToArray();
////        }

////        public override string[] GetUsersInRole(string roleName) {
////            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));
////            return this.usersInRolesMongoCollection.FindAs<BsonDocument>(query).ToList().Select(bsonDocument => bsonDocument["Username"].AsString).ToArray();
////        }

////        protected IRepository<Role> RoleRepository {
////            get {
////                return DependencyResolver.Current.GetService<IRepository<Role>>();
////            }
////        }

////        protected IContext Context {
////            get {
////                return DependencyResolver.Current.GetService<IContext>();
////            }
////        }

////        public override void Initialize(string name, NameValueCollection config) {
////            this.ApplicationName = config["applicationName"] ?? HostingEnvironment.ApplicationVirtualPath;

////            var mongoDatabase = MongoServer.Create(config["connectionString"] ?? "mongodb://localhost").GetDatabase(config["database"] ?? "ASPNETDB");
////            this.rolesMongoCollection = mongoDatabase.GetCollection(config["collection"] ?? "Roles");
////            this.usersInRolesMongoCollection = mongoDatabase.GetCollection("UsersInRoles");

////            this.rolesMongoCollection.EnsureIndex("ApplicationName");
////            this.rolesMongoCollection.EnsureIndex("ApplicationName", "Role");
////            this.usersInRolesMongoCollection.EnsureIndex("ApplicationName", "Role");
////            this.usersInRolesMongoCollection.EnsureIndex("ApplicationName", "Username");
////            this.usersInRolesMongoCollection.EnsureIndex("ApplicationName", "Role", "Username");

////            base.Initialize(name, config);
////        }

////        public override bool IsUserInRole(string username, string roleName) {
////            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName), Query.EQ("Username", username));
////            return this.usersInRolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0;
////        }

////        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) {
////            foreach (var username in usernames) {
////                foreach (var roleName in roleNames) {
////                    var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName), Query.EQ("Username", username));
////                    this.usersInRolesMongoCollection.Remove(query);
////                }
////            }
////        }

////        public override bool RoleExists(string roleName) {
////            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Role", roleName));
////            return this.rolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0;
////        }
////    }
////}

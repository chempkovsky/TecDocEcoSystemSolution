using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

using ApplicationBoilerplate.DataProvider;

namespace MVCBootstrap.Web.Security {

	public class SimpleRoleProvider : RoleProvider {

		protected IContext Context {
			get {
				return DependencyResolver.Current.GetService<IContext>(); ;
			}
		}

		protected IRepository<User> UserRepository {
			get {
				return DependencyResolver.Current.GetService<IRepository<User>>(); ;
			}
		}

		protected IRepository<Role> RoleRepository {
			get {
				return DependencyResolver.Current.GetService<IRepository<Role>>();
			}
		}

		public override void AddUsersToRoles(String[] usernames, String[] roleNames) {
			// TODO: Unit test this!!!!!!!!!!!!
			IEnumerable<Role> roles = this.RoleRepository.ReadMany(new RoleSpecifications.SpecificNames(roleNames));
			if (roles.Any()) {
				// TODO: Unit test this!!!!!!!!!!!!
				IEnumerable<User> users = this.UserRepository.ReadMany(new UserSpecifications.SpecificUsernames(usernames));

				foreach (User user in users) {
					foreach (Role role in roles) {
						//this.UserInRoleRepository.Create(new UserInRole() { Role = role, User = user });
						user.Roles.Add(role);
					}
				}

				this.Context.SaveChanges();
			}
		}

		public override String ApplicationName { get; set; }

		public override void CreateRole(String roleName) {
			Role role = this.RoleRepository.ReadOne(new RoleSpecifications.SpecificName(roleName));
			if (role == null) {
				role = new Role();
				role.Name = roleName;
				this.RoleRepository.Create(role);

				this.Context.SaveChanges();
			}
		}

		/// <summary>
		/// Removes a role from the data source for the configured applicationName.
		/// </summary>
		/// <param name="roleName">The name of the role to delete.</param>
		/// <param name="throwOnPopulatedRole">If true, throw an exception if roleName has one or more members and do not delete roleName.</param>
		/// <returns>True if the role was successfully deleted; otherwise, false.</returns>
		public override Boolean DeleteRole(String roleName, Boolean throwOnPopulatedRole) {
			// TODO: Optimize this!!! raw SQL ???
			Role role = this.RoleRepository.ReadOne(new RoleSpecifications.SpecificName(roleName));
			if (role != null) {
				//IEnumerable<UserInRole> uirs = this.UserInRoleRepository.ReadMany(new UserInRoleSpecifications.SpecificRole(role));
				if (role.Users.Any()) {
					if (throwOnPopulatedRole) {
						// TODO: Another exception!?!?
						throw new ApplicationException("Role in use");
					}
					// TODO: deleted?
				}

				this.RoleRepository.Delete(role);

				this.Context.SaveChanges();
			}

			return true;
		}

		/// <summary>
		/// Gets an array of user names in a role where the user name contains the specified user name to match.
		/// </summary>
		/// <param name="roleName">The role to search in.</param>
		/// <param name="usernameToMatch">The user name to search for.</param>
		/// <returns>A string array containing the names of all the users where the user name matches usernameToMatch and the user is a member of the specified role.</returns>
		public override String[] FindUsersInRole(String roleName, String usernameToMatch) {
			throw new NotImplementedException("");
			// TODO:

			Role role = this.RoleRepository.ReadOne(new RoleSpecifications.SpecificName(roleName));
			if (role != null) {

			}

			return new String[] { };
		}

		public override String[] GetAllRoles() {
			return this.RoleRepository.ReadAll().Select(r => r.Name).ToArray();
		}

		public override String[] GetRolesForUser(String username) {
			User user = this.UserRepository.ReadOne(new UserSpecifications.SpecificUsername(username));
			if (user != null) {
				//IEnumerable<Role> roles = this.UserInRoleRepository.ReadMany(new UserInRoleSpecifications.SpecificUser(user)).Select(ur => ur.Role);
				return user.Roles.Select(r => r.Name).ToArray();
			}

			return new String[] { };
		}

		public override String[] GetUsersInRole(String roleName) {
			Role role = this.RoleRepository.ReadOne(new RoleSpecifications.SpecificName(roleName));
			if (role != null) {
				return role.Users.Select(r => r.Username).ToArray();
			}

			return new String[] { };
		}

		public override Boolean IsUserInRole(String username, String roleName) {
			Role role = this.RoleRepository.ReadOne(new RoleSpecifications.SpecificName(roleName));
			if (role != null) {
				User user = this.UserRepository.ReadOne(new UserSpecifications.SpecificUsername(username));
				if (user != null) {
					return role.Users.Contains(user);
				}
			}

			return false;
		}

		/// <summary>
		/// Removes the specified user names from the specified roles for the configured applicationName.
		/// </summary>
		/// <param name="usernames">A string array of user names to be removed from the specified roles.</param>
		/// <param name="roleNames">A string array of role names to remove the specified user names from.</param>
		public override void RemoveUsersFromRoles(String[] usernames, String[] roleNames) {
			// TODO: Optimize this!!! raw SQL ???
			IEnumerable<User> users = this.UserRepository.ReadMany(new UserSpecifications.SpecificUsernames(usernames));
			IEnumerable<Role> roles = this.RoleRepository.ReadMany(new RoleSpecifications.SpecificNames(roleNames));
			foreach (User user in users) {
				IEnumerable<Role> removeRoles = user.Roles.Where(r => roles.Contains(r) == true).ToList();
				foreach (Role role in removeRoles) {
					user.Roles.Remove(role);
				}
			}

			this.Context.SaveChanges();
		}

		public override Boolean RoleExists(String roleName) {
			return this.RoleRepository.ReadOne(new RoleSpecifications.SpecificName(roleName)) != null;
		}
	}
}
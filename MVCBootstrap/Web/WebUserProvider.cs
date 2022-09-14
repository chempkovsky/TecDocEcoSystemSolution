using System;
using ApplicationBoilerplate.DataProvider;
using System.Web.Security;
using System.Web;

namespace MVCBootstrap.Web {

	/// <summary>
	/// A user provider for ASP.NET websites.
	/// </summary>
	public class WebUserProvider : IUserProvider {
		private readonly IRepository<User> userRepo;

		public WebUserProvider(IRepository<User> userRepo) {
			this.userRepo = userRepo;
		}

		private User user;
		/// <summary>
		/// Get the authenticated user.
		/// </summary>
		public User ActiveUser {
			get {
				if (this.Authenticated) {
					return user;
				}
				return null;
			}
		}

		public Boolean checkedAuthenticated = false;
		public Boolean authenticated = false;
		/// <summary>
		/// Do we have an authenticated user?
		/// </summary>
		public Boolean Authenticated {
			get {
				if (!this.checkedAuthenticated) {
					MembershipUser u = System.Web.Security.Membership.GetUser(false);
					this.authenticated = (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated && u != null);
					if (this.authenticated) {
						try {
							user = this.userRepo.ReadOne(new UserSpecifications.SpecificProviderUserKey((Guid)u.ProviderUserKey));
						}
						catch { }
						this.authenticated = (user != null);
					}
					this.checkedAuthenticated = true;
				}
				return this.authenticated;
			}
		}
	}
}
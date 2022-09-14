using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Filters {

	/// <summary>
	/// A class that can be used to decorate a class or action to limit access.
	/// The class will allow local calls (from the server to the server) and will also check access against
	/// IP addresses mentioned in the app.config/web.config file.
	/// </summary>
	public class OnlyLocalCallsAttribute : FilterAttribute, IAuthorizationFilter {
		private Object typeId;
		/// <summary>
		/// List of additional allowed IP addresses.
		/// </summary>
		private List<IPAddress> addresses = new List<IPAddress>();

		public OnlyLocalCallsAttribute()
			: base() {

			this.typeId = new Object();

			String additionalIPs = ConfigurationManager.AppSettings["LocalIPAddresses"];
			if (!String.IsNullOrWhiteSpace(additionalIPs)) {
				IPAddress address;
				String[] parts = additionalIPs.Split(new Char[] { ';' }, StringSplitOptions.None);
				foreach (String part in parts) {
					if (IPAddress.TryParse(part, out address)) {
						this.addresses.Add(address);
					}
				}
			}
		}

		public override Object TypeId {
			get {
				return base.TypeId;
			}
		}

		public void OnAuthorization(AuthorizationContext filterContext) {
			if (filterContext == null) {
				throw new ArgumentNullException("filterContext");
			}
			IPAddress address;
			if (IPAddress.TryParse(filterContext.HttpContext.Request.UserHostAddress, out address)) {
				//DependencyResolver.Current.GetService<ILog>().Log(EventType.Info, "Should be local : " + address.ToString());
				// Is the address in the list of additional IPs, or is it a local (IPv4 or IPv6) address?
				if (this.addresses.Contains(address) ||
					(address.AddressFamily == AddressFamily.InterNetwork && address.ToString() == "127.0.0.1") ||
					 (address.AddressFamily == AddressFamily.InterNetworkV6 && address.ToString() == "::1")) {

					// Ok, no worries!
					return;
				}
			}

			try {
				if (filterContext.HttpContext.Trace.IsEnabled) {
					filterContext.HttpContext.Trace.Write("Not a valid IP address, or not a local IP address " + filterContext.HttpContext.Request.UserHostAddress);
				}
			}
			catch { }

			// Nope, not letting this call go through!
			filterContext.Result = new HttpUnauthorizedResult();
		}
	}
}
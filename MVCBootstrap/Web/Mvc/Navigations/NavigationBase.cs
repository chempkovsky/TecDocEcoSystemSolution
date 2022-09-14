using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using SimpleLocalisation;

namespace MVCBootstrap.Web.Mvc.Navigations {

	public abstract class NavigationBase : INavigation {

		public virtual String Area {
			get {
				return String.Empty;
			}
		}

		public virtual Boolean Initialized { get; set; }
		public abstract String Name { get; }

		protected abstract IEnumerable<NavigationItem> Items { get; }
		protected virtual String GetTitle(NavigationItem item) {
			return (this.TextManager == null ? item.Title : this.TextManager.Get(
												String.Format("{2}{0}.{1}", item.Controller, item.Action, (String.IsNullOrWhiteSpace(item.Area) ? "" : String.Format("{0}.", item.Area))),
												ns: String.Format("Navigation.{0}", this.Name)
											)
					);
		}

		private TextManager text;
		private TextManager TextManager {
			get {
				if (text == null) {
					text = DependencyResolver.Current.GetService<TextManager>();
				}
				return text;
			}
		}

		public virtual void Initialize(UrlHelper url) {
			foreach (NavigationItem navItem in this.Items) {
				this.InitializeItem(url, navItem);
			}

			this.Initialized = true;
		}

		protected virtual void InitializeItem(UrlHelper url, NavigationItem item) {
			if (!String.IsNullOrWhiteSpace(this.Area) || !String.IsNullOrWhiteSpace(item.Area)) {
				item.URL = url.Action(item.Action, item.Controller, new { area = (String.IsNullOrWhiteSpace(item.Area) ? this.Area : item.Area) });
			}
			else {
				item.URL = url.Action(item.Action, item.Controller, new { area = String.Empty });
			}

			item.Title = this.GetTitle(item);
			item.Selected = false;
			item.Visible = false;

			if (item.SubPages.Any()) {
				foreach (NavigationItem navItem in item.SubPages) {
					this.InitializeItem(url, navItem);
				}
			}
		}

		/// <summary>
		/// This method returns the list of visible/accessible navigation items.
		/// </summary>
		/// <param name="html">The HtmlHelper</param>
		/// <param name="onlyVisible"></param>
		/// <returns>A list of visible/accessible navigation items.</returns>
		public virtual IEnumerable<NavigationItem> GetNavigation(HtmlHelper html) {
			Boolean signedIn = html.ViewContext.HttpContext.User.Identity.IsAuthenticated;
			List<NavigationItem> output = new List<NavigationItem>();
			String controller = String.Empty;
			if (html.ViewContext.RouteData.Values.ContainsKey("controller")) {
				controller = (String)html.ViewContext.RouteData.Values["controller"];
			}
			String action = String.Empty;
			if (html.ViewContext.RouteData.Values.ContainsKey("action")) {
				action = (String)html.ViewContext.RouteData.Values["action"];
			}
			String area = String.Empty;
			if (html.ViewContext.RouteData.DataTokens.ContainsKey("area")) {
				area = (String)html.ViewContext.RouteData.DataTokens["area"];
			}

			this.HandlePageList(signedIn, controller, action, area, this.Items, output);

			return output;
		}

		/// <summary>
		/// This method generates a list of visible/accessible navigation items.
		/// </summary>
		/// <param name="signedIn">Is the current user known/authenticted.</param>
		/// <param name="controller">The current controller.</param>
		/// <param name="action">The current action.</param>
		/// <param name="area">The current area.</param>
		/// <param name="pages">The list of pages in the navigation</param>
		/// <param name="output">The list of visible/accessible pages.</param>
		protected virtual void HandlePageList(Boolean signedIn, String controller, String action, String area, IEnumerable<NavigationItem> pages, List<NavigationItem> output) {
			foreach (NavigationItem page in pages) {
				Boolean visible = false;
				if (page.Visibility == PageVisibility.Always || (page.Visibility == PageVisibility.Anonymous && !signedIn) || (page.Visibility == PageVisibility.Authenticated && signedIn)) {
					if (page.Visibility == PageVisibility.Authenticated && signedIn) {
						if (page.Groups != null && page.Groups.Length > 0) {
							foreach (String group in page.Groups) {
								visible = Roles.IsUserInRole(group);
								if (visible) {
									break;
								}
							}
						}
						else {
							visible = true;
						}
					}
					else {
						visible = true;
					}
				}
				if (visible) {
					NavigationItem newPage = new NavigationItem { Title = page.Title, Controller = page.Controller, Action = page.Action, Area = page.Area, URL = page.URL, Selected = this.IsActive(page, controller, action, area), AdditionalData = page.AdditionalData };
					output.Add(newPage);
					if (page.SubPages.Any()) {
						this.HandlePageList(signedIn, controller, action, area, page.SubPages, newPage.SubPages);
					}
				}
			}
		}

		/// <summary>
		/// This method determines whether or not the given navigation item is the active/selected one.
		/// </summary>
		/// <param name="item">The navigation item</param>
		/// <param name="controller">The current controller.</param>
		/// <param name="action">The current action.</param>
		/// <param name="area">The current area.</param>
		/// <returns>True if the given navigation item is the active/selected one.</returns>
		protected virtual Boolean IsActive(NavigationItem item, String controller, String action, String area) {
			return (
						((String.IsNullOrWhiteSpace(item.Area) && String.IsNullOrWhiteSpace(area))
							|| (!String.IsNullOrWhiteSpace(item.Area) && item.Area.ToLower() == area.ToLower())) &&
						item.Controller.ToLower() == controller.ToLower() &&
						item.Action.ToLower() == action.ToLower()
					);
		}
	}
}
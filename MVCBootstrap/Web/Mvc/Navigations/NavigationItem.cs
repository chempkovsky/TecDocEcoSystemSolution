using System;
using System.Collections.Generic;
using System.Linq;

namespace MVCBootstrap.Web.Mvc.Navigations {

	public class NavigationItem {

		public NavigationItem() {
			this.Groups = new String[] { };
			this.SubPages = new List<NavigationItem>();
		}

		public String Action { get; set; }
		public String Controller { get; set; }
		public String Area { get; set; }

		public List<NavigationItem> SubPages { get; set; }
		public String Title { get; set; }
		public String URL { get; set; }
		public Boolean Visible { get; set; }

		public PageVisibility Visibility { get; set; }
		public String[] Groups { get; set; }

		public Object AdditionalData { get; set; }

		private Boolean selected = false;
		public Boolean Selected {
			get {
				return (selected || (this.SubPages != null && this.SubPages.Any(p => p.Selected)));
			}
			set {
				this.selected = value;
			}
		}
	}
}
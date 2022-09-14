using System;

namespace MVCBootstrap.Web.Events {

	public class AsynConfiguration : IAsyncConfiguration {
		public Func<String> SiteUrl { get; set; }
		public Func<String> EndPoint { get; set; }
	}
}

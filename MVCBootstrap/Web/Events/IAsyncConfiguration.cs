using System;

namespace MVCBootstrap.Web.Events {

	public interface IAsyncConfiguration {
		Func<String> SiteUrl { get; set; }
		Func<String> EndPoint { get; set; }
	}
}
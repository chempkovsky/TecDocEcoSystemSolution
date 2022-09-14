using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Mvc;

using Newtonsoft.Json;

using ApplicationBoilerplate.Events;
using ApplicationBoilerplate.Logging;

using MVCBootstrap.Web.Mvc.Filters;

namespace MVCBootstrap.Web.Mvc.Controllers {

	public class AsyncController : Controller {
		private readonly ILogger log;

		public AsyncController(ILogger log)
			: base() {
			this.log = log;
		}

		[OnlyLocalCalls]
		public String Execute(String listener, String data, String type, String assembly) {
			this.log.Log(EventType.Info, String.Format("ASync task arrived, listener: {0}, type: {1}, assembly: {2}", listener, type, assembly));
			try {
				Assembly ass = Assembly.Load(assembly);
				Type t = ass.GetType(type);

				TextReader reader = new StringReader(data);

				JsonSerializer seri = new JsonSerializer();
				Object payload = seri.Deserialize(new JsonTextReader(reader), t);

				Type listenerGeneric = typeof(IEventListener<>);
				IEnumerable<IEventListener> listeners = DependencyResolver.Current.GetServices<IEventListener>();

				foreach (IEventListener l in listeners) {
					if (l.GetType().FullName == listener) {
						l.Handle(payload);
					}
				}
			}
			catch (Exception ex) {
				this.log.Log(EventType.Error, "Async handling", ex);
			}

			// TODO:
			return "";
		}
	}
}
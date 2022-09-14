using System;
using System.Web;
using System.Web.Caching;

using ApplicationBoilerplate.Events;
using System.Reflection;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Events {

	public class AsyncTask : IAsyncTask {

		public AsyncTask() { }

		public virtual void Execute(IEventListener listener, Object data, Int32 delay) {
			String key = this.GenerateKey(listener);
			if (!listener.UniqueEvent || (listener.UniqueEvent && HttpContext.Current.Cache[key] == null)) {
				HttpContext.Current.Cache.Add(key, data, null,
												DateTime.UtcNow.AddSeconds(delay),
												Cache.NoSlidingExpiration,
												CacheItemPriority.Normal,
												new CacheItemRemovedCallback(delegate(String key2, Object data2, CacheItemRemovedReason reason) {
					this.Execute(key2, data2);
				}));
			}
		}

		public virtual void Execute(String key, Object payload) {
			String listener = this.GetListener(key);
			Type listenerGeneric = typeof(IEventListener<>);
			IEnumerable<IEventListener> listeners = DependencyResolver.Current.GetServices<IEventListener>();

			foreach (IEventListener l in listeners) {
				if (l.GetType().FullName == listener) {
					l.Handle(payload);
				}
			}
		}

		protected String GenerateKey(IEventListener listener) {
			return (listener.UniqueEvent ? listener.GetType().FullName : String.Format("[{0}]{1}", Guid.NewGuid().ToString(), listener.GetType().FullName));
		}

		/// <summary>
		/// Get the full name of the listener
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected String GetListener(String key) {
			return (key.IndexOf("]") > -1 ? key.Substring(key.IndexOf(']') + 1) : key);
		}
	}
}
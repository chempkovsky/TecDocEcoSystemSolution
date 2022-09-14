using System;
using System.Collections.Generic;

using Ninject;
using Ninject.Parameters;

namespace MVCBootstrap.Ninject {

	public class DependencyResolver : System.Web.Mvc.IDependencyResolver, ApplicationBoilerplate.DependencyInjection.IDependencyResolver {
		private readonly IKernel kernel;

		public DependencyResolver(IKernel kernel) {
			this.kernel = kernel;
		}

		public Object GetService(Type serviceType) {
			return this.kernel.TryGet(serviceType, new IParameter[0]);
		}

		public IEnumerable<Object> GetServices(Type serviceType) {
			return this.kernel.GetAll(serviceType, new IParameter[0]);
		}

		public T GetService<T>() {
			return this.kernel.TryGet<T>(new IParameter[0]);
		}

		public IEnumerable<T> GetServices<T>() {
			return this.kernel.GetAll<T>(new IParameter[0]);
		}

		public void Register(Type serviceType, Func<Object> activator) {
			this.kernel.Bind(serviceType).ToMethod(_ => activator());
		}

		public void Register(Type serviceType, IEnumerable<Func<Object>> activators) {
			foreach (Func<Object> activator in activators) {
				this.kernel.Bind(serviceType).ToMethod(_ => activator());
			}
		}
	}
}
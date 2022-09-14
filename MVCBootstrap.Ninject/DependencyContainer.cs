using System;
using System.Collections.Generic;

using ApplicationBoilerplate.DependencyInjection;

using Ninject;
using Ninject.Modules;
using Ninject.Planning.Bindings;
using Ninject.Syntax;

using Ninject.Web.Common;

namespace MVCBootstrap.Ninject {

	public class DependencyContainer : IDependencyContainer {
		private readonly IKernel kernel;
		private readonly Boolean debug;
		private Dictionary<Type, Int32> bindings = new Dictionary<Type, Int32>();

		public DependencyContainer() : this(false) { }

		public DependencyContainer(Boolean debug) {
			this.debug = debug;
			this.kernel = new Bootstrapper().Kernel;
		}

		private void Store(Type type) {
			if (debug) {
				if (!bindings.ContainsKey(type)) {
					bindings.Add(type, 0);
				}
				bindings[type] = bindings[type] + 1;
			}
		}

		private void Unstore(Type type) {
			if (debug) {
				if (bindings.ContainsKey(type)) {
					bindings[type] = bindings[type] - 1;
				}
			}
		}

		#region Binding
		public void Register<TService, TImplementation>() where TImplementation : TService {
			this.kernel.Bind<TService>().To<TImplementation>();
			this.Store(typeof(TService));
		}

		public void RegisterGeneric(Type service, Type implementation) {
			this.kernel.Bind(service).To(implementation);
			this.Store(service);
		}

		public void RegisterGenericPerRequest(Type service, Type implementation) {
			this.kernel.Bind(service).To(implementation).InRequestScope();
			this.Store(service);
		}

		public void RegisterSingleton<TService>(TService instance) {
			this.kernel.Bind<TService>().ToConstant(instance).InSingletonScope();
			this.Store(typeof(TService));
		}

		public void RegisterSingleton<TService, TImplementation>() where TImplementation : TService {
			this.kernel.Bind<TService>().To<TImplementation>().InSingletonScope();
			this.Store(typeof(TService));
		}

		public void RegisterPerRequest<TService, TImplementation>() where TImplementation : TService {
			this.kernel.Bind<TService>().To<TImplementation>().InRequestScope();
			this.Store(typeof(TService));
		}

		public void RegisterPerRequest<TService, TImplementation>(IDictionary<String, Object> constructorParameters) where TImplementation : TService {
			IBindingWithOrOnSyntax<TImplementation> binding = (IBindingWithOrOnSyntax<TImplementation>)this.kernel.Bind<TService>().To<TImplementation>().InRequestScope<TImplementation>();
			foreach (KeyValuePair<String, Object> pair in constructorParameters) {
				binding = (IBindingWithOrOnSyntax<TImplementation>)binding.WithConstructorArgument(pair.Key, pair.Value);
			}
			this.Store(typeof(TService));
		}

		public void Register<TService, TImplementation>(IDictionary<String, Object> constructorParameters) where TImplementation : TService {
			IBindingWithOrOnSyntax<TService> binding = (IBindingWithOrOnSyntax<TService>)this.kernel.Bind<TService>().To<TImplementation>();
			foreach (KeyValuePair<String, Object> pair in constructorParameters) {
				binding = binding.WithConstructorArgument(pair.Key, pair.Value);
			}
			this.Store(typeof(TService));
		}
		#endregion

		public void UnRegister<TService>() {
			this.kernel.Unbind<TService>();
			this.Unstore(typeof(TService));
		}

		public void Configure() {
			// Let's register the container itself, we might need to to register/unregister services!
			this.RegisterSingleton<IDependencyContainer>(this);

			// Set our very own custom DependencyResolver.
			DependencyResolver resolver = new DependencyResolver(this.kernel);

			// Let's Bind the resolver itself.
			this.kernel.Bind<IDependencyResolver>().ToConstant(resolver);
			//this.kernel.Bind<IKernel>().ToConstant(this.kernel);
			// We'll tell asp.net mvc to use our resolver!
			System.Web.Mvc.DependencyResolver.SetResolver(resolver);
		}

		public Dictionary<Type, Int32> Bindings {
			get {
				return this.bindings;
			}
		}
	}
}
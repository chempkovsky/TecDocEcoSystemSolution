using System;

using ApplicationBoilerplate.DependencyInjection;

namespace ApplicationBoilerplate.DataProvider {

	/// <summary>
	/// Base implementation of the Context interface.
	/// </summary>
	public abstract class ContextBase : IContext {
		/// <summary>
		/// Dependency container instance.
		/// </summary>
		protected readonly IDependencyContainer container;

		/// <summary>
		/// Constructor for the ContextBase class.
		/// </summary>
		/// <param name="container">A dependency container.</param>
		protected ContextBase(IDependencyContainer container) {
			this.container = container;
		}

		/// <summary>
		/// Method for disposing the ContextBase object.
		/// </summary>
		public void Dispose() {
			this.Dispose(true);
		}

		public abstract IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
		public abstract void SaveChanges();
		public abstract void Dispose(Boolean disposing);
	}
}
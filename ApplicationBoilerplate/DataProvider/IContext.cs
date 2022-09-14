using System;

namespace ApplicationBoilerplate.DataProvider {

	/// <summary>
	/// Context interface used for getting to the data repositories and as a unit of work object.
	/// </summary>
	public interface IContext : IDisposable {
		/// <summary>
		/// Generic method for getting a data repository.
		/// </summary>
		/// <typeparam name="TEntity">Generic type for the data object.</typeparam>
		/// <returns>A repository for the given entity.</returns>
		IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
		/// <summary>
		/// Save any changes done in the life-time of the context.
		/// </summary>
		void SaveChanges();
	}
}
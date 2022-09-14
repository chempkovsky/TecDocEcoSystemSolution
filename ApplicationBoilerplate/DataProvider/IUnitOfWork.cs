using System;

namespace ApplicationBoilerplate.DataProvider {

	public interface IUnitOfWork : IDisposable {
		void Commit();
		void Rollback();
	}
}
using System;

namespace ApplicationBoilerplate.DataProvider {

	public abstract class UnitOfWorkBase : IUnitOfWork {

		public UnitOfWorkBase() { }

		public void Dispose() {
			this.Dispose(true);
		}

		public abstract void Dispose(Boolean disposing);
		public abstract void Commit();
		public abstract void Rollback();
	}
}
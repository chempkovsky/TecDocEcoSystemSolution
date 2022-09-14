using System;

namespace MVCBootstrap {

	public interface IUserProvider {
		Boolean Authenticated { get; }
		User ActiveUser { get; }
	}
}
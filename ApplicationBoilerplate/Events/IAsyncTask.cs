using System;
using System.Collections.Generic;

namespace ApplicationBoilerplate.Events {

	public interface IAsyncTask {
		/// <summary>
		/// Method for executing a asynchronously task.
		/// </summary>
		/// <param name="listener"></param>
		/// <param name="data">The data that should be pushed to the task asynchronously.</param>
		/// <param name="delay">The delay, in seconds, before the task should be executed.</param>
		void Execute(IEventListener listener, Object data, Int32 delay);
	}
}
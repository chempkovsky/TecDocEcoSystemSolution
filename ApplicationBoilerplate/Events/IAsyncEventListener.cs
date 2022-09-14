using System;

namespace ApplicationBoilerplate.Events {

	/// <summary>
	/// Interface for asynchronously event listeners.
	/// </summary>
	/// <typeparam name="TPayload">The type of the payload.</typeparam>
	public interface IAsyncEventListener<TPayload> : IEventListener<TPayload> where TPayload : class {
		/// <summary>
		/// Method for queueing an event.
		/// </summary>
		/// <typeparam name="TPayload">The type of the payload.</typeparam>
		void Queue(TPayload payload);
		/// <summary>
		/// Can this handler handle asynchronously events?
		/// </summary>
		Boolean RunAsynchronously { get; }
	}
}
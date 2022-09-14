using System;

namespace ApplicationBoilerplate.Events {

	/// <summary>
	/// Interface for event publisher classes.
	/// </summary>
	public interface IEventPublisher {
		/// <summary>
		/// Method used when publishing an event.
		/// </summary>
		/// <typeparam name="TPayload">The type of the payload.</typeparam>
		/// <param name="payload">The data detailing the event being published.</param>
		void Publish<TPayload>(TPayload payload) where TPayload : class;
	}
}
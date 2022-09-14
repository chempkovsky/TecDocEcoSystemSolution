using System;
using System.Collections.Generic;
using System.Linq;

using ApplicationBoilerplate.Logging;

namespace ApplicationBoilerplate.Events {

	/// <summary>
	/// Class used for publishing event.
	/// </summary>
	public class EventPublisher : IEventPublisher {
		/// <summary>
		/// A list of event listeners.
		/// </summary>
		private readonly IEnumerable<IEventListener> listeners;
		/// <summary>
		/// A logger service.
		/// </summary>
		private readonly ILogger logger;

		/// <summary>
		/// Constructor for the EventPublisher class.
		/// </summary>
		/// <param name="listeners">The list of event listeners.</param>
		public EventPublisher(IEnumerable<IEventListener> listeners, ILogger logger) {
			this.listeners = listeners;
			this.logger = logger;
		}

		/// <summary>
		/// Publish an event with the given payload.
		/// </summary>
		/// <typeparam name="TPayload">Generic type of the payload/data.</typeparam>
		/// <param name="payload">The data detailing the event being published.</param>
		public void Publish<TPayload>(TPayload payload) where TPayload : class {
			// Get the event listeners that listen to events with the given payload.
			IEnumerable<IEventListener<TPayload>> handlersForPayload = this.listeners.OfType<IEventListener<TPayload>>();
			// Run through the listeners, in priority order!
			foreach (var handler in handlersForPayload.OrderBy(h => h.Priority)) {
				// Is this an async handler, and does it accept events asynchronously??
				if (handler is IAsyncEventListener<TPayload> && ((IAsyncEventListener<TPayload>)handler).RunAsynchronously) {
					// Queue the event!!
					((IAsyncEventListener<TPayload>)handler).Queue(payload);
				}
				else {
					try {
						// Nope, run it now!!
						handler.Handle(payload);
					}
					catch (Exception ex) {
						this.logger.Log(EventType.Error, "Event handler failed", ex);
					}
				}
			}
		}
	}
}
using System;

namespace ApplicationBoilerplate.Events {

	/// <summary>
	/// Interface for event listener classes.
	/// </summary>
	public interface IEventListener {
		/// <summary>
		/// Method for handling the event here and now.
		/// </summary>
		/// <param name="payload"></param>
		void Handle(Object payload);
		/// <summary>
		/// Can this listener only have one event of the given type at a given moment in time?
		/// </summary>
		Boolean UniqueEvent { get; }
		/// <summary>
		/// Priority, in ascending order.
		/// </summary>
		Byte Priority { get; }
	}

	/// <summary>
	/// A generic interface for event listener classes.
	/// </summary>
	/// <typeparam name="TPayload"></typeparam>
	public interface IEventListener<in TPayload> : IEventListener where TPayload : class {
		/// <summary>
		/// The generic handle method.
		/// </summary>
		/// <param name="payload"></param>
		void Handle(TPayload payload);
	}
}
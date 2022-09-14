using System;
using System.Collections.Generic;

namespace SimpleLocalisation {

	/// <summary>
	/// Interface for a text source
	/// </summary>
	public interface ITextSource {
		/// <summary>
		/// Get a localised text entry for the given language, namespace and key.
		/// </summary>
		/// <param name="key">The key for the text.</param>
		/// <param name="ns">The namespace of the text.</param>
		/// <param name="language">The language of the text.</param>
		/// <returns>A localised text (or null).</returns>
		LocalisedTextEntry Get(String key, String ns, Language language);
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IEnumerable<LocalisedText> Get();
	}
}
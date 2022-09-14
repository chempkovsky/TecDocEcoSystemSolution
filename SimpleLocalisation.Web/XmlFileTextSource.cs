using System;
using System.Collections.Generic;

namespace SimpleLocalisation.Web {

	public class XmlFileTextSource : SimpleLocalisation.Providers.XmlFileTextSource {
		private readonly Func<String> rootProvider;

		public XmlFileTextSource(Func<String> rootProvider)
			: base(String.Empty) {

			this.rootProvider = rootProvider;
		}

		public override LocalisedTextEntry Get(String key, String ns, Language language) {
			if (!this.DataLoaded) {
				this.LoadData(this.rootProvider());
			}
			return this.Get(language, ns, key);
		}

		/// <summary>
		/// Method for getting all the texts the <see cref="ITextSource"/> holds.
		/// </summary>
		/// <returns>A collection of all texts.</returns>
		public new IEnumerable<LocalisedText> Get() {
			if (!this.DataLoaded) {
				this.LoadData(this.rootProvider());
			}
			return base.Get();
		}
	}
}
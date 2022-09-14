using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SimpleLocalisation.Providers {

	public class XmlFileTextSource : ITextSource {
		private static Dictionary<String, Dictionary<String, Dictionary<String, String>>> data = new Dictionary<String, Dictionary<String, Dictionary<String, String>>>();
		private static Object loadLock = new Object();

		public XmlFileTextSource(String root) {
			this.LoadData(root);
		}

		protected void LoadData(String root) {
			if (!String.IsNullOrWhiteSpace(root) && data.Count == 0) {
				lock (loadLock) {
					if (data.Count == 0) {
						try {
							foreach (String path in Directory.GetFiles(root, "*.xml")) {
								XDocument xml = XDocument.Load(File.OpenText(path));
								foreach (XElement ns in xml.Root.Elements("Namespace")) {
									foreach (XElement text in ns.Elements("Text")) {
										foreach (XElement translation in text.Elements("Translation")) {
											if (!data.ContainsKey(translation.Attribute("Language").Value)) {
												data.Add(translation.Attribute("Language").Value, new Dictionary<String, Dictionary<String, String>>());
											}
											if (!data[translation.Attribute("Language").Value].ContainsKey(ns.Attribute("Name").Value)) {
												data[translation.Attribute("Language").Value].Add(ns.Attribute("Name").Value, new Dictionary<String, String>());
											}
											if (!data[translation.Attribute("Language").Value][ns.Attribute("Name").Value].ContainsKey(text.Attribute("Key").Value)) {
												data[translation.Attribute("Language").Value][ns.Attribute("Name").Value].Add(text.Attribute("Key").Value, translation.Value);
											}
										}
									}
								}
							}
						}
						catch { }
					}
				}
			}
		}

		protected Boolean DataLoaded {
			get {
				return data.Count > 0;
			}
		}

		public virtual LocalisedTextEntry Get(String key, String ns, Language language) {
			return this.Get(language, ns, key);
		}

		protected LocalisedTextEntry Get(Language language, String ns, String key) {
			if (data.ContainsKey(language.Culture.Name) && data[language.Culture.Name].ContainsKey(ns) && data[language.Culture.Name][ns].ContainsKey(key)) {
				return new LocalisedTextEntry {
					Text = new LocalisedText {
						Pattern = data[language.Culture.Name][ns][key],
						Key = key,
						Namespace = ns,
						Language = language.Culture.Name
					}
				};
			}

			// TODO: Fallback languages!!

			return null;
		}

		public virtual IEnumerable<LocalisedText> Get() {
			return data.SelectMany(e => e.Value.SelectMany(f => f.Value.Select(g => new LocalisedText { Language = e.Key, Namespace = f.Key, Key = g.Key, Pattern = g.Value })));
		}
	}
}
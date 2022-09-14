using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.Mvc;
using SimpleLocalisation;

namespace MVCBootstrap.Web.Mvc.Attributes {

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class LocalizedDisplayAttribute : DisplayNameAttribute {
		private readonly String key;
		private readonly String @namespace;

		public LocalizedDisplayAttribute(Type type, String key)
			: this(type.FullName, key) {
		}

		public LocalizedDisplayAttribute(String @namespace, String key)
			: base() {
			this.key = key;
			this.@namespace = @namespace;
		}

		private String GetText(String key) {
			TextManager manager = DependencyResolver.Current.GetService<TextManager>();
			if (manager != null) {
				String output = manager.Get(key, ns: this.@namespace);
				return  String.IsNullOrWhiteSpace(output) ? "[" + key + "]" : output;
			}
			return key;
		}

		public override String DisplayName {
			get {
				return this.GetText(this.key);
			}
		}
	}
}
using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Reflection;
using SimpleLocalisation;

namespace MVCBootstrap.Web.Mvc.Attributes {

	[AttributeUsage(AttributeTargets.Property)]
	public class LocalizedCompareAttribute : ValidationAttribute /*, IClientValidatable*/ {

		// TODO: Error message for the base class???
		public LocalizedCompareAttribute(String otherProperty, Type type, String key)
			: base() {
			//			: base(LocalizedCompareAttribute.GetText("MustMatch", typeof(LocalizedCompareAttribute), otherProperty)) {

			if (String.IsNullOrWhiteSpace(otherProperty)) {
				throw new ArgumentNullException("otherProperty");
			}

			if (String.IsNullOrWhiteSpace(key)) {
				throw new ArgumentNullException("key");
			}

			if (type == null) {
				throw new ArgumentNullException("type");
			}

			this.OtherProperty = otherProperty;
			this.Key = key;
			this.Type = type;
		}

		public String OtherProperty { get; private set; }
		public String Key { get; private set; }
		public Type Type { get; private set; }

		private static String GetText(String key, Type type, String otherProperty) {
			TextManager manager = DependencyResolver.Current.GetService<TextManager>();
			if (manager != null) {
				return manager.Get(key, values: new { OtherProperty = otherProperty }, ns: type.FullName);
			}
			return key;
		}

		protected override ValidationResult IsValid(Object value, ValidationContext validationContext) {
			Object[] objArray;
			PropertyInfo property = validationContext.ObjectType.GetProperty(this.OtherProperty);
			if (property == null) {
				return new ValidationResult(LocalizedCompareAttribute.GetText("UnknownProperty", this.GetType(), this.OtherProperty));
			}
			Object obj = property.GetValue(validationContext.ObjectInstance, null);
			if (!Object.Equals(value, obj)) {
				return new ValidationResult(LocalizedCompareAttribute.GetText(this.Key, this.Type, this.OtherProperty));
			}
			return null;
		}
	}
}
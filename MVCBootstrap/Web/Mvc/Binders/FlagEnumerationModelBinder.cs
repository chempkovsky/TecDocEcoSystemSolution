// By Nathan Taylor
// http://blog.nathan-taylor.net

using System;
using System.Linq;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc {

	public class FlagEnumerationModelBinder : DefaultModelBinder {

		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
			if (bindingContext == null) {
				throw new ArgumentNullException("bindingContext");
			}

			if (bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName)) {
				var values = GetValue<string[]>(bindingContext, bindingContext.ModelName);

				if (values != null && values.Length > 1 && (bindingContext.ModelType.IsEnum && bindingContext.ModelType.IsDefined(typeof(FlagsAttribute), false))) {
					long byteValue = 0;
					foreach (var value in values.Where(v => Enum.IsDefined(bindingContext.ModelType, v))) {
						byteValue |= (int)Enum.Parse(bindingContext.ModelType, value);
					}

					return Enum.Parse(bindingContext.ModelType, byteValue.ToString());
				}
				else {
					return base.BindModel(controllerContext, bindingContext);
				}
			}

			return base.BindModel(controllerContext, bindingContext);
		}

		private static T GetValue<T>(ModelBindingContext bindingContext, string key) {
			try {
				if (bindingContext.ValueProvider.ContainsPrefix(key)) {
					ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(key);
					if (valueResult != null) {
						bindingContext.ModelState.SetModelValue(key, valueResult);
						return (T)valueResult.ConvertTo(typeof(T));
					}
				}
			}
			catch { }
			return default(T);
		}
	}
}

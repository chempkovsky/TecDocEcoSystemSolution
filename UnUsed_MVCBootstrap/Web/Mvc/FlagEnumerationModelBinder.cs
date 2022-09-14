// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.FlagEnumerationModelBinder
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc
{
  public class FlagEnumerationModelBinder : DefaultModelBinder
  {
    public override object BindModel(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext)
    {
      if (bindingContext == null)
        throw new ArgumentNullException(nameof (bindingContext));
      if (!bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName))
        return base.BindModel(controllerContext, bindingContext);
      string[] strArray = FlagEnumerationModelBinder.GetValue<string[]>(bindingContext, bindingContext.ModelName);
      if (strArray == null || strArray.Length <= 1 || (!bindingContext.ModelType.IsEnum || !bindingContext.ModelType.IsDefined(typeof (FlagsAttribute), false)))
        return base.BindModel(controllerContext, bindingContext);
      long num = 0;
      foreach (string str in ((IEnumerable<string>) strArray).Where<string>((Func<string, bool>) (v => Enum.IsDefined(bindingContext.ModelType, (object) v))))
        num |= (long) (int) Enum.Parse(bindingContext.ModelType, str);
      return Enum.Parse(bindingContext.ModelType, num.ToString());
    }

    private static T GetValue<T>(ModelBindingContext bindingContext, string key)
    {
      try
      {
        if (bindingContext.ValueProvider.ContainsPrefix(key))
        {
          ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(key);
          if (valueProviderResult != null)
          {
            bindingContext.ModelState.SetModelValue(key, valueProviderResult);
            return (T) valueProviderResult.ConvertTo(typeof (T));
          }
        }
      }
      catch
      {
      }
      return default (T);
    }
  }
}

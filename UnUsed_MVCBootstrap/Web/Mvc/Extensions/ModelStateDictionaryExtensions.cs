// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Extensions.ModelStateDictionaryExtensions
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Extensions
{
  public static class ModelStateDictionaryExtensions
  {
    public static string ErrorString(this ModelStateDictionary state)
    {
      return string.Join(", ", state.Values.SelectMany<ModelState, string>((Func<ModelState, IEnumerable<string>>) (v => v.Errors.Select<ModelError, string>((Func<ModelError, string>) (y => y.ErrorMessage)))));
    }
  }
}

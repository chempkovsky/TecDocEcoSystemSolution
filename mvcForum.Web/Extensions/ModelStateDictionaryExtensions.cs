// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Extensions.ModelStateDictionaryExtensions
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Extensions
{
  public static class ModelStateDictionaryExtensions
  {
    public static string ErrorString(this ModelStateDictionary state)
    {
      return string.Join(", ", state.Values.SelectMany<ModelState, string>((Func<ModelState, IEnumerable<string>>) (v => v.Errors.Select<ModelError, string>((Func<ModelError, string>) (y => y.ErrorMessage)))));
    }
  }
}

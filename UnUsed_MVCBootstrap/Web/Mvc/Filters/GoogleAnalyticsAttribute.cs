// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Filters.GoogleAnalyticsAttribute
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Filters
{
  [AttributeUsage(AttributeTargets.Method)]
  public class GoogleAnalyticsAttribute : ActionFilterAttribute
  {
    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
      base.OnResultExecuted(filterContext);
    }
  }
}

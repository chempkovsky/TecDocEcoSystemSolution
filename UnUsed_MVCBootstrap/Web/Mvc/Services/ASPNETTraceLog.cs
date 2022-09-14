// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Services.ASPNETTraceLog
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using ApplicationBoilerplate.Logging;
using System;
using System.Web;

namespace MVCBootstrap.Web.Mvc.Services
{
  public class ASPNETTraceLog : ILogger
  {
    public void Log(EventType type, string message)
    {
      this.Log(type, type.ToString(), message, (Exception) null);
    }

    public void Log(EventType type, string message, Exception ex)
    {
      this.Log(type, type.ToString(), message, ex);
    }

    private void Log(EventType type, string category, string message, Exception ex)
    {
      if (HttpContext.Current == null || !HttpContext.Current.Trace.IsEnabled)
        return;
      if (type == EventType.Error || type == EventType.Fatal || type == EventType.Warning)
        HttpContext.Current.Trace.Warn(category, message, ex);
      else
        HttpContext.Current.Trace.Write(category, message, ex);
    }
  }
}

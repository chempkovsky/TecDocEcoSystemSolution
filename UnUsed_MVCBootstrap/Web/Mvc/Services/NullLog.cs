// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Services.NullLog
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using ApplicationBoilerplate.Logging;
using System;

namespace MVCBootstrap.Web.Mvc.Services
{
  public class NullLog : ILogger
  {
    public void Log(EventType type, string message)
    {
    }

    public void Log(EventType type, string message, Exception ex)
    {
    }
  }
}

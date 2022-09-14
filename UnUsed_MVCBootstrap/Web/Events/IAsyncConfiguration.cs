// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Events.IAsyncConfiguration
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System;

namespace MVCBootstrap.Web.Events
{
  public interface IAsyncConfiguration
  {
    Func<string> SiteUrl { get; set; }

    Func<string> EndPoint { get; set; }
  }
}

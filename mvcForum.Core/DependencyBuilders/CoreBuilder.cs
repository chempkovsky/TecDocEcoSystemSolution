// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.DependencyBuilders.CoreBuilder
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DependencyInjection;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Configuration;
using System.Collections.Generic;

namespace mvcForum.Core.DependencyBuilders
{
  public class CoreBuilder : IDependencyBuilder
  {
    public void Configure(IDependencyContainer container)
    {
      container.RegisterPerRequest<IConfiguration, MVCForumDBConfig>();
    }

    public void ValidateRequirements(IList<ApplicationRequirement> feedback)
    {
    }
  }
}

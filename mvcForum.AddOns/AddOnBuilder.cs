// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.AddOnBuilder
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DependencyInjection;
using BoC.Web.Mvc.PrecompiledViews;
using System.Collections.Generic;

namespace mvcForum.AddOns
{
  public class AddOnBuilder : IDependencyBuilder
  {
    public void Configure(IDependencyContainer container)
    {
      ApplicationPartRegistry.Register(this.GetType().Assembly);
    }

    public void ValidateRequirements(IList<ApplicationRequirement> feedback)
    {
    }
  }
}

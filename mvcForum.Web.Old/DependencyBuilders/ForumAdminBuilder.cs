// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.DependencyBuilders.ForumAdminBuilder
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DependencyInjection;
using mvcForum.Web.Areas.ForumAdmin.Navigations;
using mvcForum.Web.Navigations;
using System.Collections.Generic;

namespace mvcForum.Web.DependencyBuilders
{
  public class ForumAdminBuilder : IDependencyBuilder
  {
    public void Configure(IDependencyContainer container)
    {
      container.Register<INavigation, AdminTopNavigation>();
    }

    public void ValidateRequirements(IList<ApplicationRequirement> feedback)
    {
    }
  }
}

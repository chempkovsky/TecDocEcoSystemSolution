// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.DependencyBuilders.StorageBuilder
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DependencyInjection;
using mvcForum.Core.Interfaces;
using mvcForum.Web.Services;
using System.Collections.Generic;
using System.Web;

namespace mvcForum.Web.DependencyBuilders
{
  public class StorageBuilder : IDependencyBuilder
  {
    public void Configure(IDependencyContainer container)
    {
      IAttachmentStorage instance = (IAttachmentStorage) new NTFSAttachmentStorage(HttpContext.Current.Server.MapPath("~"), "/attachments");
      container.RegisterSingleton<IAttachmentStorage>(instance);
    }

    public void ValidateRequirements(IList<ApplicationRequirement> feedback)
    {
    }
  }
}

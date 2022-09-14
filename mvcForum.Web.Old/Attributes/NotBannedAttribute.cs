// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Attributes.NotBannedAttribute
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Specifications;
using mvcForum.Web.Results;
using System;
using System.Web.Mvc;

namespace mvcForum.Web.Attributes
{
  public class NotBannedAttribute : FilterAttribute, IAuthorizationFilter
  {
    private object typeId;

    public NotBannedAttribute()
    {
      this.typeId = new object();
    }

    public override object TypeId
    {
      get
      {
        return base.TypeId;
      }
    }

    public void OnAuthorization(AuthorizationContext filterContext)
    {
      if (filterContext == null)
        throw new ArgumentNullException(nameof (filterContext));
      if (DependencyResolver.Current.GetService<IRepository<BannedIP>>().ReadOne((ISpecification<BannedIP>) new BannedIPSpecifications.SpecificIP(filterContext.HttpContext.Request.UserHostAddress)) == null)
        return;
      filterContext.Result = (ActionResult) new BannedResult("You're IP address has been banned. Please contact us for further details.");
    }
  }
}

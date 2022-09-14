// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Filters.OnlyLocalCallsAttribute
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Filters
{
  public class OnlyLocalCallsAttribute : FilterAttribute, IAuthorizationFilter
  {
    private List<IPAddress> addresses = new List<IPAddress>();
    private object typeId;

    public OnlyLocalCallsAttribute()
    {
      this.typeId = new object();
      string appSetting = ConfigurationManager.AppSettings["LocalIPAddresses"];
      if (string.IsNullOrWhiteSpace(appSetting))
        return;
      string str = appSetting;
      char[] separator = new char[1]{ ';' };
      foreach (string ipString in str.Split(separator, StringSplitOptions.None))
      {
        IPAddress address;
        if (IPAddress.TryParse(ipString, out address))
          this.addresses.Add(address);
      }
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
      IPAddress address;
      if (IPAddress.TryParse(filterContext.HttpContext.Request.UserHostAddress, out address))
      {
        if (this.addresses.Contains(address) || address.AddressFamily == AddressFamily.InterNetwork && address.ToString() == "127.0.0.1")
          return;
        if (address.AddressFamily == AddressFamily.InterNetworkV6)
        {
          if (address.ToString() == "::1")
            return;
        }
      }
      try
      {
        if (filterContext.HttpContext.Trace.IsEnabled)
          filterContext.HttpContext.Trace.Write("Not a valid IP address, or not a local IP address " + filterContext.HttpContext.Request.UserHostAddress);
      }
      catch
      {
      }
      filterContext.Result = (ActionResult) new HttpUnauthorizedResult();
    }
  }
}

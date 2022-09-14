// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Attributes.LocalizedDisplayAttribute
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using SimpleLocalisation;
using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Attributes
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class LocalizedDisplayAttribute : DisplayNameAttribute
  {
    private readonly string key;
    private readonly string @namespace;

    public LocalizedDisplayAttribute(Type type, string key)
      : this(type.FullName, key)
    {
    }

    public LocalizedDisplayAttribute(string @namespace, string key)
    {
      this.key = key;
      this.@namespace = @namespace;
    }

    private string GetText(string key)
    {
      TextManager service = DependencyResolver.Current.GetService<TextManager>();
      if (service == null)
        return key;
      TextManager textManager = service;
      string str1 = this.@namespace;
      string key1 = key;
      string ns = str1;
      string str2 = textManager.Get(key1, (object) null, ns);
      if (!string.IsNullOrWhiteSpace(str2))
        return str2;
      return "[" + key + "]";
    }

    public override string DisplayName
    {
      get
      {
        return this.GetText(this.key);
      }
    }
  }
}

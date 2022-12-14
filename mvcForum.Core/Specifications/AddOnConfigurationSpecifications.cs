// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.AddOnConfigurationSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class AddOnConfigurationSpecifications
  {
    public class SpecificKey : ISpecification<AddOnConfiguration>
    {
      private readonly string key;

      public SpecificKey(string key)
      {
        this.key = key;
      }

      public Expression<Func<AddOnConfiguration, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<AddOnConfiguration, bool>>) (x => x.Key == this.key);
        }
      }
    }
  }
}

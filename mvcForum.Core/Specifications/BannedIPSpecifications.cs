// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.BannedIPSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class BannedIPSpecifications
  {
    public class SpecificIP : ISpecification<BannedIP>
    {
      private readonly string ip;

      public SpecificIP(string ip)
      {
        this.ip = ip;
      }

      public Expression<Func<BannedIP, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<BannedIP, bool>>) (x => x.IP == this.ip);
        }
      }
    }
  }
}

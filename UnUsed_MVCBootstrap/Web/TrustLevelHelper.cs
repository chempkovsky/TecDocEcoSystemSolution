// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.TrustLevelHelper
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System.Security;
using System.Web;

namespace MVCBootstrap.Web
{
  public static class TrustLevelHelper
  {
    public static AspNetHostingPermissionLevel GetCurrentTrustLevel()
    {
      AspNetHostingPermissionLevel[] hostingPermissionLevelArray = new AspNetHostingPermissionLevel[5]
      {
        AspNetHostingPermissionLevel.Unrestricted,
        AspNetHostingPermissionLevel.High,
        AspNetHostingPermissionLevel.Medium,
        AspNetHostingPermissionLevel.Low,
        AspNetHostingPermissionLevel.Minimal
      };
      foreach (AspNetHostingPermissionLevel level in hostingPermissionLevelArray)
      {
        try
        {
          new AspNetHostingPermission(level).Demand();
        }
        catch (SecurityException ex)
        {
          continue;
        }
        return level;
      }
      return AspNetHostingPermissionLevel.None;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.CalculatedCaptcha
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using MVCBootstrap.Web.Mvc.ViewModels;
using System.Web;

namespace MVCBootstrap.Web.Mvc
{
  public class CalculatedCaptcha
  {
    private string prefix;

    public CalculatedCaptcha()
      : this(string.Empty)
    {
    }

    public CalculatedCaptcha(string prefix)
    {
      this.prefix = prefix;
    }

    private string Key
    {
      get
      {
        return string.Format("{0}Captcha", (object) this.prefix);
      }
    }

    public bool ValidResult(AlgebraCaptcha result)
    {
      if (!result.Result.HasValue || string.IsNullOrWhiteSpace(result.Prefix))
        return false;
      this.prefix = result.Prefix;
      return this.ValidResult(result.Result.Value);
    }

    public bool ValidResult(int result)
    {
      if (!string.IsNullOrWhiteSpace(this.Key) && HttpContext.Current != null && (HttpContext.Current.Session != null && HttpContext.Current.Session[this.Key] != null) && HttpContext.Current.Session[this.Key] is int)
        return (int) HttpContext.Current.Session[this.Key] == result;
      return false;
    }

    public void StoreResult(int result)
    {
      HttpContext.Current.Session[this.Key] = (object) result;
    }
  }
}

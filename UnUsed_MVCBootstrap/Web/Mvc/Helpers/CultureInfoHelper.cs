// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Helpers.CultureInfoHelper
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Helpers
{
  public static class CultureInfoHelper
  {
    public static IEnumerable<SelectListItem> GetCultures(
      string selectedCulture)
    {
      List<SelectListItem> selectListItemList = new List<SelectListItem>();
      foreach (CultureInfo cultureInfo in (IEnumerable<CultureInfo>) ((IEnumerable<CultureInfo>) CultureInfo.GetCultures(CultureTypes.SpecificCultures)).OrderBy<CultureInfo, string>((Func<CultureInfo, string>) (c => c.EnglishName)))
        selectListItemList.Add(new SelectListItem()
        {
          Text = cultureInfo.DisplayName,
          Value = cultureInfo.Name,
          Selected = cultureInfo.Name == selectedCulture
        });
      return (IEnumerable<SelectListItem>) selectListItemList;
    }

    public static CultureInfo GetCultureInfo(string cultureInfoId)
    {
      return new CultureInfo(cultureInfoId);
    }
  }
}

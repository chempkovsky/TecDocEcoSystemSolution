// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Results.BannedResult
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System;
using System.Web.Mvc;

namespace mvcForum.Web.Results
{
  public class BannedResult : ActionResult
  {
    private string description;

    public BannedResult(string description)
    {
      this.description = description;
    }

    public override void ExecuteResult(ControllerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      context.Controller.TempData.Add("Reason", (object) this.description);
      context.HttpContext.Response.Redirect("/Forum/NoAccess");
    }
  }
}

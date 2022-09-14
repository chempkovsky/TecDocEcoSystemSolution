// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.Controllers.HeadlineController
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Web.Controllers;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI.Controllers
{
  public class HeadlineController : BaseAPIController
  {
    public ActionResult Read(string mode)
    {
      string empty = string.Empty;
      string content;
      try
      {
        content = new WebClient()
        {
          Encoding = Encoding.UTF8
        }.DownloadString("http://mvcforum.org/forumapi/json/forum/listlatesttopics/7?count=3");
      }
      catch
      {
        content = "[]";
      }
      return (ActionResult) this.Content(content, "application/json", Encoding.UTF8);
    }
  }
}

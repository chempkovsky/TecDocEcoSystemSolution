// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.ContentParsing.RegularParser
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using mvcForum.Core.Interfaces.AddOns;
using mvcForum.Web.Interfaces;
using System;
using System.Web.Mvc;

namespace mvcForum.AddOns.ContentParsing
{
  public class RegularParser : IContentParser, IAddOn
  {
    public MvcHtmlString Parse(string content)
    {
      return MvcHtmlString.Create(content.Replace("<", "&lt;").Replace(">", "&gt;").Replace(Environment.NewLine, "<br />"));
    }

    public string Quote(string author, string content)
    {
      return string.Format("{0} wrote: {1}", (object) author, (object) content);
    }

    public string Name
    {
      get
      {
        return "Regular";
      }
    }
  }
}

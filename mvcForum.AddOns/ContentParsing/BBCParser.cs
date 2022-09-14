// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.ContentParsing.BBCParser
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using CodeKicker.BBCode;
using mvcForum.Core.Interfaces.AddOns;
using mvcForum.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace mvcForum.AddOns.ContentParsing
{
  public class BBCParser : IContentParser, IAddOn
  {
    public MvcHtmlString Parse(string content)
    {
      return MvcHtmlString.Create(BBCParser.BBCodeToHTML(content));
    }

    private static string BBCodeToHTML(string text)
    {
      return BBCParser.BBCodeToHTML(text, (IList<BBTag>) new BBTag[11]
      {
        new BBTag("b", "<b>", "</b>", new BBAttribute[0]),
        new BBTag("i", "<i>", "</i>", new BBAttribute[0]),
        new BBTag("u", "<u>", "</u>", new BBAttribute[0]),
        new BBTag("strike", "<span style=\"text-decoration: line-through;\">", "</span>", new BBAttribute[0]),
        new BBTag("code", "<div class=\"ca-bbcode\"><div class=\"ca-bbcode2 ca-code\"><dl class=\"codebox\"><dt>Code:</dt><dd><code>", "</code></dd></dl></div></div>", new BBAttribute[0]),
        new BBTag("img", "<img src=\"${content}\" />", "", false, true, new BBAttribute[0]),
        new BBTag("quote", "<div class=\"ca-bbcode\"><div class=\"ca-bbcode2 ca-quote\"><blockquote><div><cite>${user} wrote:</cite>", "</div></blockquote></div></div>", new BBAttribute[2]
        {
          new BBAttribute("user", ""),
          new BBAttribute("user", "user")
        }),
        new BBTag("list", "<ul>", "</ul>", new BBAttribute[1]
        {
          new BBAttribute("class", "")
        }),
        new BBTag("list=1", "<ol>", "</ol>", new BBAttribute[1]
        {
          new BBAttribute("class", "")
        }),
        new BBTag("*", "<li>", "</li>", true, false, new BBAttribute[0]),
        new BBTag("url", "<a href=\"${href}\">", "</a>", new BBAttribute[2]
        {
          new BBAttribute("href", ""),
          new BBAttribute("href", "href")
        })
      });
    }

    private static string BBCodeToHTML(string text, IList<BBTag> tags)
    {
      return new BBCodeParser(ErrorMode.ErrorFree, tags).ToHtml(text).Replace(Environment.NewLine, "<br />");
    }

    public string Quote(string author, string content)
    {
      return string.Format("[quote={0}]{1}[/quote]", (object) author, (object) content);
    }

    public string Name
    {
      get
      {
        return "BBCode";
      }
    }
  }
}

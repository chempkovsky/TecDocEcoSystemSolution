// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.HTMLSanitizer
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mvcForum.AddOns
{
  public static class HTMLSanitizer
  {
    public static string SanitizeHTML(this string html)
    {
      return html.SanitizeHTML(new string[0]);
    }

    public static string SanitizeHTML(this string html, string[] blacklist)
    {
      HtmlDocument htmlDocument = new HtmlDocument();
      htmlDocument.LoadHtml(html.Replace("&", "&amp;"));
      StringWriter stringWriter = new StringWriter();
      HTMLSanitizer.ConvertTo(htmlDocument.DocumentNode, (TextWriter) stringWriter, blacklist);
      stringWriter.Flush();
      return stringWriter.ToString();
    }

    private static void ConvertContentTo(HtmlNode node, TextWriter outText, string[] blacklist)
    {
      foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) node.ChildNodes)
        HTMLSanitizer.ConvertTo(childNode, outText, blacklist);
    }

    public static void ConvertTo(HtmlNode node, TextWriter outText, string[] blacklist)
    {
      switch (node.NodeType)
      {
        case HtmlNodeType.Document:
          HTMLSanitizer.ConvertContentTo(node, outText, blacklist);
          break;
        case HtmlNodeType.Element:
          if (!((IEnumerable<string>) blacklist).Contains<string>(node.Name.ToLower()))
            outText.Write(string.Format("<{0}>", (object) node.Name));
          else
            outText.Write(string.Format("&lt;{0} {1}&gt;", (object) node.Name, (object) string.Join(" ", node.Attributes.Select<HtmlAttribute, string>((Func<HtmlAttribute, string>) (a => string.Format("{0}=\"{1}\"", (object) a.Name, (object) a.Value))).ToArray<string>())));
          if (node.HasChildNodes)
            HTMLSanitizer.ConvertContentTo(node, outText, blacklist);
          if (!((IEnumerable<string>) blacklist).Contains<string>(node.Name.ToLower()))
          {
            outText.Write(string.Format("</{0}>", (object) node.Name));
            break;
          }
          outText.Write(string.Format("&lt;/{0}&gt;", (object) node.Name));
          break;
        case HtmlNodeType.Text:
          string text = ((HtmlTextNode) node).Text;
          if (HtmlNode.IsOverlappedClosingElement(text) || string.IsNullOrWhiteSpace(text))
            break;
          outText.Write(HtmlEntity.DeEntitize(text));
          break;
      }
    }
  }
}

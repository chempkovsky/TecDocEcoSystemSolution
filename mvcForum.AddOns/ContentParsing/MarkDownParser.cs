// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.ContentParsing.MarkDownParser
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using MarkdownSharp;
using mvcForum.AddOns.ContentParsing.MarkDown;
using mvcForum.Core.Interfaces.AddOns;
using mvcForum.Web.Interfaces;
using System.Collections.Generic;
using System.Web.Mvc;

namespace mvcForum.AddOns.ContentParsing
{
  public class MarkDownParser : IContentParser, IAddOn
  {
    private readonly MarkDownConfiguration config;

    public MarkDownParser(MarkDownConfiguration config)
    {
      this.config = config;
    }

    public MvcHtmlString Parse(string content)
    {
      string startTag1 = "<div class=\"ca-bbcode\"><div class=\"ca-bbcode2 ca-quote\"><blockquote><div>";
      string endTag1 = "</div></blockquote></div></div>";
      if (!string.IsNullOrWhiteSpace(this.config.QuoteBefore))
        startTag1 = this.config.QuoteBefore;
      if (!string.IsNullOrWhiteSpace(this.config.QuoteAfter))
        endTag1 = this.config.QuoteAfter;
      string startTag2 = "<p>";
      string endTag2 = "</p>";
      if (!string.IsNullOrWhiteSpace(this.config.ParagraphBefore))
        startTag2 = this.config.ParagraphBefore;
      if (!string.IsNullOrWhiteSpace(this.config.ParagraphAfter))
        endTag2 = this.config.ParagraphAfter;
      string startTag3 = "<div class=\"ca-bbcode\"><div class=\"ca-bbcode2 ca-code\"><dl class=\"codebox\"><dt>Code:</dt><dd><code>";
      string endTag3 = "</code></dd></dl></div></div>";
      if (!string.IsNullOrWhiteSpace(this.config.CodeBefore))
        startTag3 = this.config.CodeBefore;
      if (!string.IsNullOrWhiteSpace(this.config.CodeAfter))
        endTag3 = this.config.CodeAfter;
      return MvcHtmlString.Create(new Markdown((IList<MarkDownTag>) new MarkDownTag[3]
      {
        new MarkDownTag("quote", startTag1, endTag1),
        new MarkDownTag("paragraph", startTag2, endTag2),
        new MarkDownTag("code", startTag3, endTag3)
      }).Transform(content.SanitizeHTML(new string[1]
      {
        "script"
      })));
    }

    public string Quote(string author, string content)
    {
      string format = "> <cite>{0} wrote:</cite> {1}";
      if (!string.IsNullOrWhiteSpace(this.config.Quote))
        format = this.config.Quote;
      return string.Format(format, (object) author, (object) content);
    }

    public string Name
    {
      get
      {
        return "MarkDown";
      }
    }
  }
}

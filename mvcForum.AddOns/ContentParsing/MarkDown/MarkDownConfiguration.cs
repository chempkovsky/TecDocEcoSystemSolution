// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.ContentParsing.MarkDown.MarkDownConfiguration
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions;

namespace mvcForum.AddOns.ContentParsing.MarkDown
{
  public class MarkDownConfiguration : AddOnConfiguration<MarkDownParser>
  {
    public MarkDownConfiguration(IRepository<AddOnConfiguration> configRepo)
      : base(configRepo)
    {
    }

    public string CodeAfter
    {
      get
      {
        return this.GetString(nameof (CodeAfter));
      }
      set
      {
        this.Set(nameof (CodeAfter), value);
      }
    }

    public string CodeBefore
    {
      get
      {
        return this.GetString(nameof (CodeBefore));
      }
      set
      {
        this.Set(nameof (CodeBefore), value);
      }
    }

    public string Quote
    {
      get
      {
        return this.GetString(nameof (Quote));
      }
      set
      {
        this.Set(nameof (Quote), value);
      }
    }

    public string QuoteAfter
    {
      get
      {
        return this.GetString(nameof (QuoteAfter));
      }
      set
      {
        this.Set(nameof (QuoteAfter), value);
      }
    }

    public string QuoteBefore
    {
      get
      {
        return this.GetString(nameof (QuoteBefore));
      }
      set
      {
        this.Set(nameof (QuoteBefore), value);
      }
    }

    public string ParagraphAfter
    {
      get
      {
        return this.GetString(nameof (ParagraphAfter));
      }
      set
      {
        this.Set(nameof (ParagraphAfter), value);
      }
    }

    public string ParagraphBefore
    {
      get
      {
        return this.GetString(nameof (ParagraphBefore));
      }
      set
      {
        this.Set(nameof (ParagraphBefore), value);
      }
    }

    private static class Keys
    {
      public const string QuoteBefore = "QuoteBefore";
      public const string QuoteAfter = "QuoteAfter";
      public const string ParagraphBefore = "ParagraphBefore";
      public const string ParagraphAfter = "ParagraphAfter";
      public const string CodeBefore = "CodeBefore";
      public const string CodeAfter = "CodeAfter";
      public const string Quote = "Quote";
    }
  }
}

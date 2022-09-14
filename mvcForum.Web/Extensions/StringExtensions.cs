// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Extensions.StringExtensions
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System;
using System.Text;
using System.Web.Mvc;

namespace mvcForum.Web.Extensions
{
  public static class StringExtensions
  {
    public static string Wrap(this string input)
    {
      return string.Empty;
    }

    public static string ToSlug(this string input)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char c in input.ToLower())
      {
        switch (c)
        {
          case ' ':
          case '-':
          case '.':
          case '=':
            stringBuilder.Append('-');
            break;
          default:
            if (char.IsLetterOrDigit(c))
            {
              stringBuilder.Append(c);
              break;
            }
            break;
        }
      }
      return stringBuilder.ToString().Replace("--", "-").Trim('-');
    }

    public static string ToEnglishSlug(this string input)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char ch in input.ToLower())
      {
        if (ch == ' ' || ch == '.' || (ch == '=' || ch == '-'))
          stringBuilder.Append('-');
        else if (ch <= 'z' && ch >= 'a' || ch <= '9' && ch >= '0')
          stringBuilder.Append(ch);
      }
      return stringBuilder.ToString().Replace("--", "-").Trim('-');
    }

    public static string ToDigest(this string input, int maxLength)
    {
      if (input.Length <= maxLength)
        return input;
      string str = input.Substring(0, maxLength);
      int num;
      for (num = maxLength; num > 0; --num)
      {
        char c = str[num - 1];
        if (char.IsWhiteSpace(c) || char.IsPunctuation(c) || char.IsSeparator(c))
          break;
      }
      if (num <= 0)
        num = maxLength + 1;
      return str.Substring(0, num - 1);
    }

    public static MvcHtmlString ToParagraphs(this string input)
    {
      string empty = string.Empty;
      if (!string.IsNullOrWhiteSpace(input))
      {
        string str = input;
        char[] separator = new char[1]{ '\n' };
        foreach (string innerText in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
        {
          TagBuilder tagBuilder = new TagBuilder("p");
          tagBuilder.SetInnerText(innerText);
          empty += tagBuilder.ToString(TagRenderMode.Normal);
        }
      }
      return new MvcHtmlString(empty);
    }
  }
}

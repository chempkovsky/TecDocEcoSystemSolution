// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Interfaces.Services.IPostService
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.Collections.Generic;

namespace mvcForum.Core.Interfaces.Services
{
  public interface IPostService
  {
    Post Create(
      ForumUser author,
      Topic topic,
      string subject,
      string body,
      string authorIP,
      string userAgent,
      string topicUrl,
      List<string> feedbackOutput,
      Post replyTo);

    Post Create(
      ForumUser author,
      Topic topic,
      string subject,
      string body,
      string authorIP,
      string userAgent,
      string topicUrl,
      List<string> feedbackOutput);
  }
}

// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Events.TopicFlagUpdatedEvent
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

namespace mvcForum.Core.Events
{
  public class TopicFlagUpdatedEvent : InformationBearingEvent
  {
    public int TopicId { get; set; }

    public TopicFlag OriginalFlag { get; set; }

    public string ForumRelativeURL { get; set; }
  }
}

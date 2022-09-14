// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Events.InformationBearingEvent
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.Collections.Generic;

namespace mvcForum.Core.Events
{
  public abstract class InformationBearingEvent
  {
    private List<EventFeedback> feedback = new List<EventFeedback>();

    public void AddFeedback(string textKey, string @namespace, object arguments)
    {
      this.feedback.Add(new EventFeedback()
      {
        TextKey = textKey,
        Namespace = @namespace,
        Arguments = arguments
      });
    }

    public IEnumerable<EventFeedback> GetFeedback()
    {
      return (IEnumerable<EventFeedback>) this.feedback;
    }
  }
}

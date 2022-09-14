// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Events.InstallationEventListener
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.Events;
using System;
using System.Net;

namespace mvcForum.Web.Events
{
  public class InstallationEventListener : IAsyncEventListener<InstallationEvent>, IEventListener<InstallationEvent>, IEventListener
  {
    private readonly IAsyncTask task;

    public InstallationEventListener(IAsyncTask task)
    {
      this.task = task;
    }

    public void Queue(InstallationEvent payload)
    {
      this.task.Execute((IEventListener) this, (object) payload, 2);
    }

    public void Handle(InstallationEvent payload)
    {
      new WebClient().DownloadData(string.Format("http://mvcforum.org/api/json/installation/add?version={0}", (object) payload.Version));
    }

    public void Handle(object payload)
    {
      if (!(payload is InstallationEvent))
        throw new ApplicationException("Unknown payload!");
      this.Handle((InstallationEvent) payload);
    }

    public bool RunAsynchronously
    {
      get
      {
        return false;
      }
    }

    public byte Priority
    {
      get
      {
        return 125;
      }
    }

    public bool UniqueEvent
    {
      get
      {
        return true;
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Events.PostReportResolvedEventListener
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Events;
using ApplicationBoilerplate.Logging;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Events;
using SimpleLocalisation;
using System;
using System.Net.Mail;
using System.Text;

namespace mvcForum.Web.Events
{
  public class PostReportResolvedEventListener : IAsyncEventListener<PostReportResolvedEvent>, IEventListener<PostReportResolvedEvent>, IEventListener
  {
    private readonly IAsyncTask task;
    private readonly IRepository<PostReport> prRepo;
    private readonly IConfiguration config;
    private readonly ILogger logger;
    private readonly TextManager texts;

    public PostReportResolvedEventListener(
      IAsyncTask task,
      IRepository<PostReport> prRepo,
      IConfiguration config,
      ILogger logger,
      TextManager texts)
    {
      this.task = task;
      this.prRepo = prRepo;
      this.config = config;
      this.logger = logger;
      this.texts = texts;
    }

    public void Queue(PostReportResolvedEvent payload)
    {
      this.task.Execute((IEventListener) this, (object) payload, 15);
    }

    public void Handle(object payload)
    {
      if (!(payload is PostReportResolvedEvent))
        throw new ApplicationException("Unknown payload!");
      this.Handle((PostReportResolvedEvent) payload);
    }

    public void Handle(PostReportResolvedEvent payload)
    {
      PostReport postReport = this.prRepo.Read(payload.PostReportId);
      if (!postReport.Feedback || postReport.ReportedBy == null || string.IsNullOrWhiteSpace(postReport.ReportedBy.EmailAddress))
        return;
      string emailAddress = postReport.ReportedBy.EmailAddress;
      string name = postReport.ReportedBy.Name;
      MailMessage message = new MailMessage();
      message.From = new MailAddress(this.config.RobotEmailAddress, this.config.RobotName);
      message.To.Add(new MailAddress(emailAddress, name));
      message.SubjectEncoding = Encoding.UTF8;
      message.BodyEncoding = Encoding.UTF8;
      string culture = postReport.ReportedBy.Culture;
      message.Subject = this.texts.Get<ForumConfigurator>("PostReportResolvedSubject", (object) new
      {
        Post = postReport.Post,
        Report = postReport
      });
      message.Body = this.texts.Get<ForumConfigurator>("PostReportResolvedBody", (object) new
      {
        Post = postReport.Post,
        Report = postReport
      });
      message.IsBodyHtml = false;
      try
      {
        new SmtpClient().Send(message);
        this.logger.Log(EventType.Debug, string.Format("The e-mail to user {0} ({1}) has been sent.", (object) name, (object) emailAddress));
      }
      catch (Exception ex)
      {
        this.logger.Log(EventType.Error, string.Format("Could not send e-mail to {0} from AsyncController, MessageAdded", (object) emailAddress), ex);
      }
    }

    public byte Priority
    {
      get
      {
        return byte.MaxValue;
      }
    }

    public bool RunAsynchronously
    {
      get
      {
        return true;
      }
    }

    public bool UniqueEvent
    {
      get
      {
        return false;
      }
    }
  }
}

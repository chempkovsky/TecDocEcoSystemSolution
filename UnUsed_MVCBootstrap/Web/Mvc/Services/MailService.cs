// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Services.MailService
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using MVCBootstrap.Web.Mvc.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;

namespace MVCBootstrap.Web.Mvc.Services
{
  public class MailService : IMailService
  {
    public void Send(string to, string subject, string body)
    {
      this.Send(((SmtpSection) ConfigurationManager.GetSection("system.net/mailSettings/smtp")).From, to, subject, body);
    }

    public void Send(string from, string to, string subject, string body)
    {
      this.Send(new MailAddress(from), (IList<MailAddress>) new List<MailAddress>()
      {
        new MailAddress(to)
      }, subject, body);
    }

    public void Send(string from, IList<string> to, string subject, string body)
    {
      List<MailAddress> mailAddressList = new List<MailAddress>();
      foreach (string address in (IEnumerable<string>) to)
        mailAddressList.Add(new MailAddress(address));
      this.Send(new MailAddress(from), (IList<MailAddress>) mailAddressList, subject, body);
    }

    public void Send(MailAddress from, IList<MailAddress> to, string subject, string body)
    {
      this.Send(from, to, (IList<MailAddress>) new List<MailAddress>(), subject, body);
    }

    public void Send(
      MailAddress from,
      IList<MailAddress> to,
      IList<MailAddress> replyTo,
      string subject,
      string body)
    {
      using (SmtpClient smtpClient = new SmtpClient())
      {
        using (MailMessage message = new MailMessage())
        {
          foreach (MailAddress mailAddress in (IEnumerable<MailAddress>) to)
            message.To.Add(mailAddress);
          foreach (MailAddress mailAddress in (IEnumerable<MailAddress>) replyTo)
            message.ReplyToList.Add(mailAddress);
          message.From = from;
          message.Subject = subject;
          message.Body = body;
          smtpClient.Send(message);
        }
      }
    }
  }
}

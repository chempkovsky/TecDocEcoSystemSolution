// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Interfaces.IMailService
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System.Collections.Generic;
using System.Net.Mail;

namespace MVCBootstrap.Web.Mvc.Interfaces
{
  public interface IMailService
  {
    void Send(string to, string subject, string body);

    void Send(string from, string to, string subject, string body);

    void Send(MailAddress from, IList<MailAddress> to, string subject, string body);

    void Send(
      MailAddress from,
      IList<MailAddress> to,
      IList<MailAddress> replyTo,
      string subject,
      string body);
  }
}

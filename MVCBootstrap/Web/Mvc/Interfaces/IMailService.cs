using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace MVCBootstrap.Web.Mvc.Interfaces {

	public interface IMailService {
		void Send(String to, String subject, String body);
		void Send(String from, String to, String subject, String body);
		void Send(MailAddress from, IList<MailAddress> to, String subject, String body);
		void Send(MailAddress from, IList<MailAddress> to, IList<MailAddress> replyTo, String subject, String body);
	}
}
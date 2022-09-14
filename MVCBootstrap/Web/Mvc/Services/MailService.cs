using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;

using MVCBootstrap.Web.Mvc.Interfaces;

namespace MVCBootstrap.Web.Mvc.Services {

	public class MailService : IMailService {

		public MailService() { }

		public void Send(String to, String subject, String body) {
			SmtpSection settings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
			Send(settings.From, to, subject, body);
		}

		public void Send(String from, String to, String subject, String body) {
			Send(new MailAddress(from), new List<MailAddress> { new MailAddress(to) }, subject, body);
		}

		public void Send(String from, IList<String> to, String subject, String body) {
			List<MailAddress> toAddresses = new List<MailAddress>();
			foreach (String address in to) {
				toAddresses.Add(new MailAddress(address));
			}
			Send(new MailAddress(from), toAddresses, subject, body);
		}

		public void Send(MailAddress from, IList<MailAddress> to, String subject, String body) {
			this.Send(from, to, new List<MailAddress>(), subject, body);
		}

		public void Send(MailAddress from, IList<MailAddress> to, IList<MailAddress> replyTo, String subject, String body) {
			using (SmtpClient client = new SmtpClient()) {
				using (MailMessage message = new MailMessage()) {

					foreach (MailAddress address in to) {
						message.To.Add(address);
					}

					foreach (MailAddress address in replyTo) {
						message.ReplyToList.Add(address);
					}

					message.From = from;
					message.Subject = subject;

					message.Body = body;

					client.Send(message);
				}
			}
		}
	}
}
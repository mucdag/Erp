using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;

namespace Erp.Core.Utilities
{
	public class EmailHelper
	{
		public static bool IsValid(string emailaddress)
		{
			try
			{
				MailAddress m = new MailAddress(emailaddress);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

		public static void SendEmail(string to, string body, string subject)
		{
			var message = new MailMessage();
			message.From = new MailAddress("mucdag@mucdag");
			message.To.Add(new MailAddress(to));
			message.Subject = subject;
			message.IsBodyHtml = true;
			message.Body = body;
			message.SubjectEncoding = System.Text.Encoding.UTF8;
			message.BodyEncoding = System.Text.Encoding.UTF8;

			var smtpClient = new SmtpClient
			{
				Host = "smtp.office365.com",
				Port = 587,
				EnableSsl = true,
				Credentials = new NetworkCredential("mucdag@mucdag", "mucdag"),
			};
			smtpClient.Send(message);
			message.Dispose();
		}

		public static void SendEmail(string to, string body, string subject, Dictionary<string, string> attachments)
		{
			var message = new MailMessage();
			message.From = new MailAddress("mucdag@mucdag");
			message.To.Add(new MailAddress(to));
			message.Subject = subject;
			message.IsBodyHtml = true;
			message.Body = body;
			message.SubjectEncoding = System.Text.Encoding.UTF8;
			message.BodyEncoding = System.Text.Encoding.UTF8;

			foreach (var attachment in attachments)
			{
				byte[] bytes = Convert.FromBase64String(attachment.Value);
				var stream = new MemoryStream(bytes);
				message.Attachments.Add(new Attachment(stream, attachment.Key));
			}

			var smtpClient = new SmtpClient
			{
				Host = "smtp.office365.com",
				Port = 587,
				EnableSsl = true,
				Credentials = new NetworkCredential("mucdag@mucdag", "mucdag"),
			};
			smtpClient.Send(message);
			message.Dispose();
		}
	}
}

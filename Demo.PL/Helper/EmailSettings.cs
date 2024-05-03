using Demo.DAL.Entities;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
	public static class EmailSettings
	{
		public static void SendEmail (Email email)
		{
           
			var client =  new SmtpClient ("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("folann83@gmail.com", "qaz!qaz987");
			client.Send("folann83@gmail.com", email.Recipients , email.Subject ,email.Body);
        }
	}
}

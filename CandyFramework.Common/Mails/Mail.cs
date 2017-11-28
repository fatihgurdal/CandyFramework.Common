using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
namespace CandyFramework.Common.Mails
{
    public class Mail
    {
        public MailMessage BaseMail { get; private set; }
        public SmtpClient Smtp { get; private set; }
        public Mail(string to, string from) : this(new List<string>() { to }, from)
        {

        }
        public Mail(List<string> to, string from)
        {
            BaseMail = new MailMessage { From = new MailAddress(from) };
            foreach (var item in to)
            {
                BaseMail.To.Add(item);
            }
        }
        public void LoadSMTP(string email, string password, string host, string port, bool ssl = false)
        {
            int _port;
            int.TryParse(port, out _port);
            Smtp = new SmtpClient
            {
                Credentials = new System.Net.NetworkCredential(email, password),
                Port = _port,
                Host = host,
                EnableSsl = ssl
            };
        }
        public void Send()
        {
            Smtp.Send(BaseMail);
        }
        public void SendAsync()
        {
            Task.Run(() =>
            {
                Smtp.Send(BaseMail);

            });
        }

    }
}

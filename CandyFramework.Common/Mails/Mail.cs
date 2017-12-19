using System.Collections.Generic;
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
        /// <summary>
        /// Dikkat eğer daha önceden Body  bölümüne ekleme yapıldı ise silinir !
        /// </summary>
        /// <param name="htmlString"></param>
        public void LoadBodyTamplete(string htmlString)
        {
            this.BaseMail.Body = htmlString;
            this.BaseMail.IsBodyHtml = true;
            this.BaseMail.BodyEncoding = Encoding.UTF8;
            this.BaseMail.SubjectEncoding = Encoding.UTF8;
        }
        /// <summary>
        /// Replace tagları {{OrnekTag}} şeklinde olmalıdır. Kullanılmayan taglar boş metin ile değiştirilir.
        /// </summary>
        /// <param name="oldString"></param>
        /// <param name="newString"></param>
        public void ReplaceMail(string oldString, string newString)
        {
            this.BaseMail.Body.Replace(oldString, newString);
            this.BaseMail.Subject.Replace(oldString, newString);
            this.BaseMail.From.DisplayName.Replace(oldString, newString);
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

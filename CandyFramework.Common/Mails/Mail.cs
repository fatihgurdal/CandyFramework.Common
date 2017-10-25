using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyFramework.Common.Mails
{
    public class Mail
    {
        public List<string> To { get; set; }
        public Mail(string to) : this(new List<string>() { to })
        {

        }
        public Mail(List<string> to)
        {
            To = new List<string>();
            to.AddRange(to);
        }
        public void Send()
        {

        }
        public void SendAsync()
        {
            Task.Run(() =>
            {

            });
        }

    }
}

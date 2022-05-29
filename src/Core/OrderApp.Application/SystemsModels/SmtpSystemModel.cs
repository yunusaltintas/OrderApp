using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.SystemsModels
{
    public class SmtpSystemModel
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string MailAdress { get; set; }
        public string Password { get; set; }
    }
}

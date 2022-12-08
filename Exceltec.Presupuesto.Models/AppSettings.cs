using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceltec.Presupuesto.Models
{
 
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string EmailSenderAccount { get; set; }
        public string EmailSenderPassword { get; set; }
        public string EmailSenderHost { get; set; }
        public string ApiUrl { get; set; }


    }
}

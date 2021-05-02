using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary.Alerts
{
    public class ChatAlert : Alert
    {
        public string messageToSend { get; set; }

        public ChatAlert(string v)
        {
            messageToSend = v;
        }

    }
}

using RequestLibrary.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary
{
    public class SendChatRequest : AuthenticatedRequest
    {
        public string textToSend { get; set; }
    }
}

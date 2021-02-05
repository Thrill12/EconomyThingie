using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary.Accounts
{
    public class AuthenticatedRequest
    {
        public string name { get; set; }
        public string seshID { get; set; }
        public int sysID { get; set; }
    }
}

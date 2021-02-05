using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary
{
    public class LoginRequest
    {

        public string usernameReq;
        public string passwordReq;

        public LoginRequest(string usernameReq, string passwordReq)
        {
            this.usernameReq = usernameReq;
            this.passwordReq = passwordReq;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary
{
    public class UserDisconnectRequest
    {
        public User userDisconnected;

        public UserDisconnectRequest(User userDisconnected)
        {
            this.userDisconnected = userDisconnected;
        }
    }
}

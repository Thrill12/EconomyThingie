using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary
{
    public class UpdateClientOnServerRequest
    {

        public User user;

        public UpdateClientOnServerRequest(User user)
        {
            this.user = user;
        }
    }
}

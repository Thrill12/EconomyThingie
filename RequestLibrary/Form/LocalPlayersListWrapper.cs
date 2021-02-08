using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary.Form
{
    public class LocalPlayersListWrapper
    {

        public List<User> users = new List<User>();

        public LocalPlayersListWrapper(List<User> users)
        {
            this.users = users;
        }

    }
}

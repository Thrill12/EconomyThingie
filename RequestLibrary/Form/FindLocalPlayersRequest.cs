using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary
{
    public class FindLocalPlayersRequest
    {
        List<User> users = new List<User>();
        public StarSystem startSystem;

        public FindLocalPlayersRequest(StarSystem startSystem)
        {
            this.startSystem = startSystem;
        }
    }
}

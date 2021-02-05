using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary
{
    public class FindJumpableSystemsRequest
    {

        List<StarSystem> systems = new List<StarSystem>();
        public StarSystem startSystem;

        public FindJumpableSystemsRequest(StarSystem startSystem)
        {
            this.startSystem = startSystem;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary
{
    public class StarSystemListWrapper
    {

        public List<StarSystem> systems = new List<StarSystem>();

        public StarSystemListWrapper(List<StarSystem> systems)
        {
            this.systems = systems;
        }
    }
}

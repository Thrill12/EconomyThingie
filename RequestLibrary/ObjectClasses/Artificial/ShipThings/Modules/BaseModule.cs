using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Slots;

namespace RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules
{
    public class BaseModule
    {
        public long weight;
        public long energyReq;

        public BaseModule(long weight, long energyReq)
        {
            this.weight = weight;
            this.energyReq = energyReq;
        }

        public BaseModule()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules.PassiveModules
{
    public class BasePassiveModule : BaseModule
    {
        public BasePassiveModule(long weight, long energyReq) : base(weight, energyReq)
        {
        }

        public BasePassiveModule()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules.PassiveModules
{
    class P_CargoContainer : BaseModule
    {

        public int cargoProvided;
        public long cargoWeight;

        public P_CargoContainer(int cargoProvided, long weightToAdd) : base(weightToAdd, 1)
        {         
            this.cargoProvided = cargoProvided;
        }
    }
}

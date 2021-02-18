using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules.PassiveModules
{
    class QEDriveTier1 : BasePassiveModule
    {
        public int QEIncrease = 15;
        
        public QEDriveTier1() : base(500, 5)
        {

        }
    }
}

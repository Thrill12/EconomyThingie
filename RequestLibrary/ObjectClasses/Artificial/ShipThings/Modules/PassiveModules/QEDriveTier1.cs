using RequestLibrary.ObjectClasses.Artificial.ShipThings.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules.PassiveModules
{
    public class QEDriveTier1 : BaseModule
    {
        public int QEIncrease = 15;
        
        public QEDriveTier1() : base(500, 5)
        {

        }

        public override void ApplyEffect(BaseShip shipToApplyTo)
        {
            shipToApplyTo.QELimit += 15;
        }
    }
}

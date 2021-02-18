using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules;

namespace RequestLibrary.ObjectClasses.Artificial.ShipThings.Slots
{
    public class Slot<T> where T : BaseModule
    {
        public string slotName;
        public T slotType;

        public Slot(string slotName)
        {
            this.slotName = slotName;
        }
    }
}

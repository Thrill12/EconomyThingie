using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Ships;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RequestLibrary.ObjectClasses.Artificial.ShipThings.Slots
{
    public class Slot<T> where T : BaseModule
    {
        public int id { get; set; }
        public string slotName { get; set; }
        public T slotType { get; set; }

        public Slot(string slotName)
        {
            this.slotName = slotName;
        }
    }
}

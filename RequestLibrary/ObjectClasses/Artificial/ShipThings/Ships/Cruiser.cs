using RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules.PassiveModules;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Slots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary.ObjectClasses.Artificial.ShipThings.Ships
{
    public class Cruiser : BaseShip
    {
        public List<Slot<BasePassiveModule>> passiveSlots = new List<Slot<BasePassiveModule>>();
        
        //public List<Slot<>> activeSlots;

        public Cruiser() : base(5000, 100, 100, 500, 50)
        {
            passiveSlots.Add(new Slot<BasePassiveModule>("Stern"));
            passiveSlots.Add(new Slot<BasePassiveModule>("Bow"));
            passiveSlots.Add(new Slot<BasePassiveModule>("Engine Room"));
        }

        public Cruiser(int id, int health, int shield, int cargolimit, float weight, float QELimit, float energy, int ownerID) : base()
        {
            this.id = id;
            this.health = health;
            this.shield = shield;
            this.cargoLimit = cargolimit;
            this.weight = weight;
            this.QELimit = QELimit;
            this.energy = energy;
            this.ownerID = ownerID;
        }
    }
}

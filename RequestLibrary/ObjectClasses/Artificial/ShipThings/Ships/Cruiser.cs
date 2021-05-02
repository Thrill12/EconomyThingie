using RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules.PassiveModules;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Slots;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary.ObjectClasses.Artificial.ShipThings.Ships
{
    public class Cruiser : BaseShip
    {       
        //public List<Slot<>> activeSlots;

        public Cruiser() : base(5000, 100, 100, 500, 50)
        {
            UpdateShipStats();
            this.shipType = "Cruiser";
        }

        public Cruiser(int id, int health, int shield, int cargolimit, float weight, float QELimit, float energy, int ownerID) : base()
        {
            UpdateShipStats();
            this.id = id;
            this.health = health;
            this.shield = shield;
            this.cargoLimit = cargolimit;
            this.weight = weight;
            this.QELimit = QELimit;
            this.energy = energy;
            this.ownerID = ownerID;
            this.shipType = "Cruiser";
        }

        public Cruiser(BaseShip ship)
        {
            UpdateShipStats();
            this.id = ship.id;
            this.health = ship.health;
            this.shield = ship.shield;
            this.cargoLimit = ship.cargoLimit;
            this.weight = ship.weight;
            this.QELimit = ship.QELimit;
            this.energy = ship.energy;
            this.ownerID = ship.ownerID;
            this.shipType = "Cruiser";
        }

        public override void ResetStats()
        {
            base.ResetStats();
            health = 5000;
            cargoLimit = 100;
            weight = 100;
            energy = 500;
            QELimit = 50;
        }

        public override void UpdateShipStats()
        {
            ResetStats();
            AddSlots();
            UpdateModules();      
        }

        public override void AddSlots()
        {
            //base.AddSlots();
            ResetStats();
            if(passiveSlots.Count != 3)                             // <---- MAX SLOTS HERE, UPDATE IF NEED TO
            {
                passiveSlots.Add(new Slot<BasePassiveModule>("Stern"));
                passiveSlots.Add(new Slot<BasePassiveModule>("Bow"));
                passiveSlots.Add(new Slot<BasePassiveModule>("EngineRoom"));
            }            
        }
    }
}

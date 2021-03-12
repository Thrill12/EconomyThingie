using RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules.PassiveModules;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Slots;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules;

namespace RequestLibrary.ObjectClasses.Artificial.ShipThings.Ships
{
    [Table("ships")]
    public class BaseShip
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [ForeignKey(typeof(User))]
        public int ownerID { get; set; }
        [Column("health")]
        public int health { get; set; }
        [Column("shield")]
        public int shield { get; set; }
        [Column("cargolimit")]
        public int cargoLimit { get; set; }
        [Column("weight")]
        public float weight { get; set; }
        [Column("livecrew")]
        public int liveCrew { get; set; }
        [Column("crewcap")]
        public int crewCap { get; set; }
        [Column("qelimit")]
        public float QELimit { get; set; }
        [Column("energy")]
        public float energy { get; set; }
        [Column("shiptype")]
        public string shipType { get; set; }

        [ManyToMany(typeof(BaseModule))]
        public List<BaseModule> equippedModules { get; set; }

        [Ignore]
        public List<Slot<BasePassiveModule>> passiveSlots { get; set; }

        public BaseShip(int health,  int cargoLimit, int weight, float energy, float qELimit, int shield = 0)
        {
            UpdateShipStats();

            this.health = health;
            this.shield = shield;
            this.cargoLimit = cargoLimit;
            this.weight = weight;
            this.energy = energy;
            QELimit = qELimit;
        }

        //public BaseShip(int health, int cargoLimit, int weight, float energy, float qELimit, int shield = 0)
        //{
        //    passiveSlots = new List<Slot<BasePassiveModule>>();

        //    this.health = health;
        //    this.shield = shield;
        //    this.cargoLimit = cargoLimit;
        //    this.weight = weight;
        //    this.energy = energy;
        //    QELimit = qELimit;
        //}

        public BaseShip(BaseShip shipToReplace)
        {
            shipToReplace.UpdateShipStats();
            id = shipToReplace.id;
            health = shipToReplace.health;
            shield = shipToReplace.shield;
            cargoLimit = shipToReplace.cargoLimit;
            weight = shipToReplace.weight;
            energy = shipToReplace.energy;
            QELimit = shipToReplace.QELimit;
            shipType = shipToReplace.shipType;
        }

        public BaseShip()
        {

        }

        public virtual void UpdateShipStats()
        {
            equippedModules = new List<BaseModule>();
            passiveSlots = new List<Slot<BasePassiveModule>>();
           
            if(equippedModules.Count() > 0)
            {
                foreach (BaseModule module in equippedModules)
                {
                    module.ApplyEffect();
                }
            }           
        }

        public virtual void ResetStats()
        {

        }

        public virtual void AddSlots()
        {

        }

        public BaseShip ChangeType()
        {
            switch (shipType)
            {
                case "Cruiser":
                    Cruiser ship = new Cruiser(this);
                    ship.UpdateShipStats();
                    return ship;               
            }

            return null;
        }
    }
}
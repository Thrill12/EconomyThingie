using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary.ObjectClasses.Artificial.ShipThings.Ships
{
    public class BaseShip
    {
        public int id;
        public int ownerID;
        public int health;
        public int shield = 0;
        public int cargoLimit;
        public float weight;   
        public int liveCrew;
        public int crewCap;
        public float QELimit;
        public float energy;
        public string shipType;

        public BaseShip(int id, int health,  int cargoLimit, int weight, float energy, float qELimit, int shield = 0)
        {
            this.id = id;
            this.health = health;
            this.shield = shield;
            this.cargoLimit = cargoLimit;
            this.weight = weight;
            this.energy = energy;
            QELimit = qELimit;
        }

        public BaseShip(int health, int cargoLimit, int weight, float energy, float qELimit, int shield = 0)
        {
            this.health = health;
            this.shield = shield;
            this.cargoLimit = cargoLimit;
            this.weight = weight;
            this.energy = energy;
            QELimit = qELimit;
        }

        public BaseShip()
        {

        }
    }
}
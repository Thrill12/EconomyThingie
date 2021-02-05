using System;
using System.Collections.Generic;
using System.Text;

namespace RequestLibrary
{
    public class StarSystem
    {

        public int ID;
        public string name;
        public int starClass;
        public int posX;
        public int posY;
        public List<Planet> planets = new List<Planet>();

        public StarSystem(int ID, string name, int starClass, int posX, int posY)
        {
            this.ID = ID;
            this.name = name;
            this.starClass = starClass;
            this.posX = posX;
            this.posY = posY;
        }
    }
}

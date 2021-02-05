using System;
using System.Collections.Generic;
using System.Text;

namespace RequestLibrary
{
    public class Planet
    {

        public int ID;
        public StarSystem sysID;
        public int systemID;
        public string name;
        public int size;
        public string biome;

        public Planet(StarSystem sysID, string name, int size, string biome)
        {
            this.sysID = sysID;
            this.name = name;
            this.size = size;
            this.biome = biome;
        }

        public Planet(int systemID, string name, int size, string biome)
        {
            this.systemID = systemID;
            this.name = name;
            this.size = size;
            this.biome = biome;
        }
    }
}

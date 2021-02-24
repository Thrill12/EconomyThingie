using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestLibrary
{
    [Table("planets")]
    public class Planet
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int ID { get; set; }
        public StarSystem sysID;
        [Column("name")]
        public string name { get; set; }

        [Column("sysid")]
        public int systemID { get; set; }

        public StarSystem sys { get; set; }
        [Column("size")]
        public int size { get; set; }
        [Column("biome")]
        public string biome { get; set; }

        public Planet(StarSystem sysID, string name, int size, string biome)
        {
            this.sys = sysID;
            this.systemID = sysID.ID;
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

        public Planet()
        {

        }
    }
}

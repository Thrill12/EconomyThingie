using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;

namespace RequestLibrary
{
    [Table("starsystems")]
    public class StarSystem
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int ID { get; set; }

        [Column("name")]
        public string name { get; set; }

        [Column("class")]
        public int starClass { get; set; }

        [Column("positionx")]
        public int posX { get; set; }

        [Column("positiony")]
        public int posY { get; set; }

        public List<Planet> planets = new List<Planet>();

        public StarSystem(int ID, string name, int starClass, int posX, int posY)
        {
            this.ID = ID;
            this.name = name;
            this.starClass = starClass;
            this.posX = posX;
            this.posY = posY;
        }

        public StarSystem(string name, int starClass, int posX, int posY)
        {
            this.name = name;
            this.starClass = starClass;
            this.posX = posX;
            this.posY = posY;
        }

        public StarSystem()
        {

        }
    }
}

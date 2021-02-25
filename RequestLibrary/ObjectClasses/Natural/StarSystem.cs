using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace RequestLibrary
{
    [Table("starsystems")]
    public class StarSystem
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int ID { get; set; }

        [Indexed]
        [Column("name")]
        public string name { get; set; }

        [Column("class")]
        public int starClass { get; set; }

        [Column("positionx")]
        public int posX { get; set; }

        [Column("positiony")]
        public int posY { get; set; }

        [OneToMany]
        public List<Planet> planets { get; set; }

        [ManyToMany(typeof(StarSystem))]
        public List<StarSystem> hyperlanes { get; set; }

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

using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Server.DatabaseFiles
{
    [Table("starsystems")]
    public class Systems
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int id { get; set; }

        [Column("name")]
        public string name { get; set; }

        [Column("class")]
        public int starclass { get; set; }

        [Column("positionx")]
        public int positionX { get; set; }

        [Column("positiony")]
        public int positionY { get; set; }
    }
}

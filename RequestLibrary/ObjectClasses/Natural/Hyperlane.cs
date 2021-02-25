using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RequestLibrary
{
    [Table("hyperlanes")]
    public class Hyperlane
    {
        [ForeignKey(typeof(StarSystem))]
        [Column("sysid1")]
        public int sysId1 { get; set; }

        [ForeignKey(typeof(StarSystem))]
        [Column("sysid2")]
        public int sysId2 { get; set; }

        [Column("length")]
        public double length { get; set; }

        public Hyperlane(StarSystem sysid1, StarSystem sysid2, double length)
        {
            this.sysId1 = sysid1.ID;
            this.sysId2 = sysid2.ID;
            this.length = length;
        }

        //public Hyperlane(int sysid1ID, int sysid2ID, double length)
        //{
        //    this.sysid1ID = sysid1ID;
        //    this.sysid2ID = sysid2ID;
        //    this.length = length;
        //}

        public Hyperlane()
        {

        }
    }
}

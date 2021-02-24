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
        public StarSystem sysid1;
        public StarSystem sysid2;
        [Column("sysid1")]
        public int sysid1ID { get; set; }
        public StarSystem sys1 { get; set; }
        [Column("sysid2")]
        public int sysid2ID { get; set; }
        public StarSystem sys2 { get; set; }
        [Column("length")]
        public double length { get; set; }

        public Hyperlane(StarSystem sysid1, StarSystem sysid2, double length)
        {
            this.sys1 = sysid1;
            this.sysid1ID = sysid1.ID;
            this.sys2 = sysid2;
            this.sysid2ID = sysid2.ID;
            this.length = length;
        }

        public Hyperlane(int sysid1ID, int sysid2ID, double length)
        {
            this.sysid1ID = sysid1ID;
            this.sysid2ID = sysid2ID;
            this.length = length;
        }

        public Hyperlane()
        {

        }
    }
}

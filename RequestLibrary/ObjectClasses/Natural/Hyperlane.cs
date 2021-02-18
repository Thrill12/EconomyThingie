using System;
using System.Collections.Generic;
using System.Text;

namespace RequestLibrary
{
    public class Hyperlane
    {

        public StarSystem sysid1;
        public StarSystem sysid2;
        public int sysid1ID;
        public int sysid2ID;
        public double length;

        public Hyperlane(StarSystem sysid1, StarSystem sysid2, double length)
        {
            this.sysid1 = sysid1;
            this.sysid2 = sysid2;
            this.length = length;
        }

        public Hyperlane(int sysid1ID, int sysid2ID, double length)
        {
            this.sysid1ID = sysid1ID;
            this.sysid2ID = sysid2ID;
            this.length = length;
        }
    }
}

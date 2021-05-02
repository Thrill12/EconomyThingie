using RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Ships;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary.ObjectClasses
{
    [Table("commodities")]
    public class Commodity
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [Column("value")]
        public double value { get; set; }
        [Column("level")]
        public string level { get; set; }
        [Column("weight")]
        public double weight { get; set; }
        [Column("amount")]
        public int amount { get; set; }
        [ForeignKey(typeof(BaseModule))]
        public int owningID { get; set; }

        public Commodity(double value, string level, double weight)
        {
            this.value = value;
            this.level = level;
            this.weight = weight;
        }

        public Commodity()
        {
        }
    }
}

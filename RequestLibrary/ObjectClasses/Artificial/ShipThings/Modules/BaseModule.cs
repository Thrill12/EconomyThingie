using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Ships;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Slots;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules
{
    [Table("Modules")]
    public class BaseModule
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [Column("weight")]
        public long weight { get; set; }
        [Column("energyreq")]
        public long energyReq { get; set; }
        [ForeignKey(typeof(BaseShip))]
        public int equippedShipID { get; set; }        

        public BaseModule(long weight, long energyReq)
        {
            this.weight = weight;
            this.energyReq = energyReq;
        }

        public BaseModule()
        {

        }

        public virtual void ApplyEffect(BaseShip shipToApplyTo)
        {

        }
    }
}

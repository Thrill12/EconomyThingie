using RequestLibrary;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Ships;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RequestLibrary
{
    [Table("users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [Ignore]
        public string seshID { get; set; }
        [Indexed]
        [Column("username")]
        public string username { get; set; }
        [Column("password")]
        public string password { get; set; }

        [ForeignKey(typeof(StarSystem))]
        public int positionID { get; set; }

        [Ignore]
        public StarSystem position { get; set; }

        [Column("galacticcredits")]
        public int galacticCredits { get; set; }
        [Column("diplomaticweight")]
        public int diplomaticWeight { get; set; }

        [OneToOne]
        public BaseShip equippedShip { get; set; }
        [ForeignKey(typeof(BaseShip))]
        public int equippedShipID { get; set; }

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
            this.galacticCredits = 1000;
            this.diplomaticWeight = 0;
        }

        public User()
        {

        }

        public User(int galacticCredits, int diplomaticWeight)
        {
            this.galacticCredits = galacticCredits;
            this.diplomaticWeight = diplomaticWeight;
        }

        public User(string username, string password, int positionID, int galacticCredits, int diplomaticWeight)
        {
            this.username = username;
            this.password = password;
            this.galacticCredits = 1000;
            this.diplomaticWeight = 0;
            this.galacticCredits = galacticCredits;
            this.diplomaticWeight = diplomaticWeight;
            this.positionID = positionID;            
        }

        public User(int id, string username, string password, int positionID, int galacticCredits, int diplomaticWeight, int shipID)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.galacticCredits = 1000;
            this.diplomaticWeight = 0;
            this.galacticCredits = galacticCredits;
            this.diplomaticWeight = diplomaticWeight;
            this.positionID = positionID;
            equippedShipID = shipID;
        }
    }
}

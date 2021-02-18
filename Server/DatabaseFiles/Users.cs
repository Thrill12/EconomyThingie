using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DatabaseFiles
{
    [Table("users")]
    public class Users
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int id { get; set; }

        [Column("username")]
        public string username { get; set; }

        [Column("password")]
        public string password { get; set; }

        [Indexed]
        [Column("sysid")]
        public int positionID { get; set; }

        [Column("galacticcredits")]
        public int galacticCredits { get; set; }

        [Column("diplomaticweight")]
        public int diplomaticWeight { get; set; }

        [Indexed]
        [Column("currentship")]
        public int equippedShipId { get; set; }
    }
}

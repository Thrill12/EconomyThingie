//using SQLite;
//using SQLiteNetExtensions.Attributes;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RequestLibrary.ObjectClasses.Natural
//{
//    [Table("sectors")]
//    public class Sector
//    {
//        [PrimaryKey, AutoIncrement]
//        public int id { get; set; }

//        [Column("name")]
//        public string name { get; set; }

//        [Column("top-left")]
//        public int top_left {get;set;}

//        [Column("top-right")]
//        public int top_right { get; set; }

//        [Column("bot-left")]
//        public int bot_left { get; set; }

//        [Column("bot-right")]
//        public int bot_right { get; set; }

//        [OneToMany]
//        public List<StarSystem> systems { get; set; }

//        public Sector(string name, int top_left, int top_right, int bot_left, int bot_right)
//        {
//            systems = new List<StarSystem>();

//            this.name = name;
//            this.top_left = top_left;
//            this.top_right = top_right;
//            this.bot_left = bot_left;
//            this.bot_right = bot_right;
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Server.DatabaseFiles
{
    public class DatabaseHandler
    {
        private SQLiteConnection db;

        public DatabaseHandler()
        {
            db = new SQLiteConnection("testingDB.db");
            db.CreateTable<Users>();
            db.CreateTable<Systems>();
        }

        public void AddUser()
        {
            var userToAdd = new Users();
            userToAdd.password = "password";
            userToAdd.username = "pickledthrill";
            userToAdd.positionID = 1;
            userToAdd.galacticCredits = 69;
            userToAdd.diplomaticWeight = 69;
            userToAdd.equippedShipId = 420;

            db.Insert(userToAdd);
        }

        public void AddSystem()
        {
            var sysToAdd = new Systems();
            sysToAdd.name = "Sol";
            sysToAdd.starclass = 5;
            sysToAdd.positionX = 0;
            sysToAdd.positionY = 0;

            db.Insert(sysToAdd);
        }

        public void GetSystem()
        {
            var systems = db.Query<Systems>("SELECT * FROM starsystems");

            foreach(Systems sys in systems)
            {
                Console.WriteLine(sys.name);
            }
        }

        public void GetUser()
        {
            var users = db.Query<Users>("SELECT * FROM users");

            foreach (Users u in users)
            {
                Console.WriteLine(u.password);
            }
        }
    }
}

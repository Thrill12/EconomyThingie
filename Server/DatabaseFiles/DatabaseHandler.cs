using System;
using System.Collections.Generic;
using System.Text;
using RequestLibrary;
using SQLite;

namespace Server.DatabaseFiles
{
    public static class DatabaseHandler
    {
        public static SQLiteConnection db;

        static DatabaseHandler()
        {
            db = new SQLiteConnection("testingDB.db");
            db.CreateTable<StarSystem>();
            db.CreateTable<Planet>();
            db.CreateTable<Hyperlane>();
        }

        //public static void AddUser()
        //{
        //    var userToAdd = new Users();
        //    userToAdd.password = "password";
        //    userToAdd.username = "pickledthrill";
        //    userToAdd.positionID = 1;
        //    userToAdd.galacticCredits = 69;
        //    userToAdd.diplomaticWeight = 69;
        //    userToAdd.equippedShipId = 420;

        //    db.Insert(userToAdd);
        //}

        public static void AddSystem(StarSystem systemToAdd)
        {
            db.Insert(systemToAdd);
        }

        public static void AddListOfSystems(List<StarSystem> listToAdd)
        {
            db.RunInTransaction(() =>
            {
                foreach(StarSystem sys in listToAdd)
                {
                    db.Insert(sys);
                }
            });
        }

        public static void AddPlanet(Planet planetToAdd)
        {
            db.Insert(planetToAdd);
        }

        public static void AddListOfPlanets(List<Planet> listToAdd)
        {
            db.RunInTransaction(() =>
            {
                foreach (Planet planet in listToAdd)
                {
                    db.Insert(planet);
                }
            });
        }

        public static void AddHyperlane(Hyperlane hyperlane)
        {
            db.Insert(hyperlane);
        }

        public static void AddListOfHyperlanes(List<Hyperlane> listToAdd)
        {
            db.RunInTransaction(() =>
            {
                foreach (Hyperlane hyperlane in listToAdd)
                {
                    db.Insert(hyperlane);
                }
            });
        }

        //public StarSystem GetSystem()
        //{
        //    var systems = db.Query<Systems>("SELECT * FROM starsystems");

        //    foreach(Systems sys in systems)
        //    {
        //        Console.WriteLine(sys.name);
        //    }

        //    return systems;
        //}

        public static Boolean GetSystemByID(int id)
        {
            var system = db.Query<StarSystem>($"SELECT * FROM starsystems WHERE id = {id}");

            if(system == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //public static Boolean GetUser()
        //{
        //    var users = db.Query<Users>("SELECT * FROM users");

        //    foreach (Users u in users)
        //    {
        //        Console.WriteLine(u.password);
        //    }

        //    return true;
        //}
    }
}

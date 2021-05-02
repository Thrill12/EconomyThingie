using System;
using System.Collections.Generic;
using System.Text;
using RequestLibrary;
using SQLite;
using System.Linq;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Ships;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Slots;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules.PassiveModules;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules;
using RequestLibrary.ObjectClasses;

namespace Server.DatabaseFiles
{
    public class DatabaseHandler
    {
        public static SQLiteConnection db = new SQLiteConnection("testingDB.db");

        public DatabaseHandler()
        {
            db.CreateTable<StarSystem>();
            db.CreateTable<Planet>();
            db.CreateTable<Hyperlane>();
            db.CreateTable<User>();
            db.CreateTable<BaseShip>();
            db.CreateTable<BaseModule>();
            db.CreateTable<Commodity>();
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

        public static void InsertUser(User user)
        {
            if(db.Query<User>($"SELECT * FROM users WHERE username='{user.username}'").ToList().Count > 0)
            {
                Console.WriteLine("User " + user.username + " already exists.");
            }
            else
            {
                db.Insert(user);
            }           
        }

        public static void InsertShip(BaseShip shipToAdd)
        {
            db.Insert(shipToAdd);
        }

        public static void InsertModule(BaseModule modToAdd)
        {
            db.Insert(modToAdd);
        }

        public static BaseShip GetShipById(int id)
        {
            var ship = db.Query<BaseShip>($"SELECT * FROM ships WHERE id={id}").ToList()[0];
            return ship;
        }

        public static int GetNumOfUsers()
        {
            var res = db.Query<User>("SELECT * FROM users").Count();
            return res;
        }

        public static User GetWholeUser(string username)
        {
            var q = db.Query<User>($"SELECT * FROM users WHERE username='{username}'");

            if (q.ToList().Count > 0)
            {
                return q[0];
            }
            else
            {
                Console.WriteLine("Required user " + username + " doesn't exist.");
                return null;
            }
        }

        public static List<User> GetUsersInSystem(StarSystem sysToCheck)
        {
            var res = db.Query<User>($"SELECT * FROM users WHERE positionID={sysToCheck.ID}").ToList();

            return res;
        }
        
        public static void PopPosSystem(User user)
        {
            //MAY NEED TO WRITE THIS
        }

        public static void InsertMatter(Commodity matterToInsert)
        {
            db.Insert(matterToInsert);
        }

        #region SystemStuff

        public static List<StarSystem> FindJumpableSystems(StarSystem sys)
        {
            var query = db.Query<StarSystem>($"SELECT * FROM hyperlanes INNER JOIN starsystems ON hyperlanes.sysid2 = starsystems.id WHERE hyperlanes.sysid1 = {sys.ID}").ToList();
            //var query = db.Table<Hyperlane>().Where(h => h.sysId1 == sys.ID).Join(db.Table<StarSystem>(), h => h.sysId2, s => s.ID, (h, s) => s).ToList();

            return query;
        }

        public static void RefreshCurrentGalaxy()
        {
            db.DropTable<StarSystem>();
            db.CreateTable<StarSystem>();
            db.DropTable<Planet>();
            db.CreateTable<Planet>();
            db.DropTable<Hyperlane>();
            db.CreateTable<Hyperlane>();

            Console.WriteLine("Delete users too?");
            var ans = Console.ReadLine();

            if (ans == "y")
            {
                db.DropTable<User>();
                db.CreateTable<User>();
                db.DropTable<BaseShip>();
                db.CreateTable<BaseShip>();
                db.DropTable<BaseModule>();
                db.CreateTable<BaseModule>();
            }
        }

        public static List<StarSystem> GetMainCluster()
        {
            List<List<StarSystem>> clusters = GetClusters();

            clusters.OrderByDescending(list => list.Count());

            for (int i = 1; i < clusters.Count(); i++)
            {
                foreach (StarSystem system in clusters[i])
                {
                    int starID = system.ID;

                    db.Delete<StarSystem>(starID);
                }
                clusters[i].Clear();
            }

            Console.WriteLine("There are " + clusters[0].Count() + " systems in this cluster");

            return clusters[0];
        }

        public static List<List<StarSystem>> GetClusters()
        {
            List<int> allSystems = GetAllSystems().Select(s => s.ID).ToList();
            List<List<StarSystem>> clusters = new List<List<StarSystem>>();

            while (allSystems.Count > 0)
            {
                Console.WriteLine("Cluster added to main list");
                clusters.Add(GetCluster(GetSystemByID(allSystems[0])));
                allSystems = allSystems.Where(s => !clusters.Last().Select(boo => boo.ID).Contains(s)).ToList();
                Console.WriteLine("System count: " + allSystems.Count());
            }

            //foreach(List<StarSystem> list in clusters)
            //{
            //    Console.WriteLine(list.Count());
            //}

            return clusters;
        }

        public static List<StarSystem> GetCluster(StarSystem startSystem)
        {
            List<StarSystem> visitedSystems = new List<StarSystem>();
            Queue<StarSystem> uncheckedSystems = new Queue<StarSystem>();
            uncheckedSystems.Enqueue(startSystem);

            visitedSystems.Add(startSystem);

            while (uncheckedSystems.Count > 0)
            {
                var systems = FindJumpableSystems(uncheckedSystems.Dequeue());

                foreach (StarSystem sys in systems)
                {
                    if (visitedSystems.All(s => s.ID != sys.ID))
                    {
                        visitedSystems.Add(sys);
                        uncheckedSystems.Enqueue(sys);
                    }
                }
            }

            return visitedSystems;
        }

        public void AddSystem(StarSystem systemToAdd)
        {
            db.Insert(systemToAdd);
        }

        public void AddListOfSystems(List<StarSystem> listToAdd)
        {
            db.RunInTransaction(() =>
            {
                foreach (StarSystem sys in listToAdd)
                {
                    db.Insert(sys);
                }
            });
        }

        public void AddPlanet(Planet planetToAdd)
        {
            db.Insert(planetToAdd);
        }

        public void AddListOfPlanets(List<Planet> listToAdd)
        {
            db.RunInTransaction(() =>
            {
                foreach (Planet planet in listToAdd)
                {
                    db.Insert(planet);
                }
            });
        }

        public void AddHyperlane(Hyperlane hyperlane)
        {
            db.Insert(hyperlane);
        }

        public void AddListOfHyperlanes(List<Hyperlane> listToAdd)
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

        public static StarSystem GetSystemByID(int id)
        {
            var system = db.Query<StarSystem>($"SELECT * FROM starsystems WHERE id = {id}");

            if (system == null)
            {
                Console.WriteLine("Couldn't find system.");
                return null;
            }
            else
            {
                return system[0];
            }
        }

        public static List<StarSystem> GetAllSystems()
        {
            var systems = db.Query<StarSystem>("SELECT * FROM starsystems").ToList();
            return systems;
        }

        #endregion
    }
}

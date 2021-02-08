using RequestLibrary;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class StarDatabaseCode
    {

        public static SQLiteConnection sqlite_conn;

        public static void StartDB()
        {                     
            sqlite_conn = CreateConnection();
            CreateTables(sqlite_conn);
        }

        static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            //Create new database connection
            sqlite_conn = new SQLiteConnection("Data Source= StarsDB.db; Version = 3; New = True; Compress = True; ");
            //Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Couldn't connect to database. Valve, pls fix. ERROR: " + ex.Message);
            }
            return sqlite_conn;
        }

        public static void CreateTables(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            string CreateSystemTable = @"CREATE TABLE IF NOT EXISTS starsystems(
                                                                    _id INTEGER PRIMARY KEY, 
                                                                    name VARCHAR(25), 
                                                                    class INTEGER,
                                                                    positionX INTEGER,
                                                                    positionY INTEGER)";
            string CreatePlanetTable = @"CREATE TABLE IF NOT EXISTS planets(
                                                                    _id INTEGER PRIMARY KEY,
                                                                    name VARCHAR(20), 
                                                                    sysid INTEGER,
                                                                    size INTEGER,
                                                                    biome VARCHAR(20),
                                                                    FOREIGN KEY (sysid)
                                                                        REFERENCES starsystems(id)
                                                                            ON DELETE CASCADE
                                                                            ON UPDATE NO ACTION)";
            string CreateHyperlanesTable = @"CREATE TABLE IF NOT EXISTS hyperlanes(
                                                                    sysid1 INTEGER NOT NULL,
                                                                    sysid2 INTEGER NOT NULL,
                                                                    length INTEGER NOT NULL,

                                                                    FOREIGN KEY (sysid1)
                                                                        REFERENCES starsystems(id)
                                                                            ON DELETE CASCADE
                                                                            ON UPDATE NO ACTION,
                                                                    
                                                                    FOREIGN KEY (sysid2)
                                                                        REFERENCES starsystems(id)
                                                                            ON DELETE CASCADE
                                                                            ON UPDATE NO ACTION)";
            string CreateUsersTable = @"CREATE TABLE IF NOT EXISTS users(
                                                                    _id INTEGER PRIMARY KEY,
                                                                    username VARCHAR(30),
                                                                    password VARCHAR(50),
                                                                    sysid INTEGER,                                                            
                                                                    galacticcredits INTEGER,    
                                                                    diplomaticweight INTEGER,

                                                                    FOREIGN KEY (sysid)
                                                                        REFERENCES starsystems(id)
                                                                            ON UPDATE NO ACTION)";

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = CreateSystemTable;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = CreatePlanetTable;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = CreateHyperlanesTable;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = CreateUsersTable;
            sqlite_cmd.ExecuteNonQuery();
        }

        public static void RefreshCurrentGalaxy()
        {
            SQLiteCommand cmd;
            cmd = sqlite_conn.CreateCommand();
            cmd.CommandText = "DELETE FROM starsystems";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DELETE FROM planets";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DELETE FROM hyperlanes";
            cmd.ExecuteNonQuery();

            CreateTables(sqlite_conn);
        }

        public static void InsertSystem(StarSystem system)
        {
            SQLiteCommand addSystem;
            addSystem = sqlite_conn.CreateCommand();

            string chkStr = $"SELECT COUNT(*) FROM starsystems WHERE _id={system.ID}";
            using var cmd = new SQLiteCommand(chkStr, sqlite_conn);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            int count = 0;

            while (rdr.Read())
            {
                count = rdr.GetInt32(0);
            }

            if(count == 0)
            {
                addSystem.CommandText = @$"INSERT INTO starsystems(name, class, positionX, positionY) 
                                                            VALUES('{system.name}', {system.starClass},{system.posX}, {system.posY}); ";
                addSystem.ExecuteNonQuery();
            }           
        }

        public static void InsertPlanet(Planet planet)
        {
            SQLiteCommand addPlanet;
            addPlanet = sqlite_conn.CreateCommand();

            string chkStr = $"SELECT COUNT(*) FROM planets WHERE _id={planet.ID}";
            using var cmd = new SQLiteCommand(chkStr, sqlite_conn);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            int count = 0;

            while (rdr.Read())
            {
                count = rdr.GetInt32(0);
            }

            if (count == 0)
            {
                addPlanet.CommandText = @$"INSERT INTO planets(name, sysid, size, biome) 
                                                            VALUES('{planet.name}', {planet.sysID.ID},{planet.size}, '{planet.biome}'); ";
                addPlanet.ExecuteNonQuery();
            }                
        }

        public static void InsertHyperlanes(Hyperlane hyperlane)
        {
            SQLiteCommand addHyperLane;
            addHyperLane = sqlite_conn.CreateCommand();
            addHyperLane.CommandText = @$"INSERT INTO hyperlanes(sysid1, sysid2, length) 
                                                            VALUES({hyperlane.sysid1.ID}, {hyperlane.sysid2.ID}, {hyperlane.length.ToString(CultureInfo.InvariantCulture)}); ";
            addHyperLane.ExecuteNonQuery();
        }

        public static void InsertUser(User user)
        {
            SQLiteCommand addUser;
            addUser = sqlite_conn.CreateCommand();

            string chkStr = $"SELECT COUNT(*) FROM users WHERE username='{user.username}'";
            using var cmd = new SQLiteCommand(chkStr, sqlite_conn);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            int count = 0;

            while (rdr.Read())
            {
                count = rdr.GetInt32(0);
            }

            if (count == 0)
            {
                addUser.CommandText = $@"INSERT INTO users(username, password, sysid, galacticcredits, diplomaticweight)
                                                            VALUES('{user.username}', '{user.password}', 1, 1000, 0);";
                addUser.ExecuteNonQuery();
            }                
        }

        public static List<User> GetUsersInSystem(StarSystem sysName)
        {
            string selectStartSystemCommand = $"SELECT * FROM users WHERE sysid = {sysName.ID}";
            using var cmd = new SQLiteCommand(selectStartSystemCommand, StarDatabaseCode.sqlite_conn);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            User userToFetch;
            List<User> usersInSystem = new List<User>();

            while (rdr.Read())
            {
                userToFetch = new User(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetInt32(5));
                PopPosSystem(userToFetch);
                usersInSystem.Add(userToFetch);
            }

            return usersInSystem;
        }

        public static User GetWholeUser(string username)
        {
            SQLiteCommand user = StarDatabaseCode.sqlite_conn.CreateCommand();
            user.CommandText = $"SELECT * FROM users WHERE username='{username}'";
            SQLiteDataReader rdr = user.ExecuteReader();

            User userToFetch;

            rdr.Read();
            userToFetch = new User(rdr.GetInt32(0),rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetInt32(5));
            PopPosSystem(userToFetch);

            return userToFetch;
        }

        public static void PopPosSystem(User user)
        {
            string selectStartSystemCommand = $"SELECT * FROM starsystems WHERE _id = {user.positionID}";
            using var cmd = new SQLiteCommand(selectStartSystemCommand, StarDatabaseCode.sqlite_conn);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            StarSystem system;

            rdr.Read();
            system = new StarSystem(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4));
            PopPlanetsInSystem(system);
            user.position = system;      
        }

        public static void PopPlanetsInSystem(StarSystem system)
        {
            string selectStartSystemCommand = $"SELECT * FROM planets WHERE sysid = {system.ID}";
            using var cmd = new SQLiteCommand(selectStartSystemCommand, StarDatabaseCode.sqlite_conn);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            List<Planet> planets = new List<Planet>();
            Planet planet;

            while (rdr.Read())
            {
                planet = new Planet(system, rdr.GetString(1), rdr.GetInt32(3), rdr.GetString(4));
                planets.Add(planet);
                return;
            }

            system.planets = planets;
        }

        public static StarSystem FindSystem(string name)
        {
            string selectStartSystemCommand = $"SELECT * FROM starsystems WHERE name = '{name}'";
            using var cmd = new SQLiteCommand(selectStartSystemCommand, StarDatabaseCode.sqlite_conn);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            StarSystem system;

            rdr.Read();
            system = new StarSystem(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4));
            PopPlanetsInSystem(system);

            return system;
        }

        public static StarSystem FindSystemByID(int id)
        {
            string selectStartSystemCommand = $"SELECT * FROM starsystems WHERE _id = {id}";
            using var cmd = new SQLiteCommand(selectStartSystemCommand, StarDatabaseCode.sqlite_conn);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            StarSystem system;

            rdr.Read();
            system = new StarSystem(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4));
            PopPlanetsInSystem(system);

            return system;
        }

        public static Planet FindPlanet(string name)
        {
            string selectStartSystemCommand = $"SELECT * FROM planets WHERE name = '{name}'";
            using var cmd = new SQLiteCommand(selectStartSystemCommand, StarDatabaseCode.sqlite_conn);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            Planet planet;
            rdr.Read();
            planet = new Planet(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(3), rdr.GetString(4));

            return planet;
        }

        public static List<StarSystem> FindJumpableSystems(StarSystem startSystem)
        {
            string selectStartSystemCommand = $"SELECT * FROM hyperlanes INNER JOIN starsystems ON hyperlanes.sysid2 = starsystems._id WHERE hyperlanes.sysid1 = {startSystem.ID}";
            using var cmd = new SQLiteCommand(selectStartSystemCommand, StarDatabaseCode.sqlite_conn);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            List<StarSystem> systems = new List<StarSystem>();
            StarSystem sys;

            while (rdr.Read())
            {
                sys = new StarSystem(rdr.GetInt32(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7));
                systems.Add(sys);
            }

            return systems;
        }

        public static List<StarSystem> GetMainCluster()
        {
            List<List<StarSystem>> clusters = GetClusters();

            clusters.OrderByDescending(list => list.Count());

            using (var transaction = StarDatabaseCode.sqlite_conn.BeginTransaction())
            {
                for (int i = 1; i < clusters.Count(); i++)
                {
                    foreach (StarSystem system in clusters[i])
                    {
                        int starID = system.ID;

                        SQLiteCommand removeSystem;
                        removeSystem = sqlite_conn.CreateCommand();
                        removeSystem.CommandText = @$"DELETE FROM starsystems WHERE _id = {system.ID}";
                        removeSystem.ExecuteNonQuery();
                    }                   
                    clusters[i].Clear();
                }

                transaction.Commit();
            }
            Console.WriteLine("There are " + clusters[0].Count() + " systems in this cluster");

            return clusters[0];            
        }

        public static List<List<StarSystem>> GetClusters()
        {
            List<int> allSystems = GetAllSystems().Select(s => s.ID).ToList();
            List<List<StarSystem>> clusters = new List<List<StarSystem>>();
            
            while(allSystems.Count > 0)
            {
                Console.WriteLine("Cluster added to main list");
                clusters.Add(GetCluster(FindSystemByID(allSystems[0])));
                allSystems = allSystems.Where(s => !clusters.Last().Select(boo => boo.ID).Contains(s)).ToList();
                Console.WriteLine("System count: " + allSystems.Count());
            }

            //foreach(List<StarSystem> list in clusters)
            //{
            //    Console.WriteLine(list.Count());
            //}

            return clusters;
        }

        public static List<StarSystem> GetAllSystems()
        {
            string selectStartSystemCommand = $"SELECT * FROM starsystems";
            using var cmd = new SQLiteCommand(selectStartSystemCommand, StarDatabaseCode.sqlite_conn);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            List<StarSystem> systems = new List<StarSystem>();
            StarSystem system;

            while (rdr.Read())
            {
                system = new StarSystem(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4));
                systems.Add(system);
            }

            return systems;
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
    }
}

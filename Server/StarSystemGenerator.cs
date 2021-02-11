using RequestLibrary;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Server
{
    public class StarSystemGenerator
    {
        int starCounter = 00000;

        List<StarSystem> systems = new List<StarSystem>();
        List<Planet> planets = new List<Planet>();
        List<Hyperlane> hyperLanes = new List<Hyperlane>();

        public void GenerateGalaxy()
        {
            Stopwatch watch = Stopwatch.StartNew();

            Console.WriteLine("Started systems...");

            for (int i = 0; i < 10000; i++)
            {
                CreateNewSystem();
            }
            CreateHyperlanes();

            using (var transaction = StarDatabaseCode.sqlite_conn.BeginTransaction())
            {
                Console.WriteLine("Starting scan...");
                foreach (StarSystem system in systems)
                {
                    StarDatabaseCode.InsertSystem(system);
                }
                Console.WriteLine("Systems added.");
                foreach (Planet planet in planets)
                {
                    StarDatabaseCode.InsertPlanet(planet);
                }
                Console.WriteLine("Planets constructed.");
                foreach (Hyperlane hyperlane in hyperLanes)
                {
                    StarDatabaseCode.InsertHyperlanes(hyperlane);
                }
                Console.WriteLine("Hyperlanes discovered.");

                transaction.Commit();
                Console.WriteLine("Transaction made.");
            }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
        }

        public void CreateNewSystem()
        {
            Random rand = new Random();

            int id = starCounter + 1;
            string name = "SUN-" + id;
            int starClass = rand.Next(0, 10);
            int posX = rand.Next(-10000, 10001);
            int posY = rand.Next(-10000, 10001);       

            StarSystem currentSystem = new StarSystem(id, name, starClass, posX, posY);
            systems.Add(currentSystem);
            //Console.WriteLine("System " + name + " added");

            for (int i = 0; i < rand.Next(0, 10); i++)
            {
                CreatePlanet(currentSystem);
            }

            starCounter += 1;
        }

        void CreatePlanet(StarSystem sysID)
        {            
            Random rand = new Random();

            string name;

            name = sysID.name + " - " + (sysID.planets.Count() + 1);
            int planetClass = rand.Next(0, 10);
            string biome = "[INSERT BIOME]";

            Planet currentPlanet = new Planet(sysID, name, planetClass, biome);
            sysID.planets.Add(currentPlanet);

            planets.Add(currentPlanet);
            //Console.WriteLine("Planet " + name + " added");
        }

        public void CreateHyperlanes()
        {
            Console.WriteLine("Started Hyperlanes...");

            List<Hyperlane> existingLanes = new List<Hyperlane>();

            StarSystem sys1;
            StarSystem sys2;

            foreach(StarSystem system in systems)
            {
                foreach(StarSystem system2 in systems)
                {
                    if (system != system2)
                    {
                        double length = Math.Sqrt(Math.Pow(system.posX - system2.posX, 2) + Math.Pow(system.posY - system2.posY, 2));

                        if (length <= 300 && length >= 10)
                        {
                            sys1 = system;
                            sys2 = system2;
                            Hyperlane currentLane = new Hyperlane(sys1, sys2, Math.Round(length,2));                              
                            hyperLanes.Add(currentLane);
                            //Console.WriteLine(sys1.name + " connected to " + sys2.name + " via hyperlink");                          
                        }
                    }
                }
            }            
        }
    }
}

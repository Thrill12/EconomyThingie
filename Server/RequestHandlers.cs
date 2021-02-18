using RequestLibrary;
using RequestLibrary.Alerts;
using RequestLibrary.Form;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Ships;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Server
{
    public static class RequestHandlers
    {
        static Dictionary<string, User> liveUsers = ServerProgram.liveUsers;

        public static Response ReadRequestHandler(ReadRequest arg)
        {
            return Response.From<string>(arg.message);
        }

        public static Response WriteRequestHandler(WriteRequest arg)
        {
            Console.WriteLine(arg.message);

            return Response.Ok();
        }

        public static Response CreateAccountRequestHandler(CreateAccountRequest arg)
        {
            Console.WriteLine("User " + arg.username + " attempted to create account.");

            User currentUser = new User(arg.username, arg.password);
            currentUser.equippedShip = new Cruiser();
            currentUser.equippedShip.ownerID = currentUser.id;
            currentUser.equippedShip.shipType = "Cruiser";
            StarDatabaseCode.InsertShip(currentUser.equippedShip, currentUser);
            StarDatabaseCode.SetProgramIDOfShip(currentUser.equippedShip);

            SQLiteCommand checkUser = StarDatabaseCode.sqlite_conn.CreateCommand();
            checkUser.CommandText = $"SELECT COUNT(*) FROM users WHERE username='{currentUser.username}'";
            SQLiteDataReader r = checkUser.ExecuteReader();
            r.Read();
            long count = (long)r["COUNT(*)"];

            if (count > 1)
            {
                return Response.From<User>(null);
            }
            else
            {
                string selectStartSystemCommand = "SELECT * FROM starsystems WHERE NAME = 'SUN-1'";
                using var cmd = new SQLiteCommand(selectStartSystemCommand, StarDatabaseCode.sqlite_conn);
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    StarSystem startSystem = new StarSystem(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4));
                    currentUser.position = startSystem;
                }

                currentUser.seshID = CreateSessionID(currentUser.username, DateTime.Now);
                liveUsers.Add(currentUser.seshID, currentUser);

                StarDatabaseCode.InsertUser(currentUser);
                RequestListener.alerter.RegisterUser(currentUser);
                return Response.From(currentUser);
            }
        }

        public static Response LoginRequestHandler(LoginRequest arg)
        {
            Console.WriteLine("User " + arg.usernameReq + " attempted to login.");

            User currUser = StarDatabaseCode.GetWholeUser(arg.usernameReq);

            List<User> usersInDB = new List<User>();

            try
            {
                SQLiteCommand checkUser = StarDatabaseCode.sqlite_conn.CreateCommand();
                checkUser.CommandText = $"SELECT COUNT(*) FROM users WHERE username='{currUser.username}' AND password='{currUser.password}'";
                SQLiteDataReader rdr = checkUser.ExecuteReader();
                int count = 0;

                while (rdr.Read())
                {
                    count = rdr.GetInt32(0);
                }
                rdr.Close();

                if (count >= 1)
                {
                    if (!liveUsers.ContainsValue(currUser))
                    {
                        Console.WriteLine(currUser.id);
                        SQLiteCommand checkUserr = StarDatabaseCode.sqlite_conn.CreateCommand();
                        checkUserr.CommandText = $"SELECT s._id, s.name, s.class, s.positionX, s.positionY FROM starsystems AS s, users AS p WHERE p.username='{currUser.username}' AND s._id = p.sysid";
                        rdr = checkUserr.ExecuteReader();

                        while (rdr.Read())
                        {
                            StarSystem savedPos = new StarSystem(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4));
                            currUser.position = savedPos;
                        }
                        rdr.Close();

                        currUser.seshID = CreateSessionID(currUser.username, DateTime.Now);
                        currUser.equippedShip = StarDatabaseCode.GetShipByID(currUser.id);

                        if (!liveUsers.ContainsKey(currUser.seshID))
                        {
                            liveUsers.Add(currUser.seshID, currUser);
                        }
                        else
                        {
                            liveUsers[currUser.seshID] = currUser;
                        }

                        RequestListener.alerter.RegisterUser(currUser);
                        return Response.From(currUser);
                    }
                    else
                    {
                        return Response.From<User>(null);
                    }
                }
                else
                {
                    return Response.From<User>(null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Response.From<User>(null);

        }

        public static Response UpdateClientOnServerRequestHandler(UpdateClientOnServerRequest arg)
        {         
            string command = $"UPDATE users SET galacticcredits={arg.user.galacticCredits} WHERE username='{arg.user.username}'";
            using var cmd = new SQLiteCommand(command, StarDatabaseCode.sqlite_conn);
            cmd.ExecuteNonQuery();
            command = $"UPDATE users SET diplomaticweight={arg.user.diplomaticWeight} WHERE username='{arg.user.username}'";
            using var cmdd = new SQLiteCommand(command, StarDatabaseCode.sqlite_conn);
            cmdd.ExecuteNonQuery();
            command = $"UPDATE users SET sysid={arg.user.positionID} WHERE username='{arg.user.username}'";
            using var cmddd = new SQLiteCommand(command, StarDatabaseCode.sqlite_conn);
            cmddd.ExecuteNonQuery();

            bool x = liveUsers.Keys.First() == arg.user.seshID;

            ServerProgram.liveUsers[arg.user.seshID] = arg.user;

            Console.WriteLine(arg.user.username + "'s data has been updated - " + DateTime.Now.ToString("G"));


            foreach(User user in liveUsers.Values)
            {
                Console.WriteLine(user.seshID + " : " + user.username);
            }

            Console.WriteLine(liveUsers.Count + " users are live");

            return Response.From(ServerProgram.liveUsers[arg.user.seshID]);
        }

        public static Response FindJumpableSystemsRequestHandler(FindJumpableSystemsRequest arg)
        {
            StarSystemListWrapper wrapper = new StarSystemListWrapper(StarDatabaseCode.FindJumpableSystems(arg.startSystem));

            return Response.From(wrapper);
        }

        public static Response FindLocalPlayersRequestHandler(FindLocalPlayersRequest arg)
        {
            //LocalPlayersListWrapper wrapper = new LocalPlayersListWrapper(StarDatabaseCode.GetUsersInSystem(StarDatabaseCode.FindSystemByID(arg.startSystem.ID)));

            List<User> nearbyUsers = ServerProgram.liveUsers.Values.Where(x => x.position.ID == arg.startSystem.ID).ToList();

            LocalPlayersListWrapper wrapper = new LocalPlayersListWrapper(nearbyUsers);

            return Response.From(wrapper);
        }

        public static Response SendChatRequestHandler(SendChatRequest arg)
        {
            List<User> usersInSystem = StarDatabaseCode.GetUsersInSystem(StarDatabaseCode.FindSystemByID(arg.sysID));

            foreach(User u in usersInSystem)
            {
                if(arg.textToSend.Trim() != "")
                {
                    RequestListener.alerter.SendAlerts(new ChatAlert(arg.name + ": " + arg.textToSend), u);
                }                
            }            

            return Response.Ok();
        }

        public static string CreateSessionID(string name, DateTime time)
        {
            byte[] data = Encoding.ASCII.GetBytes(name + time.ToString());

            using (SHA256 sha256 = SHA256.Create())
            {
                return Encoding.ASCII.GetString(sha256.ComputeHash(data));
            }
        }
    }
}

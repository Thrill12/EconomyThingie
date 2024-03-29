﻿using RequestLibrary;
using RequestLibrary.Alerts;
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

        //public static Response CreateAccountRequestHandler(CreateAccountRequest arg)
        //{
        //    Console.WriteLine("User " + arg.username + " attempted to create account.");

        //    User currentUser = new User(arg.username, arg.password);
        //    currentUser.equippedShip = new Cruiser();
        //    currentUser.equippedShip.ownerID = currentUser.id;
        //    currentUser.equippedShip.shipType = "Cruiser";
        //    StarDatabaseCode.InsertShip(currentUser.equippedShip, currentUser);
        //    StarDatabaseCode.SetProgramIDOfShip(currentUser.equippedShip);

        public static Response CreateAccountRequestHandler(CreateAccountRequest arg)
        {
            Console.WriteLine("User " + arg.username + " attempted to create account.");

            User currentUser = new User(arg.username, arg.password);

            if(DatabaseFiles.DatabaseHandler.db.Query<User>($"SELECT * FROM users WHERE username='{arg.username}'").ToList().Count > 0)
            {
                return Response.From<User>(null);
            }
            else
            {
                currentUser.id = DatabaseFiles.DatabaseHandler.GetNumOfUsers() + 1;

                var startSystem = DatabaseFiles.DatabaseHandler.db.Query<StarSystem>("SELECT * FROM starsystems WHERE ID=1").ToList()[0];
                currentUser.position = startSystem;
                currentUser.positionID = startSystem.ID;
                currentUser.equippedShip = new Cruiser();
                currentUser.equippedShip.shipType = "Cruiser";
                currentUser.equippedShip.ownerID = currentUser.id;
                currentUser.equippedShip.id = DatabaseFiles.DatabaseHandler.db.Query<BaseShip>("SELECT * FROM ships").ToList().Count() + 1;
                currentUser.equippedShipID = currentUser.equippedShip.id;

                BaseShip shipToReplace = new BaseShip(currentUser.equippedShip);
                shipToReplace.ownerID = currentUser.id;
                //shipToReplace.shipType = "Cruiser";

                DatabaseFiles.DatabaseHandler.InsertShip(shipToReplace);
                currentUser.seshID = CreateSessionID(currentUser.username + DateTime.Today.ToString(), DateTime.Now);
                liveUsers.Add(currentUser.seshID, currentUser);
                DatabaseFiles.DatabaseHandler.InsertUser(currentUser);
                RequestListener.alerter.RegisterUser(currentUser);
                return Response.From(currentUser);
            }
        }

        //public static Response LoginRequestHandler(LoginRequest arg)
        //{
        //    Console.WriteLine("User " + arg.usernameReq + " attempted to login.");

        //    User currUser = StarDatabaseCode.GetWholeUser(arg.usernameReq);

        //    List<User> usersInDB = new List<User>();

        //    try
        //    {
        //        SQLiteCommand checkUser = StarDatabaseCode.sqlite_conn.CreateCommand();
        //        checkUser.CommandText = $"SELECT COUNT(*) FROM users WHERE username='{currUser.username}' AND password='{currUser.password}'";
        //        SQLiteDataReader rdr = checkUser.ExecuteReader();
        //        int count = 0;

        //        while (rdr.Read())
        //        {
        //            count = rdr.GetInt32(0);
        //        }
        //        rdr.Close();

        //        if (count >= 1)
        //        {
        //            if (!liveUsers.ContainsValue(currUser))
        //            {
        //                Console.WriteLine(currUser.id);
        //                SQLiteCommand checkUserr = StarDatabaseCode.sqlite_conn.CreateCommand();
        //                checkUserr.CommandText = $"SELECT s._id, s.name, s.class, s.positionX, s.positionY FROM starsystems AS s, users AS p WHERE p.username='{currUser.username}' AND s._id = p.sysid";
        //                rdr = checkUserr.ExecuteReader();

        //                while (rdr.Read())
        //                {
        //                    StarSystem savedPos = new StarSystem(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4));
        //                    currUser.position = savedPos;
        //                }
        //                rdr.Close();

        //                currUser.seshID = CreateSessionID(currUser.username, DateTime.Now);
        //                currUser.equippedShip = StarDatabaseCode.GetShipByID(currUser.id);

        //                if (!liveUsers.ContainsKey(currUser.seshID))
        //                {
        //                    liveUsers.Add(currUser.seshID, currUser);
        //                }
        //                else
        //                {
        //                    liveUsers[currUser.seshID] = currUser;
        //                }

        //                RequestListener.alerter.RegisterUser(currUser);
        //                return Response.From(currUser);
        //            }
        //            else
        //            {
        //                return Response.From<User>(null);
        //            }
        //        }
        //        else
        //        {
        //            return Response.From<User>(null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return Response.From<User>(null);

        //}

        public static Response LoginRequestHandler(LoginRequest arg)
        {
            Console.WriteLine("User " + arg.usernameReq + " attempted to login.");
            User currUser = DatabaseFiles.DatabaseHandler.GetWholeUser(arg.usernameReq);

            try
            {
                int count = DatabaseFiles.DatabaseHandler.db.Query<User>($"SELECT COUNT(*) FROM users WHERE username='{currUser.username}' AND password='{currUser.password}'").ToList().Count();

                if (count >= 1)
                {
                    if (!liveUsers.ContainsValue(currUser))
                    {
                        Console.WriteLine(currUser.username + " logging in...");
                        currUser.seshID = CreateSessionID(currUser.username, DateTime.Now);
                        currUser.position = DatabaseFiles.DatabaseHandler.db.Query<StarSystem>($"SELECT * FROM starsystems WHERE id={currUser.positionID}").ToList()[0];
                        currUser.equippedShip = DatabaseFiles.DatabaseHandler.GetShipById(currUser.equippedShipID);
                        currUser.equippedShip = currUser.equippedShip.ChangeType();
                        Console.WriteLine(currUser.username + " logging in in system " + currUser.position.name);

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
            return Response.From(currUser);
        }

        public static Response AddModuleRequestHandler(AddModuleRequest arg)
        {
            arg.owningUser.equippedShip.equippedModules.Add(arg.modToAdd);
            return Response.From<User>(arg.owningUser);
        }

        public static Response UpdateClientOnServerRequestHandler(UpdateClientOnServerRequest arg)
        {         
            //string command = $"UPDATE users SET galacticcredits={arg.user.galacticCredits} WHERE username='{arg.user.username}'";
            //using var cmd = new SQLiteCommand(command, StarDatabaseCode.sqlite_conn);
            //cmd.ExecuteNonQuery();
            DatabaseFiles.DatabaseHandler.db.Query<User>($"UPDATE users SET galacticcredits={arg.user.galacticCredits} WHERE username='{arg.user.username}'");
            //command = $"UPDATE users SET diplomaticweight={arg.user.diplomaticWeight} WHERE username='{arg.user.username}'";
            //using var cmdd = new SQLiteCommand(command, StarDatabaseCode.sqlite_conn);
            //cmdd.ExecuteNonQuery();
            DatabaseFiles.DatabaseHandler.db.Query<User>($"UPDATE users SET diplomaticweight={arg.user.diplomaticWeight} WHERE username='{arg.user.username}'");
            //command = $"UPDATE users SET sysid={arg.user.positionID} WHERE username='{arg.user.username}'";
            //using var cmddd = new SQLiteCommand(command, StarDatabaseCode.sqlite_conn);
            //cmddd.ExecuteNonQuery();
            DatabaseFiles.DatabaseHandler.db.Query<User>($"UPDATE users SET positionID={arg.user.positionID} WHERE username='{arg.user.username}'");

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
            StarSystemListWrapper wrapper = new StarSystemListWrapper(DatabaseFiles.DatabaseHandler.FindJumpableSystems(arg.startSystem));

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

        public static Response UserDisconnectRequestHandler(UserDisconnectRequest arg)
        {
            ServerProgram.RemoveFromLive(arg.userDisconnected);

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

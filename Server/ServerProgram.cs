using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Data.SQLite;
using System.Text;
using RequestLibrary;
using System.IO;
using RequestLibrary.Form;
using Server.DatabaseFiles;

namespace Server
{
    public class ServerProgram
    {
        public static Dictionary<string, User> liveUsers = new Dictionary<string, User>();

        static void Main(string[] args)
        {        
            Console.WriteLine("Generate new galaxy?");
            string ans = Console.ReadLine();            

            if (ans == "y")
            {
                if (File.Exists("StarsDB.db"))
                {
                    File.Delete("StarsDB.db");
                }                

                StarDatabaseCode.StartDB();
                StarDatabaseCode.RefreshCurrentGalaxy();

                StarSystemGenerator gen = new StarSystemGenerator();
                gen.GenerateGalaxy();

                StarDatabaseCode.GetMainCluster();
            }
            else
            {
                StarDatabaseCode.StartDB();
            }
            
            RequestListener listener = new RequestListener(57253);

            #region Request Registering

            listener.RegisterRequest<LoginRequest>(RequestHandlers.LoginRequestHandler);
            listener.RegisterRequest<WriteRequest>(RequestHandlers.WriteRequestHandler);
            listener.RegisterRequest<ReadRequest>(RequestHandlers.ReadRequestHandler);
            listener.RegisterRequest<CreateAccountRequest>(RequestHandlers.CreateAccountRequestHandler);
            listener.RegisterRequest<UpdateClientOnServerRequest>(RequestHandlers.UpdateClientOnServerRequestHandler);
            listener.RegisterRequest<FindJumpableSystemsRequest>(RequestHandlers.FindJumpableSystemsRequestHandler);
            listener.RegisterRequest<FindLocalPlayersRequest>(RequestHandlers.FindLocalPlayersRequestHandler);
            listener.RegisterRequest<SendChatRequest>(RequestHandlers.SendChatRequestHandler);

            #endregion

            Console.WriteLine("Started Listening");
            listener.StartListening();        
        }   
        
        public void RemoveFromLive(User user)
        {
            liveUsers.Remove(user.seshID);
        }
    }
}

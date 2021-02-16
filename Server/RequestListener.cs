using Newtonsoft.Json;
using RequestLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class RequestListener
    {

        private Dictionary<Type, Func<object, Response>> registeredRequests = new Dictionary<Type, Func<object, Response>>();
        private TcpListener tcpListener;

        private readonly Dictionary<Thread, User> users = new Dictionary<Thread, User>();
        private readonly List<AlertBroadcast> alerts = new List<AlertBroadcast>();

        public static RequestListener alerter;

        public RequestListener(ushort port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            alerter = this;
        }

        public void RegisterUser(User user)
        {
            lock (users)
            {
                //TODO crashes if same thread logouts and logins in again
                users.Add(Thread.CurrentThread, user);
            }
        }

        public void SendAlerts<T>(T alert, params User[] receivers) where T : Alert
        {
            List<User> activeUsers = users.Values.Intersect(receivers).ToList();
            lock (alerts)
            {
                alerts.Add(new AlertBroadcast(activeUsers, alert));
            }
        }

        public void RegisterRequest<T>(Func<T, Response> callback)
        {
            string typeName = typeof(T).FullName;
            registeredRequests.Add(typeof(T), CallbackWrapper(callback));
        }

        public void StartListening()
        {
            tcpListener.Start();
            Console.WriteLine("Server is listening...");

            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();

                ThreadPool.QueueUserWorkItem(HandleConnection, client);
            }
        }

        private void HandleConnection(object obj)
        {
            TcpClient client = obj as TcpClient;

            Stopwatch watch = Stopwatch.StartNew();
            long timeout = 1000 * 60 * 10; // 10 minutes

            using (NetworkStream stream = client.GetStream())
            {
                while (watch.ElapsedMilliseconds <= timeout && client.Connected)
                {
                    if (stream.DataAvailable)
                    {
                        HandleRequest(stream);

                        watch.Restart();
                    }

                    HandleAlerts(stream);
                }

                stream.Flush();
            }

            client.Close();

            //Register user logout
            if (users.ContainsKey(Thread.CurrentThread))
            {
                User user = users[Thread.CurrentThread];
                lock (users)
                {
                    ServerProgram.liveUsers.Remove(user.seshID);
                    users.Remove(Thread.CurrentThread);
                }
            }
        }

        private void HandleRequest(NetworkStream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII, leaveOpen: true))
            {
                string[] request = reader.ReadString().Split("\n");
                string type = request[0];
                string json = request[1];

                Response response = CallRequestHandler(type, json);

                if (response.type != null)
                {
                    SendResponse(response, stream);
                }              
            }
        }

        private void HandleAlerts(NetworkStream stream)
        {
            User user = null;
            lock (users)
            {
                if (users.ContainsKey(Thread.CurrentThread))
                    user = users[Thread.CurrentThread];
            }

            if (user != null)
            {
                bool sentAlerts = false;

                lock (alerts)
                {
                    for (int i = 0; i < alerts.Count; i++)
                    {
                        AlertBroadcast alertBroadcast = alerts[i];
                        if (alertBroadcast == null)
                            continue;

                        bool shouldSend = false;
                        lock (alertBroadcast)
                        {
                            shouldSend = alertBroadcast.Receivers.Select(r => r.username).Contains(user.username);
                        }

                        if (shouldSend)
                        {
                            SendResponse(Response.From(alertBroadcast.Content), stream);
                            Console.WriteLine("Sent an alert of type " + alertBroadcast.Content.GetType());
                            sentAlerts = true;
                        }
                    }
                }               

                //Cleanup
                if (sentAlerts)
                {
                    lock (alerts)
                    {
                        for (int i = alerts.Count - 1; i >= 0; i--)
                        {
                            alerts[i].Receivers.RemoveAll(r => r.username == user.username);
                            if (alerts[i].Receivers.Count == 0)
                                alerts.RemoveAt(i);
                        }
                    }
                }
            }
        }

        private void SendResponse(Response response, Stream stream)
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII, leaveOpen: true))
            {
                writer.Write(response.DataString);
            }
        }

        private Response CallRequestHandler(string strType, string json)
        {
            Type type = Type.GetType(strType);
            object data = JsonConvert.DeserializeObject(json, type);

            return registeredRequests[type].Invoke(data);
        }

        //Horrible work around, we lose type safety doing this
        private Func<object, Response> CallbackWrapper<T>(Func<T, Response> callback)
        {
            return (o) =>
            {
                if (o is T data)
                    return callback(data);
                else
                    throw new Exception("RequestListener failed: Failed to cast into " + typeof(T));
            };
        }

        public class AlertBroadcast
        {
            public List<User> Receivers { get; set; }
            public object Content { get; set; }

            public AlertBroadcast(List<User> receivers, Alert content)
            {
                Receivers = receivers;
                Content = content;
            }
        }
    }
}

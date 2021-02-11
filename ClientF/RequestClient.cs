using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server;
using System.Threading;
using RequestLibrary;
using System.Windows.Threading;
using System.Diagnostics;
using RequestLibrary.Accounts;

namespace ClientF
{
    public class RequestClient : IDisposable
    {

        private TcpClient client;
        private IPAddress ip;
        private ushort port;

        public bool IsConnected { get { return client != null && client.Connected; } }

        private readonly Thread receivingThread;
        private readonly Queue<object> responses = new Queue<object>();
        private readonly Dictionary<Type, Dictionary<object, List<Action<object>>>> callbacks = new Dictionary<Type, Dictionary<object, List<Action<object>>>>();
        private readonly Dispatcher dispatcher;

        public RequestClient(IPAddress ip, ushort port)
        {
            this.ip = ip;
            this.port = port;
            dispatcher = Dispatcher.CurrentDispatcher;

            receivingThread = new Thread(ReceivePackets)
            {
                IsBackground = true
            };
            receivingThread.Start();
        }

        public void EnforceConnection()
        {
            if (!IsConnected)
            {
                if (client != null)
                    client.Dispose();

                client = new TcpClient();
                client.Connect(new IPEndPoint(ip, port));
            }
        }

        public void SendRequest(object request)
        {
            EnforceConnection();

            using (BinaryWriter writer = new BinaryWriter(client.GetStream(), Encoding.ASCII, leaveOpen: true))
            {
                writer.Write($"{request.GetType().AssemblyQualifiedName}\n{JsonConvert.SerializeObject(request)}");
            }
        }

        public void SendRequest(AuthenticatedRequest request, User user)
        {
            request.name = user.username;
            request.seshID = user.seshID;
            request.sysID = user.positionID;
            SendRequest(request);
            
            
        }

        public T SendRequest<T>(AuthenticatedRequest request, User user) where T : class
        {
            request.name = user.username;
            request.seshID = user.seshID;
            request.sysID = user.positionID;
            SendRequest(request);
            Stopwatch timer = Stopwatch.StartNew();

            while (responses.Count == 0)
            {
                if (timer.ElapsedMilliseconds > 3000)
                {
                    return SendRequest<T>(request);
                }
            }
            lock (responses)
            {
                return (T)responses.Dequeue();
            }
        }

        public T SendRequest<T>(object request) where T:class
        {
            SendRequest(request);
            Stopwatch timer = Stopwatch.StartNew();

            while (responses.Count == 0)
            {
                if(timer.ElapsedMilliseconds > 3000)
                {
                    return SendRequest<T>(request);
                }
            }
            lock (responses)
            {
                return (T)responses.Dequeue();
            }       
        }

        private void ReceivePackets(object obj)
        {
            while (true)
            {
                if (IsConnected && client.Available > 0)
                {
                    using (BinaryReader reader = new BinaryReader(client.GetStream(), Encoding.ASCII, leaveOpen: true))
                    {
                        string[] response = reader.ReadString().Split('\n');
                        Type type = Type.GetType(response[0]);
                        object data = JsonConvert.DeserializeObject(response[1], type);

                        if (type.IsSubclassOf(typeof(Alert)) && callbacks.ContainsKey(type))
                        {
                            lock (callbacks)
                            {
                                foreach (var x in callbacks[type].Values)
                                {
                                    foreach (var callback in x)
                                        dispatcher.InvokeAsync(() => callback(data));
                                }
                            }
                        }
                        else
                        {
                            lock (responses)
                            {
                                responses.Enqueue(data);
                            }
                        }
                    }
                }
                else
                {
                    EnforceConnection();
                }
            }
        }

        public void SubscribeTo<T>(object sender, Action<T> callback) where T : Alert
        {
            Action<object> wrappedCallback = (o) => callback((T)o);

            lock (callbacks)
            {
                if (!callbacks.ContainsKey(typeof(T)))
                    callbacks.Add(typeof(T), new Dictionary<object, List<Action<object>>>());

                if (!callbacks[typeof(T)].ContainsKey(sender))
                    callbacks[typeof(T)].Add(sender, new List<Action<object>>());

                callbacks[typeof(T)][sender].Add(wrappedCallback);
            }
        }

        public void Unsubscribe(object sender)
        {
            lock (callbacks)
            {
                foreach (var type in callbacks.Values)
                {
                    if (type.ContainsKey(sender))
                        type.Remove(sender);
                }
            }
        }

        public void Dispose()
        {
            receivingThread.Abort();
            client.Close();
            client.Dispose();
        }
    }
}

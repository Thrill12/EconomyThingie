using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class RequestListener
    {

        private Dictionary<Type, Func<object, Response>> registeredRequests = new Dictionary<Type, Func<object, Response>>();
        private TcpListener tcpListener;

        public RequestListener(ushort port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
        }

        public void RegisterRequest<T>(Func<T, Response> callback)
        {
            string typeName = typeof(T).FullName;
            registeredRequests.Add(typeof(T), CallbackWrapper(callback));
        }

        public void StartListening()
        {
            tcpListener.Start();

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
                while (watch.ElapsedMilliseconds <= timeout)
                {
                    if (stream.DataAvailable)
                    {
                        HandleRequest(stream);

                        watch.Restart();
                    }
                }

                stream.Flush();
            }

            client.Close();
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

    }
}

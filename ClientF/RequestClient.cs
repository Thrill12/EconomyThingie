using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClientF
{
    class RequestClient
    {

        private TcpClient client;
        private IPAddress ip;
        private ushort port;

        public bool IsConnected { get { return client != null && client.Connected; } }

        public RequestClient(IPAddress ip, ushort port)
        {
            this.ip = ip;
            this.port = port;
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

        public T SendRequest<T>(object request)
        {
            SendRequest(request);

            using (BinaryReader reader = new BinaryReader(client.GetStream(), Encoding.ASCII, leaveOpen: true))
            {
                string[] response = reader.ReadString().Split('\n');
                Type type = Type.GetType(response[0]);
                object data = JsonConvert.DeserializeObject(response[1], type);

                return (T)data;
            }

        }

    }
}

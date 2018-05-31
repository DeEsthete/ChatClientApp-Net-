using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChatClientApp
{
    public class ServerConnect
    {
        public bool ServerIsConnect { get; set; }
        private string userName;
        private const int SERVER_PORT = 3535;
        private const string SERVER_IP = "127.0.0.1";
        private Socket remoteServerSocket;
        private IPEndPoint endPoint;

        public void CreateConnect(string user)
        {
            userName = user;
            remoteServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            endPoint = new IPEndPoint(IPAddress.Parse(SERVER_IP), SERVER_PORT);

            try
            {
                Console.WriteLine("Соединяемся с сервером...");
                remoteServerSocket.Connect(endPoint);
                Console.WriteLine("Соединено..");
                ServerIsConnect = true;
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                string serialized = JsonConvert.SerializeObject(new UserMessage { UserName = userName, Message = message });
                remoteServerSocket.Send(Encoding.Default.GetBytes(serialized));
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CloseConnect()
        {
            SendMessage("exit");
            ServerIsConnect = false;
            remoteServerSocket.Close();
            Console.WriteLine("Сеанс завершен...");
        }

        public async void StartAcceptMessage()
        {
            await AcceptMessage();
        }

        public Task AcceptMessage()
        {
            return Task.Run(() =>
            {
                while (ServerIsConnect)
                {
                    int bytes;
                    byte[] buffer = new byte[1024];
                    StringBuilder stringBuilder = new StringBuilder();

                    do
                    {
                        bytes = remoteServerSocket.Receive(buffer);
                        stringBuilder.Append(Encoding.Default.GetString(buffer, 0, bytes));
                    }
                    while (remoteServerSocket.Available > 0);

                    Console.WriteLine(stringBuilder.ToString());
                }
            });
        }
    }
}

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
        private string userName;
        private const int SERVER_PORT = 3535;
        Socket remoteServerSocket;
        IPEndPoint endPoint;

        public void CreateConnect()
        {
            remoteServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), SERVER_PORT);

            try
            {
                Console.WriteLine("Соединяемся с сервером...");
                remoteServerSocket.Connect(endPoint);
                Console.WriteLine("Соединено..");
            }
            catch(SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                string serialized = JsonConvert.SerializeObject(new UserMessage { UserName = userName, Message = message});
                remoteServerSocket.Send(Encoding.Default.GetBytes(serialized));
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        ~ServerConnect()
        {
            remoteServerSocket.Close();
            Console.WriteLine("Сеанс завершен...");
        }
    }
}

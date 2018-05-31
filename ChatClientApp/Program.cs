using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string userName = Console.ReadLine();
            ServerConnect server = new ServerConnect();
            Console.WriteLine("-----------------------");
            Console.WriteLine("Для того что бы выйти из чата, введите слово 'exit'");
            Console.WriteLine("-----------------------");
            server.CreateConnect(userName);
            server.SendMessage("init");
            server.StartAcceptMessage();
            while (true)
            {
                //server.CreateConnect(userName);
                string message = Console.ReadLine();
                if (message == "exit")
                {
                    server.CloseConnect();
                    break;
                }
                else
                {
                    server.SendMessage(message);
                }
            }
        }
    }
}

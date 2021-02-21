using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_App
{
    class Program
    {
        static void Main(string[] args) {
            if (args.Length > 0) {
                switch (args[0]) {
                    case "-server":
                        if (args.Length >= 2) {
                            int port = 1;
                            bool validPort = int.TryParse(args[1], out port);

                            if (!validPort)
                                throw new Exception("Argument two should be a port number; the provided value is not a number");

                            Server theServer = new Server(port);
                            theServer.Run();
                        } else
                            throw new Exception("Please enter the second argument and should be a port number");
                        break;
                    case "-client":
                        if (args.Length >= 3) {
                            IPAddress theIp;
                            bool validIp = IPAddress.TryParse(args[1], out theIp);
                            if (!validIp)
                                throw new Exception("Argument two should be a port number; the provided IP is invalid");

                            int port = 1;
                            bool validPort = int.TryParse(args[2], out port);
                            if (!validPort)
                                throw new Exception("Argument three should be a port number; the provided value is not a number");

                            Client theClient = new Client(theIp, port);
                            theClient.Run();
                        } else
                            throw new Exception("Please three argument; second one being the IP and the third is the port number");
                        break;
                }
            } else {
                Console.WriteLine("Please provide arguments to this application either type -server or -client");
            }
        }
    }
}
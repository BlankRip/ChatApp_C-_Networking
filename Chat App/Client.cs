using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_App
{
    public class Client
    {
        IPAddress theIp;
        int port;

        public Client(IPAddress theIp, int port) {
            this.theIp = theIp;
            this.port = port;
        }

        public void Run()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Connection...");
            socket.Connect(new IPEndPoint(theIp, port));
            Console.WriteLine("Connected to Server!!");
            socket.Blocking = false;

            string strToSend = null;
            Console.Write("Enter your Chat Tag:");
            string theTag = Console.ReadLine();
            theTag += ": ";

            Console.WriteLine("Type Now, It will work: ");
            while (true) {
                try {
                    if (Console.KeyAvailable) {
                        ConsoleKeyInfo key = Console.ReadKey();

                        if (key.Key == ConsoleKey.Enter) {
                            strToSend = theTag + strToSend;
                            socket.Send(ASCIIEncoding.ASCII.GetBytes(strToSend));
                            strToSend = null;
                            Console.WriteLine();
                        } else {
                            strToSend += key.KeyChar;
                        }
                    }

                    Byte[] recievedBuffer = new Byte[1024];
                    int bytesRecieved = socket.Receive(recievedBuffer);
                    string strToPrint = ASCIIEncoding.ASCII.GetString(recievedBuffer);
                    strToPrint = strToPrint.Substring(0, bytesRecieved);
                    Console.WriteLine(strToPrint);
                } catch (SocketException ex) {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(ex);
                }
            }
        }
    }
}

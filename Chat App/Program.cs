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
            if(args.Length > 0) {
                if(args[0] == "-server") {
                    Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    listeningSocket.Bind(new IPEndPoint(IPAddress.Any, 420));
                    Console.WriteLine("Waiting for connection...");
                    listeningSocket.Listen(10);

                    Socket cliantSocket = listeningSocket.Accept();

                    string strToSend = Console.ReadLine();
                    cliantSocket.Send(ASCIIEncoding.ASCII.GetBytes("Hello Mustafa 1!\n" + strToSend));

                    Byte[] receivedBuffer = new Byte[1024];
                    cliantSocket.Receive(receivedBuffer);
                    Console.WriteLine(ASCIIEncoding.ASCII.GetString(receivedBuffer));

                } else if(args[0] == "-client") {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    Console.WriteLine("Connection...");
                    socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 420));
                    Console.WriteLine("Connected to Server!!");

                    Byte[] recievedBuffer = new Byte[1024];
                    socket.Receive(recievedBuffer);

                    Console.WriteLine(ASCIIEncoding.ASCII.GetString(recievedBuffer));

                    string strToSend = Console.ReadLine();
                    socket.Send(ASCIIEncoding.ASCII.GetBytes("Hello server 6969!\n" + strToSend));
                }
            } else {
                Console.WriteLine("Please provide arguments to this application either type -server or -client");
            }
        }
    }
}
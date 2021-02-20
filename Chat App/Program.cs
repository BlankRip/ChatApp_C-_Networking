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
            List<Socket> clientSockets = new List<Socket>();
            if (args.Length > 0) {
                if(args[0] == "-server") {
                    Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    listeningSocket.Blocking = false;

                    listeningSocket.Bind(new IPEndPoint(IPAddress.Any, 420));
                    Console.WriteLine("Waiting for connection...");
                    listeningSocket.Listen(10);
                    while(true) {
                        try {
                            clientSockets.Add(listeningSocket.Accept());
                        } catch (SocketException ex) {
                            if(ex.SocketErrorCode != SocketError.WouldBlock)
                                Console.WriteLine(ex);
                        }

                        for (int i = 0; i < clientSockets.Count; i++)
                        {
                            try {
                                Byte[] receivedBuffer = new Byte[1024];
                                clientSockets[i].Receive(receivedBuffer);
                                for (int j = 0; j < clientSockets.Count; j++) {
                                    if (i != j)
                                        clientSockets[j].Send(receivedBuffer);
                                }
                            } catch (SocketException ex) {
                                if (ex.SocketErrorCode == SocketError.ConnectionAborted || ex.SocketErrorCode == SocketError.ConnectionReset) {
                                    clientSockets[i].Close();
                                    clientSockets.Remove(clientSockets[i]);
                                }
                                if (ex.SocketErrorCode != SocketError.WouldBlock && ex.SocketErrorCode != SocketError.ConnectionAborted
                                    && ex.SocketErrorCode != SocketError.ConnectionReset)
                                    Console.WriteLine(ex);
                            }
                        }
                    }
                } else if(args[0] == "-client") {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    Console.WriteLine("Connection...");
                    socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 420));
                    Console.WriteLine("Connected to Server!!");
                    socket.Blocking = false;

                    while(true) {
                        try {
                            Console.Write("Type Now, Here: ");
                            string strToSend = Console.ReadLine();
                            socket.Send(ASCIIEncoding.ASCII.GetBytes(strToSend));

                            Byte[] recievedBuffer = new Byte[1024];
                            socket.Receive(recievedBuffer);
                            Console.WriteLine(ASCIIEncoding.ASCII.GetString(recievedBuffer));
                        }
                        catch (SocketException ex) {
                            if (ex.SocketErrorCode != SocketError.WouldBlock)
                                Console.WriteLine(ex);
                        }
                    }
                }
            } else {
                Console.WriteLine("Please provide arguments to this application either type -server or -client");
            }
        }
    }
}
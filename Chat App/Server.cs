using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat_App
{
    public class Server
    {
        Dictionary<Socket, string> dic = new Dictionary<Socket, string>();
        List<Socket> clientSockets = new List<Socket>();
        int port;

        public Server(int port) {
            this.port = port;
            clientSockets = new List<Socket>();
        }

        public void Run() {
            Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listeningSocket.Blocking = false;

            listeningSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            Console.WriteLine("Waiting for connection...");
            listeningSocket.Listen(10);
            while (true) {
                try {
                    clientSockets.Add(listeningSocket.Accept());
                } catch (SocketException ex) {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(ex);
                }

                for (int i = 0; i < clientSockets.Count; i++) {
                    try {
                        Byte[] receivedBuffer = new Byte[1024];
                        int bytesRecieved = clientSockets[i].Receive(receivedBuffer);

                        if(!dic.ContainsKey(clientSockets[i])) {
                            Packet recieved = (Packet)Util.ByteArrayToObject(receivedBuffer);
                            dic.Add(clientSockets[i], recieved.tag);
                            Console.WriteLine($"{recieved.tag} just Connected");
                        }

                        for (int j = 0; j < clientSockets.Count; j++) {
                            if (i != j)
                                clientSockets[j].Send(receivedBuffer, bytesRecieved, SocketFlags.None);
                        }
                    } catch (SocketException ex) {
                        if (ex.SocketErrorCode == SocketError.ConnectionAborted || ex.SocketErrorCode == SocketError.ConnectionReset) {
                            Socket disconnectedSocket = clientSockets[i];
                            string tag = dic[disconnectedSocket];
                            Console.WriteLine($"{tag} just Disconnected");

                            clientSockets[i].Close();
                            dic.Remove(disconnectedSocket);
                            clientSockets.Remove(clientSockets[i]);

                            Packet dcMessage = new Packet();
                            dcMessage.message = $"Server: {tag} Disconnected.";
                            dcMessage.txtColor = ConsoleColor.Red;

                            for (int j = 0; j < clientSockets.Count; j++)
                                clientSockets[j].Send(Util.ObjectToByteArray(dcMessage));
                        }
                        if (ex.SocketErrorCode != SocketError.WouldBlock && ex.SocketErrorCode != SocketError.ConnectionAborted
                            && ex.SocketErrorCode != SocketError.ConnectionReset)
                            Console.WriteLine(ex);
                    }
                }
            }
        }
    }
}

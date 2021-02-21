using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_App
{
    public class Server
    {
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

                        for (int j = 0; j < clientSockets.Count; j++) {
                            if (i != j)
                                clientSockets[j].Send(receivedBuffer, bytesRecieved, SocketFlags.None);
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
        }
    }
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

            string strToSend = "";
            Console.Write("Enter your Chat Tag:");
            string theTag = Console.ReadLine();
            theTag += ": ";

            Console.WriteLine("Type Now, It will work: ");
            while (true) {
                try {
                    if (Console.KeyAvailable) {
                        ConsoleKeyInfo key = Console.ReadKey();

                        if (key.Key == ConsoleKey.Enter) {
                            if(strToSend.Length > 0) {
                                strToSend = theTag + strToSend;
                                socket.Send(ASCIIEncoding.ASCII.GetBytes(strToSend));
                                Console.Write($"\r{new string(' ', (Console.WindowWidth - 1))}");
                                Console.WriteLine($"\r{strToSend}");
                                strToSend = "";
                            }
                        } else if (key.Key == ConsoleKey.Backspace) {
                            if(strToSend.Length > 0) {
                                strToSend = strToSend.Remove(strToSend.Length - 1, 1);
                                Console.Write($"\r{new string(' ', (Console.WindowWidth - 1))}");
                                Console.Write($"\r{strToSend}");
                            }
                        } else {
                            strToSend += key.KeyChar;
                        }
                    }

                    Byte[] recievedBuffer = new Byte[1024];
                    int bytesRecieved = socket.Receive(recievedBuffer);
                    string strToPrint = ASCIIEncoding.ASCII.GetString(recievedBuffer);
                    strToPrint = strToPrint.Substring(0, bytesRecieved);
                    if (strToSend.Length > 0) {
                        Console.Write($"\r{new string(' ', (Console.WindowWidth - 1))}\r");
                        Console.WriteLine(strToPrint);
                        Console.Write(strToSend);
                    } else
                        Console.WriteLine(strToPrint);
                } catch (SocketException ex) {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(ex);
                }
            }
        }
    }
}

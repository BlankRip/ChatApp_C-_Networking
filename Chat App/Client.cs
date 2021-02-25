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

        Packet packetToSend = new Packet();
        Packet packetToRecieve = new Packet();

        public Client(IPAddress theIp, int port) {
            this.theIp = theIp;
            this.port = port;
        }

        public void Run()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Connection...");
            socket.Connect(new IPEndPoint(theIp, port));
            Console.WriteLine("Connected to Server!!");
            socket.Blocking = false;

            packetToSend.message = "";
            Console.Write("Enter your Chat Tag: ");
            packetToSend.tag = Console.ReadLine();

            Console.WriteLine();
            packetToSend.txtColor = Util.GetColor(Util.GetColorChoice());

            Console.Clear();
            Console.WriteLine("Type Now, It will work: ");
            while (true) {
                try {
                    if (Console.KeyAvailable) {
                        Console.ForegroundColor = packetToSend.txtColor;
                        ConsoleKeyInfo key = Console.ReadKey();

                        if (key.Key == ConsoleKey.Enter) {
                            if(packetToSend.message.Length > 0) {
                                packetToSend.message = $"{packetToSend.tag}: {packetToSend.message}";
                                socket.Send(Util.ObjectToByteArray(packetToSend));
                                Console.Write($"\r{new string(' ', (Console.WindowWidth - 1))}");
                                Console.WriteLine($"\r{packetToSend.message}");
                                packetToSend.message = "";
                            }
                        } else if (key.Key == ConsoleKey.Backspace) {
                            if(packetToSend.message.Length > 0) {
                                packetToSend.message = packetToSend.message.Remove(packetToSend.message.Length - 1, 1);
                                Console.Write($"\r{new string(' ', (Console.WindowWidth - 1))}");
                                Console.Write($"\r{packetToSend.message}");
                            }
                        } else {
                            packetToSend.message += key.KeyChar;
                        }
                    }

                    Byte[] recievedBuffer = new Byte[1024];
                    int bytesRecieved = socket.Receive(recievedBuffer);
                    packetToRecieve = (Packet)Util.ByteArrayToObject(recievedBuffer);
                    Console.ForegroundColor = packetToRecieve.txtColor;
                    if (packetToSend.message.Length > 0) {
                        Console.Write($"\r{new string(' ', (Console.WindowWidth - 1))}\r");
                        Console.WriteLine(packetToRecieve.message);

                        Console.ForegroundColor = packetToSend.txtColor;
                        Console.Write(packetToSend.message);
                    } else
                        Console.WriteLine(packetToRecieve.message);
                } catch (SocketException ex) {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(ex);
                }
            }
        }
    }
}

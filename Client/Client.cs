using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    class Client
    {
        public static string ip { get; private set; } = "127.0.0.1";
        public static int port { get; private set; } = 8000;
        Socket socket;
        IPEndPoint iPEndPoint;
        public Client()
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Start()
        {
            try
            {
                socket.Connect(iPEndPoint);
                Console.WriteLine($"Welcome to server[{ip}].");
                int bytes = 0;
                byte[] data = new byte[256];
                StringBuilder stringBuilder = new StringBuilder();
                while (true)
                {
                    stringBuilder.Clear();
                    bytes = 0;
                    data = new byte[256];

                    do
                    {
                        bytes = socket.Receive(data);
                        stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (socket.Available > 0);
                    Console.WriteLine(stringBuilder);
                    if (stringBuilder.ToString().StartsWith("\nRES")) break;

                    do
                    {
                        Console.WriteLine("Сделайте свой ход(в формате \"x y\":");
                        socket.Send(Encoding.Unicode.GetBytes(Console.ReadLine()));
                        bytes = 0;
                        data = new byte[256];
                        stringBuilder.Clear();
                        do
                        {
                            bytes = socket.Receive(data);
                            stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        } while (socket.Available > 0);
                        if (stringBuilder.ToString().StartsWith("\nRES")) break;
                    } while (stringBuilder.ToString().StartsWith("repeat"));
                    Console.WriteLine(stringBuilder);
                    if (stringBuilder.ToString().StartsWith("\nRES")) break;
                }
                stringBuilder.Clear();
                do
                {
                    bytes = socket.Receive(data);
                    stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (socket.Available > 0);
                Console.WriteLine(stringBuilder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }

    }
}

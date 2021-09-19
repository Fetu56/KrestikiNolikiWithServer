using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Server
    {
        
        public static string ip { get; private set; } = "127.0.0.1";
        public static int port { get; private set; } = 8000;
        Socket socket;
        IPEndPoint iPEndPoint;
        public Server()
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Start()
        {
            Console.WriteLine("Start game server...");
            try
            {
                socket.Bind(iPEndPoint);
                socket.Listen(10);

                while (true)
                {
                    Socket socketClient = socket.Accept();
                    Console.WriteLine("Registrated new user, start game.");
                    StringBuilder StringBuilder = new StringBuilder();
                    Game.KNGame game = new Game.KNGame();
                    int bytes;
                    byte[] data;
                    while (game.GameStatus == Field.Null)
                    {
                        StringBuilder.Clear();
                        bytes = 0;
                        data = new byte[256];

                        Console.WriteLine(game);
                        socketClient.Send(Encoding.Unicode.GetBytes(game.ToString()));

                        do
                        {
                            bytes = socketClient.Receive(data);
                            StringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        } while (socketClient.Available > 0);

                        int resX = -1;
                        int resY = -1;
                        if (StringBuilder.ToString().Length != 3 || StringBuilder.ToString().Split(" ").Length<2 || !int.TryParse(StringBuilder.ToString().Split(" ")[0], out resX) | !int.TryParse(StringBuilder.ToString().Split(" ")[1], out resY) | !game.SetToField(resX, resY))
                        {
                            do
                            {
                                socketClient.Send(Encoding.Unicode.GetBytes("repeat"));
                                StringBuilder.Clear();
                                bytes = 0;
                                data = new byte[256];
                                do
                                {
                                    bytes = socketClient.Receive(data);
                                    StringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                                } while (socketClient.Available > 0);
                            } while (StringBuilder.ToString().Length != 3 | StringBuilder.ToString().Split(" ").Length < 2 || !int.TryParse(StringBuilder.ToString().Split(" ")[0], out resX) || !int.TryParse(StringBuilder.ToString().Split(" ")[1], out resY) || !game.SetToField(resX, resY));
                        }

                        Console.WriteLine(game);
                        socketClient.Send(Encoding.Unicode.GetBytes(game.ToString()));

                        if(game.GameStatus != Field.Null)
                        {
                            break;
                        }

                        Console.WriteLine("Сделайте свой ход(в формате \"x y\":");
                        string inp;
                        resX = -1;
                        resY = -1;
                        while (true)
                        {
                            inp = Console.ReadLine();
                            if (inp.Length == 3 && inp.Split(' ').Length == 2 && int.TryParse(inp.Split(' ')[0], out resX) && int.TryParse(inp.Split(' ')[1], out resY) 
                                && game.SetToField(resX, resY))
                            {
                                break;
                            }
                            Console.WriteLine("Неверный ввод.");
                        }
                        
                    }

                    socketClient.Send(Encoding.Unicode.GetBytes("\nRES: " + (game.GameStatus == Field.UKN ? "DRAW!" : "WINNER:" + (game.GameStatus == Field.Krest ? "KREST(CLIENT)" : "NOLIK(SERVER)") + "!")));
                    Console.WriteLine("\nRES: " + (game.GameStatus == Field.UKN ? "DRAW!" : "WINNER:" + (game.GameStatus == Field.Krest ? "KREST(CLIENT)" : "NOLIK(SERVER)") + "!"));
                    Console.WriteLine(game);
                    socketClient.Send(Encoding.Unicode.GetBytes(game.ToString()));
                    socketClient.Shutdown(SocketShutdown.Both);
                    socketClient.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}

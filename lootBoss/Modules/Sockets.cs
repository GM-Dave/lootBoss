using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Timers;

namespace lootBoss.Modules
{
    class Sockets
    {
        readonly Drop _drop;
        readonly ScreenShot _ss;
        private Timer timer;
        private string focus;
        private bool isrestricted;

        public Sockets()
        {
            _ = StartServerSocket();
            _drop = new Drop();
            _ss = new ScreenShot();
        }
        public async Task StartServerSocket()
        {
            await Task.Run(() => MessageServerAsync());
        }
        public async Task MessageServerAsync()
        {
            timer = new Timer();
            timer.Interval = 15000;

            timer.Elapsed += OnTimedEvent;
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 51515);
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                try
                {
                    byte[] buffer = new byte[1024];
                    stream.Read(buffer, 0, buffer.Length);
                    int recv = 0;
                    foreach (byte b in buffer)
                    {
                        if (b != 0)
                        {
                            recv++;
                        }
                    }
                    string request = Encoding.UTF8.GetString(buffer, 0, recv);
                    IPAddress charip = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                    string[] messageparams = request.Split('|');
                    string header = messageparams[0];
                    string author = messageparams[1];

                    if (header == "handshake")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Handshake: {charip} was connected as {author}");
                        Console.ResetColor();
                    }
                    else
                    {
                        if (isrestricted != true)
                        {
                            isrestricted = true;
                            focus = author;
                            timer.Stop();
                            timer.Start();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("---------> Focus has been set on {0}!", focus);
                            Console.ResetColor();
                        }
                        if (isrestricted == true && focus != author)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Skipping message from {author} while focus is on {focus}");
                            Console.ResetColor();
                        }
                        if (isrestricted == true && focus == author)
                        {
                            switch (header)
                            {
                                case "drop":
                                    Console.WriteLine($"Doing work with LOOT message from {author}");
                                    string message = messageparams[2];
                                    await _drop.PostDropsFromSocketMessage(message);
                                    break;
                                case "screenshot":
                                    Console.WriteLine($"Doing work with SCREENSHOT message from {author}");
                                    string duration = messageparams[2];
                                    string boss = messageparams[3];
                                    string filename = messageparams[4];
                                    string attendees = messageparams[5];
                                    await _ss.PostScreenShotFromSocketMessage(duration, boss, filename, attendees);
                                    break;
                                default:
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Unknown or Invalid socket message parameters from {charip}");
                                    Console.ResetColor();
                                    break;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {e}");
                    Console.ResetColor();
                }
            }
        }
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (isrestricted == true)
            {
                isrestricted = false; //open it back up
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("---------> Focus no longer set on {0}", focus);
                Console.ResetColor();
            }
        }
    }
}

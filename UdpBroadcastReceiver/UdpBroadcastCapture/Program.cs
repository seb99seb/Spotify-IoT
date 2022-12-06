using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SpotifyREST.Models;

namespace UdpBroadcastCapture
{
    class Program
    {
        // https://msdn.microsoft.com/en-us/library/tst0kwb1(v=vs.110).aspx
        // IMPORTANT Windows firewall must be open on UDP port 7000
        // https://www.windowscentral.com/how-open-port-windows-firewall
        // Use the network MGV-xxx to capture from local IoT devices (fake or real)
        private const int Port = 7000;
        //private static readonly IPAddress IpAddress = IPAddress.Parse("192.168.5.137"); 
        // Listen for activity on all network interfaces
        // https://msdn.microsoft.com/en-us/library/system.net.ipaddress.ipv6any.aspx
        static void Main()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
            using (UdpClient socket = new UdpClient(ipEndPoint))
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(0, 0);
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast {0}", socket.Client.LocalEndPoint);
                    byte[] datagramReceived = socket.Receive(ref remoteEndPoint);

                    string message = Encoding.ASCII.GetString(datagramReceived, 0, datagramReceived.Length);
                    Console.WriteLine("Receives {0} bytes from {1} port {2} message {3}", datagramReceived.Length, remoteEndPoint.Address, remoteEndPoint.Port, message);

                    ReceiveStickEvent(message);

                    CreateAsync(message).GetAwaiter().GetResult();
                }
            }
        }

        static async Task RunAsync()
        {
            //foreach (var item in GetAllAsync().Result)
            //{
            //    Console.WriteLine($"{item.Id} {item.Message}");
            //}

            await CreateAsync("TEST");
        }

        private const string URI = "https://localhost:7234/swagger/index.html";

        public static async Task<List<Direction>> GetAllAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(URI);

                if (response.IsSuccessStatusCode)
                {
                    List<Direction> contentList = await response.Content.ReadFromJsonAsync<List<Direction>>();

                    return contentList;
                }

                return new List<Direction>();
            }
        }

        public static async Task CreateAsync(string message)
        {
            using (HttpClient client = new HttpClient())
            {
                JsonContent content = JsonContent.Create(new Direction(1000, message));
                HttpResponseMessage response = await client.PostAsync(URI + "/Direction", content);
            }
        }

        public static void ReceiveStickEvent(string direction)
        {
            switch (direction)
            {
                case "up":
                    Console.WriteLine($"Direction: {direction}, emotion is neutral.");
                    break;
                case "down":
                    Console.WriteLine($"Direction: {direction}, emotion is cleared.");
                    break;
                case "left":
                    Console.WriteLine($"Direction: {direction}, emotion is happy.");
                    break;
                case "right":
                    Console.WriteLine($"Direction: {direction}, emotion is sad.");
                    break;
                case "middle":
                    Console.WriteLine($"Direction: {direction}, emotion is cleared.");
                    break;
                default:
                    Console.WriteLine($"Message does not contain a valid direction.");
                    break;
            }
        }
    }
}
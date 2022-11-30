using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
            //Receives messages from the RaspberryPi
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
                }
            }
        }

        public static void ReceiveStickEvent(string message)
        {
            //Split message into 2 parts
            string[] result = message.Split(' ');

            //Check if message has the correct length
            if (result.Length != 2)
            {
                throw new Exception($"Message must contain 2 strings. Message length: {result.Length}");
            }

            //Check if the second part of the message contains "pressed", not "released" or "held"
            if (result[1] == "pressed")
            {
                //Check if the message contains a valid direction and execute code
                string direction = result[0];
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
}
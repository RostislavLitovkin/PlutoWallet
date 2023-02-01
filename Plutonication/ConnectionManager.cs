using System.Net;
using System.Net.Sockets;

namespace Plutonication
{
    // All the code in this file is included in all platforms.
    public class ConnectionManager
    {
        public static Socket Connect(IPAddress address, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(address, port);

            try
            {
                socket.Connect(endpoint);
                return socket;
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Failed to connect: " + ex.Message);
                return null;
            }
        }
        public static Socket Listen(int port)
        {
            IPAddress ipLocal = IPAddress.Any;
            string ipStr = ipLocal.ToString();
            IPEndPoint localEndPoint = new IPEndPoint(ipLocal, port);

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen();

            return listener;
        }
    }
}

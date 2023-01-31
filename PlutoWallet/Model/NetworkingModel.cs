using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace PlutoWallet.Model
{
    class NetworkingModel
    {
        static readonly HttpClient client = new HttpClient();
        public static string RequestSample()
        {
            using (var ws = new WebSocket("ws://192.168.18.133/Echo"))
            {
                ws.Connect();
                Console.WriteLine("Connected");

                ws.Send("Cus brasko");
                //Console.ReadKey();w
                return "connected succesfully";
            }
        }
    }
}

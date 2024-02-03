using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.WebSockets;

namespace PlutoWallet.Model
{
	public class WebSocketModel
	{
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<string> GetFastestWebSocketAsync(string[] urls)
        {
            TimeSpan fastestTime = TimeSpan.MaxValue;
            string fastestUrl = null;

            foreach (var url in urls)
            {
                var connectionTime = await MeasureConnectionTimeAsync(url);

                if (connectionTime.HasValue && connectionTime < fastestTime)
                {
                    fastestTime = connectionTime.Value;
                    fastestUrl = url;
                }

                // <500 ms is fast enough
                if (connectionTime.Value.TotalMilliseconds < 500)
                {
                    return url;
                }
            }

            if (fastestUrl != null)
            {
                Console.WriteLine($"Fastest server: {fastestUrl} with response time {fastestTime.TotalMilliseconds} ms");
                // Here you can add code to connect to the fastest server
                return fastestUrl;
            }
            else
            {
                Console.WriteLine("Failed to connect to any server.");
                return urls[0];
            }
        }

        private static async Task<TimeSpan?> MeasureConnectionTimeAsync(string url)
        {
            try
            {
                using (ClientWebSocket ws = new ClientWebSocket())
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    await ws.ConnectAsync(new Uri(url), CancellationToken.None);
                    stopwatch.Stop();
                    return stopwatch.Elapsed;
                }
            }
            catch
            {
                Console.WriteLine(url + " <-> Failed to connect and measure the connection time.");
                // Handle connection failure
                return null;
            }
        }
    }
}


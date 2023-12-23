using System;
using System.Diagnostics;
using System.Net.Http;

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
                var responseTime = await MeasureResponseTimeAsync(url);

                if (responseTime.HasValue && responseTime < fastestTime)
                {
                    fastestTime = responseTime.Value;
                    fastestUrl = url;
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

        public static async Task<TimeSpan?> MeasureResponseTimeAsync(string url)
        {
            try
            {
                var watch = Stopwatch.StartNew();
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Throws an exception if the HTTP response status is not a success status code
                watch.Stop();

                return watch.Elapsed;
            }
            catch (HttpRequestException)
            {
                // Handle connection failure
                return null;
            }
        }
    }
}


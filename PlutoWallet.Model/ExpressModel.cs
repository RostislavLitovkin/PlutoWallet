using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlutoWallet.Model
{
    public class ExpressModel
    {
        public static async Task RunAsync()
        {
            var nodeAppPath = "Express"; // Replace with the path to your Node.js application
            var nodeFilePath = System.IO.Path.Combine(nodeAppPath, "index.js");

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "node",
                Arguments = nodeFilePath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();

                // Read output asynchronously
                var outputTask = Task.Run(() => ReadOutput(process.StandardOutput));
                var errorTask = Task.Run(() => ReadOutput(process.StandardError));

                Console.WriteLine("Node.js Express server started. Press Enter to stop.");

                // Stop the Node.js process
                /*if (!process.HasExited)
                {
                    process.Kill();
                }*/

                await Task.WhenAll(outputTask, errorTask);
            }
        }

        private static async Task ReadOutput(System.IO.StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                Console.WriteLine(line);
            }
        }
    }
}

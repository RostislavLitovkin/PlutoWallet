using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlutoWallet.Model
{
    public class FileModel
    {
        public static async Task SaveImageAsync(string imageUrl, string fileName)
        {
            try
            {
                // Create HttpClient
                using HttpClient client = new HttpClient();

                // Download the image data as byte array
                var imageBytes = await client.GetByteArrayAsync(imageUrl);

                // Get the local path to save the image
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                // Save the image to the device
                await File.WriteAllBytesAsync(filePath, imageBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}

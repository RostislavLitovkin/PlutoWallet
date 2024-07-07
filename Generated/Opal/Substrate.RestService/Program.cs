using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Substrate.RestService
{
   /// <summary>
   /// >> Program
   /// Setting up the Rest Service.
   /// </summary>
   public class Program
   {
      /// <summary>
      /// Entrypoint
      /// </summary>
      /// <param name="args">Command line arguments.</param>
      public static void Main(string[] args)
      {
         Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Verbose()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
             .Enrich.WithThreadId()
             .Enrich.WithThreadName()
             .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}:{ThreadId}] {Message:lj}{NewLine}{Exception}")
             .CreateLogger();

         // Fire up the webserver.
         CreateHostBuilder(args).Build().Run();
      }

      /// <summary>
      /// Setup the Rest Service host.
      /// </summary>
      /// <param name="args">Command line arguments.</param>
      /// <returns>The host.</returns>
      public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .UseSerilog()
              .ConfigureWebHostDefaults(webBuilder =>
              {
                 webBuilder.UseStartup<Startup>();
              });
   }
}

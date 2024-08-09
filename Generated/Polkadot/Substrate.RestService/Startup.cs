using Substrate.AspNetCore;
using Substrate.AspNetCore.Persistence;
using Substrate.AspNetCore.Extensions;
using Substrate.RestService.Formatters;
using Substrate.ServiceLayer;
using Substrate.ServiceLayer.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.IO;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

namespace Substrate.RestService
{
   class SubstrateOutputFormatterSetup : IConfigureOptions<MvcOptions>
   {
      void IConfigureOptions<MvcOptions>.Configure(MvcOptions options)
      {
         options.OutputFormatters.Insert(0, new SubstrateOutputFormatter());
      }
   }

   public static class MvcBuilderExtensions
   {
      public static IMvcBuilder AddSubstrateOutputFormatter(this IMvcBuilder builder)
      {
         builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, SubstrateOutputFormatterSetup>());
         return builder;
      }
   }

   /// <summary>
   /// This class implements configuration and setting up services.
   /// </summary>
   public class Startup
   {
      private readonly CancellationTokenSource CTS = new CancellationTokenSource();

      private IStorageDataProvider _storageDataProvider;
      private readonly StorageSubscriptionChangeDelegate _storageChangeDelegate = new StorageSubscriptionChangeDelegate();

      // Delegate for adding local persistence for any Storage Changes
      // Changes are going to be saved in a CSV file. The default location of the CSV file will be in project root.
      // Alternatively, please set the fileDirectory parameter in the constructor below.
      private readonly StoragePersistenceChangeDelegate _storagePersistenceChangeDelegate = new StoragePersistenceChangeDelegate();
      
      // Set to true to activate persistence 
      private readonly bool _useLocalStoragePersistence = false;


      /// <summary>
      /// >> Startup
      /// Constructs and initializes the Startup class.
      /// Stores the configuration object
      /// </summary>
      /// <param name="configuration">The service configuration.</param>
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      /// <summary>
      /// Retrieves the service configuration.
      /// </summary>
      public IConfiguration Configuration { get; }

      /// <summary>
      /// This method gets called by the runtime. Use this method to add services to the container. 
      /// </summary>
      /// <param name="services">Service collection to configure.</param>
      public void ConfigureServices(IServiceCollection services)
      {
         string useMockupProvider = Environment.GetEnvironmentVariable("SUBSTRATE_USE_MOCKUP_PROVIDER") ?? "false";
         if (!string.IsNullOrEmpty(useMockupProvider) && useMockupProvider.Equals("true", StringComparison.InvariantCultureIgnoreCase))
         {
            // Configure mockup data provider
            _storageDataProvider = new SubstrateMockupDataProvider(File.ReadAllText(Path.Combine("..",".substrate","metadata.txt")));
         }
         else
         {
            // Configure regular data provider
            _storageDataProvider = new SubstrateDataProvider(Environment.GetEnvironmentVariable("SUBSTRATE_WEBSOCKET_ENDPOINT") ?? "wss://polkadot.api.onfinality.io/public-ws");
         }

         // Configure web sockets to allow clients to subscribe to storage changes.
         services.AddSubstrateSubscriptionHandler();

         // Configure storage services
         services.AddSubstrateStorageService(new SubstrateStorageServiceConfiguration()
         {
            CancellationToken = CTS.Token,
            DataProvider = _storageDataProvider,
            Storages = GetRuntimeStorages(),
            IsLazyLoadingEnabled = false // Set to true if you prefer to avoid loading all initial Storage values at the service startup
         });

         // Register data provider as singleton.
         services.AddSingleton(_storageDataProvider);
         services.AddRouting(options => { options.LowercaseQueryStrings = true; options.LowercaseUrls = true; });
         services.AddControllers(options => { })
            .AddSubstrateOutputFormatter();

         services.AddSwaggerGen(c =>
         {
            c.CustomSchemaIds(type => type.ToString());
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Substrate.RestService", Version = "v1" });
         });
      }

      private List<IStorage> GetRuntimeStorages()
      {
         var storageChangeDelegates = new List<IStorageChangeDelegate> {_storageChangeDelegate,};
          
         // If true, add local storage persistence
         if(_useLocalStoragePersistence)
            storageChangeDelegates.Add(_storagePersistenceChangeDelegate);
      
         return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(anyType => anyType.IsClass && typeof(IStorage).IsAssignableFrom(anyType))
            .Select(storageType => (IStorage)Activator.CreateInstance(storageType, new object[] { _storageDataProvider, storageChangeDelegates }))
            .ToList();
      }

      /// <summary>
      /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
      /// </summary>
      /// <param name="app">Application builder</param>
      /// <param name="env">Service hosting environment</param>
      /// <param name="handler">Middleware to handle web socket subscriptions</param>
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env, StorageSubscriptionHandler handler)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseSwagger();
         app.UseSwaggerUI(
             c =>
             {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Substrate.RestService v1");
             }
         );

         app.UseRouting();
         app.UseAuthorization();
         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });

         app.UseWebSockets();

         // Set the singleton subscription handler to our storage change delegate
         // so it can process and broadcast changes to any connected client.
         _storageChangeDelegate.SetSubscriptionHandler(handler);

         // Accept the subscriptions from now on.
         app.UseSubscription("/ws");
      }
   }
}

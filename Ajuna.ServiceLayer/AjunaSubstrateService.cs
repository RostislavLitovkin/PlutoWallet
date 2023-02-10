using Ajuna.ServiceLayer.Storage;
using Serilog;
using System.Threading.Tasks;

namespace Ajuna.ServiceLayer
{
   public class AjunaSubstrateService
   {
      private readonly AjunaSubstrateStorage _ajunaSubstrateStorage = new AjunaSubstrateStorage();

      /// <summary>
      /// 1. A connection to the server is established
      /// 2. Subscribe to all Storage Changes
      /// 3. Gather all storage info from metadata and laod all Storage specific Delegates
      /// 4. Start Processing Changes  
      /// </summary>
      /// <param name="configuration"></param>
      public async Task InitializeAsync(AjunaStorageServiceConfiguration configuration)
      {
         Log.Information("initialize Ajuna substrate service");

         // Initialize substrate client API
         await configuration.DataProvider.ConnectAsync(configuration.CancellationToken);

         // Initialize storage systems
         // Start by subscribing to any storage change and then start loading
         // all storages that this service is interested in.
         
         // While we are loading storages any storage subscription notification will
         // wait to be processed after the initialization is complete.
         await configuration.DataProvider.SubscribeStorageAsync(_ajunaSubstrateStorage.OnStorageUpdate);

         // Load storages we are interested in and register all Storage specific Delegates
         await _ajunaSubstrateStorage.InitializeAsync(configuration.DataProvider, configuration.Storages, configuration.IsLazyLoadingEnabled);

         // Start processing subscriptions.
         _ajunaSubstrateStorage.StartProcessingChanges();
      }

      public IStorage GetStorage<T>() => _ajunaSubstrateStorage.GetStorage<T>();
   }
}

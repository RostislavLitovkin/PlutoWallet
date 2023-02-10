using Ajuna.NetApi.Model.Types;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ajuna.ServiceLayer.Storage
{
   public class TypedStorage<T> where T : IType, new()
   {
      internal string Identifier { get; private set; }
      public T Store { get; private set; }
      public IStorageDataProvider DataProvider { get; private set; }
      
      /// <summary>
      /// A ChangeDelegate can be added in order to act upon any Storage Changes
      /// </summary>
      public List<IStorageChangeDelegate> ChangeDelegates { get; private set; }

      public TypedStorage(string identifier, IStorageDataProvider dataProvider)
      {
         Identifier = identifier;
         DataProvider = dataProvider;
      }

      public TypedStorage(string identifier, IStorageDataProvider dataProvider, List<IStorageChangeDelegate>  changeDelegates)
      {
         Identifier = identifier;
         DataProvider = dataProvider;
         ChangeDelegates = changeDelegates;
         Store = new T();
      }

      public async Task InitializeAsync(string module, string moduleItem)
      {
         Store = await DataProvider.GetStorageAsync<T>(module, moduleItem);
         Log.Information("loaded storage with {name}", moduleItem, Store.ToString());
      }

      public T Get()
      {
         return Store;
      }

      public void Update(string data)
      {
         if (string.IsNullOrEmpty(data))
         {
            Log.Debug($"[{Identifier}] item is not set or null.");
         }
         else
         {
            var iType = new T();
            iType.Create(data);

            Store = iType;
            Log.Debug($"[{Identifier}] item was updated.");

            ChangeDelegates?.ForEach(x=>x.OnUpdate(Identifier, string.Empty, data));
         }
      }
   }
}

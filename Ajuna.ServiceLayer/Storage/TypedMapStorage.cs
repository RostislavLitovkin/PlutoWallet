using Ajuna.NetApi.Model.Types;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ajuna.ServiceLayer.Storage
{
   public class TypedMapStorage<T> where T : IType, new()
   {
      internal string Identifier { get; private set; }
      public Dictionary<string, T> Dictionary { get; private set; }
      public IStorageDataProvider DataProvider { get; private set; }
      
      /// <summary>
      /// A ChangeDelegate can be added in order to act upon any Storage Changes
      /// </summary>
      public List<IStorageChangeDelegate> ChangeDelegates { get; private set; }

      public TypedMapStorage(string identifier, IStorageDataProvider dataProvider)
      {
         Identifier = identifier;
         DataProvider = dataProvider;
      }

      public TypedMapStorage(string identifier, IStorageDataProvider dataProvider, List<IStorageChangeDelegate>  changeDelegates)
      {
         Identifier = identifier;
         DataProvider = dataProvider;
         ChangeDelegates = changeDelegates;
         Dictionary = new Dictionary<string, T>();
      }

      public async Task InitializeAsync(string module, string moduleItem)
      {
         Dictionary = await DataProvider.GetStorageDictAsync<T>(module, moduleItem);
         Log.Information("loaded storage map {storage} with {count} entries", moduleItem, Dictionary.Count);
      }

      public bool ContainsKey(string key)
      {
         return Dictionary.ContainsKey(key);
      }

      public T Get(string key)
      {
         return Dictionary[key];
      }

      public void Update(string key, string data)
      {
         if (string.IsNullOrEmpty(data))
         {
            Dictionary.Remove(key);
            Log.Debug($"[{Identifier}] item {{key}} was deleted.", key);
            ChangeDelegates?.ForEach(x=>x.OnDelete(Identifier, key, data));
         }
         else
         {
            var iType = new T();
            iType.Create(data);

            if (Dictionary.ContainsKey(key))
            {
               Dictionary[key] = iType;
               Log.Debug($"[{Identifier}] item {{key}} was updated.", key);
               ChangeDelegates?.ForEach(x=>x.OnUpdate(Identifier, key, data));
            }
            else
            {
               Dictionary.Add(key, iType);
               Log.Debug($"[{Identifier}] item {{key}} was created.", key);
               ChangeDelegates?.ForEach(x=>x.OnCreate(Identifier, key, data));
            }
         }
      }
   }
}

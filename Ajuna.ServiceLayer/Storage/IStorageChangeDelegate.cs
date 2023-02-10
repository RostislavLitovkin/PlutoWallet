namespace Ajuna.ServiceLayer.Storage
{
   // This Delegate can be passed to the Storage classes and be triggered on every storage change
   public interface IStorageChangeDelegate
   {
      void OnUpdate(string identifier, string key, string data);
      void OnDelete(string identifier, string key, string data);
      void OnCreate(string identifier, string key, string data);
   }
}

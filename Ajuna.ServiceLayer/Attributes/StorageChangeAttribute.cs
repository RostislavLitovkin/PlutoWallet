using System;

namespace Ajuna.ServiceLayer.Attributes
{
   [AttributeUsage(AttributeTargets.Method)]
   public class StorageChangeAttribute : Attribute
   {
      public string Module { get; private set; }
      public string Name { get; private set; }

      public string Key
      {
         get { return $"{Module}.{Name}"; }
      }

      public StorageChangeAttribute(string module, string name)
      {
         Module = module;
         Name = name;
      }
   }
}

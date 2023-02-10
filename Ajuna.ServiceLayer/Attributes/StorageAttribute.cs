using System;

namespace Ajuna.ServiceLayer.Attributes
{
   [AttributeUsage(AttributeTargets.Class)]
   public class StorageAttribute : Attribute
   {
      public StorageAttribute()
      {
      }
   }
}

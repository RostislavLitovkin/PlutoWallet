using System;

namespace Ajuna.ServiceLayer.Attributes
{
   [AttributeUsage(AttributeTargets.Class)]
   public class AjunaControllerIgnoreAttribute : Attribute
   {
      public AjunaControllerIgnoreAttribute()
      {
      }
   }
}

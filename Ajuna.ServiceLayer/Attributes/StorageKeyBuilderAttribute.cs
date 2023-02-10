using System;

namespace Ajuna.ServiceLayer.Attributes
{
   [AttributeUsage(AttributeTargets.Method)]
   public class StorageKeyBuilderAttribute : Attribute
   {
      public Type ClassType { get; }
      public string MethodName { get; }
      public Type ParameterType { get; }

      public StorageKeyBuilderAttribute(Type classType, string methodName, Type keyType)
      {
         ClassType = classType;
         MethodName = methodName;
         ParameterType = keyType;
      }

      public StorageKeyBuilderAttribute(Type classType, string methodName)
      {
         ClassType = classType;
         MethodName = methodName;
         ParameterType = typeof(void);
      }
   }
}

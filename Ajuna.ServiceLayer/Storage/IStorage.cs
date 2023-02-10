using System.Threading.Tasks;

namespace Ajuna.ServiceLayer.Storage
{
   public interface IStorage
   {
      Task InitializeAsync(IStorageDataProvider dataProvider);
   }
}

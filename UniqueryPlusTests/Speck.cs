using Microsoft.Extensions.DependencyInjection;
using UniqueryPlus.Speck;

namespace UniqueryPlusTests
{
    internal class Speck
    {
        [Test]
        public async Task GetCollectionStatsAsync()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddSpeck()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://squid.subsquid.io/speck/graphql"));

            IServiceProvider services = serviceCollection.BuildServiceProvider();

            ISpeck client = services.GetRequiredService<ISpeck>();

            var result = await client.GetCollectionStats.ExecuteAsync("165");

            var collectionStats = result.Data.CollectionEntityById;

            Console.WriteLine(collectionStats.HighestSale);
            Console.WriteLine(collectionStats.Floor);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using StrawberryShake.Serialization;
using UniqueryPlus.Speck;
using UniqueryPlus.Stick;
using UniqueryPlus.UniqueSubquery;

namespace UniqueryPlus
{
    public static class Indexers
    {
        public static ISpeck GetSpeckClient()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddSpeck()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://squid.subsquid.io/speck/graphql"));

            IServiceProvider services = serviceCollection.BuildServiceProvider();

            return services.GetRequiredService<ISpeck>();
        }

        public static IStick GetStickClient()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddStick()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://squid.subsquid.io/speck/graphql"));

            IServiceProvider services = serviceCollection.BuildServiceProvider();

            return services.GetRequiredService<IStick>();
        }

        public static IUniqueSubquery GetUniqueSubqueryClient()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddUniqueSubquery()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://api-unique.uniquescan.io/v1/graphql"));

            serviceCollection.AddSerializer(new JsonSerializer("JSON"));

            IServiceProvider services = serviceCollection.BuildServiceProvider();

            return services.GetRequiredService<IUniqueSubquery>();
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Speck;

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
    }
}

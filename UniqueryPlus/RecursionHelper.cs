using Substrate.NetApi;
using System.Runtime.CompilerServices;

namespace UniqueryPlus
{
    internal class RecursionHelper
    {
        internal static async IAsyncEnumerable<T> ToIAsyncEnumerableAsync<T>(
            IEnumerable<SubstrateClient> clients,
            Func<SubstrateClient, NftTypeEnum, byte[]?, CancellationToken, Task<RecursiveReturn<T>>> getter,
            [EnumeratorCancellation] CancellationToken token = default
        )
        {
            foreach (var client in clients)
            {
                foreach (var nftType in GetNftTypeForClient(client))
                {
                    byte[]? lastKey = null;

                    while (true)
                    {
                        var nfts = await getter.Invoke(client, nftType, lastKey, token);

                        if (nfts.Items.Count() == 0)
                        {
                            break;
                        }

                        foreach (var item in nfts.Items)
                        {
                            yield return item;
                        }

                        lastKey = nfts.LastKey;
                    }
                }
            }
        }
        internal static async IAsyncEnumerable<T> ToIAsyncEnumerableAsync<T>(
            IEnumerable<SubstrateClient> clients,
            Func<SubstrateClient, NftTypeEnum, int, int, CancellationToken, Task<IEnumerable<T>>> getter,
            int limit,
            [EnumeratorCancellation] CancellationToken token = default
        )
        {
            foreach (var client in clients)
            {
                foreach (var nftType in GetNftTypeForClient(client))
                {
                    int offset = 0;

                    while (true)
                    {
                        var items = await getter.Invoke(client, nftType, limit, offset, token);

                        foreach (var item in items)
                        {
                            yield return item;
                        }

                        if (items.Count() != limit)
                        {
                            break;
                        }

                        offset += items.Count();
                    }
                }
            }
        }
        private static IEnumerable<NftTypeEnum> GetNftTypeForClient(SubstrateClient client)
        {
            return client switch
            {
                PolkadotAssetHub.NetApi.Generated.SubstrateClientExt => [NftTypeEnum.PolkadotAssetHub_NftsPallet],
                KusamaAssetHub.NetApi.Generated.SubstrateClientExt => [NftTypeEnum.KusamaAssetHub_NftsPallet],
                _ => []
            };
        }
    }
}

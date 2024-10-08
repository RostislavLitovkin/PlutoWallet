using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Metadata.Base;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UniqueryPlus.Nfts
{
    public static class NftModel
    {
        public static async Task<RecursiveReturn<INftBase>> GetNftsOwnedByAsync(this SubstrateClient client, NftTypeEnum type, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubNftModel.GetNftsNftsPalletOwnedByAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => await KusamaAssetHubNftModel.GetNftsNftsPalletOwnedByAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                NftTypeEnum.Unique => await UniqueNftModel.GetNftsOwnedByAsync((Unique.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                _ => throw new NotImplementedException()
            };
        }

        public static IAsyncEnumerable<INftBase> GetNftsOwnedByAsync(
            IEnumerable<SubstrateClient> clients,
            string owner,
            uint limit = 25
        )
        {
            return RecursionHelper.ToIAsyncEnumerableAsync(
                clients,
                async (SubstrateClient client, NftTypeEnum type, byte[]? lastKey, CancellationToken token) => await GetNftsOwnedByAsync(clients.First(), type, owner, limit, lastKey, token)
            );
        }
    }
}

using Substrate.NetApi;
using System.Numerics;

namespace UniqueryPlus.Nfts
{
    public static class NftModel
    {
        public static Task<RecursiveReturn<INftBase>> GetNftsOwnedByOnChainAsync(this SubstrateClient client, NftTypeEnum type, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => PolkadotAssetHubNftModel.GetNftsNftsPalletOwnedByAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => KusamaAssetHubNftModel.GetNftsNftsPalletOwnedByAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                NftTypeEnum.Unique => UniqueNftModel.GetNftsOwnedByOnChainAsync((Unique.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                _ => throw new NotImplementedException()
            };
        }

        public static Task<INftBase?> GetNftByIdAsync(this SubstrateClient client, NftTypeEnum type, BigInteger collectionId, BigInteger id, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => PolkadotAssetHubNftModel.GetNftNftsPalletByIdAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, (uint)id, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => KusamaAssetHubNftModel.GetNftNftsPalletByIdAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, (uint)id, token),
                NftTypeEnum.Unique => UniqueNftModel.GetNftByIdAsync((Unique.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, (uint)id, token),
                _ => throw new NotImplementedException()
            };
        }

        public static Task<IEnumerable<INftBase>> GetNftsOwnedByAsync(this SubstrateClient client, NftTypeEnum type, string owner, int limit, int offset, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.Unique => UniqueNftModel.GetNftsOwnedByAsync((Unique.NetApi.Generated.SubstrateClientExt)client, owner, limit, offset, token),
                _ => throw new NotImplementedException()
            };
        }

        public static IAsyncEnumerable<INftBase> GetNftsOwnedByAsync(
            IEnumerable<SubstrateClient> clients,
            string owner,
            uint limit = 25
        ) {
            return RecursionHelper.ToIAsyncEnumerableAsync(
                clients,
                (SubstrateClient client, NftTypeEnum type, int limit, int offset, CancellationToken token) => GetNftsOwnedByAsync(client, type, owner, limit, offset, token),
                (SubstrateClient client, NftTypeEnum type, byte[]? lastKey, CancellationToken token) => GetNftsOwnedByOnChainAsync(client, type, owner, limit, lastKey, token),
                (int)limit
            );
        }
    }
}

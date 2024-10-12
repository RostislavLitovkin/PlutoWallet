using Substrate.NetApi;

namespace UniqueryPlus.Nfts
{
    public static class NftModel
    {
        public static async Task<RecursiveReturn<INftBase>> GetNftsOwnedByOnChainAsync(this SubstrateClient client, NftTypeEnum type, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubNftModel.GetNftsNftsPalletOwnedByAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => await KusamaAssetHubNftModel.GetNftsNftsPalletOwnedByAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                NftTypeEnum.Unique => await UniqueNftModel.GetNftsOwnedByOnChainAsync((Unique.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                _ => throw new NotImplementedException()
            };
        }

        public static async Task<IEnumerable<INftBase>> GetNftsOwnedByAsync(this SubstrateClient client, NftTypeEnum type, string owner, int limit, int offset, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.Unique => await UniqueNftModel.GetNftsOwnedByAsync((Unique.NetApi.Generated.SubstrateClientExt)client, owner, limit, offset, token),
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
                async (SubstrateClient client, NftTypeEnum type, int limit, int offset, CancellationToken token) => await GetNftsOwnedByAsync(client, type, owner, limit, offset, token),
                async (SubstrateClient client, NftTypeEnum type, byte[]? lastKey, CancellationToken token) => await GetNftsOwnedByOnChainAsync(clients.First(), type, owner, limit, lastKey, token),
                (int)limit
            );
        }
    }
}

using Substrate.NetApi;

namespace UniqueryPlus.Nfts
{
    public static class NftModel
    {
        public static async Task<RecursiveReturn<INftBase>> GetNftsOwnedByAsync(this SubstrateClient client, NftTypeEnum type, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubNftModel.GetNftsNftsPalletOwnedByAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                _ => throw new NotImplementedException()
            };
        }
    }
}

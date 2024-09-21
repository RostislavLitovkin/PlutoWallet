using Substrate.NetApi;

namespace UniqueryPlus.Collections
{
    public static class CollectionModel
    {
        public static async Task<RecursiveReturn<ICollectionBase>> GetCollectionsOwnedByAsync(this SubstrateClient client, NftTypeEnum type, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubCollectionModel.GetCollectionsNftsPalletOwnedByAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                _ => throw new NotImplementedException()
            };
        }

        public static async Task<ICollectionBase> GetCollectionByCollectionIdAsync(this SubstrateClient client, NftTypeEnum type, uint collectionId, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubCollectionModel.GetCollectionNftsPalletByCollectionIdAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, collectionId, token),
                _ => throw new NotImplementedException()
            };

        }
    }
}

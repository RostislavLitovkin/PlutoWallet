using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;

namespace UniqueryPlus.Collections
{
    public static class CollectionModel
    {

        public static async Task<RecursiveReturn<ICollectionBase>> GetCollectionsOwnedByAsync(this SubstrateClient client, NftTypeEnum type, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubCollectionModel.GetCollectionsNftsPalletOwnedByAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => await KusamaAssetHubCollectionModel.GetCollectionsNftsPalletOwnedByAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                _ => throw new NotImplementedException()
            };
        }

        public static async Task<ICollectionBase> GetCollectionByCollectionIdAsync(this SubstrateClient client, NftTypeEnum type, uint collectionId, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubCollectionModel.GetCollectionNftsPalletByCollectionIdAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, collectionId, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => await KusamaAssetHubCollectionModel.GetCollectionNftsPalletByCollectionIdAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, collectionId, token),

                _ => throw new NotImplementedException()
            };
        }

        public static Method CreateCollection(NftTypeEnum type, string adminAddress, CollectionMintConfig config)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => PolkadotAssetHubCollectionModel.CreateCollectionNftsPallet(adminAddress, config),
                NftTypeEnum.KusamaAssetHub_NftsPallet => KusamaAssetHubCollectionModel.CreateCollectionNftsPallet(adminAddress, config),
                _ => throw new NotImplementedException(),
            };
        }

        public static async Task<uint> GetNumberOfCollectionsAsync(this SubstrateClient client, NftTypeEnum type, CancellationToken token)
        {
            return type switch {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubCollectionModel.GetNumberOfCollectionsAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => await KusamaAssetHubCollectionModel.GetNumberOfCollectionsAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, token),
                _ => throw new NotImplementedException(),
            };
        }
    }
}

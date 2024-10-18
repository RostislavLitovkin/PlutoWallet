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

        public static IAsyncEnumerable<ICollectionBase> GetCollectionsOwnedByAsync(
            IEnumerable<SubstrateClient> clients,
            string owner,
            uint limit = 25
        )
        {
            return RecursionHelper.ToIAsyncEnumerableAsync(
                clients,
                async (SubstrateClient client, NftTypeEnum type, byte[]? lastKey, CancellationToken token) => await GetCollectionsOwnedByAsync(clients.First(), type, owner, limit, lastKey, token),
                limit
            );
        }

        public static async Task<ICollectionBase> GetCollectionByCollectionIdAsync(this SubstrateClient client, NftTypeEnum type, uint collectionId, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubCollectionModel.GetCollectionNftsPalletByCollectionIdAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, collectionId, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => await KusamaAssetHubCollectionModel.GetCollectionNftsPalletByCollectionIdAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, collectionId, token),
                NftTypeEnum.Unique => await UniqueCollectionModel.GetCollectionByCollectionIdAsync((Unique.NetApi.Generated.SubstrateClientExt)client, collectionId, token),
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

        public static async Task<uint> GetTotalCountOfCollectionsAsync(this SubstrateClient client, NftTypeEnum type, CancellationToken token)
        {
            return type switch {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubCollectionModel.GetTotalCountOfCollectionsAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => await KusamaAssetHubCollectionModel.GetNumberOfCollectionsAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, token),
                _ => throw new NotImplementedException(),
            };
        }

        public static async Task<uint> GetTotalCountOfCollectionsForSaleAsync(NftTypeEnum type, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubCollectionModel.GetTotalCountOfCollectionsForSaleAsync(token),
                _ => throw new NotImplementedException(),
            };
        }

        public static async Task<IEnumerable<ICollectionBase>> GetCollectionsForSaleAsync(SubstrateClient client, NftTypeEnum type, int limit = 25, int offset = 0, CancellationToken token = default)
        {
            return type switch {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubCollectionModel.GetCollectionsForSaleAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, limit, offset, token),
                _ => throw new NotImplementedException(),
            };
        }

        public static IAsyncEnumerable<ICollectionBase> GetCollectionsForSaleAsync(
            IEnumerable<SubstrateClient> clients,
            uint limit = 25
        )
        {
            return RecursionHelper.ToIAsyncEnumerableAsync(
                clients,
                GetCollectionsForSaleAsync,
                (int)limit
            );
        }

        public static async Task<IEnumerable<ICollectionBase>> GetRandomCollectionsForSaleAsync(SubstrateClient client, NftTypeEnum type, int limit = 25, CancellationToken token = default)
        {
            var offset = new Random().Next(0, (int)await GetTotalCountOfCollectionsForSaleAsync(type, token) - limit);

            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubCollectionModel.GetCollectionsForSaleAsync(
                    (PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client,
                    limit,
                    offset,
                    token),
                _ => throw new NotImplementedException(),
            };
        }
    }
}

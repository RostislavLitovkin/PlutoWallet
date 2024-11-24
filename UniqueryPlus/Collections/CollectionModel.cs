using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using System.Numerics;

namespace UniqueryPlus.Collections
{
    public static class CollectionModel
    {

        public static Task<RecursiveReturn<ICollectionBase>> GetCollectionsOwnedByAsync(this SubstrateClient client, NftTypeEnum type, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => PolkadotAssetHubCollectionModel.GetCollectionsNftsPalletOwnedByAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => KusamaAssetHubCollectionModel.GetCollectionsNftsPalletOwnedByAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                NftTypeEnum.Mythos => MythosCollectionModel.GetCollectionsOwnedByAsync((Mythos.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
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
                (SubstrateClient client, NftTypeEnum type, byte[]? lastKey, CancellationToken token) => GetCollectionsOwnedByAsync(clients.First(), type, owner, limit, lastKey, token),
                limit
            );
        }

        public static Task<ICollectionBase> GetCollectionByCollectionIdAsync(this SubstrateClient client, NftTypeEnum type, BigInteger collectionId, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => PolkadotAssetHubCollectionModel.GetCollectionNftsPalletByCollectionIdAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => KusamaAssetHubCollectionModel.GetCollectionNftsPalletByCollectionIdAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, token),
                NftTypeEnum.Unique => UniqueCollectionModel.GetCollectionByCollectionIdAsync((Unique.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, token),
                NftTypeEnum.Mythos => MythosCollectionModel.GetCollectionByCollectionIdAsync((Mythos.NetApi.Generated.SubstrateClientExt)client, collectionId, token),
                _ => throw new NotImplementedException()
            };
        }

        public static Method CreateCollection(NftTypeEnum type, string adminAddress, CollectionMintConfig config)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => PolkadotAssetHubCollectionModel.CreateCollectionNftsPallet(adminAddress, config),
                NftTypeEnum.KusamaAssetHub_NftsPallet => KusamaAssetHubCollectionModel.CreateCollectionNftsPallet(adminAddress, config),
                NftTypeEnum.Mythos => MythosCollectionModel.CreateCollection(adminAddress, config),
                _ => throw new NotImplementedException(),
            };
        }

        public static Task<uint> GetTotalCountOfCollectionsAsync(this SubstrateClient client, NftTypeEnum type, CancellationToken token)
        {
            return type switch {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => PolkadotAssetHubCollectionModel.GetTotalCountOfCollectionsAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => KusamaAssetHubCollectionModel.GetNumberOfCollectionsAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, token),
                _ => throw new NotImplementedException(),
            };
        }

        public static Task<uint> GetTotalCountOfCollectionsForSaleAsync(NftTypeEnum type, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => PolkadotAssetHubCollectionModel.GetTotalCountOfCollectionsForSaleAsync(token),
                _ => throw new NotImplementedException(),
            };
        }

        public static Task<IEnumerable<ICollectionBase>> GetCollectionsForSaleAsync(SubstrateClient client, NftTypeEnum type, int limit = 25, int offset = 0, CancellationToken token = default)
        {
            return type switch {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => PolkadotAssetHubCollectionModel.GetCollectionsForSaleAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, limit, offset, token),
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
            var offset = new Random().Next(0, (int)await GetTotalCountOfCollectionsForSaleAsync(type, token).ConfigureAwait(false) - limit);

            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => await PolkadotAssetHubCollectionModel.GetCollectionsForSaleAsync(
                    (PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client,
                    limit,
                    offset,
                    token).ConfigureAwait(false),
                _ => throw new NotImplementedException(),
            };
        }
    }
}

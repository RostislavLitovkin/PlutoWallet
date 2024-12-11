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
                NftTypeEnum.Opal => OpalNftModel.GetNftsOwnedByOnChainAsync((Opal.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
                NftTypeEnum.Mythos => MythosNftModel.GetNftsOwnedByAsync((Mythos.NetApi.Generated.SubstrateClientExt)client, owner, limit, lastKey, token),
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
                NftTypeEnum.Opal => OpalNftModel.GetNftByIdAsync((Opal.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, (uint)id, token),
                NftTypeEnum.Mythos => MythosNftModel.GetNftByIdAsync((Mythos.NetApi.Generated.SubstrateClientExt)client, collectionId, id, token),
                _ => throw new NotImplementedException()
            };
        }

        public static Task<IEnumerable<INftBase>> GetNftsOwnedByAsync(this SubstrateClient client, NftTypeEnum type, string owner, int limit, int offset, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.Unique => UniqueNftModel.GetNftsOwnedByAsync((Unique.NetApi.Generated.SubstrateClientExt)client, owner, limit, offset, token),
                NftTypeEnum.Opal => OpalNftModel.GetNftsOwnedByAsync((Opal.NetApi.Generated.SubstrateClientExt)client, owner, limit, offset, token),
                _ => throw new NotImplementedException()
            };
        }

        public static Task<RecursiveReturn<INftBase>> GetNftsInCollectionOwnedByOnChainAsync(this SubstrateClient client, NftTypeEnum type, BigInteger collectionId, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => PolkadotAssetHubNftModel.GetNftsNftsPalletInCollectionOwnedByAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, owner, limit, lastKey, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => KusamaAssetHubNftModel.GetNftsNftsPalletInCollectionOwnedByAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, owner, limit, lastKey, token),
                NftTypeEnum.Unique => UniqueNftModel.GetNftsInCollectionOwnedByAsync((Unique.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, owner, limit, lastKey, token),
                NftTypeEnum.Opal => OpalNftModel.GetNftsInCollectionOwnedByAsync((Opal.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, owner, limit, lastKey, token),
                NftTypeEnum.Mythos => MythosNftModel.GetNftsInCollectionOwnedByAsync((Mythos.NetApi.Generated.SubstrateClientExt)client, collectionId, owner, limit, lastKey, token),
                _ => throw new NotImplementedException()
            };
        }

        public static Task<RecursiveReturn<INftBase>> GetNftsInCollectionOnChainAsync(this SubstrateClient client, NftTypeEnum type, BigInteger collectionId, uint limit, byte[]? lastKey, CancellationToken token)
        {
            return type switch
            {
                NftTypeEnum.PolkadotAssetHub_NftsPallet => PolkadotAssetHubNftModel.GetNftsNftsPalletInCollectionAsync((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, limit, lastKey, token),
                NftTypeEnum.KusamaAssetHub_NftsPallet => KusamaAssetHubNftModel.GetNftsNftsPalletInCollectionAsync((KusamaAssetHub.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, limit, lastKey, token),
                NftTypeEnum.Unique => UniqueNftModel.GetNftsInCollectionAsync((Unique.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, limit, lastKey, token),
                NftTypeEnum.Opal => OpalNftModel.GetNftsInCollectionAsync((Opal.NetApi.Generated.SubstrateClientExt)client, (uint)collectionId, limit, lastKey, token),
                NftTypeEnum.Mythos => MythosNftModel.GetNftsInCollectionAsync((Mythos.NetApi.Generated.SubstrateClientExt)client, collectionId, limit, lastKey, token),
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

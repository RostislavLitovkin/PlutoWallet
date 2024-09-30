using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Metadata.Base;

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
                _ => throw new NotImplementedException()
            };
        }

        public static async IAsyncEnumerable<INftBase> GetNftsOwnedByAsync(IEnumerable<SubstrateClient> clients, string owner, uint limit = 25)
        {
            CancellationToken token = default;
            foreach (var client in clients)
            {
                foreach(var nftType in GetNftTypeForClient(client))
                {
                    var isNotEmpty = true;
                    byte[]? lastKey = null;

                    while (isNotEmpty)
                    {
                        var nfts = await client.GetNftsOwnedByAsync(nftType, owner, limit, lastKey, token);

                        if (nfts.Items.Count() == 0)
                        {
                            isNotEmpty = false;
                        }

                        foreach(var item in nfts.Items)
                        {
                            yield return item;
                        }

                        lastKey = nfts.LastKey;
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
                _ => throw new NotImplementedException()
            };
        }
    }
}

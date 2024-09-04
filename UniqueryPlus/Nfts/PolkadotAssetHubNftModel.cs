using PolkadotAssetHub.NetApi.Generated;
using PolkadotAssetHub.NetApi.Generated.Storage;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Numerics;
using UniqueryPlus.Ipfs;
using PolkadotAssetHub.NetApi.Generated.Model.pallet_nfts.types;
namespace UniqueryPlus.Nfts
{
    public class PolkadotAssetHubNftsPalletNft : INftBase
    {
        public NftTypeEnum Type => NftTypeEnum.PolkadotAssetHub_NftsPallet;

        public BigInteger CollectionId { get; set; }
        public BigInteger Id { get; set; }
        public required string Owner { get; set; }
        public INftMetadataBase? Metadata { get; set; }
    }
    internal class PolkadotAssetHubNftModel
    {
        internal static async Task<RecursiveReturn<INftBase>> GetNftsNftsPalletInCollectionIdAsync(SubstrateClientExt client, uint collectionId, uint limit, byte[]? lastKey, CancellationToken token)
        {
            // 0x + Twox64 pallet + Twox64 storage + Blake2_128Concat U32
            var keyPrefixLength = 106;

            var keyPrefix = Utils.HexToByteArray(NftsStorage.ItemParams(new Substrate.NetApi.Model.Types.Base.BaseTuple<U32, U32>(new U32(collectionId), new U32(0))).Substring(0, keyPrefixLength));

            var fullKeys = await client.State.GetKeysPagedAsync(keyPrefix, limit, lastKey, string.Empty, token);

            // No more nfts found
            if (fullKeys == null || !fullKeys.Any())
            {
                return new RecursiveReturn<INftBase>
                {
                    Items = [],
                    LastKey = lastKey,
                };
            }

            // Filter only the nft Id keys
            var idKeys = fullKeys.Select(p => p.ToString().Substring(keyPrefixLength));

            var ids = idKeys.Select(Helpers.GetBigIntegerFromBlake2_128Concat);

            var nftDetails = await GetNftDetailsNftsPalletByFullKeysAsync(client, fullKeys.Select(p => Utils.HexToByteArray(p.ToString())).ToList(), token);

            var nftMetadatas = await GetNftMetadataNftsPalletByCollectionIdAndNftIdKeysAsync(client, collectionId, idKeys, token);

            return new RecursiveReturn<INftBase>
            {
                Items = ids.Zip(nftDetails, (BigInteger id, ItemDetails? details) => details switch
                {
                    // Should never be null
                    null => new PolkadotAssetHubNftsPalletNft
                    {
                        CollectionId = collectionId,
                        Owner = "Unknown",
                        Id = id,
                    },
                    _ => new PolkadotAssetHubNftsPalletNft
                    {
                        CollectionId = collectionId,
                        Owner = Utils.GetAddressFrom(details.Owner.Encode()),
                        Id = id,
                    }
                }).Zip(nftMetadatas, (PolkadotAssetHubNftsPalletNft nft, NftMetadata? metadata) =>
                {
                    nft.Metadata = metadata;
                    return nft;
                }),
                LastKey = Utils.HexToByteArray(fullKeys.Last().ToString())
            };
        }

        internal static async Task<IEnumerable<ItemDetails?>> GetNftDetailsNftsPalletByFullKeysAsync(SubstrateClientExt client, List<byte[]> fullKeys, CancellationToken token)
        {
            var storageChangeSets = await client.State.GetQueryStorageAtAsync(fullKeys, string.Empty, token);

            return storageChangeSets.First().Changes.Select(change => {
                if (change[1] == null)
                {
                    return null;
                }

                var details = new ItemDetails();
                details.Create(change[1]);

                return details;
            });
        }

        internal static async Task<IEnumerable<NftMetadata?>> GetNftMetadataNftsPalletByCollectionIdAndNftIdKeysAsync(SubstrateClientExt client, uint collectionId, IEnumerable<string> idKeys, CancellationToken token)
        {
            // 0x + Twox64 pallet + Twox64 storage + Blake2_128Concat U32
            var keyPrefixLength = 106;

            var keyPrefix = NftsStorage.ItemMetadataOfParams(new BaseTuple<U32, U32>(new U32(collectionId), new U32(0))).Substring(0, keyPrefixLength);

            var nftMetadataKeys = idKeys.Select(idKey => Utils.HexToByteArray(keyPrefix + idKey));
            var storageChangeSets = await client.State.GetQueryStorageAtAsync(nftMetadataKeys.ToList(), string.Empty, token);

            var metadatas = new List<NftMetadata?>();

            foreach (var change in storageChangeSets.First().Changes)
            {
                if (change[1] == null)
                {
                    metadatas.Add(null);
                    continue;
                }

                var nftMetadata = new PolkadotAssetHub.NetApi.Generated.Model.pallet_nfts.types.ItemMetadata();
                nftMetadata.Create(change[1]);

                string ipfsLink = System.Text.Encoding.UTF8.GetString(nftMetadata.Data.Value.Bytes);

                metadatas.Add(await IpfsModel.GetMetadataAsync<NftMetadata>(ipfsLink, token));
            };

            return metadatas;
        }
    }
}

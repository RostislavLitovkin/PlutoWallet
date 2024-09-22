using PolkadotAssetHub.NetApi.Generated;
using PolkadotAssetHub.NetApi.Generated.Storage;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Numerics;
using UniqueryPlus.Ipfs;
using PolkadotAssetHub.NetApi.Generated.Model.pallet_nfts.types;
using PolkadotAssetHub.NetApi.Generated.Model.sp_core.crypto;
using UniqueryPlus.Collections;
using UniqueryPlus.External;
using Substrate.NetApi.Model.Extrinsics;
using PolkadotAssetHub.NetApi.Generated.Model.sp_runtime.multiaddress;

namespace UniqueryPlus.Nfts
{
    public class PolkadotAssetHubNftsPalletNftFull : PolkadotAssetHubNftsPalletNft, INftSellable
    {
        private SubstrateClientExt client;

        public required BigInteger? Price { get; set; }

        public PolkadotAssetHubNftsPalletNftFull(SubstrateClientExt client) : base(client)
        {
            this.client = client;
        }

        public Method Sell(BigInteger price)
        {
            var whitelisted_buyer = new BaseOpt<EnumMultiAddress>();
            return NftsCalls.SetPrice(new U32((uint)CollectionId), new U32((uint)Id), new BaseOpt<U128>(new U128(price)), whitelisted_buyer);
        }
        public Method Buy()
        {
            return NftsCalls.BuyItem(new U32((uint)CollectionId), new U32((uint)Id), new U128(Price ?? 0));
        }
    }
    public class PolkadotAssetHubNftsPalletNft : INftBase, IKodaLink, INftTransferable, INftBurnable
    {
        private SubstrateClientExt client;
        public NftTypeEnum Type => NftTypeEnum.PolkadotAssetHub_NftsPallet;
        public BigInteger CollectionId { get; set; }
        public BigInteger Id { get; set; }
        public required string Owner { get; set; }
        public INftMetadataBase? Metadata { get; set; }
        public string KodaLink => $"https://koda.art/ahp/gallery/{CollectionId}-{Id}";
        public PolkadotAssetHubNftsPalletNft(SubstrateClientExt client)
        {
            this.client = client;
        }
        public async Task<ICollectionBase> GetCollectionAsync(CancellationToken token)
        {
            return await PolkadotAssetHubCollectionModel.GetCollectionNftsPalletByCollectionIdAsync(client, (uint)CollectionId, token);
        }
        public Method Transfer(string recipientAddress)
        {
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(recipientAddress));

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(MultiAddress.Id, accountId);

            return NftsCalls.Transfer(new U32((uint)CollectionId), new U32((uint)Id), multiAddress);
        }

        public Method Burn()
        {
            return NftsCalls.Burn(new U32((uint)CollectionId), new U32((uint)Id));
        }
        public async Task<INftBase> GetFullAsync(CancellationToken token)
        {
            return new PolkadotAssetHubNftsPalletNftFull(client)
            {
                Owner = Owner,
                CollectionId = CollectionId,
                Id = Id,
                Metadata = Metadata,
                Price = await PolkadotAssetHubNftModel.GetNftPriceNftsPalletAsync(client, (uint)CollectionId, (uint)Id, token)
            };
        }
    }
    internal class PolkadotAssetHubNftModel
    {
        internal static async Task<RecursiveReturn<INftBase>> GetNftsNftsPalletInCollectionAsync(SubstrateClientExt client, uint collectionId, uint limit, byte[]? lastKey, CancellationToken token)
        {
            // 0x + Twox64 pallet + Twox64 storage + Blake2_128Concat U32
            var keyPrefixLength = 106;

            var keyPrefix = Utils.HexToByteArray(NftsStorage.ItemParams(new BaseTuple<U32, U32>(new U32(collectionId), new U32(0))).Substring(0, keyPrefixLength));

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

            var baseStoragePrefixLength = 66;

            // Filter only the CollectionId and NftId keys
            var idKeys = fullKeys.Select(p => p.ToString().Substring(baseStoragePrefixLength));

            return await GetNftsNftsPalletByIdKeysAsync(client, idKeys, fullKeys.Last().ToString(), token);
        }

        internal static async Task<RecursiveReturn<INftBase>> GetNftsNftsPalletOwnedByAsync(SubstrateClientExt client, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            var accountId32 = new AccountId32();
            accountId32.Create(Utils.GetPublicKeyFrom(owner));

            // 0x + Twox64 pallet + Twox64 storage + Blake2_128Concat accountId32
            var keyPrefixLength = 162;

            var keyPrefix = Utils.HexToByteArray(NftsStorage.AccountParams(new BaseTuple<AccountId32, U32, U32>(accountId32, new U32(0), new U32(0))).Substring(0, keyPrefixLength));

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

            return await GetNftsNftsPalletByIdKeysAsync(client, idKeys, fullKeys.Last().ToString(), token);
        }

        internal static async Task<RecursiveReturn<INftBase>> GetNftsNftsPalletInCollectionOwnedByAsync(SubstrateClientExt client, uint collectionId, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            var accountId32 = new AccountId32();
            accountId32.Create(Utils.GetPublicKeyFrom(owner));

            // 0x + Twox64 pallet + Twox64 storage + Blake2_128Concat accountId32 + Blake2_128Concat collectionId
            var keyPrefixLength = 202;

            var keyPrefix = Utils.HexToByteArray(NftsStorage.AccountParams(new BaseTuple<AccountId32, U32, U32>(accountId32, new U32(collectionId), new U32(0))).Substring(0, keyPrefixLength));

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

            // 0x + Twox64 pallet + Twox64 storage + Blake2_128Concat accountId32
            var baseStoragePrefixLength = 162;

            // Filter only the nft Id keys
            var idKeys = fullKeys.Select(p => p.ToString().Substring(baseStoragePrefixLength));

            return await GetNftsNftsPalletByIdKeysAsync(client, idKeys, fullKeys.Last().ToString(), token);
        }

        internal static async Task<RecursiveReturn<INftBase>> GetNftsNftsPalletByIdKeysAsync(SubstrateClientExt client, IEnumerable<string> idKeys, string lastKey, CancellationToken token)
        {
            var ids = idKeys.Select(ids => (Helpers.GetBigIntegerFromBlake2_128Concat(ids.Substring(0, 40)), Helpers.GetBigIntegerFromBlake2_128Concat(ids.Substring(40, 40))));

            var nftDetails = await GetNftDetailsNftsPalletByIdKeysAsync(client, idKeys, token);

            var nftMetadatas = await GetNftMetadataNftsPalletByIdKeysAsync(client, idKeys, token);

            return new RecursiveReturn<INftBase>
            {
                Items = ids.Zip(nftDetails, ((BigInteger, BigInteger) ids, ItemDetails? details) => details switch
                {
                    // Should never be null
                    null => new PolkadotAssetHubNftsPalletNft(client)
                    {
                        CollectionId = ids.Item1,
                        Owner = "Unknown",
                        Id = ids.Item2,
                    },
                    _ => new PolkadotAssetHubNftsPalletNft(client)
                    {
                        CollectionId = ids.Item1,
                        Owner = Utils.GetAddressFrom(details.Owner.Encode()),
                        Id = ids.Item2,
                    }
                }).Zip(nftMetadatas, (PolkadotAssetHubNftsPalletNft nft, NftMetadata? metadata) =>
                {
                    nft.Metadata = metadata;
                    return nft;
                }),
                LastKey = Utils.HexToByteArray(lastKey)
            };
        }

        internal static async Task<IEnumerable<ItemDetails?>> GetNftDetailsNftsPalletByIdKeysAsync(SubstrateClientExt client, IEnumerable<string> idKeys, CancellationToken token)
        {
            // 0x + Twox64 pallet + Twox64 storage
            var keyPrefixLength = 66;

            var keyPrefix = NftsStorage.ItemParams(new BaseTuple<U32, U32>(new U32(0), new U32(0))).Substring(0, keyPrefixLength);

            var nftDetailsKeys = idKeys.Select(idKey => Utils.HexToByteArray(keyPrefix + idKey));

            var storageChangeSets = await client.State.GetQueryStorageAtAsync(nftDetailsKeys.ToList(), string.Empty, token);

            return storageChangeSets.First().Changes.Select(change =>
            {
                if (change[1] == null)
                {
                    return null;
                }

                var details = new ItemDetails();
                details.Create(change[1]);

                return details;
            });
        }

        internal static async Task<IEnumerable<NftMetadata?>> GetNftMetadataNftsPalletByIdKeysAsync(SubstrateClientExt client, IEnumerable<string> idKeys, CancellationToken token)
        {
            // 0x + Twox64 pallet + Twox64 storage
            var keyPrefixLength = 66;

            var keyPrefix = NftsStorage.ItemMetadataOfParams(new BaseTuple<U32, U32>(new U32(0), new U32(0))).Substring(0, keyPrefixLength);

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

                var nftMetadata = new ItemMetadata();
                nftMetadata.Create(change[1]);

                string ipfsLink = System.Text.Encoding.UTF8.GetString(nftMetadata.Data.Value.Bytes);

                metadatas.Add(await IpfsModel.GetMetadataAsync<NftMetadata>(ipfsLink, token));
            };

            return metadatas;
        }

        internal static async Task<BigInteger?> GetNftPriceNftsPalletAsync(SubstrateClientExt client, uint collectionId, uint id, CancellationToken token)
        {
            var price = await client.NftsStorage.ItemPriceOf(new BaseTuple<U32, U32>(new U32(collectionId), new U32(id)), null, token);

            if (price is null)
            {
                return null;
            }

            if (((BaseOpt<AccountId32>)price.Value[1]).OptionFlag)
            {
                return null;
            }

            return (U128)price.Value[0];
        }
    }
}

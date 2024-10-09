using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi;
using System.Numerics;
using UniqueryPlus.Collections;
using UniqueryPlus.External;
using Unique.NetApi.Generated;
using Unique.NetApi.Generated.Model.sp_runtime.multiaddress;
using Unique.NetApi.Generated.Model.sp_core.crypto;
using Unique.NetApi.Generated.Storage;
using Unique.NetApi.Generated.Model.pallet_evm.account;
using Unique.NetApi.Generated.Model.up_data_structs;
using UniqueryPlus.Ipfs;
using Unique.NetApi.Generated.Model.bounded_collections.bounded_vec;
using Unique.NetApi.Generated.Model.pallet_nonfungible;
using Newtonsoft.Json.Linq;

namespace UniqueryPlus.Nfts
{
    public record UniqueNftFull : UniqueNft
    {
        private SubstrateClientExt client;

        public required BigInteger? Price { get; set; }

        public UniqueNftFull(SubstrateClientExt client) : base(client)
        {
            this.client = client;
        }

        public Method Sell(BigInteger price)
        {
            var whitelisted_buyer = new BaseOpt<EnumMultiAddress>();
            //return UniqueCalls.
            //return NftsCalls.SetPrice(new U32((uint)CollectionId), new U32((uint)Id), new BaseOpt<U128>(new U128(price)), whitelisted_buyer);
            throw new NotImplementedException();

        }
        public Method Buy()
        {
            //return NftsCalls.BuyItem(new U32((uint)CollectionId), new U32((uint)Id), new U128(Price ?? 0));
            throw new NotImplementedException();

        }
    }
    public record UniqueNft : INftBase, IUniqueMarketplaceLink, INftTransferable, INftBurnable
    {
        private SubstrateClientExt client;
        public NftTypeEnum Type => NftTypeEnum.Unique;
        public BigInteger CollectionId { get; set; }
        public BigInteger Id { get; set; }
        public required string Owner { get; set; }
        public INftMetadataBase? Metadata { get; set; }
        public string UniqueMarketplaceLink => $"https://unqnft.io/unique/token/{CollectionId}/{Id}";
        public UniqueNft(SubstrateClientExt client)
        {
            this.client = client;
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task<ICollectionBase> GetCollectionAsync(CancellationToken token)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        {
            throw new NotImplementedException();
            //return await PolkadotAssetHubCollectionModel.GetCollectionNftsPalletByCollectionIdAsync(client, (uint)CollectionId, token);
        }
        public required bool IsTransferable { get; set; }
        public Method Transfer(string recipientAddress)
        {
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(recipientAddress));

            EnumBasicCrossAccountIdRepr destination = new EnumBasicCrossAccountIdRepr();
            destination.Create(BasicCrossAccountIdRepr.Substrate, accountId);

            CollectionId collectionId = new CollectionId();
            collectionId.Value = new U32((uint)CollectionId);

            TokenId id = new TokenId();
            id.Value = new U32((uint)Id);

            return UniqueCalls.Transfer(destination, collectionId, id, new U128(0));
        }
        public required bool IsBurnable { get; set; }
        public Method Burn()
        {
            CollectionId collectionId = new CollectionId();
            collectionId.Value = new U32((uint)CollectionId);

            TokenId id = new TokenId();
            id.Value = new U32((uint)Id);

            return UniqueCalls.BurnItem(collectionId, id, new U128(0));
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<INftBase> GetFullAsync(CancellationToken token)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return this;
        }
    }
    internal class UniqueNftModel
    {

        private static async Task<JArray> GetNftsInCollectionFullKeysAsync(SubstrateClientExt client, uint collectionId, uint limit, byte[]? lastKey, CancellationToken token)
        {
            // 0x + Twox64 pallet + Twox64 storage + Twox64Concat(u32)
            var keyPrefixLength = 90;

            CollectionId uniqueCollectionId = new CollectionId();
            uniqueCollectionId.Value = new U32(collectionId);

            TokenId uniqueTokenId = new TokenId();
            uniqueTokenId.Value = new U32(0);

            var keyPrefix = Utils.HexToByteArray(NonfungibleStorage.TokenPropertiesParams(new BaseTuple<CollectionId, TokenId>(uniqueCollectionId, uniqueTokenId)).Substring(0, keyPrefixLength));

            return await client.State.GetKeysPagedAsync(keyPrefix, 1000, null, string.Empty, token);

        }
        internal static async Task<int> GetTotalCountOfNftsInCollectionAsync(SubstrateClientExt client, uint collectionId, CancellationToken token)
        {
            var fullKeys = await GetNftsInCollectionFullKeysAsync(client, collectionId, 1000, null, token);

            return fullKeys.Count();
        }

        internal static async Task<RecursiveReturn<INftBase>> GetNftsInCollectionAsync(SubstrateClientExt client, uint collectionId, uint limit, byte[]? lastKey, CancellationToken token)
        {
            var fullKeys = await GetNftsInCollectionFullKeysAsync(client, collectionId, limit, lastKey, token);

            // No more nfts found
            if (fullKeys == null || !fullKeys.Any())
            {
                return new RecursiveReturn<INftBase>
                {
                    Items = [],
                    LastKey = lastKey,
                };
            }

            // Filter only the CollectionId and NftId keys
            var idKeys = fullKeys.Select(p => p.ToString().Substring(Constants.BASE_STORAGE_KEY_LENGTH));

            return await GetNftsNftsPalletByIdKeysAsync(client, idKeys, fullKeys.Last().ToString(), token);
        }

        internal static async Task<RecursiveReturn<INftBase>> GetNftsOwnedByAsync(SubstrateClientExt client, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            var accountId32 = new AccountId32();
            accountId32.Create(Utils.GetPublicKeyFrom(owner));

            // 0x + Twox64 pallet + Twox64 storage + Twox64Concat CollectionId + Blake2_128Concat crossReprIdEnum accountId32 + Twox64Concat tokenId 
            var substrateKeyPrefixLength = 212;

            // 0x + Twox64 pallet + Twox64 storage + Twox64Concat CollectionId + Blake2_128Concat crossReprIdEnum accountId32 + Twox64Concat tokenId 
            //var ethereumKeyPrefixLength = 188;

            CollectionId uniqueCollectionId = new CollectionId();
            uniqueCollectionId.Value = new U32(0);

            TokenId uniqueTokenId = new TokenId();
            uniqueTokenId.Value = new U32(0);

            EnumBasicCrossAccountIdRepr crossAccountId = new EnumBasicCrossAccountIdRepr();
            crossAccountId.Create(BasicCrossAccountIdRepr.Substrate, accountId32);

            var keyPrefix = Utils.HexToByteArray(NonfungibleStorage.OwnedParams(new BaseTuple<CollectionId, EnumBasicCrossAccountIdRepr, TokenId>(uniqueCollectionId, crossAccountId, uniqueTokenId)).Substring(0, Constants.BASE_STORAGE_KEY_LENGTH));

            var fullKeys = new JArray();

            while (true)
            {
                var newKeys = await client.State.GetKeysPagedAsync(keyPrefix, 1000, lastKey, string.Empty, token);

                if (newKeys == null || !newKeys.Any())
                {
                    break;
                }

                fullKeys.Merge(newKeys);

                lastKey = Utils.HexToByteArray(newKeys.Last().ToString());
            }

            var encodedAccountId = Utils.Bytes2HexString(accountId32.Encode(), Utils.HexStringFormat.Pure);

            // Filter only the nft Id keys
            var idKeys = fullKeys.Select(p => p.ToString()).Where(p => p.Length == substrateKeyPrefixLength && p.Substring(124, 64) == encodedAccountId).Select(p => p.Substring(Constants.BASE_STORAGE_KEY_LENGTH, 24) + p.Substring(188, 24));

            return await GetNftsNftsPalletByIdKeysAsync(client, idKeys, fullKeys.Last().ToString(), token);
        }

        internal static async Task<RecursiveReturn<INftBase>> GetNftsInCollectionOwnedByAsync(SubstrateClientExt client, uint collectionId, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            var accountId32 = new AccountId32();
            accountId32.Create(Utils.GetPublicKeyFrom(owner));

            // 0x + Twox64 pallet + Twox64 storage + Twox64Concat CollectionId + Blake2_128Concat crossReprIdEnum accountId32
            var substrateKeyPrefixLength = 188;

            // 0x + Twox64 pallet + Twox64 storage + Twox64Concat CollectionId + Blake2_128Concat crossReprIdEnum accountId32
            //var ethereumKeyPrefixLength = 164;

            CollectionId uniqueCollectionId = new CollectionId();
            uniqueCollectionId.Value = new U32(collectionId);

            TokenId uniqueTokenId = new TokenId();
            uniqueTokenId.Value = new U32(0);

            EnumBasicCrossAccountIdRepr crossAccountId = new EnumBasicCrossAccountIdRepr();
            crossAccountId.Create(BasicCrossAccountIdRepr.Substrate, accountId32);

            var keyPrefix = Utils.HexToByteArray(NonfungibleStorage.OwnedParams(new BaseTuple<CollectionId, EnumBasicCrossAccountIdRepr, TokenId>(uniqueCollectionId, crossAccountId, uniqueTokenId)).Substring(0, substrateKeyPrefixLength));

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

            var encodedAccountId = Utils.Bytes2HexString(accountId32.Encode(), Utils.HexStringFormat.Pure);

            // Filter only the nft Id keys
            var idKeys = fullKeys.Select(p => p.ToString()).Select(p => p.Substring(Constants.BASE_STORAGE_KEY_LENGTH, 24) + p.Substring(substrateKeyPrefixLength, 24));

            return await GetNftsNftsPalletByIdKeysAsync(client, idKeys, fullKeys.Last().ToString(), token);
        }

        internal static async Task<RecursiveReturn<INftBase>> GetNftsNftsPalletByIdKeysAsync(SubstrateClientExt client, IEnumerable<string> idKeys, string lastKey, CancellationToken token)
        {
            if (idKeys.Count() == 0)
            {
                return new RecursiveReturn<INftBase>
                {
                    Items = [],
                    LastKey = Utils.HexToByteArray(lastKey),
                };
            }

            var ids = idKeys.Select(ids => (Helpers.GetBigIntegerFromTwox_64Concat(ids.Substring(0, 24)), Helpers.GetBigIntegerFromTwox_64Concat(ids.Substring(24, 24))));

            var collectionIdKeys = ids.Select(ids => Utils.Bytes2HexString(HashExtension.Blake2Concat(new U32((uint)ids.Item1).Encode(), 128), Utils.HexStringFormat.Pure));

            var nftDatas = await GetTokenDataByIdKeysAsync(client, idKeys, token);

            var nftMetadatas = await GetNftMetadataByIdKeysAsync(client, idKeys, token);

            var collectionDatas = await UniqueCollectionModel.GetCollectionCollectionByCollectionIdKeysAsync(client, collectionIdKeys, token);

            return new RecursiveReturn<INftBase>
            {
                Items = ids.Zip(nftDatas, ((BigInteger, BigInteger) ids, ItemData? data) => data switch
                {
                    // Should never be null
                    null => new UniqueNft(client)
                    {
                        CollectionId = ids.Item1,
                        Owner = "Unknown",
                        Id = ids.Item2,

                        // Will be set later
                        IsTransferable = true,
                        IsBurnable = true,
                    },
                    _ => new UniqueNft(client)
                    {
                        CollectionId = ids.Item1,
                        Owner = data.Owner.Value switch
                        {
                            BasicCrossAccountIdRepr.Substrate => Utils.GetAddressFrom(data.Owner.Value2.Encode()),
                            BasicCrossAccountIdRepr.Ethereum => "Unknown",
                            _ => "Unknown"
                        },
                        Id = ids.Item2,

                        // Will be set later
                        IsTransferable = true,
                        IsBurnable = true,
                    }
                }).Zip(collectionDatas, (UniqueNft nft, Collection? collectionData) =>
                {
                    if (collectionData is null)
                    {
                        nft.Metadata = new NftMetadata
                        {
                            Name = "Unknown",
                        };

                        return nft;
                    }

                    nft.Metadata = new NftMetadata
                    {
                        Name = System.Text.Encoding.Unicode.GetString(Helpers.RemoveCompactIntegerPrefix(collectionData.Name.Value.Encode())),
                        Description = System.Text.Encoding.Unicode.GetString(Helpers.RemoveCompactIntegerPrefix(collectionData.Description.Value.Encode()))
                    };

                    nft.IsTransferable = (collectionData.Limits.TransfersEnabled.OptionFlag, collectionData.Limits.OwnerCanTransfer.OptionFlag) switch
                    {
                        (false, false) => true,
                        (true, false) => collectionData.Limits.TransfersEnabled.Value,
                        (false, true) => collectionData.Limits.OwnerCanTransfer.Value,
                        (true, true) => collectionData.Limits.TransfersEnabled.Value && collectionData.Limits.OwnerCanTransfer.Value,
                    };

                    // I am not 100% sure if OwnerCanDestroy == null means is Burnable, but I think it should be
                    nft.IsBurnable = collectionData.Limits.OwnerCanDestroy.OptionFlag ? collectionData.Limits.OwnerCanDestroy.Value : true;

                    return nft;
                }).Zip(nftMetadatas, (UniqueNft nft, NftMetadata? metadata) =>
                {
                    // Should never be null
                    if (metadata is null || nft.Metadata is null)
                    {
                        throw new Exception("Nft metadata was null even though it should never be null");
                    }

                    nft.Metadata.Image = metadata?.Image;
                    return nft;
                }),
                LastKey = Utils.HexToByteArray(lastKey)
            };
        }
        internal static async Task<IEnumerable<ItemData?>> GetTokenDataByIdKeysAsync(SubstrateClientExt client, IEnumerable<string> idKeys, CancellationToken token)
        {
            // 0x + Twox64 pallet + Twox64 storage
            var keyPrefixLength = 66;

            CollectionId uniqueCollectionId = new CollectionId();
            uniqueCollectionId.Value = new U32(0);

            TokenId uniqueTokenId = new TokenId();
            uniqueTokenId.Value = new U32(0);

            var keyPrefix = NonfungibleStorage.TokenDataParams(new BaseTuple<CollectionId, TokenId>(uniqueCollectionId, uniqueTokenId)).Substring(0, keyPrefixLength);

            var nftMetadataKeys = idKeys.Select(idKey => Utils.HexToByteArray(keyPrefix + idKey));
            var storageChangeSets = await client.State.GetQueryStorageAtAsync(nftMetadataKeys.ToList(), string.Empty, token);

            var itemDatas = new List<ItemData?>();

            foreach (var change in storageChangeSets.First().Changes)
            {
                if (change[1] == null)
                {
                    itemDatas.Add(null);
                    continue;
                }

                var itemData = new ItemData();
                itemData.Create(change[1]);

                itemDatas.Add(itemData);
            };

            return itemDatas;
        }

        internal static async Task<IEnumerable<NftMetadata?>> GetNftMetadataByIdKeysAsync(SubstrateClientExt client, IEnumerable<string> idKeys, CancellationToken token)
        {
            // 0x + Twox64 pallet + Twox64 storage
            var keyPrefixLength = 66;

            CollectionId uniqueCollectionId = new CollectionId();
            uniqueCollectionId.Value = new U32(0);

            TokenId uniqueTokenId = new TokenId();
            uniqueTokenId.Value = new U32(0);

            var keyPrefix = NonfungibleStorage.TokenPropertiesParams(new BaseTuple<CollectionId, TokenId>(uniqueCollectionId, uniqueTokenId)).Substring(0, keyPrefixLength);

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

                var nftProperties = new PropertiesT2();
                nftProperties.Create(change[1]);

                var ipfsNftProperty = nftProperties.Map.Value.Value.Value.Value.Where(nftProperty => System.Text.Encoding.UTF8.GetString(Helpers.RemoveCompactIntegerPrefix(((BoundedVecT12)nftProperty.Value[0]).Value.Encode())) == "i.c").First();

                string ipfsLink = System.Text.Encoding.UTF8.GetString(Helpers.RemoveCompactIntegerPrefix(((BoundedVecT14)ipfsNftProperty.Value[1]).Value.Encode()));

                metadatas.Add(new NftMetadata
                {
                    Image = IpfsModel.ToIpfsLink(ipfsLink.Replace("\"", ""), ipfsEndpoint: Constants.UNIQUE_IPFS_ENDPOINT),
                });
            };

            return metadatas;
        }
    }
}

using Mythos.NetApi.Generated;
using Mythos.NetApi.Generated.Model.account;
using Mythos.NetApi.Generated.Model.pallet_nfts.types;
using Mythos.NetApi.Generated.Model.runtime_common;
using Mythos.NetApi.Generated.Storage;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Numerics;
using UniqueryPlus.Collections;
using UniqueryPlus.Ipfs;
using UniqueryPlus.Metadata;

namespace UniqueryPlus.Nfts
{
    public record MythosNftFull : MythosNft, INftSellable, INftBuyable
    {
        private SubstrateClientExt client;

        public required BigInteger? Price { get; set; }
        public required bool IsForSale { get; set; }

        public MythosNftFull(SubstrateClientExt client) : base(client)
        {
            this.client = client;
        }

        public Method Sell(BigInteger price)
        {
            var whitelisted_buyer = new BaseOpt<AccountId20>();

            var collectionId = new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(CollectionId)
            };

            return NftsCalls.SetPrice(collectionId, new U128((uint)Id), new BaseOpt<U128>(new U128(price)), whitelisted_buyer);
        }
        public Method Buy()
        {
            return NftsCalls.BuyItem(new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(CollectionId)
            },
            new U128(Id), new U128(Price ?? 0));
        }
    }
    public record MythosNft : INftBase, INftTransferable, INftBurnable
    {
        private SubstrateClientExt client;
        public NftTypeEnum Type => NftTypeEnum.Mythos;
        public BigInteger CollectionId { get; set; }
        public BigInteger Id { get; set; }
        public required string Owner { get; set; }
        public MetadataBase? Metadata { get; set; }
        public MythosNft(SubstrateClientExt client)
        {
            this.client = client;
        }
        public Task<ICollectionBase> GetCollectionAsync(CancellationToken token) => MythosCollectionModel.GetCollectionByCollectionIdAsync(client, CollectionId, token);

        public bool IsTransferable { get; set; } = true;

        public Method Transfer(string recipientAddress)
        {
            var accountId = new AccountId20();
            accountId.Create(Helpers.AnyAddressToEthereumAccountId20Encoded(recipientAddress));

            return NftsCalls.Transfer(new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(CollectionId)
            }, new U128(Id), accountId);
        }

        public bool IsBurnable { get; set; } = true;
        public Method Burn() => NftsCalls.Burn(new IncrementableU256
        {
            Value = Helpers.GetMythosU256FromBigInteger(CollectionId)
        }, new U128(Id));

        public async Task<INftBase> GetFullAsync(CancellationToken token)
        {
            var price = await MythosNftModel.GetNftPriceAsync(client, CollectionId, Id, token).ConfigureAwait(false);

            return new MythosNftFull(client)
            {
                Owner = Owner,
                CollectionId = CollectionId,
                Id = Id,
                Metadata = Metadata,
                Price = price,
                IsForSale = price.HasValue,
            };
        }
    }
    internal class MythosNftModel
    {
        internal static async Task<INftBase?> GetNftByIdAsync(SubstrateClientExt client, BigInteger collectionId, BigInteger id, CancellationToken token)
        {
            var collectionIdOnChain = new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(collectionId)
            };

            var keyPrefix = Utils.HexToByteArray(NftsStorage.ItemParams(new BaseTuple<IncrementableU256, U128>(collectionIdOnChain, new U128(id))));

            var fullKeys = await client.State.GetKeysPagedAsync(keyPrefix, 1, null, string.Empty, token).ConfigureAwait(false);

            // No nfts found
            if (fullKeys == null || !fullKeys.Any())
            {
                return null;
            }

            // Filter only the CollectionId and NftId keys
            var idKeys = fullKeys.Select(p => p.ToString().Substring(Constants.BASE_STORAGE_KEY_LENGTH));

            return (await GetNftsByIdKeysAsync(client, idKeys, fullKeys.Last().ToString(), token).ConfigureAwait(false)).Items.First();
        }
        internal static async Task<RecursiveReturn<INftBase>> GetNftsInCollectionAsync(SubstrateClientExt client, BigInteger collectionId, uint limit, byte[]? lastKey, CancellationToken token)
        {
            // 0x + Twox64 pallet + Twox64 storage + Blake2_128Concat collection id
            var collectionIdOnChain = new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(collectionId)
            };
            var keyPrefixLength = Constants.BASE_STORAGE_KEY_LENGTH + Constants.BLAKE2_128HASH_LENGTH + collectionIdOnChain.Encode().Length * 2;

            var keyPrefix = Utils.HexToByteArray(NftsStorage.ItemParams(new BaseTuple<IncrementableU256, U128>(collectionIdOnChain, new U128(0))).Substring(0, keyPrefixLength));

            var fullKeys = await client.State.GetKeysPagedAsync(keyPrefix, limit, lastKey, string.Empty, token).ConfigureAwait(false);

            // No more nfts found
            if (fullKeys == null || !fullKeys.Any())
            {
                return new RecursiveReturn<INftBase>
                {
                    Items = [],
                    LastKey = lastKey,
                };
            }

            var baseStoragePrefixLength = Constants.BASE_STORAGE_KEY_LENGTH;

            // Filter only the CollectionId and NftId keys
            var idKeys = fullKeys.Select(p => p.ToString().Substring(baseStoragePrefixLength));

            return await GetNftsByIdKeysAsync(client, idKeys, fullKeys.Last().ToString(), token).ConfigureAwait(false);
        }

        internal static async Task<RecursiveReturn<INftBase>> GetNftsOwnedByAsync(SubstrateClientExt client, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            var accountId = new AccountId20();
            accountId.Create(Helpers.AnyAddressToEthereumAccountId20Encoded(owner));

            // 0x + Twox64 pallet + Twox64 storage + Blake2_128Concat accountId32
            var keyPrefixLength = Constants.BASE_STORAGE_KEY_LENGTH + Constants.BLAKE2_128HASH_LENGTH + accountId.TypeSize * 2;

            var keyPrefix = Utils.HexToByteArray(NftsStorage.AccountParams(new BaseTuple<AccountId20, IncrementableU256, U128>(accountId, new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(0)
            },
            new U128(0))).Substring(0, keyPrefixLength));

            var fullKeys = await client.State.GetKeysPagedAsync(keyPrefix, limit, lastKey, string.Empty, token).ConfigureAwait(false);

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

            return await GetNftsByIdKeysAsync(client, idKeys, fullKeys.Last().ToString(), token).ConfigureAwait(false);
        }

        internal static async Task<RecursiveReturn<INftBase>> GetNftsInCollectionOwnedByAsync(SubstrateClientExt client, BigInteger collectionId, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            var accountId = new AccountId20();
            accountId.Create(Helpers.AnyAddressToEthereumAccountId20Encoded(owner));

            var collectionIdOnChain = new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(collectionId)
            };

            // 0x + Twox64 pallet + Twox64 storage + Blake2_128Concat accountId32 + Blake2_128Concat collectionId
            var keyPrefixLength = Constants.BASE_STORAGE_KEY_LENGTH + Constants.BLAKE2_128HASH_LENGTH + accountId.TypeSize * 2 + Constants.BLAKE2_128HASH_LENGTH + collectionIdOnChain.Encode().Length * 2;

            var keyPrefix = Utils.HexToByteArray(NftsStorage.AccountParams(new BaseTuple<AccountId20, IncrementableU256, U128>(accountId, collectionIdOnChain, new U128(0))).Substring(0, keyPrefixLength));

            var fullKeys = await client.State.GetKeysPagedAsync(keyPrefix, limit, lastKey, string.Empty, token).ConfigureAwait(false);

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
            var baseStoragePrefixLength = Constants.BASE_STORAGE_KEY_LENGTH + Constants.BLAKE2_128HASH_LENGTH + accountId.TypeSize * 2;

            // Filter only the nft Id keys
            var idKeys = fullKeys.Select(p => p.ToString().Substring(baseStoragePrefixLength));

            return await GetNftsByIdKeysAsync(client, idKeys, fullKeys.Last().ToString(), token).ConfigureAwait(false);
        }

        internal static async Task<RecursiveReturn<INftBase>> GetNftsByIdKeysAsync(SubstrateClientExt client, IEnumerable<string> idKeys, string lastKey, CancellationToken token)
        {
            var ids = idKeys.Select(ids => (Helpers.GetBigIntegerFromBlake2_128Concat_MythosU256(ids.Substring(0, 96)), Helpers.GetBigIntegerFromBlake2_128Concat(ids.Substring(96))));

            var nftDetails = await GetNftDetailsByIdKeysAsync(client, idKeys, token).ConfigureAwait(false);

            var nftMetadatas = await GetNftMetadataByIdKeysAsync(client, idKeys, token).ConfigureAwait(false);

            return new RecursiveReturn<INftBase>
            {
                Items = ids.Zip(nftDetails, ((BigInteger, BigInteger) ids, ItemDetails? details) => details switch
                {
                    // Should never be null
                    null => new MythosNft(client)
                    {
                        CollectionId = ids.Item1,
                        Owner = "Unknown",
                        Id = ids.Item2,
                    },
                    _ => new MythosNft(client)
                    {
                        CollectionId = ids.Item1,
                        Owner = Utils.Bytes2HexString(details.Owner.Encode()),
                        Id = ids.Item2,
                    }
                }).Zip(nftMetadatas, (MythosNft nft, MetadataBase? metadata) =>
                {
                    nft.Metadata = metadata;
                    return nft;
                }),
                LastKey = Utils.HexToByteArray(lastKey)
            };
        }

        internal static async Task<IEnumerable<ItemDetails?>> GetNftDetailsByIdKeysAsync(SubstrateClientExt client, IEnumerable<string> idKeys, CancellationToken token)
        {
            // 0x + Twox64 pallet + Twox64 storage
            var keyPrefixLength = Constants.BASE_STORAGE_KEY_LENGTH;

            var keyPrefix = NftsStorage.ItemParams(new BaseTuple<IncrementableU256, U128>(new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(0)
            }, new U128(0))).Substring(0, keyPrefixLength);

            var nftDetailsKeys = idKeys.Select(idKey => Utils.HexToByteArray(keyPrefix + idKey));

            var storageChangeSets = await client.State.GetQueryStorageAtAsync(nftDetailsKeys.ToList(), string.Empty, token).ConfigureAwait(false);

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

        internal static async Task<IEnumerable<MetadataBase?>> GetNftMetadataByIdKeysAsync(SubstrateClientExt client, IEnumerable<string> idKeys, CancellationToken token)
        {
            // 0x + Twox64 pallet + Twox64 storage
            var keyPrefixLength = Constants.BASE_STORAGE_KEY_LENGTH;

            var keyPrefix = NftsStorage.ItemMetadataOfParams(new BaseTuple<IncrementableU256, U128>(new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(0)
            }, new U128(0))).Substring(0, keyPrefixLength);

            var nftMetadataKeys = idKeys.Select(idKey => Utils.HexToByteArray(keyPrefix + idKey));
            var storageChangeSets = await client.State.GetQueryStorageAtAsync(nftMetadataKeys.ToList(), string.Empty, token).ConfigureAwait(false);

            var metadatas = new List<MetadataBase?>();

            foreach (var change in storageChangeSets.First().Changes)
            {
                if (change[1] == null)
                {
                    // Maybe it uses BaseUri on Collection    

                    var collectionIdKey = change[0].Substring(Constants.BASE_STORAGE_KEY_LENGTH, 96);
                    var collectionId = Helpers.GetBigIntegerFromBlake2_128Concat_MythosU256(collectionIdKey);

                    if (!MythosMetadataBaseUriModel.BaseUriCache.ContainsKey(collectionId))
                    {
                        await MythosCollectionModel.GetCollectionMetadataByCollectionIdKeysAsync(client, [collectionIdKey], token);
                    }

                    if (MythosMetadataBaseUriModel.BaseUriCache.ContainsKey(collectionId))
                    {
                        var id = Helpers.GetBigIntegerFromBlake2_128Concat(change[0].Substring(Constants.BASE_STORAGE_KEY_LENGTH + 96));
                        var ipfsBaseUriLink = $"{MythosMetadataBaseUriModel.BaseUriCache[collectionId]}{id}";
                        metadatas.Add(await IpfsModel.GetMetadataAsync<MetadataBase>(ipfsBaseUriLink, Constants.DEFAULT_IPFS_ENDPOINT, token).ConfigureAwait(false));
                        continue;
                    }


                    metadatas.Add(null);

                    continue;
                }

                var nftMetadata = new ItemMetadata();
                nftMetadata.Create(change[1]);

                string ipfsLink = System.Text.Encoding.UTF8.GetString(nftMetadata.Data.Value.Bytes);

                metadatas.Add(await IpfsModel.GetMetadataAsync<MetadataBase>(ipfsLink, Constants.DEFAULT_IPFS_ENDPOINT, token).ConfigureAwait(false));
            };

            return metadatas;
        }

        internal static async Task<BigInteger?> GetNftPriceAsync(SubstrateClientExt client, BigInteger collectionId, BigInteger id, CancellationToken token)
        {
            var idu128 = new U128();
            idu128.Bytes = id.ToByteArray()[0..16];

            var price = await client.NftsStorage.ItemPriceOf(new BaseTuple<IncrementableU256, U128>(new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(collectionId)
            },
            idu128), null, token).ConfigureAwait(false);

            if (price is null)
            {
                return null;
            }

            if (((BaseOpt<AccountId20>)price.Value[1]).OptionFlag)
            {
                return null;
            }

            return (U128)price.Value[0];
        }
    }
}

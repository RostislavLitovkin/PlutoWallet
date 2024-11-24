using Mythos.NetApi.Generated;
using Mythos.NetApi.Generated.Model.account;
using Mythos.NetApi.Generated.Model.pallet_nfts.types;
using Mythos.NetApi.Generated.Model.runtime_common;
using Mythos.NetApi.Generated.Storage;
using Newtonsoft.Json;
using StrawberryShake;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Numerics;
using UniqueryPlus.Ipfs;
using UniqueryPlus.Metadata;
using UniqueryPlus.Nfts;
using SubstrateCollectionMetadata = Mythos.NetApi.Generated.Model.pallet_nfts.types.CollectionMetadata;

namespace UniqueryPlus.Collections
{
    public record MythosCollectionFull : MythosCollection, ICollectionMintConfig, ICollectionTransferable
    {
        private SubstrateClientExt client;
        public uint? NftMaxSuply { get; set; }
        public required MintType MintType { get; set; }
        public BigInteger? MintStartBlock { get; set; }
        public BigInteger? MintEndBlock { get; set; }
        public BigInteger? MintPrice { get; set; }
        public MythosCollectionFull(SubstrateClientExt client) : base(client)
        {
            this.client = client;
        }
        public bool IsTransferable { get; set; } = true;
        public Method Transfer(string recipientAddress)
        {
            var accountId = new AccountId20();
            accountId.Create(Helpers.AnyAddressToEthereumAccountId20Encoded(recipientAddress));

            return NftsCalls.TransferOwnership(new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(CollectionId)
            }, accountId);
        }
    }

    public record MythosCollection : ICollectionBase
    {
        private SubstrateClientExt client;
        public NftTypeEnum Type => NftTypeEnum.Mythos;
        public required BigInteger CollectionId { get; set; }
        public required string Owner { get; set; }
        public required uint NftCount { get; set; }
        public MetadataBase? Metadata { get; set; }
        public MythosCollection(SubstrateClientExt client)
        {
            this.client = client;
        }
        public async Task<IEnumerable<INftBase>> GetNftsAsync(uint limit, byte[]? lastKey = null, CancellationToken token = default)
        {
            if (NftCount == 0)
            {
                return [];
            }

            var result = await MythosNftModel.GetNftsInCollectionAsync(client, CollectionId, limit, lastKey, token).ConfigureAwait(false);

            return result.Items;
        }

        public async Task<IEnumerable<INftBase>> GetNftsOwnedByAsync(string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            if (NftCount == 0)
            {
                return [];
            }

            var result = await MythosNftModel.GetNftsInCollectionOwnedByAsync(client, CollectionId, owner, limit, lastKey, token).ConfigureAwait(false);

            return result.Items;
        }

        public async Task<ICollectionBase> GetFullAsync(CancellationToken token)
        {
            var mintConfig = await MythosCollectionModel.GetCollectionMintConfigAsync(client, CollectionId, token).ConfigureAwait(false);

            return new MythosCollectionFull(client)
            {
                CollectionId = CollectionId,
                Owner = Owner,
                NftCount = NftCount,
                Metadata = Metadata,
                MintType = mintConfig.MintType,
                MintPrice = mintConfig.MintPrice,
                MintStartBlock = mintConfig.MintStartBlock,
                MintEndBlock = mintConfig.MintEndBlock,
                NftMaxSuply = mintConfig.NftMaxSuply
            };
        }
    }

    internal class MythosCollectionModel
    {
        internal static async Task<RecursiveReturn<ICollectionBase>> GetCollectionsOwnedByAsync(SubstrateClientExt client, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            var accountId = new AccountId20();
            accountId.Create(Helpers.AnyAddressToEthereumAccountId20Encoded(owner));

            // 0x + Twox64 pallet + Twox64 storage + Blake2_128Concat accountId32
            var keyPrefixLength = Constants.BASE_STORAGE_KEY_LENGTH + Constants.BLAKE2_128HASH_LENGTH + accountId.TypeSize * 2;

            var keyPrefix = Utils.HexToByteArray(NftsStorage.CollectionAccountParams(new BaseTuple<AccountId20, IncrementableU256>(accountId, new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(0)
            })).Substring(0, keyPrefixLength));

            var fullKeys = await client.State.GetKeysPagedAsync(keyPrefix, limit, lastKey, string.Empty, token).ConfigureAwait(false);

            // No more collections found
            if (fullKeys == null || !fullKeys.Any())
            {
                return new RecursiveReturn<ICollectionBase>
                {
                    Items = [],
                    LastKey = lastKey,
                };
            }

            // Filter only the CollectionId keys
            var collectionIdKeys = fullKeys.Select(p => p.ToString().Substring(keyPrefixLength));

            return await GetCollectionsByIdKeysAsync(client, collectionIdKeys, fullKeys.Last().ToString(), token).ConfigureAwait(false);
        }

        internal static async Task<ICollectionBase> GetCollectionByCollectionIdAsync(SubstrateClientExt client, BigInteger collectionId, CancellationToken token)
        {
            var collectionIdKey = NftsStorage.CollectionParams(new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(collectionId)
            }
            ).Substring(Constants.BASE_STORAGE_KEY_LENGTH);

            var result = await GetCollectionsByIdKeysAsync(client, [collectionIdKey], "", token).ConfigureAwait(false);

            return result.Items.First();
        }

        internal static async Task<RecursiveReturn<ICollectionBase>> GetCollectionsByIdKeysAsync(SubstrateClientExt client, IEnumerable<string> collectionIdKeys, string lastKey, CancellationToken token)
        {
            var collectionIds = collectionIdKeys.Select(Helpers.GetBigIntegerFromBlake2_128Concat_MythosU256);

            var collectionDetails = await GetCollectionCollectionByCollectionIdKeysAsync(client, collectionIdKeys, token).ConfigureAwait(false);
            var collectionMetadatas = await GetCollectionMetadataByCollectionIdKeysAsync(client, collectionIdKeys, token).ConfigureAwait(false);

            return new RecursiveReturn<ICollectionBase>
            {
                Items = collectionIds.Zip(collectionDetails, (BigInteger collectionId, CollectionDetails? details) =>
                {
                    return details switch
                    {
                        // Should never be null
                        null => new MythosCollection(client)
                        {
                            CollectionId = collectionId,
                            Owner = "Unknown",
                            NftCount = 0
                        },
                        _ => new MythosCollection(client)
                        {
                            CollectionId = collectionId,
                            Owner = Utils.Bytes2HexString(details.Owner.Encode()),
                            NftCount = (uint)details.Items.Value
                        }
                    };
                }).Zip(collectionMetadatas, (MythosCollection collectionBase, MetadataBase? metadata) =>
                {
                    collectionBase.Metadata = metadata;
                    return collectionBase;
                }),
                LastKey = Utils.HexToByteArray(lastKey)
            };
        }

        internal static async Task<IEnumerable<CollectionDetails?>> GetCollectionCollectionByCollectionIdKeysAsync(SubstrateClientExt client, IEnumerable<string> collectionIdKeys, CancellationToken token)
        {
            var keyPrefix = NftsStorage.CollectionParams(new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(0)
            }).Substring(0, Constants.BASE_STORAGE_KEY_LENGTH);

            var collectionCollectionKeys = collectionIdKeys.Select(collectionIdKey => Utils.HexToByteArray(keyPrefix + collectionIdKey));

            var storageChangeSets = await client.State.GetQueryStorageAtAsync(collectionCollectionKeys.ToList(), string.Empty, token).ConfigureAwait(false);

            return storageChangeSets.First().Changes.Select(change =>
            {
                if (change[1] == null)
                {
                    return null;
                }

                var collectionDetails = new CollectionDetails();
                collectionDetails.Create(change[1]);

                return collectionDetails;
            });
        }

        internal static async Task<IEnumerable<MetadataBase?>> GetCollectionMetadataByCollectionIdKeysAsync(SubstrateClientExt client, IEnumerable<string> collectionIdKeys, CancellationToken token)
        {
            var keyPrefix = NftsStorage.CollectionMetadataOfParams(new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(0)
            }).Substring(0, Constants.BASE_STORAGE_KEY_LENGTH);

            var collectionMetadataKeys = collectionIdKeys.Select(collectionIdKey => Utils.HexToByteArray(keyPrefix + collectionIdKey));
            var storageChangeSets = await client.State.GetQueryStorageAtAsync(collectionMetadataKeys.ToList(), string.Empty, token).ConfigureAwait(false);

            var metadatas = new List<MetadataBase?>();

            foreach (var change in storageChangeSets.First().Changes)
            {
                if (change[1] == null)
                {
                    metadatas.Add(null);
                    continue;
                }

                var collectionMetadata = new SubstrateCollectionMetadata();
                collectionMetadata.Create(change[1]);

                string ipfsLink = System.Text.Encoding.UTF8.GetString(collectionMetadata.Data.Value.Bytes);

                Console.WriteLine("ipfsLink: " + ipfsLink);
                Console.WriteLine(ipfsLink is null);

                if (ipfsLink is null)
                {
                    metadatas.Add(null);
                    continue;
                }

                if (!ipfsLink.Contains("{"))
                {
                    metadatas.Add(await IpfsModel.GetMetadataAsync<MetadataBase>(ipfsLink, Constants.DEFAULT_IPFS_ENDPOINT, token).ConfigureAwait(false));
                    continue;
                }

                var metadataString = ipfsLink.Substring(ipfsLink.IndexOf("{"), ipfsLink.LastIndexOf("}") - ipfsLink.IndexOf("{") + 1);

                if (!metadataString.Contains("\"baseUri\":"))
                {
                    metadatas.Add(JsonConvert.DeserializeObject<MetadataBase>(metadataString));
                    continue;
                }

                var metadataWithBaseUri = JsonConvert.DeserializeObject<MetadataWithBaseUri>(metadataString);

                var collectionId = Helpers.GetBigIntegerFromBlake2_128Concat_MythosU256(change[0].ToString().Substring(Constants.BASE_STORAGE_KEY_LENGTH));

                if (metadataWithBaseUri?.BaseUri is null) {

                    metadatas.Add(null);
                    continue;
                }

                // Save to the cache
                MythosMetadataBaseUriModel.BaseUriCache[collectionId] = metadataWithBaseUri?.BaseUri ?? "";

                var newMetadata = await IpfsModel.GetMetadataAsync<MetadataBase>(metadataWithBaseUri?.BaseUri + "1", Constants.DEFAULT_IPFS_ENDPOINT, token).ConfigureAwait(false);

                if (newMetadata is not null && metadataWithBaseUri?.Name is not null)
                {
                    newMetadata.Name = metadataWithBaseUri.Name;
                }

                metadatas.Add(newMetadata);
            }

            return metadatas;
        }

        internal static async Task<CollectionMintConfig> GetCollectionMintConfigAsync(SubstrateClientExt client, BigInteger collectionId, CancellationToken token)
        {
            var collectionMintConfig = await client.NftsStorage.CollectionConfigOf(new IncrementableU256
            {
                Value = Helpers.GetMythosU256FromBigInteger(collectionId)
            }, null, token).ConfigureAwait(false);

            return new CollectionMintConfig
            {
                MintType = collectionMintConfig.MintSettings.MintType.Value switch
                {
                    Mythos.NetApi.Generated.Model.pallet_nfts.types.MintType.Public => new MintType
                    {
                        Type = MintTypeEnum.Public,
                    },
                    Mythos.NetApi.Generated.Model.pallet_nfts.types.MintType.Issuer => new MintType
                    {
                        Type = MintTypeEnum.Issuer,
                    },
                    Mythos.NetApi.Generated.Model.pallet_nfts.types.MintType.HolderOf => new MintType
                    {
                        Type = MintTypeEnum.HolderOfCollection,
                        CollectionId = (U32)collectionMintConfig.MintSettings.MintType.Value2
                    },
                    _ => throw new NotImplementedException()
                },
                MintPrice = collectionMintConfig.MintSettings.Price.GetValueOrNull(),
                MintStartBlock = collectionMintConfig.MintSettings.StartBlock.GetValueOrNull(),
                MintEndBlock = collectionMintConfig.MintSettings.EndBlock.GetValueOrNull(),
                NftMaxSuply = (uint?)collectionMintConfig.MaxSupply.GetValueOrNull(),
            };
        }
        internal static Method CreateCollection(string adminAddress, CollectionMintConfig collectionConfig)
        {
            var accountId = new AccountId20();
            accountId.Create(Helpers.AnyAddressToEthereumAccountId20Encoded(adminAddress));

            var mintType = new Mythos.NetApi.Generated.Model.pallet_nfts.types.EnumMintType();
            switch (collectionConfig.MintType.Type)
            {
                case MintTypeEnum.HolderOfCollection:
                    if (collectionConfig.MintType.CollectionId is null) throw new Exception("CollectionId can not be null when MintType is HolderOfCollection");
                    mintType.Create(Mythos.NetApi.Generated.Model.pallet_nfts.types.MintType.HolderOf, new U32((uint)collectionConfig.MintType.CollectionId));
                    break;
                case MintTypeEnum.Issuer:
                    mintType.Create(Mythos.NetApi.Generated.Model.pallet_nfts.types.MintType.Issuer, new BaseVoid());
                    break;
                case MintTypeEnum.Public:
                    mintType.Create(Mythos.NetApi.Generated.Model.pallet_nfts.types.MintType.Public, new BaseVoid());
                    break;
            }

            var config = new CollectionConfig
            {
                Settings = new BitFlagsT1 { Value = new U64(0) },

                MaxSupply = collectionConfig.NftMaxSuply is null ? new BaseOpt<U128>() : new BaseOpt<U128>(new U128((uint)collectionConfig.NftMaxSuply)),

                MintSettings = new MintSettings
                {
                    StartBlock = collectionConfig.MintStartBlock is null ? new BaseOpt<U32>() : new BaseOpt<U32>(new U32((uint)collectionConfig.MintStartBlock)),
                    EndBlock = collectionConfig.MintEndBlock is null ? new BaseOpt<U32>() : new BaseOpt<U32>(new U32((uint)collectionConfig.MintEndBlock)),
                    MintType = mintType,
                    Price = collectionConfig.MintPrice is null ? new BaseOpt<U128>() : new BaseOpt<U128>(new U128((ulong)collectionConfig.MintPrice)),
                    DefaultItemSettings = new BitFlagsT2 { Value = new U64(0) }
                }
            };

            return NftsCalls.Create(accountId, config);
        }

        internal static async Task<uint> GetTotalCountOfCollectionsAsync(SubstrateClientExt client, CancellationToken token)
        {
            return (uint)Helpers.GetBigIntegerFromMythosU256((await client.NftsStorage.NextCollectionId(null, token).ConfigureAwait(false)).Value);
        }

        internal static async Task<uint> GetTotalCountOfCollectionsForSaleAsync(CancellationToken token)
        {
            var speckClient = Indexers.GetSpeckClient();

            var result = await speckClient.GetTotalCountOfCollectionsForSale.ExecuteAsync().ConfigureAwait(false);

            result.EnsureNoErrors();

            if (result.Data is null)
            {
                return 0u;
            }

            return (uint)result.Data.CollectionEntitiesConnection.TotalCount;
        }

        internal static async Task<IEnumerable<ICollectionBase>> GetCollectionsForSaleAsync(SubstrateClientExt client, int limit = 25, int offset = 0, CancellationToken token = default)
        {
            var speckClient = Indexers.GetSpeckClient();

            var result = await speckClient.GetCollectionsForSale.ExecuteAsync(offset, limit).ConfigureAwait(false);

            result.EnsureNoErrors();

            if (result.Data is null)
            {
                return [];
            }

            return result.Data.CollectionEntities.Select(collectionEntity =>
            {
                return new MythosCollection(client)
                {
                    CollectionId = uint.Parse(collectionEntity.Id),
                    Owner = collectionEntity.CurrentOwner,
                    NftCount = (uint)collectionEntity.NftCount,
                    Metadata = new MetadataBase
                    {
                        Name = collectionEntity.Meta?.Name ?? "Unknown",
                        Description = collectionEntity.Meta?.Description ?? "",
                        Image = IpfsModel.ToIpfsLink(collectionEntity.Meta?.Image ?? "")
                    }
                };
            });
        }
    }
}

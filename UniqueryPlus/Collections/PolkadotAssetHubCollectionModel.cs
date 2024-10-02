﻿
using Substrate.NetApi;
using PolkadotAssetHub.NetApi.Generated;
using PolkadotAssetHub.NetApi.Generated.Model.sp_core.crypto;
using Substrate.NetApi.Model.Types.Primitive;
using PolkadotAssetHub.NetApi.Generated.Storage;
using PolkadotAssetHub.NetApi.Generated.Model.pallet_nfts.types;
using System.Numerics;
using UniqueryPlus.Ipfs;
using UniqueryPlus.Nfts;
using SubstrateCollectionMetadata = PolkadotAssetHub.NetApi.Generated.Model.pallet_nfts.types.CollectionMetadata;
using Substrate.NetApi.Model.Types.Base;
using UniqueryPlus.External;
using StrawberryShake;
using Substrate.NetApi.Model.Extrinsics;
using PolkadotAssetHub.NetApi.Generated.Model.sp_runtime.multiaddress;


namespace UniqueryPlus.Collections
{

    public class PolkadotAssetHubNftsPalletCollectionFull : PolkadotAssetHubNftsPalletCollection, ICollectionMintConfig, ICollectionStats, ICollectionCreatedAt, ICollectionTransferable
    {
        private SubstrateClientExt client;
        public uint? NftMaxSuply { get; set; }
        public required MintType MintType { get; set; }
        public BigInteger? MintStartBlock { get; set; }
        public BigInteger? MintEndBlock { get; set; }
        public BigInteger? MintPrice { get; set; }
        public required BigInteger HighestSale { get; set; }
        public required BigInteger FloorPrice { get; set; }
        public required BigInteger Volume { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public PolkadotAssetHubNftsPalletCollectionFull(SubstrateClientExt client) : base(client)
        {
            this.client = client;
        }
        public Method Transfer(string recipientAddress)
        {
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(recipientAddress));

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(MultiAddress.Id, accountId);

            return NftsCalls.TransferOwnership(new U32((uint)CollectionId), multiAddress);
        }
    }

    public class PolkadotAssetHubNftsPalletCollection : ICollectionBase, IKodaLink
    {
        private SubstrateClientExt client;
        public NftTypeEnum Type => NftTypeEnum.PolkadotAssetHub_NftsPallet;
        public required BigInteger CollectionId { get; set; }
        public required string Owner { get; set; }
        public required uint NftCount { get; set; }
        public ICollectionMetadataBase? Metadata { get; set; }
        public string KodaLink => $"https://koda.art/ahp/collection/{CollectionId}";
        public PolkadotAssetHubNftsPalletCollection(SubstrateClientExt client)
        {
            this.client = client;
        }
        public async Task<IEnumerable<INftBase>> GetNftsAsync(uint limit, byte[]? lastKey = null, CancellationToken token = default)
        {
            if (NftCount == 0)
            {
                return [];
            }

            var result = await PolkadotAssetHubNftModel.GetNftsNftsPalletInCollectionAsync(client, (uint)CollectionId, limit, lastKey, token);

            return result.Items;
        }

        public async Task<IEnumerable<INftBase>> GetNftsOwnedByAsync(string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            if (NftCount == 0)
            {
                return [];
            }

            var result = await PolkadotAssetHubNftModel.GetNftsNftsPalletInCollectionOwnedByAsync(client, (uint)CollectionId, owner, limit, lastKey, token);

            return result.Items;
        }

        public async Task<ICollectionBase> GetFullAsync()
        {
            var mintConfig = await PolkadotAssetHubCollectionModel.GetCollectionMintConfigNftsPalletAsync(client, (uint)CollectionId, default);

            var speckClient = Indexers.GetSpeckClient();

            var collectionStats = await speckClient.GetCollectionStats.ExecuteAsync(CollectionId.ToString());
            
            collectionStats.EnsureNoErrors();

            return new PolkadotAssetHubNftsPalletCollectionFull(client)
            {
                CollectionId = CollectionId,
                Owner = Owner,
                NftCount = NftCount,
                Metadata = Metadata,
                MintType = mintConfig.MintType,
                MintPrice = mintConfig.MintPrice,
                MintStartBlock = mintConfig.MintStartBlock,
                MintEndBlock = mintConfig.MintEndBlock,
                NftMaxSuply = mintConfig.NftMaxSuply,
                FloorPrice = BigInteger.Parse(collectionStats.Data?.CollectionEntityById?.Floor ?? "0"),
                HighestSale = BigInteger.Parse(collectionStats.Data?.CollectionEntityById?.HighestSale ?? "0"),
                Volume = BigInteger.Parse(collectionStats.Data?.CollectionEntityById?.Volume ?? "0"),
                CreatedAt = collectionStats.Data?.CollectionEntityById?.CreatedAt ?? default
            };
        }
    }

    internal class PolkadotAssetHubCollectionModel
    {
        internal static async Task<RecursiveReturn<ICollectionBase>> GetCollectionsNftsPalletOwnedByAsync(SubstrateClientExt client, string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            var accountId32 = new AccountId32();
            accountId32.Create(Utils.GetPublicKeyFrom(owner));

            // 0x + Twox64 pallet + Twox64 storage + Blake2_128Concat accountId32
            var keyPrefixLength = 162;

            var keyPrefix = Utils.HexToByteArray(NftsStorage.CollectionAccountParams(new BaseTuple<AccountId32, U32>(accountId32, new U32(0))).Substring(0, keyPrefixLength));

            var fullKeys = await client.State.GetKeysPagedAsync(keyPrefix, limit, lastKey, string.Empty, token);

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

            return await GetCollectionsNftsPalletByIdKeysAsync(client, collectionIdKeys, fullKeys.Last().ToString(), token);
        }

        internal static async Task<ICollectionBase> GetCollectionNftsPalletByCollectionIdAsync(SubstrateClientExt client, uint collectionId, CancellationToken token)
        {
            var collectionIdKey = NftsStorage.CollectionParams(new U32(collectionId)).Substring(Constants.BASE_STORAGE_KEY_LENGTH);

            var result = await GetCollectionsNftsPalletByIdKeysAsync(client, [collectionIdKey], "", token);

            return result.Items.First();
        }

        internal static async Task<RecursiveReturn<ICollectionBase>> GetCollectionsNftsPalletByIdKeysAsync(SubstrateClientExt client, IEnumerable<string> collectionIdKeys, string lastKey, CancellationToken token)
        {
            var collectionIds = collectionIdKeys.Select(Helpers.GetBigIntegerFromBlake2_128Concat);

            var collectionDetails = await GetCollectionCollectionNftsPalletByCollectionIdKeysAsync(client, collectionIdKeys, token);
            var collectionMetadatas = await GetCollectionMetadataNftsPalletByCollectionIdKeysAsync(client, collectionIdKeys, token);

            return new RecursiveReturn<ICollectionBase>
            {
                Items = collectionIds.Zip(collectionDetails, (BigInteger collectionId, CollectionDetails? details) =>
                {
                    return details switch
                    {
                        // Should never be null
                        null => new PolkadotAssetHubNftsPalletCollection(client)
                        {
                            CollectionId = collectionId,
                            Owner = "Unknown",
                            NftCount = 0
                        },
                        _ => new PolkadotAssetHubNftsPalletCollection(client)
                        {
                            CollectionId = collectionId,
                            Owner = Utils.GetAddressFrom(details.Owner.Encode()),
                            NftCount = details.Items.Value
                        }
                    };
                }).Zip(collectionMetadatas, (PolkadotAssetHubNftsPalletCollection collectionBase, CollectionMetadata? metadata) =>
                {
                    collectionBase.Metadata = metadata;
                    return collectionBase;
                }),
                LastKey = Utils.HexToByteArray(lastKey)
            };
        }

        internal static async Task<IEnumerable<CollectionDetails?>> GetCollectionCollectionNftsPalletByCollectionIdKeysAsync(SubstrateClientExt client, IEnumerable<string> collectionIdKeys, CancellationToken token)
        {
            var keyPrefix = NftsStorage.CollectionParams(new U32(0)).Substring(0, Constants.BASE_STORAGE_KEY_LENGTH);

            var collectionCollectionKeys = collectionIdKeys.Select(collectionIdKey => Utils.HexToByteArray(keyPrefix + collectionIdKey));

            var storageChangeSets = await client.State.GetQueryStorageAtAsync(collectionCollectionKeys.ToList(), string.Empty, token);

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

        internal static async Task<IEnumerable<CollectionMetadata?>> GetCollectionMetadataNftsPalletByCollectionIdKeysAsync(SubstrateClientExt client, IEnumerable<string> collectionIdKeys, CancellationToken token)
        {
            var keyPrefix = NftsStorage.CollectionMetadataOfParams(new U32(0)).Substring(0, Constants.BASE_STORAGE_KEY_LENGTH);

            var collectionMetadataKeys = collectionIdKeys.Select(collectionIdKey => Utils.HexToByteArray(keyPrefix + collectionIdKey));
            var storageChangeSets = await client.State.GetQueryStorageAtAsync(collectionMetadataKeys.ToList(), string.Empty, token);

            var metadatas = new List<CollectionMetadata?>();

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

                metadatas.Add(await IpfsModel.GetMetadataAsync<CollectionMetadata>(ipfsLink, token));
            };

            return metadatas;
        }

        internal static async Task<CollectionMintConfig> GetCollectionMintConfigNftsPalletAsync(SubstrateClientExt client, uint collectionId, CancellationToken token)
        {
            var collectionMintConfig = await client.NftsStorage.CollectionConfigOf(new U32(collectionId), null, token);

            return new CollectionMintConfig
            {
                MintType = collectionMintConfig.MintSettings.MintType.Value switch
                {
                    PolkadotAssetHub.NetApi.Generated.Model.pallet_nfts.types.MintType.Public => new MintType
                    {
                        Type = MintTypeEnum.Public,
                    },
                    PolkadotAssetHub.NetApi.Generated.Model.pallet_nfts.types.MintType.Issuer => new MintType
                    {
                        Type = MintTypeEnum.Issuer,
                    },
                    PolkadotAssetHub.NetApi.Generated.Model.pallet_nfts.types.MintType.HolderOf => new MintType
                    {
                        Type = MintTypeEnum.HolderOfCollection,
                        CollectionId = (U32)collectionMintConfig.MintSettings.MintType.Value2
                    },
                    _ => throw new NotImplementedException()
                },
                MintPrice = collectionMintConfig.MintSettings.Price.GetValueOrNull(),
                MintStartBlock = collectionMintConfig.MintSettings.StartBlock.GetValueOrNull(),
                MintEndBlock = collectionMintConfig.MintSettings.EndBlock.GetValueOrNull(),
                NftMaxSuply = collectionMintConfig.MaxSupply.GetValueOrNull(),
            };
        }
        internal static Method CreateCollectionNftsPallet(string adminAddress, CollectionMintConfig collectionConfig)
        {
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(adminAddress));

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(MultiAddress.Id, accountId);


            var mintType = new PolkadotAssetHub.NetApi.Generated.Model.pallet_nfts.types.EnumMintType();
            switch (collectionConfig.MintType.Type)
            {
                case MintTypeEnum.HolderOfCollection:
                    if (collectionConfig.MintType.CollectionId is null) throw new Exception("CollectionId can not be null when MintType is HolderOfCollection");
                    mintType.Create(PolkadotAssetHub.NetApi.Generated.Model.pallet_nfts.types.MintType.HolderOf, new U32((uint)collectionConfig.MintType.CollectionId));
                    break;
                case MintTypeEnum.Issuer:   
                    mintType.Create(PolkadotAssetHub.NetApi.Generated.Model.pallet_nfts.types.MintType.Issuer, new BaseVoid());
                    break;
                case MintTypeEnum.Public:
                    mintType.Create(PolkadotAssetHub.NetApi.Generated.Model.pallet_nfts.types.MintType.Public, new BaseVoid());
                    break;
            }

            var config = new CollectionConfig
            {
                Settings = new BitFlagsT1 { Value = new U64(0) },

                MaxSupply = collectionConfig.NftMaxSuply is null ? new BaseOpt<U32>() : new BaseOpt<U32>(new U32((uint)collectionConfig.NftMaxSuply)),

                MintSettings = new MintSettings
                {
                    StartBlock = collectionConfig.MintStartBlock is null ? new BaseOpt<U32>() : new BaseOpt<U32>(new U32((uint)collectionConfig.MintStartBlock)),
                    EndBlock = collectionConfig.MintEndBlock is null ? new BaseOpt<U32>() : new BaseOpt<U32>(new U32((uint)collectionConfig.MintEndBlock)),
                    MintType = mintType,
                    Price = collectionConfig.MintPrice is null ? new BaseOpt<U128>() : new BaseOpt<U128>(new U128((ulong)collectionConfig.MintPrice)),
                    DefaultItemSettings = new BitFlagsT2 { Value = new U64(0) }
                }
            };

            return NftsCalls.Create(multiAddress, config);
        }

        internal static async Task<uint> GetTotalCountOfCollectionsAsync(SubstrateClientExt client, CancellationToken token)
        {
            return await client.NftsStorage.NextCollectionId(null, token);
        }

        internal static async Task<uint> GetTotalCountOfCollectionsForSaleAsync(CancellationToken token)
        {
            var speckClient = Indexers.GetSpeckClient();


            var result = await speckClient.GetTotalCountOfCollectionsForSale.ExecuteAsync();

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

            var result = await speckClient.GetCollectionsForSale.ExecuteAsync(offset, limit);

            result.EnsureNoErrors();

            if (result.Data is null)
            {
                return [];
            }

            return result.Data.CollectionEntities.Select(collectionEntity =>
            {
                return new PolkadotAssetHubNftsPalletCollection(client)
                {
                    CollectionId = uint.Parse(collectionEntity.Id),
                    Owner = collectionEntity.CurrentOwner,
                    NftCount = (uint)collectionEntity.NftCount,
                    Metadata = new CollectionMetadata
                    {
                        Name = collectionEntity.Meta?.Name ?? "Unknown",
                        Description = collectionEntity.Meta?.Description ?? "",
                        Image = collectionEntity.Meta?.Image ?? ""
                    }
                };
            });
        }   
    }
}
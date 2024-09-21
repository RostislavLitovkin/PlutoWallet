
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
using UniqueryPlus;
using UniqueryPlus.External;

namespace UniqueryPlus.Collections
{

    public class PolkadotAssetHubNftsPalletCollectionFull : PolkadotAssetHubNftsPalletCollection, ICollectionMintConfig
    {
        private SubstrateClientExt client;
        public uint? NftMaxSuply { get; set; }
        public required MintType MintType { get; set; }
        public BigInteger? MintStartBlock { get; set; }
        public BigInteger? MintEndBlock { get; set; }
        public BigInteger? MintPrice { get; set; }
        public PolkadotAssetHubNftsPalletCollectionFull(SubstrateClientExt client) : base(client)
        {
            this.client = client;
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
                            Owner = owner,
                            NftCount = 0
                        },
                        _ => new PolkadotAssetHubNftsPalletCollection(client)
                        {
                            CollectionId = collectionId,
                            Owner = owner,
                            NftCount = details.Items.Value
                        }
                    };
                }).Zip(collectionMetadatas, (PolkadotAssetHubNftsPalletCollection collectionBase, CollectionMetadata? metadata) =>
                {
                    collectionBase.Metadata = metadata;
                    return collectionBase;
                }),
                LastKey = Utils.HexToByteArray(fullKeys.Last().ToString())
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
                    }
                },
                MintPrice = collectionMintConfig.MintSettings.Price.GetValueOrNull(),
                MintStartBlock = collectionMintConfig.MintSettings.StartBlock.GetValueOrNull(),
                MintEndBlock = collectionMintConfig.MintSettings.EndBlock.GetValueOrNull(),
                NftMaxSuply = collectionMintConfig.MaxSupply.GetValueOrNull(),
            };
        }
    }
}

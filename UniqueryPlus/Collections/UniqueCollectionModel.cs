using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi;
using System.Numerics;
using UniqueryPlus.External;
using UniqueryPlus.Nfts;
using Unique.NetApi.Generated;
using Unique.NetApi.Generated.Storage;
using Unique.NetApi.Generated.Model.up_data_structs;
using UniqueryPlus.Ipfs;
using Unique.NetApi.Generated.Model.sp_core.crypto;
using Unique.NetApi.Generated.Model.bounded_collections.bounded_vec;

namespace UniqueryPlus.Collections
{
    public record UniqueCollectionFull : UniqueCollection, ICollectionStats, ICollectionCreatedAt, ICollectionTransferable
    {
        private SubstrateClientExt client;
        public required BigInteger HighestSale { get; set; }
        public required BigInteger FloorPrice { get; set; }
        public required BigInteger Volume { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public UniqueCollectionFull(SubstrateClientExt client) : base(client)
        {
            this.client = client;
        }
        public Method Transfer(string recipientAddress)
        {
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(recipientAddress));

            CollectionId uniqueCollectionId = new CollectionId();
            uniqueCollectionId.Value = new U32((uint)CollectionId);

            return UniqueCalls.ChangeCollectionOwner(uniqueCollectionId, accountId);
        }
    }

    public record UniqueCollection : ICollectionBase, IUniqueMarketplaceLink, ICollectionMintConfig
    {
        private SubstrateClientExt client;
        public NftTypeEnum Type => NftTypeEnum.KusamaAssetHub_NftsPallet;
        public required BigInteger CollectionId { get; set; }
        public required string Owner { get; set; }
        public required uint NftCount { get; set; }
        public ICollectionMetadataBase? Metadata { get; set; }
        public uint? NftMaxSuply { get; set; }
        public required MintType MintType { get; set; }
        public BigInteger? MintStartBlock { get; set; }
        public BigInteger? MintEndBlock { get; set; }
        public BigInteger? MintPrice { get; set; }
        public string UniqueMarketplaceLink => $"https://unqnft.io/unique/collection/{CollectionId}";
        public UniqueCollection(SubstrateClientExt client)
        {
            this.client = client;
        }
        public async Task<IEnumerable<INftBase>> GetNftsAsync(uint limit, byte[]? lastKey = null, CancellationToken token = default)
        {
            if (NftCount == 0)
            {
                return [];
            }

            var result = await UniqueNftModel.GetNftsInCollectionAsync(client, (uint)CollectionId, limit, lastKey, token);

            return result.Items;
        }

        public async Task<IEnumerable<INftBase>> GetNftsOwnedByAsync(string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            if (NftCount == 0)
            {
                return [];
            }

            var result = await UniqueNftModel.GetNftsInCollectionOwnedByAsync(client, (uint)CollectionId, owner, limit, lastKey, token);

            return result.Items;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<ICollectionBase> GetFullAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return this;
        }
    }

    class UniqueCollectionModel
    {
        internal static async Task<ICollectionBase> GetCollectionByCollectionIdAsync(SubstrateClientExt client, uint collectionId, CancellationToken token)
        {
            CollectionId uniqueCollectionId = new CollectionId();
            uniqueCollectionId.Value = new U32(collectionId);

            var collectionIdKey = CommonStorage.CollectionByIdParams(uniqueCollectionId).Substring(Constants.BASE_STORAGE_KEY_LENGTH);

            var result = await GetCollectionsByIdKeysAsync(client, [collectionIdKey], "", token);

            return result.Items.First();
        }

        internal static async Task<RecursiveReturn<ICollectionBase>> GetCollectionsByIdKeysAsync(SubstrateClientExt client, IEnumerable<string> collectionIdKeys, string lastKey, CancellationToken token)
        {
            var collectionIds = collectionIdKeys.Select(Helpers.GetBigIntegerFromBlake2_128Concat);

            var collectionDetails = await GetCollectionCollectionByCollectionIdKeysAsync(client, collectionIdKeys, token);
            var collectionMetadatas = await GetCollectionMetadataNftsPalletByCollectionIdKeysAsync(client, collectionIdKeys, token);
            var nftCounts = await GetTotalCountOfNftsInCollectionByCollectionIdKeysAsync(client, collectionIds, token);

            return new RecursiveReturn<ICollectionBase>
            {
                Items = collectionIds.Zip(collectionDetails, (BigInteger collectionId, Collection? details) =>
                {
                    return details switch
                    {
                        // Should never be null
                        null => new UniqueCollection(client)
                        {
                            CollectionId = collectionId,
                            Owner = "Unknown",
                            NftCount = 0,
                            MintType = new MintType
                            {
                                Type = MintTypeEnum.Unknown,
                                CollectionId = null
                            },
                            MintPrice = default
                        },
                        _ => new UniqueCollection(client)
                        {
                            CollectionId = collectionId,
                            Owner = Utils.GetAddressFrom(details.Owner.Encode()),
                            NftCount = 0, // Unknown at this point
                            Metadata = new CollectionMetadata
                            {
                                Name = System.Text.Encoding.Unicode.GetString(Helpers.RemoveCompactIntegerPrefix(details.Name.Value.Encode())),
                                Description = System.Text.Encoding.Unicode.GetString(Helpers.RemoveCompactIntegerPrefix(details.Description.Value.Encode()))
                            },
                            MintType = new MintType
                            {
                                Type = (details.Permissions.Access.OptionFlag, details.Permissions.MintMode.OptionFlag) switch
                                {
                                    (false, false) => MintTypeEnum.Public,
                                    (true, false) => details.Permissions.Access.Value.Value switch
                                    {
                                        AccessMode.Normal => MintTypeEnum.Public,
                                        AccessMode.AllowList => MintTypeEnum.Whitelist,
                                        _ => MintTypeEnum.Unknown
                                    },
                                    (false, true) => details.Permissions.MintMode.Value.Value ? MintTypeEnum.Public : MintTypeEnum.CannotMint,
                                    (true, true) => (details.Permissions.MintMode.Value.Value, details.Permissions.Access.Value.Value) switch
                                    {
                                        (true, AccessMode.Normal) => MintTypeEnum.Public,
                                        (true, AccessMode.AllowList) => MintTypeEnum.Whitelist,
                                        (false, _) => MintTypeEnum.CannotMint,
                                        _ => MintTypeEnum.Unknown,
                                    }
                                },
                                CollectionId = null
                            },
                            NftMaxSuply = details.Limits.TokenLimit.OptionFlag ? details.Limits.TokenLimit.Value : null,
                            MintPrice = null,
                            MintEndBlock = null,
                            MintStartBlock = null
                        },
                    };
                }).Zip(nftCounts, (UniqueCollection collectionBase, uint nftCount) =>
                {
                    collectionBase.NftCount = nftCount;
                    return collectionBase;
                }).Zip(collectionMetadatas, (UniqueCollection collectionBase, CollectionMetadata? metadata) =>
                {
                    if (metadata is null || collectionBase.Metadata is null)
                    {
                        return collectionBase;
                    }

                    collectionBase.Metadata.Image = metadata.Image;
                    return collectionBase;
                }),
                LastKey = Utils.HexToByteArray(lastKey)
            };
        }


        internal static async Task<IEnumerable<Collection?>> GetCollectionCollectionByCollectionIdKeysAsync(SubstrateClientExt client, IEnumerable<string> collectionIdKeys, CancellationToken token)
        {
            CollectionId uniqueCollectionId = new CollectionId();
            uniqueCollectionId.Value = new U32(0);

            var keyPrefix = CommonStorage.CollectionByIdParams(uniqueCollectionId).Substring(0, Constants.BASE_STORAGE_KEY_LENGTH);

            var collectionCollectionKeys = collectionIdKeys.Select(collectionIdKey => Utils.HexToByteArray(keyPrefix + collectionIdKey));

            var storageChangeSets = await client.State.GetQueryStorageAtAsync(collectionCollectionKeys.ToList(), string.Empty, token);

            return storageChangeSets.First().Changes.Select(change =>
            {
                if (change[1] == null)
                {
                    return null;
                }

                var collectionDetails = new Collection();
                collectionDetails.Create(change[1]);

                return collectionDetails;
            });
        }

        internal static async Task<IEnumerable<CollectionMetadata?>> GetCollectionMetadataNftsPalletByCollectionIdKeysAsync(SubstrateClientExt client, IEnumerable<string> collectionIdKeys, CancellationToken token)
        {
            CollectionId uniqueCollectionId = new CollectionId();
            uniqueCollectionId.Value = new U32(0);

            var keyPrefix = CommonStorage.CollectionPropertiesParams(uniqueCollectionId).Substring(0, Constants.BASE_STORAGE_KEY_LENGTH);

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

                var collectionProperties = new PropertiesT1();
                collectionProperties.Create(change[1]);


                var ipfsCollectionProperty = collectionProperties.Map.Value.Value.Value.Value.Where(collectionProperty => new string[] { "baseURI", "coverPicture.ipfsCid", "collectionInfo" }.Contains(System.Text.Encoding.UTF8.GetString(Helpers.RemoveCompactIntegerPrefix(((BoundedVecT12)collectionProperty.Value[0]).Value.Encode())))).First();

                var ipfsLink = System.Text.Encoding.UTF8.GetString(Helpers.RemoveCompactIntegerPrefix(((BoundedVecT12)ipfsCollectionProperty.Value[0]).Value.Encode())) switch
                {
                    "coverPicture.ipfsCid" => System.Text.Encoding.UTF8.GetString(Helpers.RemoveCompactIntegerPrefix(((BoundedVecT14)ipfsCollectionProperty.Value[1]).Value.Encode())),
                    "baseURI" => System.Text.Encoding.UTF8.GetString(Helpers.RemoveCompactIntegerPrefix(((BoundedVecT14)ipfsCollectionProperty.Value[1]).Value.Encode())),
                    // Add option for Metadata V2
                    _ => null
                };

                Console.WriteLine("ipfsLink: " + ipfsLink);
                Console.WriteLine(ipfsLink is null);

                if (ipfsLink is null)
                {
                    metadatas.Add(null);
                    continue;
                }

                metadatas.Add(new CollectionMetadata
                {
                    Image = IpfsModel.ToIpfsLink(ipfsLink.Replace("\"", ""), ipfsEndpoint: Constants.UNIQUE_IPFS_ENDPOINT),
                });
            };

            return metadatas;
        }

        internal static async Task<IEnumerable<uint>> GetTotalCountOfNftsInCollectionByCollectionIdKeysAsync(SubstrateClientExt client, IEnumerable<BigInteger> collectionIds, CancellationToken token)
        {
            List<uint> nftCounts = new List<uint>();
            foreach (BigInteger collectionId in collectionIds)
            {
                nftCounts.Add((uint)await UniqueNftModel.GetTotalCountOfNftsInCollectionAsync(client, (uint)collectionId, token));
            }

            return nftCounts;
        }
    }
}

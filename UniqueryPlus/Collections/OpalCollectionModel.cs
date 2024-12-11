using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi;
using System.Numerics;
using UniqueryPlus.External;
using UniqueryPlus.Nfts;
using Opal.NetApi.Generated;
using Opal.NetApi.Generated.Storage;
using Opal.NetApi.Generated.Model.up_data_structs;
using UniqueryPlus.Ipfs;
using Opal.NetApi.Generated.Model.sp_core.crypto;
using Opal.NetApi.Generated.Model.bounded_collections.bounded_vec;
using Newtonsoft.Json;
using System.Collections.Immutable;
using UniqueryPlus.Metadata;
using UniqueryPlus.EVM;
using Nethereum.Contracts;
using Nethereum.Web3;
using Microsoft.VisualStudio.Threading;

namespace UniqueryPlus.Collections
{
    public record OpalCollectionFull : OpalCollection, ICollectionStats, ICollectionCreatedAt, ICollectionTransferable, ICollectionEVMClaimable
    {
        private SubstrateClientExt client;
        public required BigInteger HighestSale { get; set; }
        public required BigInteger FloorPrice { get; set; }
        public required BigInteger Volume { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public OpalCollectionFull(SubstrateClientExt client) : base(client)
        {
            this.client = client;
        }
        public bool IsTransferable { get; set; } = true;
        public Method Transfer(string recipientAddress)
        {
            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(recipientAddress));

            CollectionId uniqueCollectionId = new CollectionId();
            uniqueCollectionId.Value = new U32((uint)CollectionId);

            return UniqueCalls.ChangeCollectionOwner(uniqueCollectionId, accountId);
        }
        public async Task<EventConfig?> GetEventInfoAsync(CancellationToken token)
        {
            var web3 = new Web3(Constants.OPAL_EVM_RPC);
            var contractHandler = web3.Eth.GetContractHandler(UniqueContracts.OPAL_EVENT_MANAGER_CONTRACT_ADDRESS);

            var getEventConfigFunction = new GetEventConfigFunction();
            getEventConfigFunction.CollectionAddress = EVM.Helpers.GetCollectionAddress((uint)CollectionId);
            var getEventConfigOutputDTO = await contractHandler.QueryDeserializingToObjectAsync<GetEventConfigFunction, GetEventConfigOutputDTO>(getEventConfigFunction).WithCancellation(token).ConfigureAwait(false);

            return getEventConfigOutputDTO?.ReturnValue1;
        }

        public Method Claim(string sender) => EVM.Helpers.GetOpalEVMCallMethod(
            sender,
            UniqueContracts.OPAL_EVENT_MANAGER_CONTRACT_ADDRESS,
            OpalCollectionModel.GetCreateTokenFunctionEncoded(EVM.Helpers.GetCollectionAddress((uint)CollectionId), sender),
            0
        );

    }

    public record OpalCollection : ICollectionBase, ICollectionMintConfig, ICollectionNestable
    {
        private SubstrateClientExt client;
        public NftTypeEnum Type => NftTypeEnum.Opal;
        public required BigInteger CollectionId { get; set; }
        public required string Owner { get; set; }
        public required uint NftCount { get; set; }
        public MetadataBase? Metadata { get; set; }
        public uint? NftMaxSuply { get; set; }
        public required MintType MintType { get; set; }
        public BigInteger? MintStartBlock { get; set; }
        public BigInteger? MintEndBlock { get; set; }
        public BigInteger? MintPrice { get; set; }
        public required bool IsNestableByTokenOwner { get; set; }
        public required bool IsNestableByCollectionOwner { get; set; }
        public IEnumerable<BigInteger>? RestrictedByCollectionIds { get; set; }
        public OpalCollection(SubstrateClientExt client)
        {
            this.client = client;
        }
        public async Task<IEnumerable<INftBase>> GetNftsAsync(uint limit, byte[]? lastKey = null, CancellationToken token = default)
        {
            if (NftCount == 0)
            {
                return [];
            }

            var result = await OpalNftModel.GetNftsInCollectionAsync(client, (uint)CollectionId, limit, lastKey, token).ConfigureAwait(false);

            return result.Items;
        }

        public async Task<IEnumerable<INftBase>> GetNftsOwnedByAsync(string owner, uint limit, byte[]? lastKey, CancellationToken token)
        {
            if (NftCount == 0)
            {
                return [];
            }

            var result = await OpalNftModel.GetNftsInCollectionOwnedByAsync(client, (uint)CollectionId, owner, limit, lastKey, token).ConfigureAwait(false);

            return result.Items;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<ICollectionBase> GetFullAsync(CancellationToken token)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return this;
        }
    }

    public class OpalCollectionModel
    {
        internal static async Task<ICollectionBase> GetCollectionByCollectionIdAsync(SubstrateClientExt client, uint collectionId, CancellationToken token)
        {
            CollectionId uniqueCollectionId = new CollectionId();
            uniqueCollectionId.Value = new U32(collectionId);

            var collectionIdKey = CommonStorage.CollectionByIdParams(uniqueCollectionId).Substring(Constants.BASE_STORAGE_KEY_LENGTH);

            var result = await GetCollectionsByIdKeysAsync(client, [collectionIdKey], "", token).ConfigureAwait(false);

            return result.Items.First();
        }

        internal static async Task<RecursiveReturn<ICollectionBase>> GetCollectionsByIdKeysAsync(SubstrateClientExt client, IEnumerable<string> collectionIdKeys, string lastKey, CancellationToken token)
        {
            var collectionIds = collectionIdKeys.Select(Helpers.GetBigIntegerFromBlake2_128Concat);

            var collectionDetails = await GetCollectionCollectionByCollectionIdKeysAsync(client, collectionIdKeys, token).ConfigureAwait(false);
            var collectionMetadatas = await GetCollectionMetadataNftsPalletByCollectionIdKeysAsync(client, collectionIdKeys, token).ConfigureAwait(false);
            var nftCounts = await GetTotalCountOfNftsInCollectionByCollectionIdKeysAsync(client, collectionIds, token).ConfigureAwait(false);

            return new RecursiveReturn<ICollectionBase>
            {
                Items = collectionIds.Zip(collectionDetails, (BigInteger collectionId, Collection? details) =>
                {
                    // Code the check the Sponsorship AccountId
                    /*
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    if (details?.Sponsorship?.Value == SponsorshipState.Unconfirmed)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        Console.WriteLine("Sponsorship: " + ((AccountId)details?.Sponsorship?.Value2).Value);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    */

                    return details switch
                    {
                        // Should never be null
                        null => new OpalCollection(client)
                        {
                            CollectionId = collectionId,
                            Owner = "Unknown",
                            NftCount = 0,
                            MintType = new MintType
                            {
                                Type = MintTypeEnum.Unknown,
                                CollectionId = null
                            },
                            MintPrice = default,
                            IsNestableByTokenOwner = false,
                            IsNestableByCollectionOwner = false,
                        },
                        _ => new OpalCollection(client)
                        {
                            CollectionId = collectionId,
                            Owner = Utils.GetAddressFrom(details.Owner.Encode()),
                            NftCount = 0, // Unknown at this point
                            Metadata = new MetadataBase
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
                            NftMaxSuply = details.Limits.TokenLimit.OptionFlag ? (uint)details.Limits.TokenLimit.Value : null,
                            MintPrice = null,
                            MintEndBlock = null,
                            MintStartBlock = null,
                            IsNestableByTokenOwner = details.Permissions.Nesting.OptionFlag ? details.Permissions.Nesting.Value.TokenOwner : false,
                            IsNestableByCollectionOwner = details.Permissions.Nesting.OptionFlag ? details.Permissions.Nesting.Value.CollectionAdmin : false,
                            RestrictedByCollectionIds = details.Permissions.Nesting.OptionFlag ? (details.Permissions.Nesting.Value.Restricted.OptionFlag ? details.Permissions.Nesting.Value.Restricted.Value.Value.Value.Value.Value.Select(collectionId => (BigInteger)collectionId.Value.Value) : null) : null,
                        },
                    };
                }).Zip(nftCounts, (OpalCollection collectionBase, uint nftCount) =>
                {
                    collectionBase.NftCount = nftCount;
                    return collectionBase;
                }).Zip(collectionMetadatas, (OpalCollection collectionBase, MetadataBase? metadata) =>
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

            var collectionKeysArray = collectionIdKeys.ToImmutableArray();
            var collectionCollectionKeys = collectionIdKeys.Select(collectionIdKey => Utils.HexToByteArray(keyPrefix + collectionIdKey)).ToList();

            var storageChangeSets = await client.State.GetQueryStorageAtAsync(collectionCollectionKeys, string.Empty, token).ConfigureAwait(false);

            Dictionary<string, string[]> collectionStorageSets = new Dictionary<string, string[]>();

            int i = 0;
            foreach (var storageChangeSet in storageChangeSets.First().Changes)
            {
                while (collectionStorageSets.ContainsKey(collectionKeysArray[i]))
                {
                    i++;
                }
                collectionStorageSets.Add(collectionKeysArray[i], storageChangeSet);

                i++;
            }


            return collectionKeysArray.Select(key =>
            {
                var change = collectionStorageSets[key];

                if (change[1] == null)
                {
                    return null;
                }

                var collectionDetails = new Collection();
                collectionDetails.Create(change[1]);

                return collectionDetails;
            });
        }
        internal static async Task<IEnumerable<MetadataBase?>> GetCollectionMetadataNftsPalletByCollectionIdKeysAsync(SubstrateClientExt client, IEnumerable<string> collectionIdKeys, CancellationToken token)
        {
            CollectionId uniqueCollectionId = new CollectionId();
            uniqueCollectionId.Value = new U32(0);

            var keyPrefix = CommonStorage.CollectionPropertiesParams(uniqueCollectionId).Substring(0, Constants.BASE_STORAGE_KEY_LENGTH);

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

                var collectionProperties = new PropertiesT1();
                collectionProperties.Create(change[1]);

                try
                {
                    var ipfsCollectionProperty = collectionProperties.Map.Value.Value.Value.Value.Where(collectionProperty => new string[] { "baseURI", "coverPicture.ipfsCid", "collectionInfo", "coverPicture.url" }.Contains(System.Text.Encoding.UTF8.GetString(Helpers.RemoveCompactIntegerPrefix(((BoundedVecT12)collectionProperty.Value[0]).Value.Encode())))).First();

                    var ipfsLink = System.Text.Encoding.UTF8.GetString(Helpers.RemoveCompactIntegerPrefix(((BoundedVecT12)ipfsCollectionProperty.Value[0]).Value.Encode())) switch
                    {
                        "coverPicture.ipfsCid" => System.Text.Encoding.UTF8.GetString(Helpers.RemoveCompactIntegerPrefix(((BoundedVecT14)ipfsCollectionProperty.Value[1]).Value.Encode())),
                        "baseURI" => System.Text.Encoding.UTF8.GetString(Helpers.RemoveCompactIntegerPrefix(((BoundedVecT14)ipfsCollectionProperty.Value[1]).Value.Encode())),
                        "coverPicture.url" => System.Text.Encoding.UTF8.GetString(Helpers.RemoveCompactIntegerPrefix(((BoundedVecT14)ipfsCollectionProperty.Value[1]).Value.Encode())),
                        "collectionInfo" => JsonConvert.DeserializeObject<UniqueCollectionMetadataV2>(System.Text.Encoding.UTF8.GetString(Helpers.RemoveCompactIntegerPrefix(((BoundedVecT14)ipfsCollectionProperty.Value[1]).Value.Encode())))?.CoverImage.Url,
                        _ => null
                    };

                    Console.WriteLine("ipfsLink: " + ipfsLink);
                    Console.WriteLine(ipfsLink is null);

                    if (ipfsLink is null)
                    {
                        metadatas.Add(null);
                        continue;
                    }

                    metadatas.Add(new MetadataBase
                    {
                        Image = IpfsModel.ToIpfsLink(ipfsLink.Replace("\"", ""), ipfsEndpoint: Constants.UNIQUE_IPFS_ENDPOINT),
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unexpected UniqueryPlus exception: " + ex.ToString());
                    metadatas.Add(new MetadataBase
                    {
                    });
                }
            };

            return metadatas;
        }

        internal static async Task<int> GetTotalCountOfNftsInCollectionOnChainAsync(SubstrateClientExt client, uint collectionId, CancellationToken token)
        {
            var fullKeys = await OpalNftModel.GetNftsInCollectionFullKeysAsync(client, collectionId, 1000, null, token).ConfigureAwait(false);

            return fullKeys.Count();
        }

        internal static async Task<IEnumerable<uint>> GetTotalCountOfNftsInCollectionByCollectionIdKeysAsync(SubstrateClientExt client, IEnumerable<BigInteger> collectionIds, CancellationToken token)
        {
            List<uint> nftCounts = new List<uint>();
            var opalSubqueryClient = Indexers.GetOpalSubqueryClient();

            foreach (BigInteger collectionId in collectionIds)
            {
                var result = await opalSubqueryClient.GetNftsInCollection.ExecuteAsync((double)collectionId, 0, 0).ConfigureAwait(false);

                if (
                    result is null ||
                    result.Errors.Count > 0 ||
                    result.Data is null
                )
                {
                    // Fallback to on-chain count (Very slow and inefficient)
                    nftCounts.Add((uint)await GetTotalCountOfNftsInCollectionOnChainAsync(client, (uint)collectionId, token).ConfigureAwait(false));
                    continue;
                }

                nftCounts.Add((uint)result.Data.Tokens.Count);
            }

            return nftCounts;
        }

        internal static byte[] GetCreateTokenFunctionEncoded(string collectionAddress, string receiverAddress)
        {
            var createTokenFunction = new CreateTokenFunction();
            createTokenFunction.CollectionAddress = collectionAddress;
            createTokenFunction.Owner = EVM.Helpers.ToCrossAddress(receiverAddress);

            return createTokenFunction.GetCallData();
        }
    }
}

using System;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Constants;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi;
using Newtonsoft.Json.Linq;
using PlutoWallet.NetApiExt.Generated.Model.pallet_nfts.types;
using PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto;
using Newtonsoft.Json;
using Substrate.NetApi.Model.Rpc;
using static Substrate.NetApi.Model.Meta.Storage;
using Substrate.NetApi.Model.Types.Primitive;
using Uniquery;
using System.Numerics;

namespace PlutoWallet.Model
{
	public class NFT
	{
        public string Name { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
        [JsonProperty("animation_url")]
        public string AnimationUrl { get; set; }
        public string[] Attributes { get; set; }
        [JsonProperty("external_url")]
        public string ExternalUrl { get; set; }
        public string Type { get; set; }
        public BigInteger CollectionId { get; set; }
        public BigInteger ItemId { get; set; }
        public Endpoint Endpoint { get; set; }

        public override bool Equals(object obj)
        {
            if (!obj.GetType().Equals(typeof(NFT)))
            {
                return false;
            }

            var objNft = (NFT)obj;

            return (objNft.Name == this.Name &&
                objNft.Description == this.Description &&
                objNft.Image == this.Image &&
                objNft.Endpoint.Name == this.Endpoint.Name);
        }

        public override string ToString()
        {
            return Name + " - " + Image;
        }
    }

	public class NFTsModel
	{
        public static async Task<List<NFT>> AddNFTsAsync(Endpoint endpoint)
        {
            return await GetNFTsAsync(endpoint);
        }

        public static async Task<List<NFT>> GetNFTsAsync(Endpoint endpoint)
        {
            var client = new AjunaClientExt(new Uri(endpoint.URL), ChargeTransactionPayment.Default());

            await client.ConnectAsync();

            List<NFT> nfts = new List<NFT>();

            try
            {
                List<string> collectionItemIds = await GetNftsAccountAsync(client, CancellationToken.None);

                foreach (string collectionItemId in collectionItemIds)
                {
                    NFT nft = await GetNftMetadataAsync(client, collectionItemId);
                    if (nft != null)
                    {
                        SetNftIds(ref nft, collectionItemId);

                        nft.Endpoint = endpoint;

                        nfts.Add(nft);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                // Later do something about this
            }

            try
            {
                List<string> uniquesCollectionItemIds = await GetUniquesAccountAsync(client, CancellationToken.None);

                foreach (string collectionItemId in uniquesCollectionItemIds)
                {
                    NFT nft = await GetUniquesMetadataAsync(client, collectionItemId);

                    if (nft != null)
                    {
                        SetNftIds(ref nft, collectionItemId);

                        nft.Endpoint = endpoint;

                        nfts.Add(nft);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                // Later do something about this
            }

            client.Dispose();

            return nfts;
        }

        public static List<NFT> GetMockNFTs()
        {
            var nfts = new List<NFT>()
            {
                new NFT
                {
                    Name = "Mock nft - version ALPHA",
                    Description = @"This is a totally mock NFT that does nothing.
Hopefully it will fulfill the test functionalities correctly.",
                    Endpoint = new Endpoint
                    {
                        Name = "Mock network",
                        Icon = "plutowalleticon.png",
                    },
                    Image = "dusan.jpg"
                }
            };

            return nfts;
        }

        private static async Task<NFT> GetNftMetadataAsync(AjunaClientExt client, string collectionItemId)
        {
            var parameters = Utils.Bytes2HexString(RequestGenerator.GetStorageKeyBytesHash("Nfts", "ItemMetadataOf")) + collectionItemId;

            ItemMetadata result = await client.GetStorageAsync<ItemMetadata>(parameters, CancellationToken.None);

            string ipfsLink = System.Text.Encoding.UTF8.GetString(result.Data.Value.Bytes);

            string metadataJson = await Model.IpfsModel.FetchIpfsAsync(ipfsLink);

            NFT nft = JsonConvert.DeserializeObject<NFT>(metadataJson);

            nft.Image = Model.IpfsModel.ToIpfsLink(nft.Image);

            return nft;
        }

        private static async Task<List<string>> GetNftsAccountAsync(AjunaClientExt client, CancellationToken token)
        {
            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(Model.KeysModel.GetSubstrateKey()));

            var keyBytes = RequestGenerator.GetStorageKeyBytesHash("Nfts", "Account");

            byte[] prefix = keyBytes.Concat(HashExtension.Hash(Hasher.BlakeTwo128Concat, account32.Encode())).ToArray();
            byte[] startKey = null;

            List<string[]> storageChanges = new List<string[]>();

            while (true)
            {
                var keysPaged = await client.State.GetKeysPagedAtAsync(prefix, 1000, startKey, string.Empty, token);

                if (keysPaged == null || !keysPaged.Any())
                {
                    break;
                }
                else
                {
                    var tt = await client.State.GetQueryStorageAtAsync(keysPaged.Select(p => Utils.HexToByteArray(p.ToString())).ToList(), string.Empty, token);
                    storageChanges.AddRange(new List<string[]>(tt.ElementAt(0).Changes));
                    startKey = Utils.HexToByteArray(tt.ElementAt(0).Changes.Last()[0]);
                }
            }

            var collectionItemIdsList = new List<string>();

            if (storageChanges != null)
            {
                foreach (var storageChangeSet in storageChanges)
                {
                    collectionItemIdsList.Add(storageChangeSet[0].Remove(0, Utils.Bytes2HexString(prefix).Length));
                    Console.WriteLine(storageChangeSet[0].Remove(0, Utils.Bytes2HexString(prefix).Length));
                }
            }
            return collectionItemIdsList;
        }
        

        private static async Task<NFT> GetUniquesMetadataAsync(AjunaClientExt client, string collectionItemId)
        {
            try
            {
                var parameters = Utils.Bytes2HexString(RequestGenerator.GetStorageKeyBytesHash("Uniques", "InstanceMetadataOf")) + collectionItemId;

                var result = await client.GetStorageAsync<PlutoWallet.NetApiExt.Generated.Model.pallet_uniques.types.ItemMetadata>(parameters, CancellationToken.None);

                string ipfsLink = System.Text.Encoding.UTF8.GetString(result.Data.Value.Bytes);

                string metadataJson = await Model.IpfsModel.FetchIpfsAsync(ipfsLink);
            
                NFT nft = JsonConvert.DeserializeObject<NFT>(metadataJson);

                nft.Image = Model.IpfsModel.ToIpfsLink(nft.Image);

                return nft;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private static async Task<List<string>> GetUniquesAccountAsync(AjunaClientExt client, CancellationToken token)
        {
            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(Model.KeysModel.GetSubstrateKey()));

            var keyBytes = RequestGenerator.GetStorageKeyBytesHash("Uniques", "Account");

            byte[] prefix = keyBytes.Concat(HashExtension.Hash(Hasher.BlakeTwo128Concat, account32.Encode())).ToArray();
            
            byte[] startKey = null;

            List<string[]> storageChanges = new List<string[]>();

            while (true)
            {
                var keysPaged = await client.State.GetKeysPagedAtAsync(prefix, 1000, startKey, string.Empty, token);

                if (keysPaged == null || !keysPaged.Any())
                {
                    break;
                }
                else
                {
                    var tt = await client.State.GetQueryStorageAtAsync(keysPaged.Select(p => Utils.HexToByteArray(p.ToString())).ToList(), string.Empty, token);
                    storageChanges.AddRange(new List<string[]>(tt.ElementAt(0).Changes));
                    startKey = Utils.HexToByteArray(tt.ElementAt(0).Changes.Last()[0]);
                }
            }

            var collectionItemIdsList = new List<string>();

            if (storageChanges != null)
            {
                foreach (var storageChangeSet in storageChanges)
                {
                    collectionItemIdsList.Add(storageChangeSet[0].Remove(0, Utils.Bytes2HexString(prefix).Length));
                    Console.WriteLine(storageChangeSet[0].Remove(0, Utils.Bytes2HexString(prefix).Length));
                }
            }
            return collectionItemIdsList;
        }

        private static void SetNftIds (ref NFT nft, string keyHash)
        {
            if (keyHash.Length == 80)
            {
                nft.CollectionId = HashModel.GetU32FromBlake2_128Concat(keyHash.Substring(0, 40)).Value;
                nft.ItemId = HashModel.GetU32FromBlake2_128Concat(keyHash.Substring(40, 40)).Value;
            }
            if (keyHash.Length == 96)
            {
                nft.CollectionId = HashModel.GetU64FromBlake2_128Concat(keyHash.Substring(0, 48)).Value;
                nft.ItemId = HashModel.GetU64FromBlake2_128Concat(keyHash.Substring(48, 48)).Value;
            }
            if (keyHash.Length == 128)
            {
                nft.CollectionId = HashModel.GetU128FromBlake2_128Concat(keyHash.Substring(0, 64)).Value;
                nft.ItemId = HashModel.GetU128FromBlake2_128Concat(keyHash.Substring(64, 64)).Value;
            }
        }
    }
}


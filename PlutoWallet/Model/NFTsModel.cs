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
    }

	public class NFTsModel
	{
        public static async Task<List<NFT>> GetNFTsAsync(Endpoint endpoint)
        {
            var client = new AjunaClientExt(new Uri(endpoint.URL), ChargeTransactionPayment.Default());

            await client.ConnectAsync();

            List<string> collectionItemIds = await GetAccountNftsAsync(client, null, "11d2df4e979aa105cf552e9544ebd2b500000000", 1000, CancellationToken.None);

            List<NFT> nfts = new List<NFT>();

            foreach (string collectionItemId in collectionItemIds)
            {
                nfts.Add(await GetNftMetadataAsync(client, collectionItemId));
            }

            List<string> uniquesCollectionItemIds = await GetAccountUniquesAsync(client, null, "11d2df4e979aa105cf552e9544ebd2b500000000", 1000, CancellationToken.None);

            foreach (string collectionItemId in uniquesCollectionItemIds)
            {
                nfts.Add(await GetUniquesMetadataAsync(client, collectionItemId));
            }

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

        private static async Task<List<string>> GetAccountNftsAsync(AjunaClientExt client, string startStorageKey, string collectionId, uint page, CancellationToken token)
        {
            if (page < 2 || page > 1000)
            {
                throw new NotSupportedException("Page size must be in the range of 2 - 1000");
            }

            var startKeyBytes = new byte[] { };
            if (startStorageKey != null)
            {
                startKeyBytes = Utils.HexToByteArray(startStorageKey);
            }

            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(Model.KeysModel.GetSubstrateKey()));

            var collectionItemIdsList = new List<string>();

            var keyBytes = RequestGenerator.GetStorageKeyBytesHash("Nfts", "Item");

            string prefix = Utils.Bytes2HexString(keyBytes) + collectionId;

            var storageKeys = await client.State.GetKeysPagedAtAsync(keyBytes, page, null, string.Empty, token);

            if (storageKeys == null || !storageKeys.Any())
            {
                return collectionItemIdsList;
            }

            var storageChangeSets = await client.State.GetQueryStorageAtAsync(storageKeys.Select(p => Utils.HexToByteArray(p.ToString())).ToList(), string.Empty, token);

            if (storageChangeSets != null)
            {
                foreach (var storageChangeSet in storageChangeSets.ElementAt(0).Changes)
                {
                    var itemDetails = new ItemDetails();
                    itemDetails.Create(storageChangeSet[1]);

                    if (account32.Value.ToString() == itemDetails.Owner.Value.ToString()) 
                    {
                        string storageKeyString = storageChangeSet[0];

                        collectionItemIdsList.Add(storageKeyString.Remove(0, Utils.Bytes2HexString(keyBytes).Length));
                    }
                }
            }
            return collectionItemIdsList;
        } 

        private static async Task<List<string>> GetNFTCollectionIds(AjunaClientExt client, string startStorageKey, uint page, CancellationToken token)
        {
            if (page < 2 || page > 1000)
            {
                throw new NotSupportedException("Page size must be in the range of 2 - 1000");
            }

            var startKeyBytes = new byte[] { };
            if (startStorageKey != null)
            {
                startKeyBytes = Utils.HexToByteArray(startStorageKey);
            }

            var collectionIdsList = new List<string>();

            var keyBytes = RequestGenerator.GetStorageKeyBytesHash("Nfts", "Collection");

            string prefix = Utils.Bytes2HexString(keyBytes);
            var storageKeys = await client.State.GetKeysPagedAtAsync(keyBytes, page, null, string.Empty, token);
            

            if (storageKeys == null || !storageKeys.Any())
            {
                return collectionIdsList;
            }

            var storageChangeSets = await client.State.GetQueryStorageAtAsync(storageKeys.Select(p => Utils.HexToByteArray(p.ToString())).ToList(), string.Empty, token);

            if (storageChangeSets != null)
            {
                foreach (var storageChangeSet in storageChangeSets.ElementAt(0).Changes)
                {
                    string storageKeyString = storageChangeSet[0];

                    var collectionDetails = new CollectionDetails();
                    collectionDetails.Create(storageChangeSet[1]);

                    collectionIdsList.Add(storageKeyString.Remove(0, prefix.Length));
                }
            }

            return collectionIdsList;
        }

        private static async Task<NFT> GetUniquesMetadataAsync(AjunaClientExt client, string collectionItemId)
        {
            Console.WriteLine("Fetching");
            var parameters = Utils.Bytes2HexString(RequestGenerator.GetStorageKeyBytesHash("Uniques", "InstanceMetadataOf")) + collectionItemId;

            var result = await client.GetStorageAsync<PlutoWallet.NetApiExt.Generated.Model.pallet_uniques.types.ItemMetadata>(parameters, CancellationToken.None);

            string ipfsLink = System.Text.Encoding.UTF8.GetString(result.Data.Value.Bytes);
            string metadataJson = await Model.IpfsModel.FetchIpfsAsync(ipfsLink);

            Console.WriteLine("Uniques: " + metadataJson);
            NFT nft = JsonConvert.DeserializeObject<NFT>(metadataJson);

            nft.Image = Model.IpfsModel.ToIpfsLink(nft.Image);

            return nft;
        }

        private static async Task<List<string>> GetAccountUniquesAsync(AjunaClientExt client, string startStorageKey, string collectionId, uint page, CancellationToken token)
        {
            if (page < 2 || page > 1000)
            {
                throw new NotSupportedException("Page size must be in the range of 2 - 1000");
            }

            var startKeyBytes = new byte[] { };
            if (startStorageKey != null)
            {
                startKeyBytes = Utils.HexToByteArray(startStorageKey);
            }

            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(Model.KeysModel.GetSubstrateKey()));

            var collectionItemIdsList = new List<string>();

            var keyBytes = RequestGenerator.GetStorageKeyBytesHash("Uniques", "Asset");

            string prefix = Utils.Bytes2HexString(keyBytes) + collectionId;

            byte[] startKey = null;

            List<string[]> storageChanges = new List<string[]>();

            while (true)
            {
                var keysPaged = await client.State.GetKeysPagedAtAsync(keyBytes, page, startKey, string.Empty, token);

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

            

            if (storageChanges != null)
            {
                foreach (var storageChangeSet in storageChanges)
                {
                    Console.WriteLine("Found something");
                    var itemDetails = new PlutoWallet.NetApiExt.Generated.Model.pallet_uniques.types.ItemDetails();
                    itemDetails.Create(storageChangeSet[1]);

                    if (account32.Value.ToString() == itemDetails.Owner.Value.ToString())
                    {
                        string storageKeyString = storageChangeSet[0];

                        collectionItemIdsList.Add(storageKeyString.Remove(0, Utils.Bytes2HexString(keyBytes).Length));
                    }
                }
            }
            return collectionItemIdsList;
        }

    }
}


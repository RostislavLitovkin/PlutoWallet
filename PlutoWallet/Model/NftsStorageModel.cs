using System;
using System.Text.Json;
using PlutoWallet.Constants;
using System.Numerics;
using PlutoWallet.Components.Nft;

namespace PlutoWallet.Model
{
	public class NftsStorageModel
	{
        public static void AddFavourite(StorageNFT newFavouriteNft)
        {
            string json = Preferences.Get("favouriteNfts", "");

            List<StorageNFT> favouriteNfts = json != "" ? JsonSerializer.Deserialize<List<StorageNFT>>(json) : new List<StorageNFT>();

            favouriteNfts.Add(newFavouriteNft);

            Preferences.Set("favouriteNfts", JsonSerializer.Serialize(favouriteNfts));

            var nftGaleryViewModel = DependencyService.Get<NftGaleryViewModel>();
            nftGaleryViewModel.Reload(StorageNFTsListToNFTsList(favouriteNfts));
        }

        public static void RemoveFavourite(StorageNFT deleteFavouriteNft)
        {
            string json = Preferences.Get("favouriteNfts", "");

            List<StorageNFT> favouriteNfts = json != "" ? JsonSerializer.Deserialize<List<StorageNFT>>(json) : new List<StorageNFT>();

            for(int i = 0; i < favouriteNfts.Count(); i++)
            {
                if (favouriteNfts[i].Equals(deleteFavouriteNft))
                {
                    favouriteNfts.RemoveAt(i);

                    i--;
                }
            }

            Preferences.Set("favouriteNfts", JsonSerializer.Serialize(favouriteNfts));

            var nftGaleryViewModel = DependencyService.Get<NftGaleryViewModel>();
            nftGaleryViewModel.Reload(StorageNFTsListToNFTsList(favouriteNfts));
        }

        public static List<NFT> GetFavouriteNFTs()
        {
            string json = Preferences.Get("favouriteNfts", "");

            Console.WriteLine("NFTS favourite saved:");
            Console.WriteLine(json);

            var storageNfts = json != "" ? JsonSerializer.Deserialize<List<StorageNFT>>(json) : new List<StorageNFT>();

            return StorageNFTsListToNFTsList(storageNfts);
        }

        private static List<NFT> StorageNFTsListToNFTsList(List<StorageNFT> storageNfts)
        {
            List<NFT> result = new List<NFT>();

            foreach (var storageNft in storageNfts)
            {
                result.Add(new NFT
                {
                    Name = storageNft.Name,
                    Description = storageNft.Description,
                    Image = storageNft.Image,
                    Favourite = storageNft.Favourite,
                    Attributes = storageNft.Attributes,
                    Endpoint = Endpoints.GetEndpointDictionary[storageNft.EndpointKey],
                    CollectionId = BigInteger.Parse(storageNft.CollectionId),
                    ItemId = BigInteger.Parse(storageNft.ItemId),
                });
            }

            return result;
        }

        public class StorageNFT
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Image { get; set; }
            public string Type { get; set; }
            public string CollectionId { get; set; }
            public string ItemId { get; set; }
            public EndpointEnum EndpointKey { get; set; }
            public object[] Attributes { get; set; }
            public bool Favourite { get; set; } = false;

            public override bool Equals(object obj)
            {
                if (!obj.GetType().Equals(typeof(StorageNFT)))
                {
                    return false;
                }

                var objNft = (StorageNFT)obj;

                return (objNft.Name == this.Name &&
                    objNft.Description == this.Description &&
                    objNft.Image == this.Image &&
                    objNft.EndpointKey == this.EndpointKey);
            }

            public override string ToString()
            {
                return Name + " - " + Image;
            }
        }

        public NftsStorageModel()
		{

		}
	}
}


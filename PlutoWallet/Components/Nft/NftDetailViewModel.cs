using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using System.Numerics;
using UniqueryPlus.Collections;
using UniqueryPlus.Nfts;

namespace PlutoWallet.Components.Nft
{
    public partial class NftDetailViewModel : ObservableObject
    {
        private INftBase nftBase;

        public INftBase NftBase
        {
            set
            {
                Name = value.Metadata.Name;
                Description = value.Metadata.Description;
                Image = value.Metadata.Image;
                nftBase = value;
            }
            get => nftBase;
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private string image;

        [ObservableProperty]
        private string ownerAddress;

        [ObservableProperty]
        private Endpoint endpoint;

        [ObservableProperty]
        private object[] attributes;

        [ObservableProperty]
        private BigInteger collectionId;

        [ObservableProperty]
        private BigInteger itemId;

        [ObservableProperty]
        private bool favourite;

        [ObservableProperty]
        private Option<string> kodadotUnlockableUrl;

        [ObservableProperty]
        private string azeroIdReservedUntil;

        [ObservableProperty]
        private string[] collectionNftImages;

        [ObservableProperty]
        private bool collectionFavourite;

        [ObservableProperty]
        private ICollectionBase collectionBase;

        [ObservableProperty]
        private bool uniqueIsVisible;

        [ObservableProperty]
        private bool kodaIsVisible;

        [ObservableProperty]
        private bool isTransferable = false;

        [ObservableProperty]
        private bool isSellable = false;

        [ObservableProperty]
        private bool isModifiable = false;

        [ObservableProperty]
        private bool isBurnable = false;
        public NftDetailViewModel()
        {
        }
    }
}

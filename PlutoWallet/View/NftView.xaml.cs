using PlutoWallet.Model;
using PlutoWallet.Constants;
using PlutoWallet.Components.Nft;
using PlutoWallet.ViewModel;
using System.Windows.Input;

namespace PlutoWallet.View;

public partial class NftView : ContentView
{
    private List<NFT> savedNfts = new List<NFT>();

	public NftView()
	{
		InitializeComponent();

        GetNFTsAsync();
    }

    /**
	* Called in the BasePageViewModel
	*/
    public async Task GetNFTsAsync()
    {
        if (((NftViewModel)this.BindingContext).IsLoading)
        {
            return;
        }

        //UpdateNfts(Model.NFTsModel.GetMockNFTs());

        ((NftViewModel)this.BindingContext).IsLoading = true;

        foreach (Endpoint endpoint in Endpoints.GetAllEndpoints)
        {
            if (endpoint.SupportsNfts)
            {
                UpdateNfts(await Model.NFTsModel.GetNFTsAsync(endpoint));
            }
        }

        UpdateNfts(await Model.UniqueryModel.GetAccountRmrk());

        ((NftViewModel)this.BindingContext).IsLoading = false;
    }

    public void UpdateNfts(List<NFT> newNfts)
    {
        foreach (NFT newNft in newNfts)
        {
            bool isContained = false;
            foreach (NFT savedNft in savedNfts)
            {
                if (savedNft.Equals(newNft))
                {
                    isContained = true;
                }
            }

            // if not contained, add the NFT to the layout and saved list
            if (!isContained)
            {
                savedNfts.Add(newNft);

                stackLayout.Children.Add(new NftThumbnailView
                {
                    Name = newNft.Name,
                    Description = newNft.Description,
                    Image = newNft.Image,
                    Endpoint = newNft.Endpoint
                });
                //((NftViewModel)this.BindingContext).Nfts.Add(newNft);
            }
        }
    }
}

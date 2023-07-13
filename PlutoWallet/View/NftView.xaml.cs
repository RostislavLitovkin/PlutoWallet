using PlutoWallet.Model;
using PlutoWallet.Constants;
using PlutoWallet.Components.Nft;

namespace PlutoWallet.View;

public partial class NftView : ContentView
{
    private List<NFT> savedNfts = new List<NFT>();

	public NftView()
	{
		InitializeComponent();
	}

    /**
	* Called in the BasePageViewModel
	*/
    public async Task GetNFTsAsync()
    {
        UpdateNfts(Model.NFTsModel.GetMockNFTs());

        foreach (Endpoint endpoint in Endpoints.GetAllEndpoints)
        {
            if (endpoint.SupportsNfts)
            {
                Model.NFTsModel.AddNFTsAsync(endpoint, UpdateNfts);
            }
        }

        Model.UniqueryModel.AddRmrkNfts(UpdateNfts);
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
            }
        }
    }
}

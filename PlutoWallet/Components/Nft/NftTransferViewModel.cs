using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.Buttons;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using UniqueryPlus.Nfts;

namespace PlutoWallet.Components.Nft
{
    internal partial class NftTransferViewModel : ObservableObject, IPopup, ISetToDefault
    {
        [ObservableProperty]
        private ButtonStateEnum confirmButtonState;

        private string address;

        public string Address { get => address; set { SetProperty(ref address, value); CheckCorrectInput(); } }

        [ObservableProperty]
        private string addressInput;

        [ObservableProperty]
        private string fee;

        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private INftBase nftBase;

        [ObservableProperty]
        private EndpointEnum endpointKey;
        public NftTransferViewModel()
        {
            SetToDefault();
        }

        public void CheckCorrectInput()
        {
            if (Address.Length == 48)
            {
                ConfirmButtonState = ButtonStateEnum.Enabled;
            }
            else
            {
                ConfirmButtonState = ButtonStateEnum.Disabled;
            }
        }

        public async Task GetFeeAsync(EndpointEnum endpointKey, INftBase nftBase)
        {
            Fee = "Estimated fee: Loading";

            var client = await Model.AjunaClientModel.GetOrAddSubstrateClientAsync(endpointKey);
            if (client is null || !await client.IsConnectedAsync() || nftBase is not INftTransferable)
            {
                Fee = "Estimated fee: Failed";

                return;
            }

            try
            {
                var transfer = ((INftTransferable)nftBase).Transfer("5DDMVdn5Ty1bn93RwL3AQWsEhNe45eFdx3iVhrTurP9HKrsJ");
                var feeAsset = await FeeModel.GetMethodFeeAsync(client, transfer);
                Fee = FeeModel.GetEstimatedFeeString(feeAsset);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nft estimated fee error: ");

                Console.WriteLine(ex);
                Fee = "Estimated fee: Unsupported";
            }
        }

        public void SetToDefault()
        {
            Address = "";
            AddressInput = "";
            IsVisible = false;
            Fee = "Estimated fee: Loading";
            ConfirmButtonState = ButtonStateEnum.Disabled;
        }
    }
}

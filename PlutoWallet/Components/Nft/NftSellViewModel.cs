﻿using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.Buttons;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.HydraDX;
using UniqueryPlus.Nfts;

namespace PlutoWallet.Components.Nft
{
    internal partial class NftSellViewModel : ObservableObject, IPopup, ISetToDefault
    {
        [ObservableProperty]
        private ButtonStateEnum confirmButtonState;

        private string amount;
        public string Amount { get => amount; set { Console.WriteLine("Setting amount: " + value); SetProperty(ref amount, value); CheckCorrectInput(); } }

        [ObservableProperty]
        private string fee;

        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private INftBase nftBase;

        [ObservableProperty]
        private Endpoint endpoint;

        private bool amountLastActive = true;

        [ObservableProperty]
        private string usdAmount;

        [ObservableProperty]
        private string usdAmountPlaceholder = "USD amount";

        [ObservableProperty]
        private bool usdAmountEnabled = true;
        public NftSellViewModel()
        {
            SetToDefault();
        }

        public void CheckCorrectInput()
        {
            if (decimal.TryParse(Amount, out decimal decimalAmount) && decimalAmount > 0)
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
            if (client is null || !await client.IsConnectedAsync() || nftBase is not INftSellable)
            {
                Fee = "Estimated fee: Failed";

                return;
            }

            try
            {
                var transfer = ((INftSellable)nftBase).Sell(1);
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
            Amount = "";
            IsVisible = false;
            Fee = "Estimated fee: Loading";
            ConfirmButtonState = ButtonStateEnum.Disabled;

            Amount = "";
            UsdAmount = "";
        }

        public void CalculateUsdValue(string symbol)
        {
            if (decimal.TryParse(Amount, out decimal decimalAmount) && decimalAmount > 0)
            {
                var price = Sdk.GetSpotPrice(symbol);

                if (price != 0)
                {
                    UsdAmountEnabled = true;
                    UsdAmountPlaceholder = "USD amount";

                    var usdAmount = String.Format("{0:0.00}", decimalAmount * (decimal)price);

                    UsdAmount = usdAmount;
                }
                else
                {
                    UsdAmountEnabled = false;
                    UsdAmountPlaceholder = "USD amount (N/A)";

                    UsdAmount = "";
                }
            }
            else
            {
                UsdAmount = "";
            }
        }

        public void CalculateCurrencyAmount()
        {
            if (decimal.TryParse(UsdAmount, out decimal decimalAmount) && decimalAmount > 0)
            {
                var price = Sdk.GetSpotPrice(Endpoint.Unit);

                if (price != 0)
                {
                    var amount = String.Format("{0:0.00000}", decimalAmount / (decimal)price);

                    Amount = amount;
                }
                else
                {
                    Amount = "";
                }
            }
            else
            {
                Amount = "";
            }
        }
    }
}

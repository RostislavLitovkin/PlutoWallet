using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model.HydraDX;

namespace PlutoWallet.Components.AssetSelect
{
    partial class AssetInputViewModel : ObservableObject
    {
        private bool amountLastActive = true;

        [ObservableProperty]
        private string amount;


        [ObservableProperty]
        private string usdAmount;

        [ObservableProperty]
        private string usdAmountPlaceholder = "USD amount";

        [ObservableProperty]
        private bool usdAmountEnabled = true;

        public void CurrencyChanged(string symbol)
        {
            var price = Sdk.GetSpotPrice(symbol);
            if (price == 0)
            {
                UsdAmountEnabled = false;
                UsdAmountPlaceholder = "USD amount (N/A)";

                UsdAmount = "";
                return;
            }

            UsdAmountEnabled = true;
            UsdAmountPlaceholder = "USD amount";

            Amount = Amount;

            CalculateUsdValue(symbol);
        }

        public void CalculateUsdValue()
        {
            var assetSelectButtonViewModel = DependencyService.Get<AssetSelectButtonViewModel>();

            CalculateUsdValue(assetSelectButtonViewModel.Symbol);
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
            var assetSelectButtonViewModel = DependencyService.Get<AssetSelectButtonViewModel>();

            if (decimal.TryParse(UsdAmount, out decimal decimalAmount) && decimalAmount > 0)
            {
                var price = Sdk.GetSpotPrice(assetSelectButtonViewModel.Symbol);

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

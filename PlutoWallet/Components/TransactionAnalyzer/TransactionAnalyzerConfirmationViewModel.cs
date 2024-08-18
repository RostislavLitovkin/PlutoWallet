using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using Substrate.NetApi.Model.Extrinsics;

namespace PlutoWallet.Components.TransactionAnalyzer
{
    public partial class TransactionAnalyzerConfirmationViewModel : ObservableObject, IPopup
    {
        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private string dAppName;

        [ObservableProperty]
        private string dAppIcon;

        [ObservableProperty]
        private Endpoint endpoint;

        [ObservableProperty]
        private string palletCallName;

        [ObservableProperty]
        private Payload payload;

        [ObservableProperty]
        private string estimatedFee;

        // Estimated time should be calculated based the client
        [ObservableProperty]
        private string estimatedTime = "Estimated time: 6 sec";
    }
}

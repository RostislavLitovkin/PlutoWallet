using System;
using Substrate.NetApi.Model.Extrinsics;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using PlutoWallet.Types;
using PlutoWallet.Model;
using Substrate.NetApi;
using PlutoWallet.Constants;

namespace PlutoWallet.Components.TransactionRequest
{
    public partial class TransactionRequestViewModel : ObservableObject
    {
        [ObservableProperty]
        private string chainIcon;

        [ObservableProperty]
        private string chainName;

        [ObservableProperty]
        private string endpointKey;

        [ObservableProperty]
        private string palletIndex;

        [ObservableProperty]
        private string callIndex;

        [ObservableProperty]
        private string parameters;

        [ObservableProperty]
        private Method ajunaMethod;

        [ObservableProperty]
        private string fee;

        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private Payload payload;

        public TransactionRequestViewModel()
        {
            isVisible = false;
        }

        public async Task CalculateFeeAsync(Method method)
        {
            if (AjunaClientModel.Client is null)
            {
                Fee = "Fee: Failed";

                return;
            }

            Fee = "Fee: " + await Model.FeeModel.GetMethodFeeAsync(AjunaClientModel.Client, method);
        }
    }
}


using System;
using Ajuna.NetApi.Model.Extrinsics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.TransactionRequest
{
    public partial class TransactionRequestViewModel : ObservableObject
    {
        [ObservableProperty]
        private string palletIndex;

        [ObservableProperty]
        private string callIndex;

        [ObservableProperty]
        private string parameters;

        private Method ajunaMethod;

        public Method AjunaMethod
        {
            get => ajunaMethod;
            set
            {
                ajunaMethod = value;
                PalletIndex = "Pallet index: " + value.ModuleIndex;
                CallIndex = "Call index: " + value.CallIndex;
                Parameters = "Parameters: 0x..";
            }
        }

        [ObservableProperty]
        private bool isVisible;

        public TransactionRequestViewModel()
        {
            isVisible = false;
        }
    }
}


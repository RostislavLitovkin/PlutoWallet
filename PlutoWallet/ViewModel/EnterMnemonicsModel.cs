using System;
using Substrate.NetApi;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using static Substrate.NetApi.Mnemonic;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using Schnorrkel.Keys;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using Substrate.NetApi.Model.Types;

namespace PlutoWallet.ViewModel
{
    public partial class EnterMnemonicsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string privateKey;

        [ObservableProperty]
        private string mnemonics;

        public EnterMnemonicsViewModel()
        {

        }
    }
}

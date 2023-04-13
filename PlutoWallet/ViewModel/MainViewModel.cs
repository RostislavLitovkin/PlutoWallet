using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Meta;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Plutonication;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace PlutoWallet.ViewModel
{
    internal partial class MainViewModel : ObservableObject
    {
        public string PublicKey => KeysModel.GetPublicKey();

        public string SubstrateKey => KeysModel.GetSubstrateKey();
    }
}

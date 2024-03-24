using System;
using PlutoWallet.Components.Balance;
using PlutoWallet.Components.AddressView;
using PlutoWallet.Components.DAppConnectionView;
using PlutoWallet.Components.Extrinsic;
using PlutoWallet.Components.CalamarView;
using PlutoWallet.Components.Staking;
using System.Collections.ObjectModel;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Constants;
using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.Components.AwesomeAjunaAvatars;
using PlutoWallet.Components.Contract;
using PlutoWallet.Components.AzeroId;
using PlutoWallet.Components.HydraDX;
using PlutoWallet.ViewModel;
using PlutoWallet.Components.Identity;
using PlutoWallet.Components.Referenda;
using Substrate.NetApi.Model.Types.Base;
using PlutoWallet.Components.Mnemonics;
using PlutoWallet.Components.Buttons;
using PlutoWallet.Components.Nft;
using PlutoWallet.Components.Fee;
using PlutoWallet.Components.VTokens;
using PlutoWallet.Components.UpdateView;

namespace PlutoWallet.Model
{
    public class LayoutItemInfo
    {
        public string Name { get; set; }
        public string PlutoLayoutId { get; set; }
    }

    public class CustomLayoutModel
    {
        public const string DEFAULT_PLUTO_LAYOUT = "plutolayout: [U, dApp, BMnR, ExSL, UsdB, RnT, SubK, ChaK, CalEx];[polkadot, kusama]";

        // This constant is used to fetch all items
        public const string ALL_ITEMS = "plutolayout: [U, dApp, ExSL, UsdB, RnT, SubK, ChaK, CalEx, " +
            "AAALeaderboard, AZEROPrimaryName, HDXOmniLiquidity, HDXDCA, id, Ref, contract, " +
            "BMnR, NftG, VDot];[";

        // EXTRA: StDash, AAASeasonCountdown, PubK, FeeA

        public static List<Endpoint> ParsePlutoEndpoints(string plutoLayoutString)
        {
            if (plutoLayoutString.Substring(0, 13) != "plutolayout: ")
            {
                throw new Exception("Could not parse the PlutoLayout");
            }

            string[] itemsAndNetworksStrings = plutoLayoutString.Split(";");

            string[] layoutEndpointKeys = itemsAndNetworksStrings[1].Trim(new char[] { '[', ']' }).Split(',');

            List<Endpoint> result = new List<Endpoint>();

            foreach (string key in layoutEndpointKeys)
            {
                result.Add(Endpoints.GetEndpointDictionary[key.Trim()]);
            }

            return result;
        }

        public static List<IView> ParsePlutoLayout(string plutoLayoutString)
		{
            if (plutoLayoutString.Substring(0, 13) != "plutolayout: ")
            {
                throw new Exception("Could not parse the PlutoLayout");
            }

            Console.WriteLine(plutoLayoutString);

            plutoLayoutString = plutoLayoutString.Substring(13);

            List<IView> result = new List<IView>();

            if (plutoLayoutString.Length == 2)
            {
                return result;
            }

            string[] itemsAndNetworksStrings = plutoLayoutString.Split(";");

            string[] layoutItemStrings = itemsAndNetworksStrings[0].Trim(new char[] { '[', ']' }).Split(',');

            foreach (string item in layoutItemStrings)
            {
                result.Add(GetItem(item.Trim()));
            }

            return result;
        }

        public static ObservableCollection<LayoutItemInfo> ParsePlutoLayoutItemInfos(string plutoLayoutString)
        {
            if (plutoLayoutString.Substring(0, 13) != "plutolayout: ")
            {
                throw new Exception("Could not parse the PlutoLayout");
            }

            plutoLayoutString = plutoLayoutString.Substring(13);

            ObservableCollection<LayoutItemInfo> result = new ObservableCollection<LayoutItemInfo>();

            if (plutoLayoutString.Length == 2)
            {
                return result;
            }

            string[] itemsAndNetworksStrings = plutoLayoutString.Split(";");

            string[] layoutItemStrings = itemsAndNetworksStrings[0].Trim(new char[] { '[', ']' }).Split(',');

            foreach (string item in layoutItemStrings)
            {
                result.Add(GetItemInfo(item.Trim()));
            }

            return result;
        }

        public static void SaveLayout(ObservableCollection<LayoutItemInfo> layoutItemInfos)
        {
            string result = "plutolayout: [";

            // Pluto items
            foreach (LayoutItemInfo info in layoutItemInfos)
            {
                result += info.PlutoLayoutId + ", ";
            }

            if (layoutItemInfos.Count() > 0)
            {
                result = result.Substring(0, result.Length - 2); // Remove last ", " (comma + space)
            }

            result += "];";

            // save Endpoints
            result += Preferences.Get("SelectedNetworks", EndpointsModel.DefaultEndpoints);
                

            // Save
            Preferences.Set("PlutoLayout", result);

            var basePageViewModel = DependencyService.Get<BasePageViewModel>();
            basePageViewModel.MainView.Setup();

            //ShowRestartNeededMessage();
        }

        public static void SaveLayout(string layoutItemInfos)
        {
            Preferences.Set("PlutoLayout", layoutItemInfos);

            SaveEndpoints(layoutItemInfos);

            var basePageViewModel = DependencyService.Get<BasePageViewModel>();
            basePageViewModel.MainView.Setup();

            //ShowRestartNeededMessage();
        }

        /**
         * This method is useful for saving a change of endpoints
         */
        public static void SaveLayout()
        {
            string plutoLayout = Preferences.Get("PlutoLayout", DEFAULT_PLUTO_LAYOUT);

            string[] plutoLayoutStrings = plutoLayout.Split(";");

            string result = plutoLayoutStrings[0] + ";";

            // save Endpoints
            result += Preferences.Get("SelectedNetworks", EndpointsModel.DefaultEndpoints);

            result = result.Substring(0, result.Length - 2) + "]";

            Preferences.Set("PlutoLayout", result);
        }

        public static void AddItemToSavedLayout(string itemId)
        {
            string savedLayout = Preferences.Get("PlutoLayout", DEFAULT_PLUTO_LAYOUT);

            string[] itemsAndNetworksStrings = savedLayout.Split(";");

            string newLayout = itemsAndNetworksStrings[0].Length != 15 ?
                itemsAndNetworksStrings[0].Substring(0, itemsAndNetworksStrings[0].Length - 1) + ", " + itemId + "]" :
                "plutolayout: [" + itemId + "]";

            SaveLayout(newLayout + ";" + itemsAndNetworksStrings[1]);
        }

        public static void RemoveItemFromSavedLayout(string itemId)
        {
            var layoutItemInfos = Model.CustomLayoutModel.ParsePlutoLayoutItemInfos(
                    Preferences.Get("PlutoLayout",
                    Model.CustomLayoutModel.DEFAULT_PLUTO_LAYOUT)
                );

            var infos = new ObservableCollection<LayoutItemInfo>();

            for (int i = 0; i < layoutItemInfos.Count(); i++)
            {
                if (itemId == layoutItemInfos[i].PlutoLayoutId)
                {
                    continue;
                }

                infos.Add(layoutItemInfos[i]);
            }

            Model.CustomLayoutModel.SaveLayout(infos);

        }
        private static void SaveEndpoints(string plutoLayoutString)
        {
            if (plutoLayoutString.Substring(0, 13) != "plutolayout: ")
            {
                throw new Exception("Could not parse the PlutoLayout");
            }

            string[] itemsAndNetworksStrings = plutoLayoutString.Split(";");

            if (itemsAndNetworksStrings[1].Length < 2)
            {
                // The endpoint is not saved in the layout

                Console.WriteLine("Endpoint is not saved in the PlutoLayout:");
                Console.WriteLine(plutoLayoutString);

                return;
            }

            // Save Selected Networks
            Preferences.Set("SelectedNetworks", itemsAndNetworksStrings[1]);

            Console.WriteLine("Save Endpoint -> Calling MultiNetworkSelectViewModel.SetupDefault()");

            var multiNetworkSelectViewModel = DependencyService.Get<MultiNetworkSelectViewModel>();
            multiNetworkSelectViewModel.SetupDefault();
        }

        private static void ShowRestartNeededMessage()
        {
            // Tell the user that they need to restart the app.
            var messagePopup = DependencyService.Get<MessagePopupViewModel>();

            messagePopup.Title = "Restart needed";
            messagePopup.Text = "Please, restart PlutoWallet app to apply changes to the layout.";

            messagePopup.IsVisible = true;
        }

        /**
         * This is used to say nicely that the PlutoLayout has been imported
         * 
         * usually overrides the ShowRestartNeededMessage() method
         */
        public static void ShowImportSuccessfulRestartNeededMessage()
        {
            // Tell the user that they need to restart the app.
            var messagePopup = DependencyService.Get<MessagePopupViewModel>();

            messagePopup.Title = "Pluto layout imported!";
            messagePopup.Text = "Please, restart PlutoWallet app to apply changes to the layout.";

            messagePopup.IsVisible = true;
        }



        public static IView GetItem(string plutoLayoutId)
        {
            switch (plutoLayoutId)
            {
                case "U":
                    return new UpdateView();
                case "dApp":
                    return new DAppConnectionView();
                case "ExSL":
                    return new ExtrinsicStatusStackLayout();
                case "UsdB":
                    return new UsdBalanceView();
                case "PubK":
                    return new AddressView
                    {
                        Title = "Public key",
                        Address = KeysModel.GetPublicKey(),
                        QrAddress = KeysModel.GetPublicKey(),
                    };
                case "SubK":
                    return new AddressView
                    {
                        Title = "Substrate key",
                        Address = KeysModel.GetSubstrateKey(),
                        QrAddress = "substrate:" + KeysModel.GetSubstrateKey()
                    };
                case "ChaK":
                    return new ChainAddressView();
                case "StDash":
                    return new StakingDashboardView();
                case "CalEx":
                    return new CalamarView();
                case "AAASeasonCountdown":
                    return new SeasonCountdownView();
                case "AAALeaderboard":
                    return new AAALeaderboard();
                case "contract":
                    return new ContractView();
                case "AZEROPrimaryName":
                    return new AzeroPrimaryNameView();
                case "HDXOmniLiquidity":
                    return new OmnipoolLiquidityView();
                case "HDXDCA":
                    return new DCAView();
                case "id":
                    return new IdentityView();
                case "Ref":
                    return new ReferendaView();
                case "BMnR":
                    return new BackupMnemonicsReminderView();
                case "RnT":
                    return new ReceiveAndTransferView();
                case "NftG":
                    return new NftGaleryView();
                case "FeeA":
                    return new FeeAssetView();
                case "VDot":
                    return new VDotTokenView();
            }

            throw new Exception("Could not parse the PlutoLayout");
        }

        public static IView GetItemPreview(string plutoLayoutId)
        {
            switch (plutoLayoutId)
            {
                case "U":
                    return new UpdateView();
                case "dApp":
                    var dAppConnectionViewModel = new DAppConnectionViewModel();
                    dAppConnectionViewModel.Name = "Galaxy Logic Game";
                    dAppConnectionViewModel.Icon = "https://rostislavlitovkin.pythonanywhere.com/logo";
                    dAppConnectionViewModel.IsVisible = true;

                    return new DAppConnectionView(dAppConnectionViewModel);
                case "ExSL":
                    ExtrinsicStatusStackViewModel extrinsicStatusViewModel = new ExtrinsicStatusStackViewModel();
                    var tempExtrinsics = new Dictionary<string, ExtrinsicInfo>();
                    tempExtrinsics.Add("18736389", new ExtrinsicInfo
                    {
                        CallName = "EVM.eth_call_v2",
                        Hash = new Hash("0x97ad595390e73c9421b21d130291bdcbc24267d3ccb58dd27e71177d15e68991"),
                        Endpoint = Endpoints.GetEndpointDictionary["acala"],
                        ExtrinsicId = "18736389",
                        Status = ExtrinsicStatusEnum.InBlock,
                    });

                    tempExtrinsics.Add("18737890", new ExtrinsicInfo
                    {
                        CallName = "XcmPallet.limited_reserve_transfer_assets",
                        Endpoint = Endpoints.GetEndpointDictionary["polkadot"],
                        Hash = new Hash("0x89bca86385b938c90e230a9837bce7e09991dde37f44b98b347c1d8ae2813654"),
                        ExtrinsicId = "18737890",
                        Status = ExtrinsicStatusEnum.Success,
                    });

                    extrinsicStatusViewModel.Extrinsics = tempExtrinsics;
                    extrinsicStatusViewModel.Update();

                    return new ExtrinsicStatusStackLayout(extrinsicStatusViewModel);
                case "UsdB":
                    return new UsdBalanceView();
                case "PubK":
                    return new AddressView
                    {
                        Title = "Public key",
                        Address = KeysModel.GetPublicKey(),
                    };
                case "SubK":
                    return new AddressView
                    {
                        Title = "Substrate key",
                        Address = KeysModel.GetPublicKey(),
                    };
                case "ChaK":
                    return new ChainAddressView();
                case "StDash":
                    return new StakingDashboardView();
                case "CalEx":
                    return new CalamarView();
                case "AAASeasonCountdown":
                    return new SeasonCountdownView();
                case "AAALeaderboard":
                    return new AAALeaderboard();
                case "contract":
                    return new ContractView();
                case "AZEROPrimaryName":
                    return new AzeroPrimaryNameView();
                case "HDXOmniLiquidity":
                    return new OmnipoolLiquidityView();
                case "HDXDCA":
                    return new DCAView();
                case "id":
                    return new IdentityView();
                case "Ref":
                    return new ReferendaView();
                case "BMnR":
                    return new BackupMnemonicsReminderView();
                case "RnT":
                    return new ReceiveAndTransferView();
                case "NftG":
                    return new NftGaleryView();
                case "FeeA":
                    return new FeeAssetView();
                case "VDot":
                    return new VDotTokenView();
            }

            throw new Exception("Could not parse the PlutoLayout");
        }

        public static LayoutItemInfo GetItemInfo(string plutoLayoutId)
        {
            switch (plutoLayoutId)
            {
                case "U":
                    return new LayoutItemInfo
                    {
                        Name = "Update notification",
                        PlutoLayoutId = "U"
                    };
                case "dApp":
                    return new LayoutItemInfo
                    {
                        Name = "dApp connection",
                        PlutoLayoutId = "dApp",
                    };
                case "ExSL":
                    return new LayoutItemInfo
                    {
                        Name = "Extrinsic status",
                        PlutoLayoutId = "ExSL",
                    };
                case "UsdB":
                    return new LayoutItemInfo
                    {
                        Name = "Balance",
                        PlutoLayoutId = "UsdB",
                    };
                case "PubK":
                    return new LayoutItemInfo
                    {
                        Name = "Public key",
                        PlutoLayoutId = "PubK",
                    };
                case "SubK":
                    return new LayoutItemInfo
                    {
                        Name = "Substrate key",
                        PlutoLayoutId = "SubK",
                    };
                case "ChaK":
                    return new LayoutItemInfo
                    {
                        Name = "Chain specific key",
                        PlutoLayoutId = "ChaK",
                    };
                case "StDash":
                    return new LayoutItemInfo
                    {
                        Name = "Staking dashboard",
                        PlutoLayoutId = "StDash",
                    };
                case "CalEx":
                    return new LayoutItemInfo
                    {
                        Name = "Calamar",
                        PlutoLayoutId = "CalEx",
                    };
                case "AAASeasonCountdown":
                    return new LayoutItemInfo
                    {
                        Name = "AAA Season countdown",
                        PlutoLayoutId = "AAASeasonCountdown",
                    };
                case "AAALeaderboard":
                    return new LayoutItemInfo
                    {
                        Name = "AAA Leaderboard",
                        PlutoLayoutId = "AAALeaderboard",
                    };
                case "contract":
                    return new LayoutItemInfo
                    {
                        Name = "Contract",
                        PlutoLayoutId = "contract",
                    };
                case "AZEROPrimaryName":
                    return new LayoutItemInfo
                    {
                        Name = "AZERO.ID Primary Name",
                        PlutoLayoutId = "AZEROPrimaryName",
                    };
                case "HDXOmniLiquidity":
                    return new LayoutItemInfo
                    {
                        Name = "HydraDX Omnipool Liquidity",
                        PlutoLayoutId = "HDXOmniLiquidity",
                    };
                case "HDXDCA":
                    return new LayoutItemInfo
                    {
                        Name = "HydraDX DCA Position",
                        PlutoLayoutId = "HDXDCA",
                    };
                case "id":
                    return new LayoutItemInfo
                    {
                        Name = "Identity",
                        PlutoLayoutId = "id",
                    };
                case "Ref":
                    return new LayoutItemInfo
                    {
                        Name = "Referenda",
                        PlutoLayoutId = "Ref",
                    };
                case "BMnR":
                    return new LayoutItemInfo
                    {
                        Name = "Backup Mnemonics Reminder",
                        PlutoLayoutId = "BMnR",
                    };
                case "RnT":
                    return new LayoutItemInfo
                    {
                        Name = "Receive and Transfer",
                        PlutoLayoutId = "RnT",
                    };
                case "NftG":
                    return new LayoutItemInfo
                    {
                        Name = "Nft Galery",
                        PlutoLayoutId = "NftG",
                    };
                case "FeeA":
                    return new LayoutItemInfo
                    {
                        Name = "Fee Asset",
                        PlutoLayoutId = "FeeA",
                    };
                case "VDot":
                    return new LayoutItemInfo
                    {
                        Name = "vDOT staking",
                        PlutoLayoutId = "VDot",
                    };
            }

            throw new Exception("Could not parse the PlutoLayout");
        }
    }
}


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

namespace PlutoWallet.Model
{
    public class LayoutItemInfo
    {
        public string Name { get; set; }
        public string PlutoLayoutId { get; set; }
    }

    public class CustomLayoutModel
    {
        public const string DEFAULT_PLUTO_LAYOUT = "plutolayout: [dApp, ExSL, UsdB, PubK, SubK, ChaK, CalEx];[0, 2, 3]";

        // This constant is used to fetch all items
        public const string ALL_ITEMS = "plutolayout: [dApp, ExSL, UsdB, PubK, SubK, ChaK, CalEx, " +
            "AAALeaderboard, AZEROPrimaryName, HDXOmniLiquidity, HDXDCA];[";

        // EXTRA: StDash, contract, AAASeasonCountdown,

        public static List<Endpoint> ParsePlutoEndpoints(string plutoLayoutString)
        {
            if (plutoLayoutString.Substring(0, 13) != "plutolayout: ")
            {
                throw new Exception("Could not parse the PlutoLayout");
            }

            string[] itemsAndNetworksStrings = plutoLayoutString.Split(";");

            string[] layoutEndpointStrings = itemsAndNetworksStrings[1].Trim(new char[] { '[', ']' }).Split(',');

            List<Endpoint> result = new List<Endpoint>();

            foreach (string item in layoutEndpointStrings)
            {
                result.Add(Endpoints.GetAllEndpoints[int.Parse(item.Trim())]);
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

            result += "];[";

            // Endpoint indexes
            for (int i = 0; i < 4; i++)
            {
                int endpointIndex = Preferences.Get("SelectedNetworks" + i, Endpoints.DefaultNetworks[i]);
                if (endpointIndex != -1)
                {
                    result += endpointIndex + ", ";
                }
            }

            result = result.Substring(0, result.Length - 2) + "]";

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

            string result = plutoLayoutStrings[0] + ";[";

            // Endpoint indexes
            for (int i = 0; i < 4; i++)
            {
                int endpointIndex = Preferences.Get("SelectedNetworks" + i, Endpoints.DefaultNetworks[i]);
                if (endpointIndex != -1)
                {
                    result += endpointIndex + ", ";
                }
            }

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

            string[] layoutEndpointStrings = itemsAndNetworksStrings[1].Trim(new char[] { '[', ']' }).Split(',');

            int[] networks = new int[4]{-1, -1, -1, -1};

            for (int i = 0; i < layoutEndpointStrings.Length; i++)
            {
                networks[i] = int.Parse(layoutEndpointStrings[i].Trim());
            }

            Preferences.Set("SelectedNetworks0", networks[0]);
            Preferences.Set("SelectedNetworks1", networks[1]);
            Preferences.Set("SelectedNetworks2", networks[2]);
            Preferences.Set("SelectedNetworks3", networks[3]);

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
                    };
                case "SubK":
                    return new AddressView
                    {
                        Title = "Substrate key",
                        Address = KeysModel.GetSubstrateKey(),
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
            }

            throw new Exception("Could not parse the PlutoLayout");
        }

        public static IView GetItemPreview(string plutoLayoutId)
        {
            switch (plutoLayoutId)
            {
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
                        ExtrinsicId = "18736389",
                        Status = ExtrinsicStatusEnum.InBlock,
                    });

                    tempExtrinsics.Add("18737890", new ExtrinsicInfo
                    {
                        ExtrinsicId = "18737890",
                        Status = ExtrinsicStatusEnum.Success,
                    });

                    extrinsicStatusViewModel.Extrinsics = tempExtrinsics;
                    extrinsicStatusViewModel.Update();

                    return new ExtrinsicStatusStackLayout(extrinsicStatusViewModel, 135);
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
            }

            throw new Exception("Could not parse the PlutoLayout");
        }

        public static LayoutItemInfo GetItemInfo(string plutoLayoutId)
        {
            switch (plutoLayoutId)
            {
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
            }

            throw new Exception("Could not parse the PlutoLayout");
        }
    }
}


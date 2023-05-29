using System;
using PlutoWallet.Components.Balance;
using PlutoWallet.Components.AddressView;
using PlutoWallet.Components.DAppConnectionView;
using PlutoWallet.Components.Extrinsic;
using PlutoWallet.Components.CalamarView;
using PlutoWallet.Components.Staking;
using System.Collections.ObjectModel;
using PlutoWallet.Components.MessagePopup;

namespace PlutoWallet.Model
{
    public class LayoutItemInfo
    {
        public string Name { get; set; }
        public string PlutoLayoutId { get; set; }
    }

    public class CustomLayoutModel
	{
        public const string DEFAULT_PLUTO_LAYOUT = "plutolayout: [dApp, ExSL, UsdB, PubK, SubK, ChaK, StDash, CalEx]";

		public static List<IView> ParsePlutoLayout(string plutoLayoutString)
		{
            if (plutoLayoutString.Substring(0, 13) != "plutolayout: ")
            {
                new Exception("Could not parse the PlutoLayout");
            }

            plutoLayoutString = plutoLayoutString.Substring(13);

            List<IView> result = new List<IView>();

            string[] layoutItemStrings = plutoLayoutString.Trim(new char[] { '[', ']' }).Split(',');

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
                new Exception("Could not parse the PlutoLayout");
            }

            plutoLayoutString = plutoLayoutString.Substring(13);

            ObservableCollection<LayoutItemInfo> result = new ObservableCollection<LayoutItemInfo>();

            string[] layoutItemStrings = plutoLayoutString.Trim(new char[] { '[', ']' }).Split(',');

            foreach (string item in layoutItemStrings)
            {
                result.Add(GetItemInfo(item.Trim()));
            }

            return result;
        }

        public static void SaveLayout(ObservableCollection<LayoutItemInfo> layoutItemInfos)
        {
            string result = "plutolayout: [";

            foreach (LayoutItemInfo info in layoutItemInfos)
            {
                result += info.PlutoLayoutId + ", ";
            }

            result = result.Substring(0, result.Length - 2); // Remove last ", " (comma + space)

            result += "]";

            Preferences.Set("PlutoLayout", result);

            ShowRestartNeededMessage();
        }

        public static void SaveLayout(string layoutItemInfos)
        {
            Preferences.Set("PlutoLayout", layoutItemInfos);

            ShowRestartNeededMessage();
        }

        private static void ShowRestartNeededMessage()
        {
            // Tell the user that they need to restart the app.
            var messagePopup = DependencyService.Get<MessagePopupViewModel>();

            messagePopup.Title = "Restart needed";
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
                    return new BalanceDashboardView();
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
                        Name = "Balance dashboard",
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
            }

            throw new Exception("Could not parse the PlutoLayout");
        }
    }
}


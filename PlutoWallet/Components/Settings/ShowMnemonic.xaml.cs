using CommunityToolkit.Maui.Alerts;
using static Substrate.NetApi.Mnemonic;
using Substrate.NetApi;
using Schnorrkel.Keys;

namespace PlutoWallet.Components.Settings;

public partial class ShowMnemonic : ContentView
{
	private int counter = 0;
	public ShowMnemonic()
	{
		InitializeComponent();
	}

    async void ShowMnemonicClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		if (counter < 10)
		{
			counter++;
			return;
		}

		var mnemonics = Preferences.Get("usePrivateKey", false) ? await SecureStorage.Default.GetAsync("privateKey") : await SecureStorage.Default.GetAsync("mnemonics");
		mnemonicLabel.Text = mnemonics;
    }
}

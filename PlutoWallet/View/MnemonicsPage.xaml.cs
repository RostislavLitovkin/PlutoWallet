using CommunityToolkit.Maui.Storage;
using PlutoWallet.Model;
using Substrate.NET.Schnorrkel.Keys;
using Substrate.NET.Wallet.Keyring;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;

namespace PlutoWallet.View;

public partial class MnemonicsPage : ContentPage
{

    /// <summary>
    /// Intended for use with `KeysModel.GetMnemonicsOrPrivateKeyAsync()`
    /// </summary>
    /// <param name="secretValues"></param>
	public MnemonicsPage((string, bool) secretValues)
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        var (mnemonicsOrPrivateKey, usePrivateKey) = secretValues;

        viewModel.Title = usePrivateKey ? "Private key:" : "Mnemonics:";

        viewModel.Mnemonics = mnemonicsOrPrivateKey;
	}

	private async void GoToEnterMnemonics(System.Object sender, System.EventArgs e)
    {
		await Navigation.PushAsync(new EnterMnemonicsPage());
    }

    private async void ContinueToMainPageClicked(System.Object sender, System.EventArgs e)
	{
		await Navigation.PopToRootAsync();
    }

    private async void ExportJsonClicked(System.Object sender, System.EventArgs e)
    {
        string rawPrivateKey = await SecureStorage.Default.GetAsync("privateKey");

        var miniSecret2 = new MiniSecret(Utils.HexToByteArray(rawPrivateKey), ExpandMode.Ed25519);

        Account account = Account.Build(KeyType.Sr25519,
            miniSecret2.ExpandToSecret().ToEd25519Bytes(),
            miniSecret2.GetPair().Public.Key);

        Console.WriteLine(Utils.Bytes2HexString(((BaseType)account).Bytes));
        Console.WriteLine(Utils.Bytes2HexString(account.PrivateKey));

        /*

        // Source: https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/essentials/file-saver?tabs=macos
        using var stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(json));
        var fileSaverResult = await FileSaver.Default.SaveAsync("PlutoWallet.json", stream, CancellationToken.None);
        */
        /*if (fileSaverResult.IsSuccessful)
        {
            await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
        }
        else
        {
            await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
        }*/

    }
}

using System.Text;

namespace PlutoWallet.View;

public partial class EnterMnemonicsPage : ContentPage
{
	public EnterMnemonicsPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();
	}

    private async void ContinueWithMnemonicClicked(System.Object sender, System.EventArgs e)
    {
        await Model.KeysModel.GenerateNewAccountAsync(
            viewModel.Mnemonics,
            await SecureStorage.Default.GetAsync("password")
        );

        await Navigation.PopToRootAsync();
    }

    private async void ContinueWithPrivateKeyClicked(System.Object sender, System.EventArgs e)
    {
        await Model.KeysModel.GenerateNewAccountFromPrivateKeyAsync(viewModel.PrivateKey);

        await Navigation.PopToRootAsync();
    }

    private async void ImportJsonClicked(System.Object sender, System.EventArgs e)
    {
        var jsonType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { "json" } }, // UTType values
                { DevicePlatform.Android, new[] { "application/json" } }, // MIME type
                { DevicePlatform.WinUI, new[] { ".json" } }, // file extension
                { DevicePlatform.Tizen, new[] { "*/*" } },
                { DevicePlatform.macOS, new[] { "json" } }, // UTType values
            });

        var result = await FilePicker.PickAsync(new PickOptions {
            PickerTitle = "Import json",
            //FileTypes = jsonType,
        });

        if (result == null && result.FileName.Contains(".json"))
            return;

        using var jsonStream = await result.OpenReadAsync();

        string json = StreamToString(jsonStream);
    }

    public static string StreamToString(Stream stream)
    {
        stream.Position = 0;
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }


}

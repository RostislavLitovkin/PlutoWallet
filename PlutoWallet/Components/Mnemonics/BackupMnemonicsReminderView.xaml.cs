using PlutoWallet.View;

namespace PlutoWallet.Components.Mnemonics;

public partial class BackupMnemonicsReminderView : ContentView
{
	public BackupMnemonicsReminderView()
	{
		InitializeComponent();
	}

    async void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        if ((await Model.KeysModel.GetMnemonicsOrPrivateKeyAsync()).IsSome(out (string, bool) secretValues))
        {
            await Navigation.PushAsync(new MnemonicsPage(secretValues));

            Model.CustomLayoutModel.RemoveItemFromSavedLayout("BMnR");
        }
    }
}

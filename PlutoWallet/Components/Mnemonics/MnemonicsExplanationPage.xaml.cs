using PlutoWallet.View;
using Microsoft.Maui.Controls;

namespace PlutoWallet.Components.Mnemonics;

public partial class MnemonicsExplanationPage : ContentPage
{
    public Command<string> OpenUrlCommand { get; }

    public MnemonicsExplanationPage()
    {
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        OpenUrlCommand = new Command<string>(async (url) => await Launcher.OpenAsync(url));
        
        InitializeComponent();
        BindingContext = this;
    }
}
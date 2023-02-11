using CommunityToolkit.Maui;
using PlutoWallet.Components.NetworkSelect;
using ZXing.Net.Maui.Controls;

namespace PlutoWallet;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()

#if ANDROID || IOS
            .UseBarcodeReader()
#endif


            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<ViewModel.CustomCallsViewModel>();

        return builder.Build();
    }
}

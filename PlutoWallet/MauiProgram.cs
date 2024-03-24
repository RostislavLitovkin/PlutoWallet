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
            .UseBarcodeReader()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fontawesome-webfont.ttf", "FontAwesome");
                fonts.AddFont("Exodar-Outline.ttf", "Exodar");
                fonts.AddFont("FontOver.ttf", "FontOver");
                fonts.AddFont("sourcecode.ttf", "SourceCode");

            });

        //builder.Services.AddSingleton<Model.PlutonicationModel>();

        return builder.Build();
    }
}

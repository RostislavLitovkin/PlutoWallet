using System.Numerics;
using Microcharts;
using SkiaSharp;

namespace PlutoWallet.Components.Referenda;

public partial class ReferendumInfoView : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(ReferendumInfoView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ReferendumInfoView)bindable;

            control.titleLabelText.Text = (string)newValue;
        });

    public static readonly BindableProperty AyesProperty = BindableProperty.Create(
        nameof(Ayes), typeof(BigInteger), typeof(ReferendumInfoView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ReferendumInfoView)bindable;

            /*control.chart.Entries = new[]
            {
                new ChartEntry(212)
                {
                    Label = "UWP",
                    ValueLabel = "112",
                    Color = SKColor.Parse("#2c3e50")
                },
            };*/
        });

    public static readonly BindableProperty NaysProperty = BindableProperty.Create(
        nameof(Nays), typeof(BigInteger), typeof(ReferendumInfoView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ReferendumInfoView)bindable;

        });

    public ReferendumInfoView()
	{
		InitializeComponent();
	}

    public string Title
    {
        get => (string)GetValue(TitleProperty);

        set => SetValue(TitleProperty, value);
    }

    public BigInteger Ayes
    {
        get => (BigInteger)GetValue(AyesProperty);

        set => SetValue(AyesProperty, value);
    }

    public BigInteger Nays
    {
        get => (BigInteger)GetValue(NaysProperty);

        set => SetValue(NaysProperty, value);
    }
}

using System.Numerics;
using PlutoWallet.Model.SubSquare;
using PlutoWallet.Constants;

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


        });

    public static readonly BindableProperty NaysProperty = BindableProperty.Create(
        nameof(Nays), typeof(BigInteger), typeof(ReferendumInfoView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ReferendumInfoView)bindable;

        });

    public static readonly BindableProperty AyesPercentageProperty = BindableProperty.Create(
        nameof(AyesPercentage), typeof(double), typeof(ReferendumInfoView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ReferendumInfoView)bindable;

            AbsoluteLayout.SetLayoutBounds(control.ayesBar, new Rect(0, 0.5, (double)newValue, 1));
        });

    public static readonly BindableProperty NaysPercentageProperty = BindableProperty.Create(
        nameof(NaysPercentage), typeof(double), typeof(ReferendumInfoView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ReferendumInfoView)bindable;

            AbsoluteLayout.SetLayoutBounds(control.naysBar, new Rect(1, 0.5, (double)newValue, 1));

        });

    public static readonly BindableProperty ReferendumIndexProperty = BindableProperty.Create(
        nameof(ReferendumIndex), typeof(int), typeof(ReferendumInfoView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ReferendumInfoView)bindable;

        });

    public static readonly BindableProperty SubSquareLinkProperty = BindableProperty.Create(
        nameof(SubSquareLink), typeof(string), typeof(ReferendumInfoView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ReferendumInfoView)bindable;

        });

    public static readonly BindableProperty EndpointProperty = BindableProperty.Create(
        nameof(Endpoint), typeof(Endpoint), typeof(ReferendumInfoView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (ReferendumInfoView)bindable;

            control.chainIcon.Source = ((Endpoint)newValue).Icon;
        });

    public static readonly BindableProperty VoteProperty = BindableProperty.Create(
       nameof(Vote), typeof(ReferendumVote), typeof(ReferendumInfoView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) => {
           var control = (ReferendumInfoView)bindable;

           switch (((ReferendumVote)newValue).Decision)
           {
               case VoteDecision.Aye:
                   control.decisionLabel.Text = "Aye";
                   control.decisionLabel.TextColor = Color.FromArgb("00AD00");
                   control.ayesBar.IsVisible = true;

                   break;
               case VoteDecision.Nay:
                   control.decisionLabel.Text = "Nay";
                   control.decisionLabel.TextColor = Color.FromArgb("AD0000");

                   control.naysBar.IsVisible = true;
                   break;
               case VoteDecision.Split:
                   control.decisionLabel.Text = "Split";
                   control.decisionLabel.TextColor = Color.FromArgb("888888");

                   control.ayesBar.IsVisible = true;
                   control.naysBar.IsVisible = true;

                   break;
           }
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

    public double AyesPercentage
    {
        get => (double)GetValue(AyesPercentageProperty);

        set => SetValue(AyesPercentageProperty, value);
    }

    public double NaysPercentage
    {
        get => (double)GetValue(NaysPercentageProperty);

        set => SetValue(NaysPercentageProperty, value);
    }

    public ReferendumVote Vote
    {
        get => (ReferendumVote)GetValue(VoteProperty);

        set => SetValue(VoteProperty, value);
    }

    public int ReferendumIndex
    {
        get => (int)GetValue(ReferendumIndexProperty);

        set => SetValue(ReferendumIndexProperty, value);
    }

    public string SubSquareLink
    {
        get => (string)GetValue(SubSquareLinkProperty);

        set => SetValue(SubSquareLinkProperty, value);
    }

    public Endpoint Endpoint
    {
        get => (Endpoint)GetValue(EndpointProperty);

        set => SetValue(EndpointProperty, value);
    }

    async void OnSubsquareClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await Navigation.PushAsync(new WebView.WebViewPage(SubSquareLink));
    }
}

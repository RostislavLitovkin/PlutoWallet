namespace PlutoWallet.Components.NavigationBar;

public partial class TopNavigationBar : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(TopNavigationBar),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (TopNavigationBar)bindable;

            control.titleText.Text = (string)newValue;
        });

    public TopNavigationBar()
	{
		InitializeComponent();
	}

    public string Title
    {
        get => (string)GetValue(TitleProperty);

        set => SetValue(TitleProperty, value);
    }

    public string ExtraTitle { set { extraLabel.Text = value; } }

    public Func<Task> ExtraFunc { get; set; }

    private async void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		await Navigation.PopAsync();
    }

    private async void OnExtraClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await ExtraFunc();
    }
}

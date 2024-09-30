using System.Collections.ObjectModel;
using PlutoWallet.Constants;

namespace PlutoWallet.Components.PredefinedLayouts;

public partial class GenericPredefinedLayoutItem : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(GenericPredefinedLayoutItem),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (GenericPredefinedLayoutItem)bindable;
            control.nameLabelText.Text = (string)newValue;
        });

    public static readonly BindableProperty PlutoLayoutProperty = BindableProperty.Create(
        nameof(PlutoLayout), typeof(string), typeof(GenericPredefinedLayoutItem),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (GenericPredefinedLayoutItem)bindable;

            var endpoints = Model.CustomLayoutModel.ParsePlutoEndpoints((string)newValue);

            var listOfIcons = new List<EndpointIcon>();

            foreach (Endpoint endpoint in endpoints)
            {
                listOfIcons.Add(new EndpointIcon { Icon = endpoint.Icon });
            }

            ((GenericPredefinedLayoutItemViewModel)control.BindingContext).EndpointIcons = new ObservableCollection<EndpointIcon>(listOfIcons);
        });

    public static readonly BindableProperty BackgroundImageProperty = BindableProperty.Create(
        nameof(BackgroundImage), typeof(string), typeof(GenericPredefinedLayoutItem),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (GenericPredefinedLayoutItem)bindable;

            control.backgroundImage.Source = (string)newValue;
        });

    public static readonly BindableProperty EndpointIconsIsVisibleProperty = BindableProperty.Create(
        nameof(EndpointIconsIsVisible), typeof(bool), typeof(GenericPredefinedLayoutItem),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (GenericPredefinedLayoutItem)bindable;

            control.endpointIcons.IsVisible = (bool)newValue;
        });

    public GenericPredefinedLayoutItem()
	{
		InitializeComponent();
	}

    public string Title
    {
        get => (string)GetValue(TitleProperty);

        set => SetValue(TitleProperty, value);
    }

    public string PlutoLayout
    {
        get => (string)GetValue(PlutoLayoutProperty);

        set => SetValue(PlutoLayoutProperty, value);
    }

    public string BackgroundImage
    {
        get => (string)GetValue(BackgroundImageProperty);

        set => SetValue(BackgroundImageProperty, value);
    }

    public bool EndpointIconsIsVisible
    {
        get => (bool)GetValue(EndpointIconsIsVisibleProperty);

        set => SetValue(EndpointIconsIsVisibleProperty, value);
    }

    async void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Model.CustomLayoutModel.SaveLayout(PlutoLayout);

        await Navigation.PopAsync();
    }
}

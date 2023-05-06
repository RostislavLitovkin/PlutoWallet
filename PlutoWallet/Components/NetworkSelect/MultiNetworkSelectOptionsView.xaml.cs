using PlutoWallet.Constants;

namespace PlutoWallet.Components.NetworkSelect;

public partial class MultiNetworkSelectOptionsView : ContentView
{
	private MultiNetworkSelectView multiSelect;

	public MultiNetworkSelectOptionsView()
	{
		InitializeComponent();

		SetupDefaults();
	}

	public MultiNetworkSelectView MultiSelect { get => multiSelect; set { multiSelect = value; } }

	public void SetupDefaults()
	{
        var defaultNetworks = Endpoints.DefaultNetworks;

        var selectedOptions = new int[4] {
            Preferences.Get("SelectedNetworks0", defaultNetworks[0]),
            Preferences.Get("SelectedNetworks1", defaultNetworks[1]),
            Preferences.Get("SelectedNetworks2", defaultNetworks[2]),
            Preferences.Get("SelectedNetworks3", defaultNetworks[3]),
        };

        MultiNetworkSelectOptionView selectedOptionView = new MultiNetworkSelectOptionView
        {
            Networks = selectedOptions,
        };

        verticalStackLayout.Children.Add(selectedOptionView);

        foreach (int[] option in Endpoints.NetworkOptions)
		{
			// Check if it is already selected
			bool selected = true;
			for(int i = 0; i < option.Length; i++)
			{
				if (selectedOptions[i] != option[i])
				{
					selected = false;
				}
			}

			if (!selected) {
				MultiNetworkSelectOptionView optionView = new MultiNetworkSelectOptionView
				{
					Networks = option,
				};

				optionView.ViewUsedForTapGesture.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => {
					Preferences.Set("SelectedNetworks0", option[0]);
					Preferences.Set("SelectedNetworks1", option[1]);
					Preferences.Set("SelectedNetworks2", option[2]);
					Preferences.Set("SelectedNetworks3", option[3]);

					multiSelect.SetupDefault();

                    ((AbsoluteLayout)this.Parent).Children.Remove(this);

                    multiSelect.Clicked = false;
                }) });

				verticalStackLayout.Children.Add(optionView);

            }
		}
	}

    void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		((AbsoluteLayout)this.Parent).Children.Remove(this);

		multiSelect.Clicked = false;
    }
}

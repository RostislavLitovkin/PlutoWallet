﻿using CommunityToolkit.Maui.Alerts;

namespace PlutoWallet.Components.AddressView;

public partial class AddressView : ContentView
{
    public static readonly BindableProperty AddressProperty = BindableProperty.Create(
        nameof(Address), typeof(string), typeof(AddressView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AddressView)bindable;
			control.addressLabel.Text = ((string)newValue).Substring(0, 12) + "..";
            //control.picker.UpdateSelectedIndex(newValue)
        });
    //private string address;
	public AddressView()
	{
		InitializeComponent();
	}

	public string Address
	{
		get => (string)GetValue(AddressProperty);
	
		set => SetValue(AddressProperty, value);
	}

	public string Title
	{
		set
		{
			titleLabel.Text = value;
		}
	}

    private async void OnTapped(System.Object sender, System.EventArgs e)
    {
		await Clipboard.Default.SetTextAsync((string)GetValue(AddressProperty));
        var toast = Toast.Make("Copied to clipboard");
        await toast.Show();
    }
}
namespace PlutoWallet.Components.Card;

public partial class Card : ContentView
{
	public Card()
	{
		InitializeComponent();
	}

	public Microsoft.Maui.Controls.View View { set { contentView.Content = value; } }

	public bool IsPopup { set { border.Padding = new Thickness(15); } }
}

namespace PlutoWallet.Components.Card;

public partial class Card : ContentView
{
	public Card()
	{
		InitializeComponent();
	}

	public Microsoft.Maui.Controls.View View { set { contentView.Content = value; } }

	public bool IsPopup { set { border.Padding = new Thickness(15); } }

    public bool IsTransparent { set {
			if (value)
			{
                border.SetAppThemeColor(Border.BackgroundColorProperty, Color.FromArgb("88ffffff"), Color.FromArgb("88000000"));
            }
		} }

	public Thickness CardPadding { set { border.Padding = value; } }
}

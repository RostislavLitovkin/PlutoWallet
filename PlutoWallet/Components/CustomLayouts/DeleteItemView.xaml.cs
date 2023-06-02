namespace PlutoWallet.Components.CustomLayouts;

public partial class DeleteItemView : ContentView
{
	public DeleteItemView()
	{
		InitializeComponent();
	}

	public bool Hovered { set {
			if (value)
			{
				border.BackgroundColor = Colors.Red;
			}
			else
			{
				border.BackgroundColor = Color.FromArgb("88FF0000");
			}
		} }
}

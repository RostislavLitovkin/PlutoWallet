namespace PlutoWallet.Components.Extrinsic;

public partial class ExtrinsicStatusView : ContentView
{
	public ExtrinsicStatusView()
	{
		InitializeComponent();
	}

	public string ExtrinsicId { set { nameLabel.Text = "Extrinsic #" + value; } }
	public ExtrinsicStatusEnum Status {
		set
		{
			switch(value)
			{
				case ExtrinsicStatusEnum.Pending:
					statusLabel.Text = "Pending";
					statusLabel.TextColor = Color.Parse("Orange");
					break;
				case ExtrinsicStatusEnum.Success:
                    statusLabel.Text = "Success";
                    statusLabel.TextColor = Color.Parse("Green");
                    break;
                case ExtrinsicStatusEnum.Failed:
                    statusLabel.Text = "Failed";
                    statusLabel.TextColor = Color.Parse("Red");
                    break;

            }
			
		}
	}

    void OnRemoveClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		
    }
}

namespace PlutoWallet.Components.Switch;

public partial class DualSwitch : ContentView
{
    private Func<bool> firstMethod;
    private Func<bool> secondMethod;

    public DualSwitch()
	{
		InitializeComponent();
	}

    public string FirstOption { set { firstOption.Text = value; selectedOptionLabel.Text = value; } }

    public string SecondOption { set { secondOption.Text = value; } }

    public Func<bool> FirstMethod { set { firstMethod = value; } }

    public Func<bool> SecondMethod { set { secondMethod = value; } }

    void OnFirstOptionClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        AbsoluteLayout.SetLayoutBounds(selectedOptionFrame, new Rect(0, 0, 0.5, 1));
        selectedOptionLabel.Text = firstOption.Text;

        firstMethod();
    }

    void OnSecondOptionClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        AbsoluteLayout.SetLayoutBounds(selectedOptionFrame, new Rect(1, 0, 0.5, 1));
        selectedOptionLabel.Text = secondOption.Text;

        secondMethod();
    }
}

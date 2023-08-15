namespace PlutoWallet.Components.Switch;

public partial class MiniDualSwitch : ContentView
{
    private int selected = 1;
    private Func<bool> firstMethod;
    private Func<bool> secondMethod;

    public MiniDualSwitch()
    {
        InitializeComponent();
    }

    public string FirstOption { set { firstOption.Text = value; selectedOptionLabel.Text = value; } }

    public string SecondOption { set { secondOption.Text = value; } }

    public Func<bool> FirstMethod { set { firstMethod = value; } }

    public Func<bool> SecondMethod { set { secondMethod = value; } }

    public int Selected => selected;

    void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Console.WriteLine("Changing switch state");
        if (selected == 2)
        {
            selected = 1;
            AbsoluteLayout.SetLayoutBounds(selectedOptionFrame, new Rect(0, 0, 0.5, 1));
            selectedOptionLabel.Text = firstOption.Text;

            firstMethod();
        }
        else
        {
            selected = 2;
            AbsoluteLayout.SetLayoutBounds(selectedOptionFrame, new Rect(1, 0, 0.5, 1));
            selectedOptionLabel.Text = secondOption.Text;

            secondMethod();
        }
    }
}

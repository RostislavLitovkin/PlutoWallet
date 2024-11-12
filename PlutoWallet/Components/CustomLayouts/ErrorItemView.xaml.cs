namespace PlutoWallet.Components.CustomLayouts;

public partial class ErrorItemView : ContentView
{
    public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
      nameof(ErrorText), typeof(string), typeof(ErrorItemView),
      defaultBindingMode: BindingMode.TwoWay,
      propertyChanging: (bindable, oldValue, newValue) =>
      {
          var control = (ErrorItemView)bindable;

          control.label.Text = (string)newValue;
      });
    public ErrorItemView()
	{
		InitializeComponent();
	}

    public string ErrorText
    {
        get => (string)GetValue(ErrorTextProperty);
        set => SetValue(ErrorTextProperty, value);
    }
}
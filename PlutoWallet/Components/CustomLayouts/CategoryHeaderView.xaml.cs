using System.ComponentModel;

namespace PlutoWallet.Components.CustomLayouts;

public partial class CategoryHeaderView : ContentView
{
    public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
      nameof(TitleText), typeof(string), typeof(CategoryHeaderView),
      defaultBindingMode: BindingMode.TwoWay,
      propertyChanging: (bindable, oldValue, newValue) =>
      {
          var control = (CategoryHeaderView)bindable;

          control.titleLabel.Text = (string)newValue;
      });

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
      nameof(ImageSource), typeof(ImageSource), typeof(CategoryHeaderView),
      defaultBindingMode: BindingMode.TwoWay,
      propertyChanging: (bindable, oldValue, newValue) =>
      {
          var control = (CategoryHeaderView)bindable;

          control.image.Source = (ImageSource)newValue;
      });
    public CategoryHeaderView()
	{
		InitializeComponent();
	}

    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    [TypeConverter(typeof(ImageSourceConverter))]
    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }   

}
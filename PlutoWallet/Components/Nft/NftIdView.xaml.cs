using Markdig;
using System.Numerics;

namespace PlutoWallet.Components.Nft;

public partial class NftIdView : ContentView
{
    public static readonly BindableProperty CollectionIdProperty = BindableProperty.Create(
        nameof(CollectionId), typeof(BigInteger), typeof(NftIdView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NftIdView)bindable;

            //control.collectionIdLabel.Text = ((BigInteger)newValue).ToString();
        });

    public static readonly BindableProperty IdProperty = BindableProperty.Create(
        nameof(Id), typeof(BigInteger), typeof(NftIdView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (NftIdView)bindable;

            control.idLabel.Text = $"# {(BigInteger)newValue}";
        });
    public NftIdView()
	{
		InitializeComponent();
	}
    public BigInteger CollectionId
    {
        get => (BigInteger)GetValue(CollectionIdProperty);

        set => SetValue(CollectionIdProperty, value);
    }
    public BigInteger Id
    {
        get => (BigInteger)GetValue(IdProperty);

        set => SetValue(IdProperty, value);
    }
}
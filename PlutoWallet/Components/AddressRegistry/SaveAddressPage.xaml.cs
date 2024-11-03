namespace PlutoWallet.Components.AddressRegistry
{
    public partial class SaveAddressPage : ContentPage
    {
        public SaveAddressPage(string address)
        {
            InitializeComponent();
            BindingContext = new SaveAddressPageViewModel(address);
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.ViewModel
{
    public partial class EnterMnemonicsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string privateKey;

        [ObservableProperty]
        private string mnemonics;

        public EnterMnemonicsViewModel()
        {

        }
    }
}

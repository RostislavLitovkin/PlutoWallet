using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlutoWallet.Model;

namespace PlutoWalletTests
{
    internal class ChopsticksTests
    {
        [Test]
        public async Task ConnectionTestAsync()
        {
            await ChopsticksModel.PostAsync();
        }
    }
}

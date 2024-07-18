using PlutoWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlutoWalletTests
{
    internal class ExpressTests
    {
        [Test]
        public async Task RunAsync()
        {
            Console.WriteLine("Trying to run express");
            ExpressModel.RunAsync();
            await Task.WhenAny(
                //,
                Task.Delay(10000));


            Console.WriteLine("Finished running express");
        }
    }
}

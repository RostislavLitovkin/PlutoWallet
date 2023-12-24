using System;
using PlutoWallet.Constants;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWalletTests;


public class Endpoints
{
    [Test]
    public async Task Connect()
    {
        foreach (Endpoint endpoint in PlutoWallet.Constants.Endpoints.GetAllEndpoints)
        {
            if (endpoint.Name == "(Local) ws://127.0.0.1:9944")
            {
                continue;
            }

            try
            {
                var client = new SubstrateClientExt(
                                endpoint,
                                new Uri(endpoint.URLs[0]),
                                Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

                await client.ConnectAsync();

                client.Dispose();
            }
            catch
            {
                Console.WriteLine(endpoint.Name);
                Assert.Fail();
            }

            Console.WriteLine(endpoint.Name + " done ^^");

        }

        Assert.Pass();
    }
}




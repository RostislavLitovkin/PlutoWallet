using System;
using Newtonsoft.Json.Linq;
using PlutoWallet.Constants;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi.Model.Types.Base;

namespace PlutoWalletTests;


public class EndpointsTests
{
    [Test]
    public async Task ConnectAndCheck()
    {
        foreach (Endpoint endpoint in PlutoWallet.Constants.Endpoints.GetAllEndpoints)
        {
            // Skip the local chain for obvious reasons
            if (endpoint.Name == "(Local) ws://127.0.0.1:9944")
            {
                continue;
            }
            
            foreach (string url in endpoint.URLs)
            {
                // Skip onfinality due to limits
                if (url.Contains("onfinality"))
                {
                    continue;
                }

                try
                {
                    SubstrateClientExt client = new SubstrateClientExt(
                                    endpoint,
                                    new Uri(url),
                                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

                    await client.ConnectAsync();

                    //Console.WriteLine(endpoint.Name + " done ^^");

                    if (!endpoint.Name.Equals("Acala") && !endpoint.Name.Equals("Bifrost"))
                    {
                        var properties = await client.InvokeAsync<Properties>("system_properties", null, CancellationToken.None);

                        if ((properties.SS58Prefix.HasValue ? properties.SS58Prefix.Value : properties.Ss58Format) != endpoint.SS58Prefix)
                        {
                            Console.WriteLine("Bad SS58 prefix: " +
                                (properties.SS58Prefix.HasValue ? properties.SS58Prefix.Value : properties.Ss58Format)
                                + " (" + endpoint.SS58Prefix + ")");
                        }
                        if (properties.TokenDecimals != endpoint.Decimals)
                        {
                            Console.WriteLine("Bad decimals: " + properties.TokenDecimals + " (" + endpoint.Decimals + ")");
                        }
                        if (!properties.TokenSymbol.Trim().Equals(endpoint.Unit.Trim()))
                        {
                            Console.WriteLine("Bad token symbol: " + properties.TokenSymbol.Trim() + " (" + endpoint.Unit + ")");
                        }
                        var chainName = await client.System.ChainAsync();
                        if (!chainName.Trim().Equals(endpoint.Name.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Bad chain name: " + chainName + " (" + endpoint.Name + ")");
                        }
                    }

                    Hash genesisHash = await client.Chain.GetBlockHashAsync(new BlockNumber(0), CancellationToken.None);

                    //Console.WriteLine("{ \"" + genesisHash.Value.ToLower() + "\": \"" + endpoint.Key + "\" },");

                    Assert.That(PlutoWallet.Constants.Endpoints.HashToKey[genesisHash.Value.ToLower()].Equals(endpoint.Key));

                    client.Dispose();
                }
                catch(Exception ex)
                {
                    // Print the info about the fail
                    Console.WriteLine(ex);
                    Console.WriteLine(endpoint.Name + " <-> " + url);

                    Assert.Fail();
                }
            }
        }
    }
}




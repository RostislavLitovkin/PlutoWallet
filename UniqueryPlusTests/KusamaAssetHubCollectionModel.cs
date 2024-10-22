using KusamaAssetHub.NetApi.Generated;
using UniqueryPlus.Collections;
using UniqueryPlus;

namespace UniqueryPlusTests
{
    internal class KusamaAssetHubCollectionModel
    {
        private const string address = "5DAM8XCuWwxkh42NFBXaAnH6v7jYbd3uQjVKkLPre5LTtmTL";

#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        private SubstrateClientExt client;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

        [SetUp]
        public async Task SetupAsync()
        {
            client = new SubstrateClientExt(new Uri("wss://ksm-rpc.stakeworld.io/assethub"), default);

            await client.ConnectAsync();
        }
    }
}

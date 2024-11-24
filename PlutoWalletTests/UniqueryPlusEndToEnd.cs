using PlutoWallet.Constants;
using PlutoWallet.Model;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlutoWallet.Model.AjunaExt;
using UniqueryPlus.Collections;
using UniqueryPlus;
using CollectionModel = UniqueryPlus.Collections.CollectionModel;

namespace PlutoWalletTests
{
    internal class UniqueryPlusEndToEnd
    {
        static Account alice;

        [SetUp]
        public async Task SetupAsync()
        {
            var keyring = new Substrate.NET.Wallet.Keyring.Keyring();

            alice = keyring.AddFromUri("//Alice", default, KeyType.Sr25519).Account;

            Console.WriteLine(alice);
        }

        [Test]
        public async Task PolkadotAssetHub()
        {
            Endpoint endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Local8000];
            Endpoint realEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.PolkadotAssetHub];


            var client = new SubstrateClientExt(
                realEndpoint,
                        new Uri(endpoint.URLs[0]),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());


            await client.ConnectAndLoadMetadataAsync();

            var newCollectionId = await ((PolkadotAssetHub.NetApi.Generated.SubstrateClientExt)client.SubstrateClient).NftsStorage.NextCollectionId(null, CancellationToken.None);

            Console.WriteLine($"new Collection id: {newCollectionId}");

            var createCollection = CollectionModel.CreateCollection(NftTypeEnum.PolkadotAssetHub_NftsPallet, alice.Value, new CollectionMintConfig
            {
                MintType = new MintType
                {
                    CollectionId = null,
                    Type = MintTypeEnum.Public,
                },
                MintStartBlock = null,
                MintEndBlock = null,
                NftMaxSuply = 100,
                MintPrice = 100
            });

            Console.WriteLine();

            #region Temp
            var extrinsic = await client.GetTempUnCheckedExtrinsicAsync(createCollection, alice, 64, CancellationToken.None);
            #endregion

            var extrinsicHash = new Hash(HashExtension.Blake2(extrinsic.Encode(), 256));
            string extrinsicHashString = Utils.Bytes2HexString(extrinsicHash);

            Action<string, ExtrinsicStatus> updateExtrinsicsCallback = (string id, ExtrinsicStatus status) =>
            {
                 Console.WriteLine(status.ExtrinsicState);
            };

            var extrinsicId = await client.SubmitExtrinsicAsync(createCollection, alice, updateExtrinsicsCallback);

            await Task.Delay(20_000);

            var collection = await CollectionModel.GetCollectionByCollectionIdAsync(client.SubstrateClient, NftTypeEnum.PolkadotAssetHub_NftsPallet, newCollectionId.Value, CancellationToken.None);

            Assert.That(collection.Metadata, Is.Null);
        }
    }
}

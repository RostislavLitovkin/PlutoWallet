using System;
using Substrate.NetApi;
using PlutoWallet.Model.Storage;
using Newtonsoft.Json;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Extrinsics;

namespace PlutoWallet.Model.AjunaExt
{
	public class AjunaClientExt : SubstrateClient
	{
        public ChargeType DefaultCharge;

        public System.Collections.Generic.Dictionary<System.Tuple<string, string>, System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>> StorageKeyDict;

        public BalancesStorage BalancesStorage;

        public SystemStorage SystemStorage;

        public AssetsStorage AssetsStorage;

        public NftsStorage NftsStorage;

        public AssetRegistryStorage AssetRegistryStorage;

        public TokensStorage TokensStorage;

        public ContractsStorage ContractsStorage;

        public OmnipoolStorage OmnipoolStorage;

        public DCAStorage DCAStorage;

        // Logic for ink! contracts
        public ExtrinsicManager ExtrinsicManger { get; }

        public SubscriptionManager SubscriptionManager { get; }

        public AjunaClientExt(System.Uri uri, Substrate.NetApi.Model.Extrinsics.ChargeType chargeType) :
                base(uri, chargeType)
        {
            StorageKeyDict = new System.Collections.Generic.Dictionary<System.Tuple<string, string>, System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>>();

            this.SystemStorage = new SystemStorage(this);
            this.BalancesStorage = new BalancesStorage(this);
            this.AssetsStorage = new AssetsStorage(this);
            this.NftsStorage = new NftsStorage(this);
            this.AssetRegistryStorage = new AssetRegistryStorage(this);
            this.TokensStorage = new TokensStorage(this);
            this.ContractsStorage = new ContractsStorage(this);
            this.OmnipoolStorage = new OmnipoolStorage(this);
            this.DCAStorage = new DCAStorage(this);

            ExtrinsicManger = new ExtrinsicManager();

            SubscriptionManager = new SubscriptionManager();
        }

        public async Task ConnectAsync()
        {
            await base.ConnectAsync();

            Metadata customMetadata = JsonConvert.DeserializeObject<Metadata>(MetaData.Serialize());

            foreach (SignedExtension signedExtension in customMetadata.NodeMetadata.Extrinsic.SignedExtensions)
            {
                if (signedExtension.SignedIdentifier == "ChargeTransactionPayment")
                {
                    DefaultCharge = ChargeTransactionPayment.Default();
                }

                if (signedExtension.SignedIdentifier == "ChargeAssetTxPayment")
                {
                    DefaultCharge = ChargeAssetTxPayment.Default();
                }
            }
        }

    }
}


using System;
using Substrate.NetApi;
using PlutoWallet.Model.Storage;

namespace PlutoWallet.Model.AjunaExt
{
	public class AjunaClientExt : SubstrateClient
	{
        public System.Collections.Generic.Dictionary<System.Tuple<string, string>, System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>> StorageKeyDict;

        public BalancesStorage BalancesStorage;

        public SystemStorage SystemStorage;

        public AssetsStorage AssetsStorage;

        public NftsStorage NftsStorage;

        public ContractsStorage ContractsStorage;

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
            this.ContractsStorage = new ContractsStorage(this);

            ExtrinsicManger = new ExtrinsicManager();

            SubscriptionManager = new SubscriptionManager();
        }
	}
}


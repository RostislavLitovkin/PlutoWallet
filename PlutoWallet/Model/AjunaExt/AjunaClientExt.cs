using System;
using Ajuna.NetApi;

namespace PlutoWallet.Model.AjunaExt
{
	public class AjunaClientExt : SubstrateClient
	{
        public System.Collections.Generic.Dictionary<System.Tuple<string, string>, System.Tuple<Ajuna.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>> StorageKeyDict;

        public BalancesStorage BalancesStorage;

        public SystemStorage SystemStorage;


        public AjunaClientExt(System.Uri uri, Ajuna.NetApi.Model.Extrinsics.ChargeType chargeType) :
                base(uri, chargeType)
        {
            StorageKeyDict = new System.Collections.Generic.Dictionary<System.Tuple<string, string>, System.Tuple<Ajuna.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>>();

            this.SystemStorage = new SystemStorage(this);
            this.BalancesStorage = new BalancesStorage(this);
        }
	}
}


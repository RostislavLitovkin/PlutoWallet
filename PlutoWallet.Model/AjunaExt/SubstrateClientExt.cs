using System;
using System.Net.Http;
using System.Diagnostics;
using Substrate.NetApi;
using PlutoWallet.Model.Storage;
using Newtonsoft.Json;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using PlutoWallet.Constants;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Generated.Storage;

namespace PlutoWallet.Model.AjunaExt
{
	public class SubstrateClientExt : SubstrateClient
	{
        private static readonly HttpClient _httpClient = new HttpClient();

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

        public IdentityStorage IdentityStorage;

        public ConvictionVotingStorage ConvictionVotingStorage;

        public LBPStorage LBPStorage;

        public BifrostAssetRegistryStorage BifrostAssetRegistryStorage;

        public VtokenMintingStorage VtokenMintingStorage;

        public Endpoint Endpoint { get; set; }

        // Logic for ink! contracts

        public Metadata CustomMetadata { get; set; }

        public SubstrateClientExt(Endpoint endpoint, Uri fastestWebSocket, Substrate.NetApi.Model.Extrinsics.ChargeType chargeType) :
                base(fastestWebSocket, chargeType)
        {
            StorageKeyDict = new System.Collections.Generic.Dictionary<System.Tuple<string, string>, System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>>();

            Endpoint = endpoint;

            this.SystemStorage = new SystemStorage(this);
            this.BalancesStorage = new BalancesStorage(this);
            this.AssetsStorage = new AssetsStorage(this);
            this.NftsStorage = new NftsStorage(this);
            this.AssetRegistryStorage = new AssetRegistryStorage(this);
            this.TokensStorage = new TokensStorage(this);
            this.ContractsStorage = new ContractsStorage(this);
            this.OmnipoolStorage = new OmnipoolStorage(this);
            this.DCAStorage = new DCAStorage(this);
            this.IdentityStorage = new IdentityStorage(this);
            this.ConvictionVotingStorage = new ConvictionVotingStorage(this);
            this.LBPStorage = new LBPStorage(this);
            this.BifrostAssetRegistryStorage = new BifrostAssetRegistryStorage(this);
            this.VtokenMintingStorage = new VtokenMintingStorage(this);
        }

        public async Task ConnectAsync()
        {
            await base.ConnectAsync();

            CustomMetadata = JsonConvert.DeserializeObject<Metadata>(MetaData.Serialize());

            foreach (SignedExtension signedExtension in CustomMetadata.NodeMetadata.Extrinsic.SignedExtensions)
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


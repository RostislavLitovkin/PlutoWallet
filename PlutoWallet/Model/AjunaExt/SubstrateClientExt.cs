using System;
using Substrate.NetApi;
using PlutoWallet.Model.Storage;
using Newtonsoft.Json;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using PlutoWallet.Components.Extrinsic;
using PlutoWallet.Constants;
using Substrate.NetApi.Model.Types.Base;

namespace PlutoWallet.Model.AjunaExt
{
	public class SubstrateClientExt : SubstrateClient
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

        public IdentityStorage IdentityStorage;

        public ConvictionVotingStorage ConvictionVotingStorage;

        public Endpoint Endpoint { get; set; }

        // Logic for ink! contracts
        public ExtrinsicManager ExtrinsicManger { get; }

        public SubscriptionManager SubscriptionManager { get; }

        public SubstrateClientExt(Endpoint endpoint, Substrate.NetApi.Model.Extrinsics.ChargeType chargeType) :
                base(new Uri(endpoint.URL), chargeType)
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

        /// <summary>
        /// A custom method for submitting extrinsics.
        /// Please prefer using this one.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="token"></param>
        /// <returns>subscription ID</returns>
        public async Task<string> SubmitExtrinsicAsync(UnCheckedExtrinsic extrinsic, CancellationToken token)
        {
            var extrinsicStackViewModel = DependencyService.Get<ExtrinsicStatusStackViewModel>();

            extrinsicStackViewModel.Update();

            Action<string, ExtrinsicStatus> callback = (string id, ExtrinsicStatus status) =>
            {
                if (status.ExtrinsicState == ExtrinsicState.Ready)
                    Console.WriteLine("Ready");
                else if (status.ExtrinsicState == ExtrinsicState.Dropped)
                {
                    extrinsicStackViewModel.Extrinsics[id].Status = ExtrinsicStatusEnum.Failed;
                    extrinsicStackViewModel.Update();
                }

                else if (status.InBlock != null)
                {
                    Console.WriteLine("In block");
                    extrinsicStackViewModel.Extrinsics[id].Status = ExtrinsicStatusEnum.InBlock;
                    //extrinsicStackViewModel.Extrinsics[id].Hash = status.InBlock;
                    extrinsicStackViewModel.Update();
                }

                else if (status.Finalized != null)
                {
                    Console.WriteLine("Finalized");
                    extrinsicStackViewModel.Extrinsics[id].Status = ExtrinsicStatusEnum.Success;
                    //extrinsicStackViewModel.Extrinsics[id].Hash = status.Finalized;
                    extrinsicStackViewModel.Update();
                }

                else
                    Console.WriteLine(status.ExtrinsicState);
            };

            string extrinsicId = await this.Author.SubmitAndWatchExtrinsicAsync(callback, Utils.Bytes2HexString(extrinsic.Encode()), token);

            extrinsicStackViewModel.Extrinsics.Add(
                    extrinsicId,
                    new ExtrinsicInfo
                    {
                        ExtrinsicId = extrinsicId,
                        Status = ExtrinsicStatusEnum.Pending,
                        Endpoint = this.Endpoint,
                        Hash = new Hash(HashExtension.Blake2(extrinsic.Encode(), 256)),
                    });

            return extrinsicId;
        }
    }
}


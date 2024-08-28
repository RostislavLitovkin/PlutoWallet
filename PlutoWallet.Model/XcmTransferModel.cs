using System;
using PlutoWallet.Constants;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types.Primitive;
using System.Numerics;
using Polkadot.NetApi.Generated.Model.xcm.v3;
using Substrate.NetApi.Model.Types.Base;
using Polkadot.NetApi.Generated.Storage;

namespace PlutoWallet.Model
{
	public class XcmTransferModel
	{
        public static Method XcmTransfer(
            SubstrateClientExt client,
            Endpoint originEndpoint,
            Endpoint destinationEndpoint,
            string address,
            BigInteger amount,
            uint feeAsset = 0
        )
        {
            /*Method reserveTransfer = XcmPalletCalls.ReserveTransferAssets(
                // Destination Parachain
                XcmModel.GetChainLocation(destinationEndpoint.ParachainId),

                // Beneficiary
                XcmModel.GetAccountId32Location(address),

                // Asset and Amount
                XcmModel.GetNativeAsset(amount),

                // Fee asset
                (U32)feeAsset
            );*/

            Method reserveTransfer = XcmPalletCalls.LimitedTeleportAssets(
                // Destination Parachain
                default, //XcmModel.GetChainLocation(destinationEndpoint.ParachainId),

                // Beneficiary
                default, //XcmModel.GetAccountId32Location(address),

                // Asset and Amount
                default, //XcmModel.GetNativeAsset(amount),

                // Fee asset
                (U32)feeAsset,

                GetUnlimitedWeight()
            );

            (byte palletIndex, byte callIndex) = XcmTeleportTransferIndex(client, originEndpoint);

            reserveTransfer.ModuleIndex = palletIndex;
            reserveTransfer.CallIndex = callIndex;

            return reserveTransfer;
        }

        public static EnumWeightLimit GetUnlimitedWeight()
        {
            var weight = new EnumWeightLimit();
            weight.Create(WeightLimit.Unlimited, new BaseVoid());

            return weight;
        }

        public static (byte, byte) XcmTeleportTransferIndex(SubstrateClientExt client, Endpoint originEndpoint)
        {
            if (originEndpoint.ParachainId?.Chain == Chain.Relay)
            {
                return PalletCallModel.GetPalletAndCallIndex(client, "XcmPallet", "limited_teleport_assets");
            }
            else
            {
                return PalletCallModel.GetPalletAndCallIndex(client, "PolkadotXcm", "limited_teleport_assets");
            }
        }
    }
}


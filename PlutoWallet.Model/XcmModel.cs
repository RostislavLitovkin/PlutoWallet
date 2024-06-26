using Polkadot.NetApi.Generated.Model.staging_xcm.v3.multilocation;
using Substrate.NetApi.Model.Types.Base;
using Polkadot.NetApi.Generated.Model.xcm.v3.junctions;
using Polkadot.NetApi.Generated.Model.xcm.v3.junction;
using Substrate.NetApi.Model.Types.Primitive;
using Polkadot.NetApi.Generated.Model.xcm;
using Substrate.NetApi;
using Polkadot.NetApi.Generated.Types.Base;
using Polkadot.NetApi.Generated.Model.xcm.v2.multiasset;
using System.Numerics;
using PlutoWallet.Constants;

namespace PlutoWallet.Model
{
	public class XcmModel
	{
		public static EnumVersionedLocation GetChainLocation(ParachainId parachainId, byte parents = 0)
		{

            EnumJunctions junctions = new EnumJunctions();

            if (parachainId.Chain == Chain.Relay)
            {
                junctions.Create(Junctions.Here, new BaseVoid());
            }
            else if (parachainId.Chain == Chain.Parachain && parachainId.Id.HasValue)
            {
                EnumJunction junction = new EnumJunction();

                junction.Create(Junction.Parachain, new BaseCom<U32>(parachainId.Id.Value));

                junctions.Create(Junctions.X1, junction);
            }
            else
            {
                throw new Exception("Other chain types are unsupported for XCM calls.");
            }


            MultiLocation multiLocation = new MultiLocation
            {
                Parents = (U8)parents,
                Interior = junctions,
            };

            EnumVersionedLocation location = new EnumVersionedLocation();
            location.Create(
                VersionedLocation.V3,
                multiLocation
            );

            return location;
        }

        public static EnumVersionedLocation GetAccountId32Location(string address)
        {
            var arr = new Arr32U8();
            arr.Create(Utils.GetPublicKeyFrom(address));

            var accountId32 = new BaseTuple<BaseOpt<EnumNetworkId>, Arr32U8>(
                new BaseOpt<EnumNetworkId>(),
                arr
            );

            EnumJunction junction = new EnumJunction();
            junction.Create(Junction.AccountId32, accountId32);

            EnumJunctions junctions = new EnumJunctions();
            junctions.Create(Junctions.X1, junction);

            MultiLocation multiLocation = new MultiLocation
            {
                Parents = (U8)0,
                Interior = junctions,
            };

            EnumVersionedLocation location = new EnumVersionedLocation();
            location.Create(VersionedLocation.V3, multiLocation);

            return location;
        }

        public static EnumVersionedAssets GetNativeAsset(BigInteger amount, byte parents = 0)
        {
            EnumFungibility fungible = new EnumFungibility();
            fungible.Create(Fungibility.Fungible, new BaseCom<U128>(amount));

            EnumJunctions junctions = new EnumJunctions();
            junctions.Create(Junctions.Here, new BaseVoid());

            MultiLocation multiLocation = new MultiLocation
            {
                Parents = (U8)parents,
                Interior = junctions,
            };

            EnumAssetId assetId = new EnumAssetId();
            assetId.Create(AssetId.Concrete, multiLocation);

            MultiAssets multiAssets = new MultiAssets
            {
                Value = new BaseVec<MultiAsset>(new MultiAsset[] {
                    new MultiAsset {
                        Fun = fungible,
                        Id = assetId,
                    },
                }),
            };

            EnumVersionedAssets assets = new EnumVersionedAssets();

            assets.Create(
                VersionedAssets.V3,
                multiAssets
            );

            return assets;
        }
    }
}


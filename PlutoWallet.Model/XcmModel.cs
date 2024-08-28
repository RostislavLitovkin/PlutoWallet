using Polkadot.NetApi.Generated.Model.staging_xcm.v4.junctions;
using System.Numerics;
using PlutoWallet.Constants;
using Location = Polkadot.NetApi.Generated.Model.staging_xcm.v4.location.Location;
using PlutoWallet.Types;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;

namespace PlutoWallet.Model
{
    public class XcmModel
    {

        public static (EndpointEnum, AssetPallet, BigInteger)? GetAssetFromXcmLocation(Location location, Endpoint endpoint)
        {
            return location switch
            {
                Location
                {
                    Parents: { Value: 0 },
                    Interior: { Value: Junctions.Here }
                } => (endpoint.Key, AssetPallet.Native, 0),

                // Support Other Locations
                _ => null
            };
        }

        /// <summary>
        /// Checks if the runtime call is an XCM call and returns the destination chain
        /// </summary>
        /// <returns>Destination chain key</returns>
        public static EndpointEnum? IsMethodXcm(Endpoint endpoint, Method call)
        {
            int p = 0;

            var callEncoded = call.Encode();

            var paraId = endpoint.Key switch
            {
                EndpointEnum.Hydration => GetHydrationXcmDestination(callEncoded),
                _ => null,
            };

            if (paraId == null)
            {
                return null;
            };

            var key = new ParachainId
            {
                Relay = endpoint.ParachainId.Relay,
                Chain = Chain.Parachain,
                Id = (uint)paraId
            };

            return Endpoints.ParachainIdToKey[key.ToString()];
        }

        /// <summary>
        /// Find the destination of the XcmMessage for the Hydration chain
        /// </summary>
        /// <returns>ParaId</returns>
        public static BigInteger? GetHydrationXcmDestination(byte[] callEncoded)
        {
            /// Decode the RuntimeCall specific to the Hydration parachain
            var runtimeCall = new Hydration.NetApi.Generated.Model.hydradx_runtime.EnumRuntimeCall();

            runtimeCall.Create(callEncoded);

            /// Find the right pallet and call
            return runtimeCall.Value switch
            {
                Hydration.NetApi.Generated.Model.hydradx_runtime.RuntimeCall.XTokens => ((Hydration.NetApi.Generated.Model.orml_xtokens.module.EnumCall)runtimeCall.Value2).Value switch
                {
                    Hydration.NetApi.Generated.Model.orml_xtokens.module.Call.transfer_multiasset => GetParaIdFromXcmLocation((Hydration.NetApi.Generated.Model.xcm.EnumVersionedLocation)((BaseTuple<Hydration.NetApi.Generated.Model.xcm.EnumVersionedAsset, Hydration.NetApi.Generated.Model.xcm.EnumVersionedLocation, Hydration.NetApi.Generated.Model.xcm.v3.EnumWeightLimit>)((Hydration.NetApi.Generated.Model.orml_xtokens.module.EnumCall)runtimeCall.Value2).Value2).Value[1]),

                    /// Handle more calls

                    _ => null,
                },

                /// Handle more pallets

                _ => null,
            };
        }

        /// <summary>
        /// Find the ParaId in the xcm location
        /// </summary>
        /// <returns>ParaId</returns>
        public static BigInteger? GetParaIdFromXcmLocation(Hydration.NetApi.Generated.Model.xcm.EnumVersionedLocation location)
        {
            return location.Value switch
            {
                Hydration.NetApi.Generated.Model.xcm.VersionedLocation.V4 => ((Hydration.NetApi.Generated.Model.staging_xcm.v4.location.Location)location.Value2).Interior.Value switch
                {
                    Hydration.NetApi.Generated.Model.staging_xcm.v4.junctions.Junctions.X1 => ((Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>)((Hydration.NetApi.Generated.Model.staging_xcm.v4.junction.Arr1EnumJunction)((Hydration.NetApi.Generated.Model.staging_xcm.v4.location.Location)location.Value2).Interior.Value2).Value.First((j) => j.Value is Hydration.NetApi.Generated.Model.staging_xcm.v4.junction.Junction.Parachain).Value2).Value.Value,

                    Hydration.NetApi.Generated.Model.staging_xcm.v4.junctions.Junctions.X2 => ((Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>)((Hydration.NetApi.Generated.Model.staging_xcm.v4.junction.Arr2EnumJunction)((Hydration.NetApi.Generated.Model.staging_xcm.v4.location.Location)location.Value2).Interior.Value2).Value.First((j) => j.Value is Hydration.NetApi.Generated.Model.staging_xcm.v4.junction.Junction.Parachain).Value2).Value.Value,

                    Hydration.NetApi.Generated.Model.staging_xcm.v4.junctions.Junctions.X3 => ((Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>)((Hydration.NetApi.Generated.Model.staging_xcm.v4.junction.Arr3EnumJunction)((Hydration.NetApi.Generated.Model.staging_xcm.v4.location.Location)location.Value2).Interior.Value2).Value.First((j) => j.Value is Hydration.NetApi.Generated.Model.staging_xcm.v4.junction.Junction.Parachain).Value2).Value.Value,

                    Hydration.NetApi.Generated.Model.staging_xcm.v4.junctions.Junctions.X4 => ((Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>)((Hydration.NetApi.Generated.Model.staging_xcm.v4.junction.Arr4EnumJunction)((Hydration.NetApi.Generated.Model.staging_xcm.v4.location.Location)location.Value2).Interior.Value2).Value.First((j) => j.Value is Hydration.NetApi.Generated.Model.staging_xcm.v4.junction.Junction.Parachain).Value2).Value.Value,

                    /// Handle other (larger) junctions

                    _ => null,
                },

                Hydration.NetApi.Generated.Model.xcm.VersionedLocation.V3 => ((Hydration.NetApi.Generated.Model.staging_xcm.v3.multilocation.MultiLocation)location.Value2).Interior.Value switch
                {
                    Hydration.NetApi.Generated.Model.xcm.v3.junctions.Junctions.X1 => ((Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>)((Hydration.NetApi.Generated.Model.xcm.v3.junction.EnumJunction)new IType[] { ((Hydration.NetApi.Generated.Model.staging_xcm.v3.multilocation.MultiLocation)location.Value2).Interior.Value2 }.First((j) => ((Hydration.NetApi.Generated.Model.xcm.v3.junction.EnumJunction)j).Value is Hydration.NetApi.Generated.Model.xcm.v3.junction.Junction.Parachain)).Value2).Value.Value,

                    Hydration.NetApi.Generated.Model.xcm.v3.junctions.Junctions.X2 => ((Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>)((Hydration.NetApi.Generated.Model.xcm.v3.junction.EnumJunction)((BaseTuple<Hydration.NetApi.Generated.Model.xcm.v3.junction.EnumJunction, Hydration.NetApi.Generated.Model.xcm.v3.junction.EnumJunction>)((Hydration.NetApi.Generated.Model.staging_xcm.v3.multilocation.MultiLocation)location.Value2).Interior.Value2).Value.First((j) => ((Hydration.NetApi.Generated.Model.xcm.v3.junction.EnumJunction)j).Value is Hydration.NetApi.Generated.Model.xcm.v3.junction.Junction.Parachain)).Value2).Value.Value,

                    /// Handle other (larger) junctions

                    _ => null,
                },

                
                /// Handle other (older) versions

                _ => null
            };
        }

        /*
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
        */
    }
}


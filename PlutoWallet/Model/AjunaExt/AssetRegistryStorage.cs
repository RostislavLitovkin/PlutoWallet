//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Meta;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWallet.Model.AjunaExt
{
    public sealed class AssetRegistryStorage
    {
        
        // Substrate client for the storage calls.
        private AjunaClientExt _client;
        
        public AssetRegistryStorage(AjunaClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("AssetRegistry", "Assets"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(PlutoWallet.NetApiExt.Generated.Model.pallet_asset_registry.types.AssetDetails)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("AssetRegistry", "NextAssetId"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("AssetRegistry", "AssetIds"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(PlutoWallet.NetApiExt.Generated.Model.sp_core.bounded.bounded_vec.BoundedVecT4), typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("AssetRegistry", "AssetLocations"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(PlutoWallet.NetApiExt.Generated.Model.hydradx_runtime.xcm.AssetLocation)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("AssetRegistry", "LocationAssets"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(PlutoWallet.NetApiExt.Generated.Model.hydradx_runtime.xcm.AssetLocation), typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("AssetRegistry", "AssetMetadataMap"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(PlutoWallet.NetApiExt.Generated.Model.pallet_asset_registry.types.AssetMetadata)));
        }
        
        /// <summary>
        /// >> AssetsParams
        ///  Details of an asset.
        /// </summary>
        public static string AssetsParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("AssetRegistry", "Assets", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> AssetsDefault
        /// Default value as hex string
        /// </summary>
        public static string AssetsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> Assets
        ///  Details of an asset.
        /// </summary>
        public async Task<PlutoWallet.NetApiExt.Generated.Model.pallet_asset_registry.types.AssetDetails> Assets(Substrate.NetApi.Model.Types.Primitive.U32 key, CancellationToken token)
        {
            string parameters = AssetRegistryStorage.AssetsParams(key);
            var result = await _client.GetStorageAsync<PlutoWallet.NetApiExt.Generated.Model.pallet_asset_registry.types.AssetDetails>(parameters, token);
            return result;
        }
        
        /// <summary>
        /// >> NextAssetIdParams
        ///  Next available asset id. This is sequential id assigned for each new registered asset.
        /// </summary>
        public static string NextAssetIdParams()
        {
            return RequestGenerator.GetStorage("AssetRegistry", "NextAssetId", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> NextAssetIdDefault
        /// Default value as hex string
        /// </summary>
        public static string NextAssetIdDefault()
        {
            return "0x00000000";
        }
        
        /// <summary>
        /// >> NextAssetId
        ///  Next available asset id. This is sequential id assigned for each new registered asset.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> NextAssetId(CancellationToken token)
        {
            string parameters = AssetRegistryStorage.NextAssetIdParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, token);
            return result;
        }
        
        /// <summary>
        /// >> AssetIdsParams
        ///  Mapping between asset name and asset id.
        /// </summary>
        public static string AssetIdsParams(PlutoWallet.NetApiExt.Generated.Model.sp_core.bounded.bounded_vec.BoundedVecT4 key)
        {
            return RequestGenerator.GetStorage("AssetRegistry", "AssetIds", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> AssetIdsDefault
        /// Default value as hex string
        /// </summary>
        public static string AssetIdsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> AssetIds
        ///  Mapping between asset name and asset id.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> AssetIds(PlutoWallet.NetApiExt.Generated.Model.sp_core.bounded.bounded_vec.BoundedVecT4 key, CancellationToken token)
        {
            string parameters = AssetRegistryStorage.AssetIdsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, token);
            return result;
        }
        
        /// <summary>
        /// >> AssetLocationsParams
        ///  Native location of an asset.
        /// </summary>
        public static string AssetLocationsParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("AssetRegistry", "AssetLocations", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> AssetLocationsDefault
        /// Default value as hex string
        /// </summary>
        public static string AssetLocationsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> AssetLocations
        ///  Native location of an asset.
        /// </summary>
        public async Task<PlutoWallet.NetApiExt.Generated.Model.hydradx_runtime.xcm.AssetLocation> AssetLocations(Substrate.NetApi.Model.Types.Primitive.U32 key, CancellationToken token)
        {
            string parameters = AssetRegistryStorage.AssetLocationsParams(key);
            var result = await _client.GetStorageAsync<PlutoWallet.NetApiExt.Generated.Model.hydradx_runtime.xcm.AssetLocation>(parameters, token);
            return result;
        }
        
        /// <summary>
        /// >> LocationAssetsParams
        ///  Local asset for native location.
        /// </summary>
        public static string LocationAssetsParams(PlutoWallet.NetApiExt.Generated.Model.hydradx_runtime.xcm.AssetLocation key)
        {
            return RequestGenerator.GetStorage("AssetRegistry", "LocationAssets", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> LocationAssetsDefault
        /// Default value as hex string
        /// </summary>
        public static string LocationAssetsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> LocationAssets
        ///  Local asset for native location.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> LocationAssets(PlutoWallet.NetApiExt.Generated.Model.hydradx_runtime.xcm.AssetLocation key, CancellationToken token)
        {
            string parameters = AssetRegistryStorage.LocationAssetsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, token);
            return result;
        }
        
        /// <summary>
        /// >> AssetMetadataMapParams
        ///  Metadata of an asset.
        /// </summary>
        public static string AssetMetadataMapParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("AssetRegistry", "AssetMetadataMap", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> AssetMetadataMapDefault
        /// Default value as hex string
        /// </summary>
        public static string AssetMetadataMapDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> AssetMetadataMap
        ///  Metadata of an asset.
        /// </summary>
        public async Task<PlutoWallet.NetApiExt.Generated.Model.pallet_asset_registry.types.AssetMetadata> AssetMetadataMap(Substrate.NetApi.Model.Types.Primitive.U32 key, CancellationToken token)
        {
            string parameters = AssetRegistryStorage.AssetMetadataMapParams(key);
            var result = await _client.GetStorageAsync<PlutoWallet.NetApiExt.Generated.Model.pallet_asset_registry.types.AssetMetadata>(parameters, token);
            return result;
        }
    }
    
    public sealed class AssetRegistryCalls
    {
        
        /// <summary>
        /// >> register
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method Register(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> name, PlutoWallet.NetApiExt.Generated.Model.pallet_asset_registry.types.EnumAssetType asset_type, Substrate.NetApi.Model.Types.Primitive.U128 existential_deposit, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U32> asset_id, Substrate.NetApi.Model.Types.Base.BaseOpt<PlutoWallet.NetApiExt.Generated.Model.pallet_asset_registry.types.Metadata> metadata, Substrate.NetApi.Model.Types.Base.BaseOpt<PlutoWallet.NetApiExt.Generated.Model.hydradx_runtime.xcm.AssetLocation> location, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U128> xcm_rate_limit)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(name.Encode());
            byteArray.AddRange(asset_type.Encode());
            byteArray.AddRange(existential_deposit.Encode());
            byteArray.AddRange(asset_id.Encode());
            byteArray.AddRange(metadata.Encode());
            byteArray.AddRange(location.Encode());
            byteArray.AddRange(xcm_rate_limit.Encode());
            return new Method(51, "AssetRegistry", 0, "register", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> update
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method Update(Substrate.NetApi.Model.Types.Primitive.U32 asset_id, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> name, PlutoWallet.NetApiExt.Generated.Model.pallet_asset_registry.types.EnumAssetType asset_type, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U128> existential_deposit, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U128> xcm_rate_limit)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(asset_id.Encode());
            byteArray.AddRange(name.Encode());
            byteArray.AddRange(asset_type.Encode());
            byteArray.AddRange(existential_deposit.Encode());
            byteArray.AddRange(xcm_rate_limit.Encode());
            return new Method(51, "AssetRegistry", 1, "update", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> set_metadata
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method SetMetadata(Substrate.NetApi.Model.Types.Primitive.U32 asset_id, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> symbol, Substrate.NetApi.Model.Types.Primitive.U8 decimals)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(asset_id.Encode());
            byteArray.AddRange(symbol.Encode());
            byteArray.AddRange(decimals.Encode());
            return new Method(51, "AssetRegistry", 2, "set_metadata", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> set_location
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method SetLocation(Substrate.NetApi.Model.Types.Primitive.U32 asset_id, PlutoWallet.NetApiExt.Generated.Model.hydradx_runtime.xcm.AssetLocation location)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(asset_id.Encode());
            byteArray.AddRange(location.Encode());
            return new Method(51, "AssetRegistry", 3, "set_location", byteArray.ToArray());
        }
    }
    
    public sealed class AssetRegistryConstants
    {
        
        /// <summary>
        /// >> SequentialIdStartAt
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 SequentialIdStartAt()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U32();
            result.Create("0x40420F00");
            return result;
        }
        
        /// <summary>
        /// >> NativeAssetId
        ///  Native Asset Id
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 NativeAssetId()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U32();
            result.Create("0x00000000");
            return result;
        }
    }
    
    public enum AssetRegistryErrors
    {
        
        /// <summary>
        /// >> NoIdAvailable
        /// Asset ID is not available. This only happens when it reaches the MAX value of given id type.
        /// </summary>
        NoIdAvailable,
        
        /// <summary>
        /// >> AssetNotFound
        /// Invalid asset name or symbol.
        /// </summary>
        AssetNotFound,
        
        /// <summary>
        /// >> TooLong
        /// Invalid asset name or symbol.
        /// </summary>
        TooLong,
        
        /// <summary>
        /// >> AssetNotRegistered
        /// Asset ID is not registered in the asset-registry.
        /// </summary>
        AssetNotRegistered,
        
        /// <summary>
        /// >> AssetAlreadyRegistered
        /// Asset is already registered.
        /// </summary>
        AssetAlreadyRegistered,
        
        /// <summary>
        /// >> InvalidSharedAssetLen
        /// Incorrect number of assets provided to create shared asset.
        /// </summary>
        InvalidSharedAssetLen,
        
        /// <summary>
        /// >> CannotUpdateLocation
        /// Cannot update asset location
        /// </summary>
        CannotUpdateLocation,
        
        /// <summary>
        /// >> NotInReservedRange
        /// Selected asset id is out of reserved range.
        /// </summary>
        NotInReservedRange,
        
        /// <summary>
        /// >> LocationAlreadyRegistered
        /// Location already registered with different asset
        /// </summary>
        LocationAlreadyRegistered,
    }
}
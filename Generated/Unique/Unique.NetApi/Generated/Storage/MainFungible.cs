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


namespace Unique.NetApi.Generated.Storage
{
    
    
    /// <summary>
    /// >> FungibleStorage
    /// </summary>
    public sealed class FungibleStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        /// <summary>
        /// >> FungibleStorage Constructor
        /// </summary>
        public FungibleStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Fungible", "TotalSupply"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Unique.NetApi.Generated.Model.up_data_structs.CollectionId), typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Fungible", "Balance"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat,
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.NetApi.Model.Types.Base.BaseTuple<Unique.NetApi.Generated.Model.up_data_structs.CollectionId, Unique.NetApi.Generated.Model.pallet_evm.account.EnumBasicCrossAccountIdRepr>), typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Fungible", "Allowance"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat,
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128,
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.NetApi.Model.Types.Base.BaseTuple<Unique.NetApi.Generated.Model.up_data_structs.CollectionId, Unique.NetApi.Generated.Model.pallet_evm.account.EnumBasicCrossAccountIdRepr, Unique.NetApi.Generated.Model.pallet_evm.account.EnumBasicCrossAccountIdRepr>), typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
        }
        
        /// <summary>
        /// >> TotalSupplyParams
        ///  Total amount of fungible tokens inside a collection.
        /// </summary>
        public static string TotalSupplyParams(Unique.NetApi.Generated.Model.up_data_structs.CollectionId key)
        {
            return RequestGenerator.GetStorage("Fungible", "TotalSupply", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> TotalSupplyDefault
        /// Default value as hex string
        /// </summary>
        public static string TotalSupplyDefault()
        {
            return "0x00000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> TotalSupply
        ///  Total amount of fungible tokens inside a collection.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> TotalSupply(Unique.NetApi.Generated.Model.up_data_structs.CollectionId key, string blockhash, CancellationToken token)
        {
            string parameters = FungibleStorage.TotalSupplyParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> BalanceParams
        ///  Amount of tokens owned by an account inside a collection.
        /// </summary>
        public static string BalanceParams(Substrate.NetApi.Model.Types.Base.BaseTuple<Unique.NetApi.Generated.Model.up_data_structs.CollectionId, Unique.NetApi.Generated.Model.pallet_evm.account.EnumBasicCrossAccountIdRepr> key)
        {
            return RequestGenerator.GetStorage("Fungible", "Balance", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat,
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, key.Value);
        }
        
        /// <summary>
        /// >> BalanceDefault
        /// Default value as hex string
        /// </summary>
        public static string BalanceDefault()
        {
            return "0x00000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> Balance
        ///  Amount of tokens owned by an account inside a collection.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> Balance(Substrate.NetApi.Model.Types.Base.BaseTuple<Unique.NetApi.Generated.Model.up_data_structs.CollectionId, Unique.NetApi.Generated.Model.pallet_evm.account.EnumBasicCrossAccountIdRepr> key, string blockhash, CancellationToken token)
        {
            string parameters = FungibleStorage.BalanceParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> AllowanceParams
        ///  Storage for assets delegated to a limited extent to other users.
        /// </summary>
        public static string AllowanceParams(Substrate.NetApi.Model.Types.Base.BaseTuple<Unique.NetApi.Generated.Model.up_data_structs.CollectionId, Unique.NetApi.Generated.Model.pallet_evm.account.EnumBasicCrossAccountIdRepr, Unique.NetApi.Generated.Model.pallet_evm.account.EnumBasicCrossAccountIdRepr> key)
        {
            return RequestGenerator.GetStorage("Fungible", "Allowance", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat,
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128,
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, key.Value);
        }
        
        /// <summary>
        /// >> AllowanceDefault
        /// Default value as hex string
        /// </summary>
        public static string AllowanceDefault()
        {
            return "0x00000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> Allowance
        ///  Storage for assets delegated to a limited extent to other users.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> Allowance(Substrate.NetApi.Model.Types.Base.BaseTuple<Unique.NetApi.Generated.Model.up_data_structs.CollectionId, Unique.NetApi.Generated.Model.pallet_evm.account.EnumBasicCrossAccountIdRepr, Unique.NetApi.Generated.Model.pallet_evm.account.EnumBasicCrossAccountIdRepr> key, string blockhash, CancellationToken token)
        {
            string parameters = FungibleStorage.AllowanceParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, blockhash, token);
            return result;
        }
    }
    
    /// <summary>
    /// >> FungibleCalls
    /// </summary>
    public sealed class FungibleCalls
    {
    }
    
    /// <summary>
    /// >> FungibleConstants
    /// </summary>
    public sealed class FungibleConstants
    {
    }
    
    /// <summary>
    /// >> FungibleErrors
    /// </summary>
    public enum FungibleErrors
    {
        
        /// <summary>
        /// >> FungibleItemsDontHaveData
        /// Tried to set data for fungible item.
        /// </summary>
        FungibleItemsDontHaveData,
        
        /// <summary>
        /// >> FungibleDisallowsNesting
        /// Fungible token does not support nesting.
        /// </summary>
        FungibleDisallowsNesting,
        
        /// <summary>
        /// >> SettingPropertiesNotAllowed
        /// Setting item properties is not allowed.
        /// </summary>
        SettingPropertiesNotAllowed,
        
        /// <summary>
        /// >> SettingAllowanceForAllNotAllowed
        /// Setting allowance for all is not allowed.
        /// </summary>
        SettingAllowanceForAllNotAllowed,
        
        /// <summary>
        /// >> FungibleTokensAreAlwaysValid
        /// Only a fungible collection could be possibly broken; any fungible token is valid.
        /// </summary>
        FungibleTokensAreAlwaysValid,
    }
}
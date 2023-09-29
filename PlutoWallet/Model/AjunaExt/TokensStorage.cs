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
    public sealed class TokensStorage
    {
        // Substrate client for the storage calls.
        private AjunaClientExt _client;
        
        public TokensStorage(AjunaClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Tokens", "TotalIssuance"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Tokens", "Locks"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat,
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32>), typeof(Substrate.NetApi.Generated.Model.sp_core.bounded.bounded_vec.BoundedVecT26)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Tokens", "Accounts"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat,
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32>), typeof(Substrate.NetApi.Generated.Model.orml_tokens.AccountData)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Tokens", "Reserves"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat,
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32>), typeof(Substrate.NetApi.Generated.Model.sp_core.bounded.bounded_vec.BoundedVecT27)));
        }
        
        /// <summary>
        /// >> TotalIssuanceParams
        ///  The total issuance of a token type.
        /// </summary>
        public static string TotalIssuanceParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("Tokens", "TotalIssuance", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> TotalIssuanceDefault
        /// Default value as hex string
        /// </summary>
        public static string TotalIssuanceDefault()
        {
            return "0x00000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> TotalIssuance
        ///  The total issuance of a token type.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> TotalIssuance(Substrate.NetApi.Model.Types.Primitive.U32 key, CancellationToken token)
        {
            string parameters = TokensStorage.TotalIssuanceParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, token);
            return result;
        }
        
        /// <summary>
        /// >> LocksParams
        ///  Any liquidity locks of a token type under an account.
        ///  NOTE: Should only be accessed when setting, changing and freeing a lock.
        /// </summary>
        public static string LocksParams(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32> key)
        {
            return RequestGenerator.GetStorage("Tokens", "Locks", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat,
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, key.Value);
        }
        
        /// <summary>
        /// >> LocksDefault
        /// Default value as hex string
        /// </summary>
        public static string LocksDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> Locks
        ///  Any liquidity locks of a token type under an account.
        ///  NOTE: Should only be accessed when setting, changing and freeing a lock.
        /// </summary>
        public async Task<Substrate.NetApi.Generated.Model.sp_core.bounded.bounded_vec.BoundedVecT26> Locks(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32> key, CancellationToken token)
        {
            string parameters = TokensStorage.LocksParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Generated.Model.sp_core.bounded.bounded_vec.BoundedVecT26>(parameters, token);
            return result;
        }
        
        /// <summary>
        /// >> AccountsParams
        ///  The balance of a token type under an account.
        /// 
        ///  NOTE: If the total is ever zero, decrease account ref account.
        /// 
        ///  NOTE: This is only used in the case that this module is used to store
        ///  balances.
        /// </summary>
        public static string AccountsParams(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32> key)
        {
            return RequestGenerator.GetStorage("Tokens", "Accounts", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat,
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, key.Value);
        }
        
        /// <summary>
        /// >> AccountsDefault
        /// Default value as hex string
        /// </summary>
        public static string AccountsDefault()
        {
            return "0x0000000000000000000000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000";
        }
        
        /// <summary>
        /// >> Accounts
        ///  The balance of a token type under an account.
        /// 
        ///  NOTE: If the total is ever zero, decrease account ref account.
        /// 
        ///  NOTE: This is only used in the case that this module is used to store
        ///  balances.
        /// </summary>
        public async Task<Substrate.NetApi.Generated.Model.orml_tokens.AccountData> Accounts(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32> key, CancellationToken token)
        {
            string parameters = TokensStorage.AccountsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Generated.Model.orml_tokens.AccountData>(parameters, token);
            return result;
        }
        
        /// <summary>
        /// >> ReservesParams
        ///  Named reserves on some account balances.
        /// </summary>
        public static string ReservesParams(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32> key)
        {
            return RequestGenerator.GetStorage("Tokens", "Reserves", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat,
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, key.Value);
        }
        
        /// <summary>
        /// >> ReservesDefault
        /// Default value as hex string
        /// </summary>
        public static string ReservesDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> Reserves
        ///  Named reserves on some account balances.
        /// </summary>
        public async Task<Substrate.NetApi.Generated.Model.sp_core.bounded.bounded_vec.BoundedVecT27> Reserves(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32> key, CancellationToken token)
        {
            string parameters = TokensStorage.ReservesParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Generated.Model.sp_core.bounded.bounded_vec.BoundedVecT27>(parameters, token);
            return result;
        }
    }
    
    public sealed class TokensCalls
    {
        
        /// <summary>
        /// >> transfer
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method Transfer(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 dest, Substrate.NetApi.Model.Types.Primitive.U32 currency_id, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U128> amount)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(dest.Encode());
            byteArray.AddRange(currency_id.Encode());
            byteArray.AddRange(amount.Encode());
            return new Method(77, "Tokens", 0, "transfer", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> transfer_all
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method TransferAll(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 dest, Substrate.NetApi.Model.Types.Primitive.U32 currency_id, Substrate.NetApi.Model.Types.Primitive.Bool keep_alive)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(dest.Encode());
            byteArray.AddRange(currency_id.Encode());
            byteArray.AddRange(keep_alive.Encode());
            return new Method(77, "Tokens", 1, "transfer_all", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> transfer_keep_alive
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method TransferKeepAlive(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 dest, Substrate.NetApi.Model.Types.Primitive.U32 currency_id, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U128> amount)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(dest.Encode());
            byteArray.AddRange(currency_id.Encode());
            byteArray.AddRange(amount.Encode());
            return new Method(77, "Tokens", 2, "transfer_keep_alive", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> force_transfer
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method ForceTransfer(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 source, Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 dest, Substrate.NetApi.Model.Types.Primitive.U32 currency_id, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U128> amount)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(source.Encode());
            byteArray.AddRange(dest.Encode());
            byteArray.AddRange(currency_id.Encode());
            byteArray.AddRange(amount.Encode());
            return new Method(77, "Tokens", 3, "force_transfer", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> set_balance
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method SetBalance(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 who, Substrate.NetApi.Model.Types.Primitive.U32 currency_id, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U128> new_free, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U128> new_reserved)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(who.Encode());
            byteArray.AddRange(currency_id.Encode());
            byteArray.AddRange(new_free.Encode());
            byteArray.AddRange(new_reserved.Encode());
            return new Method(77, "Tokens", 4, "set_balance", byteArray.ToArray());
        }
    }
    
    public sealed class TokensConstants
    {
        
        /// <summary>
        /// >> MaxLocks
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 MaxLocks()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U32();
            result.Create("0x32000000");
            return result;
        }
        
        /// <summary>
        /// >> MaxReserves
        ///  The maximum number of named reserves that can exist on an account.
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 MaxReserves()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U32();
            result.Create("0x32000000");
            return result;
        }
    }
    
    public enum TokensErrors
    {
        
        /// <summary>
        /// >> BalanceTooLow
        /// The balance is too low
        /// </summary>
        BalanceTooLow,
        
        /// <summary>
        /// >> AmountIntoBalanceFailed
        /// Cannot convert Amount into Balance type
        /// </summary>
        AmountIntoBalanceFailed,
        
        /// <summary>
        /// >> LiquidityRestrictions
        /// Failed because liquidity restrictions due to locking
        /// </summary>
        LiquidityRestrictions,
        
        /// <summary>
        /// >> MaxLocksExceeded
        /// Failed because the maximum locks was exceeded
        /// </summary>
        MaxLocksExceeded,
        
        /// <summary>
        /// >> KeepAlive
        /// Transfer/payment would kill account
        /// </summary>
        KeepAlive,
        
        /// <summary>
        /// >> ExistentialDeposit
        /// Value too low to create account due to existential deposit
        /// </summary>
        ExistentialDeposit,
        
        /// <summary>
        /// >> DeadAccount
        /// Beneficiary account must pre-exist
        /// </summary>
        DeadAccount,
        
        /// <summary>
        /// >> TooManyReserves
        /// </summary>
        TooManyReserves,
    }
}

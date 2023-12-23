using System;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Generated.Model.pallet_balances;
using Substrate.NetApi.Generated.Model.pallet_staking;
using Substrate.NetApi.Generated.Model.sp_core.bounded.bounded_vec;
using Substrate.NetApi.Generated.Model.sp_core.bounded.weak_bounded_vec;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using Substrate.NetApi.Generated.Model.sp_runtime.multiaddress;

namespace PlutoWallet.Model.AjunaExt
{


    public sealed class BalancesStorage
    {

        // Substrate client for the storage calls.
        private SubstrateClientExt _client;

        public BalancesStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Balances", "TotalIssuance"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U128)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Balances", "Account"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(AccountId32), typeof(AccountData)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Balances", "Locks"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(AccountId32), typeof(WeakBoundedVecT2)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Balances", "Reserves"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(AccountId32), typeof(BoundedVecT4)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("Balances", "StorageVersion"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(EnumReleases)));
        }

        /// <summary>
        /// >> TotalIssuanceParams
        ///  The total units issued in the system.
        /// </summary>
        public static string TotalIssuanceParams()
        {
            return RequestGenerator.GetStorage("Balances", "TotalIssuance", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
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
        ///  The total units issued in the system.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U128> TotalIssuance(CancellationToken token)
        {
            string parameters = BalancesStorage.TotalIssuanceParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U128>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> AccountParams
        ///  The Balances pallet example of storing the balance of an account.
        /// 
        ///  # Example
        /// 
        ///  ```nocompile
        ///   impl pallet_balances::Config for Runtime {
        ///     type AccountStore = StorageMapShim<Self::Account<Runtime>, frame_system::Provider<Runtime>, AccountId, Self::AccountData<Balance>>
        ///   }
        ///  ```
        /// 
        ///  You can also store the balance of an account in the `System` pallet.
        /// 
        ///  # Example
        /// 
        ///  ```nocompile
        ///   impl pallet_balances::Config for Runtime {
        ///    type AccountStore = System
        ///   }
        ///  ```
        /// 
        ///  But this comes with tradeoffs, storing account balances in the system pallet stores
        ///  `frame_system` data alongside the account data contrary to storing account balances in the
        ///  `Balances` pallet, which uses a `StorageMap` to store balances data only.
        ///  NOTE: This is only used in the case that this pallet is used to store balances.
        /// </summary>
        public static string AccountParams(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 key)
        {
            return RequestGenerator.GetStorage("Balances", "Account", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }

        /// <summary>
        /// >> AccountDefault
        /// Default value as hex string
        /// </summary>
        public static string AccountDefault()
        {
            return "0x0000000000000000000000000000000000000000000000000000000000000000000000000000000" +
                "0000000000000000000000000000000000000000000000000";
        }

        /// <summary>
        /// >> Account
        ///  The Balances pallet example of storing the balance of an account.
        /// 
        ///  # Example
        /// 
        ///  ```nocompile
        ///   impl pallet_balances::Config for Runtime {
        ///     type AccountStore = StorageMapShim<Self::Account<Runtime>, frame_system::Provider<Runtime>, AccountId, Self::AccountData<Balance>>
        ///   }
        ///  ```
        /// 
        ///  You can also store the balance of an account in the `System` pallet.
        /// 
        ///  # Example
        /// 
        ///  ```nocompile
        ///   impl pallet_balances::Config for Runtime {
        ///    type AccountStore = System
        ///   }
        ///  ```
        /// 
        ///  But this comes with tradeoffs, storing account balances in the system pallet stores
        ///  `frame_system` data alongside the account data contrary to storing account balances in the
        ///  `Balances` pallet, which uses a `StorageMap` to store balances data only.
        ///  NOTE: This is only used in the case that this pallet is used to store balances.
        /// </summary>
        public async Task<AccountData> Account(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 key, CancellationToken token)
        {
            string parameters = BalancesStorage.AccountParams(key);
            var result = await _client.GetStorageAsync<AccountData>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> LocksParams
        ///  Any liquidity locks on some account balances.
        ///  NOTE: Should only be accessed when setting, changing and freeing a lock.
        /// </summary>
        public static string LocksParams(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 key)
        {
            return RequestGenerator.GetStorage("Balances", "Locks", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
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
        ///  Any liquidity locks on some account balances.
        ///  NOTE: Should only be accessed when setting, changing and freeing a lock.
        /// </summary>
        public async Task<Substrate.NetApi.Generated.Model.sp_core.bounded.weak_bounded_vec.WeakBoundedVecT2> Locks(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 key, CancellationToken token)
        {
            string parameters = BalancesStorage.LocksParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Generated.Model.sp_core.bounded.weak_bounded_vec.WeakBoundedVecT2>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> ReservesParams
        ///  Named reserves on some account balances.
        /// </summary>
        public static string ReservesParams(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 key)
        {
            return RequestGenerator.GetStorage("Balances", "Reserves", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
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
        public async Task<Substrate.NetApi.Generated.Model.sp_core.bounded.bounded_vec.BoundedVecT4> Reserves(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 key, CancellationToken token)
        {
            string parameters = BalancesStorage.ReservesParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Generated.Model.sp_core.bounded.bounded_vec.BoundedVecT4>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> StorageVersionParams
        ///  Storage version of the pallet.
        /// 
        ///  This is set to v2.0.0 for new networks.
        /// </summary>
        public static string StorageVersionParams()
        {
            return RequestGenerator.GetStorage("Balances", "StorageVersion", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }

        /// <summary>
        /// >> StorageVersionDefault
        /// Default value as hex string
        /// </summary>
        public static string StorageVersionDefault()
        {
            return "0x00";
        }

        /// <summary>
        /// >> StorageVersion
        ///  Storage version of the pallet.
        /// 
        ///  This is set to v2.0.0 for new networks.
        /// </summary>
        public async Task<EnumReleases> StorageVersion(CancellationToken token)
        {
            string parameters = BalancesStorage.StorageVersionParams();
            var result = await _client.GetStorageAsync<EnumReleases>(parameters, token);
            return result;
        }
    }

    public sealed class BalancesCalls
    {

        /// <summary>
        /// >> transfer
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method Transfer(EnumMultiAddress dest, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U128> value)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(dest.Encode());
            byteArray.AddRange(value.Encode());
            return new Method(5, "Balances", 0, "transfer", byteArray.ToArray());
        }

        /// <summary>
        /// >> set_balance
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method SetBalance(EnumMultiAddress who, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U128> new_free, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U128> new_reserved)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(who.Encode());
            byteArray.AddRange(new_free.Encode());
            byteArray.AddRange(new_reserved.Encode());
            return new Method(5, "Balances", 1, "set_balance", byteArray.ToArray());
        }

        /// <summary>
        /// >> force_transfer
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method ForceTransfer(EnumMultiAddress source, EnumMultiAddress dest, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U128> value)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(source.Encode());
            byteArray.AddRange(dest.Encode());
            byteArray.AddRange(value.Encode());
            return new Method(5, "Balances", 2, "force_transfer", byteArray.ToArray());
        }

        /// <summary>
        /// >> transfer_keep_alive
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method TransferKeepAlive(EnumMultiAddress dest, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U128> value)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(dest.Encode());
            byteArray.AddRange(value.Encode());
            return new Method(5, "Balances", 3, "transfer_keep_alive", byteArray.ToArray());
        }

        /// <summary>
        /// >> transfer_all
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method TransferAll(EnumMultiAddress dest, Substrate.NetApi.Model.Types.Primitive.Bool keep_alive)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(dest.Encode());
            byteArray.AddRange(keep_alive.Encode());
            return new Method(5, "Balances", 4, "transfer_all", byteArray.ToArray());
        }

        /// <summary>
        /// >> force_unreserve
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method ForceUnreserve(EnumMultiAddress who, Substrate.NetApi.Model.Types.Primitive.U128 amount)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(who.Encode());
            byteArray.AddRange(amount.Encode());
            return new Method(5, "Balances", 5, "force_unreserve", byteArray.ToArray());
        }
    }

    public sealed class BalancesConstants
    {

        /// <summary>
        /// >> ExistentialDeposit
        ///  The minimum amount required to keep an account open.
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 ExistentialDeposit()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U128();
            result.Create("0xF4010000000000000000000000000000");
            return result;
        }

        /// <summary>
        /// >> MaxLocks
        ///  The maximum number of locks that should exist on an account.
        ///  Not strictly enforced, but used for weight estimation.
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
            result.Create("0x00000000");
            return result;
        }
    }

    public enum BalancesErrors
    {

        /// <summary>
        /// >> VestingBalance
        /// Vesting balance too high to send value
        /// </summary>
        VestingBalance,

        /// <summary>
        /// >> LiquidityRestrictions
        /// Account liquidity restrictions prevent withdrawal
        /// </summary>
        LiquidityRestrictions,

        /// <summary>
        /// >> InsufficientBalance
        /// Balance too low to send value
        /// </summary>
        InsufficientBalance,

        /// <summary>
        /// >> ExistentialDeposit
        /// Value too low to create account due to existential deposit
        /// </summary>
        ExistentialDeposit,

        /// <summary>
        /// >> KeepAlive
        /// Transfer/payment would kill account
        /// </summary>
        KeepAlive,

        /// <summary>
        /// >> ExistingVestingSchedule
        /// A vesting schedule already exists for this account
        /// </summary>
        ExistingVestingSchedule,

        /// <summary>
        /// >> DeadAccount
        /// Beneficiary account must pre-exist
        /// </summary>
        DeadAccount,

        /// <summary>
        /// >> TooManyReserves
        /// Number of named reserves exceed MaxReserves
        /// </summary>
        TooManyReserves,
    }
}


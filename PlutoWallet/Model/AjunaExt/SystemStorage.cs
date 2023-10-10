using System;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Generated.Model.sp_core.crypto;

namespace PlutoWallet.Model.AjunaExt
{
    public sealed class SystemStorage
    {

        // Substrate client for the storage calls.
        private SubstrateClientExt _client;

        public SystemStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "Account"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32), typeof(Substrate.NetApi.Generated.Model.frame_system.AccountInfo)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "ExtrinsicCount"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "BlockWeight"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Generated.Model.frame_support.dispatch.PerDispatchClassT1)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "AllExtrinsicsLen"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "BlockHash"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApi.Generated.Model.primitive_types.H256)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "ExtrinsicData"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, typeof(Substrate.NetApi.Model.Types.Primitive.U32), typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "Number"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "ParentHash"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Generated.Model.primitive_types.H256)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "Digest"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Generated.Model.sp_runtime.generic.digest.Digest)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "Events"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Generated.Model.frame_system.EventRecord>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "EventCount"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.U32)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "EventTopics"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.NetApi.Generated.Model.primitive_types.H256), typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "LastRuntimeUpgrade"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Generated.Model.frame_system.LastRuntimeUpgradeInfo)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "UpgradedToU32RefCount"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.Bool)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "UpgradedToTripleRefCount"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Primitive.Bool)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("System", "ExecutionPhase"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Generated.Model.frame_system.EnumPhase)));
        }

        /// <summary>
        /// >> AccountParams
        ///  The full account information for a particular account ID.
        /// </summary>
        public static string AccountParams(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 key)
        {
            return RequestGenerator.GetStorage("System", "Account", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
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
                "00000000000000000000000000000000000000000000000000000000000000000000000000000000" +
                "0";
        }

        /// <summary>
        /// >> Account
        ///  The full account information for a particular account ID.
        /// - This method was modified to be more userfriendly
        /// </summary>
        public async Task<Substrate.NetApi.Generated.Model.frame_system.AccountInfo> Account(string substrateAddress)
        {
            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(substrateAddress));
        
            string parameters = SystemStorage.AccountParams(account32);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Generated.Model.frame_system.AccountInfo>(parameters, CancellationToken.None);
            return result;
        }

        /// <summary>
        /// >> Account
        ///  The full account information for a particular account ID.
        /// </summary>
        public async Task<Substrate.NetApi.Generated.Model.frame_system.AccountInfo> Account(Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32 key, CancellationToken token)
        {
            string parameters = SystemStorage.AccountParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Generated.Model.frame_system.AccountInfo>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> ExtrinsicCountParams
        ///  Total extrinsics count for the current block.
        /// </summary>
        public static string ExtrinsicCountParams()
        {
            return RequestGenerator.GetStorage("System", "ExtrinsicCount", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }

        /// <summary>
        /// >> ExtrinsicCountDefault
        /// Default value as hex string
        /// </summary>
        public static string ExtrinsicCountDefault()
        {
            return "0x00";
        }

        /// <summary>
        /// >> ExtrinsicCount
        ///  Total extrinsics count for the current block.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> ExtrinsicCount(CancellationToken token)
        {
            string parameters = SystemStorage.ExtrinsicCountParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> BlockWeightParams
        ///  The current weight for the block.
        /// </summary>
        public static string BlockWeightParams()
        {
            return RequestGenerator.GetStorage("System", "BlockWeight", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }

        /// <summary>
        /// >> BlockWeightDefault
        /// Default value as hex string
        /// </summary>
        public static string BlockWeightDefault()
        {
            return "0x000000000000";
        }

        /// <summary>
        /// >> BlockWeight
        ///  The current weight for the block.
        /// </summary>
        public async Task<Substrate.NetApi.Generated.Model.frame_support.dispatch.PerDispatchClassT1> BlockWeight(CancellationToken token)
        {
            string parameters = SystemStorage.BlockWeightParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Generated.Model.frame_support.dispatch.PerDispatchClassT1>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> AllExtrinsicsLenParams
        ///  Total length (in bytes) for all extrinsics put together, for the current block.
        /// </summary>
        public static string AllExtrinsicsLenParams()
        {
            return RequestGenerator.GetStorage("System", "AllExtrinsicsLen", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }

        /// <summary>
        /// >> AllExtrinsicsLenDefault
        /// Default value as hex string
        /// </summary>
        public static string AllExtrinsicsLenDefault()
        {
            return "0x00";
        }

        /// <summary>
        /// >> AllExtrinsicsLen
        ///  Total length (in bytes) for all extrinsics put together, for the current block.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> AllExtrinsicsLen(CancellationToken token)
        {
            string parameters = SystemStorage.AllExtrinsicsLenParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> BlockHashParams
        ///  Map of block numbers to block hashes.
        /// </summary>
        public static string BlockHashParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("System", "BlockHash", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }

        /// <summary>
        /// >> BlockHashDefault
        /// Default value as hex string
        /// </summary>
        public static string BlockHashDefault()
        {
            return "0x0000000000000000000000000000000000000000000000000000000000000000";
        }

        /// <summary>
        /// >> BlockHash
        ///  Map of block numbers to block hashes.
        /// </summary>
        public async Task<Substrate.NetApi.Generated.Model.primitive_types.H256> BlockHash(Substrate.NetApi.Model.Types.Primitive.U32 key, CancellationToken token)
        {
            string parameters = SystemStorage.BlockHashParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Generated.Model.primitive_types.H256>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> ExtrinsicDataParams
        ///  Extrinsics data for the current block (maps an extrinsic's index to its data).
        /// </summary>
        public static string ExtrinsicDataParams(Substrate.NetApi.Model.Types.Primitive.U32 key)
        {
            return RequestGenerator.GetStorage("System", "ExtrinsicData", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.Twox64Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }

        /// <summary>
        /// >> ExtrinsicDataDefault
        /// Default value as hex string
        /// </summary>
        public static string ExtrinsicDataDefault()
        {
            return "0x00";
        }

        /// <summary>
        /// >> ExtrinsicData
        ///  Extrinsics data for the current block (maps an extrinsic's index to its data).
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>> ExtrinsicData(Substrate.NetApi.Model.Types.Primitive.U32 key, CancellationToken token)
        {
            string parameters = SystemStorage.ExtrinsicDataParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> NumberParams
        ///  The current block number being processed. Set by `execute_block`.
        /// </summary>
        public static string NumberParams()
        {
            return RequestGenerator.GetStorage("System", "Number", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }

        /// <summary>
        /// >> NumberDefault
        /// Default value as hex string
        /// </summary>
        public static string NumberDefault()
        {
            return "0x00000000";
        }

        /// <summary>
        /// >> Number
        ///  The current block number being processed. Set by `execute_block`.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> Number(CancellationToken token)
        {
            string parameters = SystemStorage.NumberParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> ParentHashParams
        ///  Hash of the previous block.
        /// </summary>
        public static string ParentHashParams()
        {
            return RequestGenerator.GetStorage("System", "ParentHash", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }

        /// <summary>
        /// >> ParentHashDefault
        /// Default value as hex string
        /// </summary>
        public static string ParentHashDefault()
        {
            return "0x0000000000000000000000000000000000000000000000000000000000000000";
        }

        /// <summary>
        /// >> ParentHash
        ///  Hash of the previous block.
        /// </summary>
        public async Task<Substrate.NetApi.Generated.Model.primitive_types.H256> ParentHash(CancellationToken token)
        {
            string parameters = SystemStorage.ParentHashParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Generated.Model.primitive_types.H256>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> DigestParams
        ///  Digest of the current block, also part of the block header.
        /// </summary>
        public static string DigestParams()
        {
            return RequestGenerator.GetStorage("System", "Digest", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }

        /// <summary>
        /// >> DigestDefault
        /// Default value as hex string
        /// </summary>
        public static string DigestDefault()
        {
            return "0x00";
        }

        /// <summary>
        /// >> Digest
        ///  Digest of the current block, also part of the block header.
        /// </summary>
        public async Task<Substrate.NetApi.Generated.Model.sp_runtime.generic.digest.Digest> Digest(CancellationToken token)
        {
            string parameters = SystemStorage.DigestParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Generated.Model.sp_runtime.generic.digest.Digest>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> EventsParams
        ///  Events deposited for the current block.
        /// 
        ///  NOTE: The item is unbound and should therefore never be read on chain.
        ///  It could otherwise inflate the PoV size of a block.
        /// 
        ///  Events have a large in-memory size. Box the events to not go out-of-memory
        ///  just in case someone still reads them from within the runtime.
        /// </summary>
        public static string EventsParams()
        {
            return RequestGenerator.GetStorage("System", "Events", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }

        /// <summary>
        /// >> EventsDefault
        /// Default value as hex string
        /// </summary>
        public static string EventsDefault()
        {
            return "0x00";
        }

        /// <summary>
        /// >> Events
        ///  Events deposited for the current block.
        /// 
        ///  NOTE: The item is unbound and should therefore never be read on chain.
        ///  It could otherwise inflate the PoV size of a block.
        /// 
        ///  Events have a large in-memory size. Box the events to not go out-of-memory
        ///  just in case someone still reads them from within the runtime.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Generated.Model.frame_system.EventRecord>> Events(CancellationToken token)
        {
            string parameters = SystemStorage.EventsParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Generated.Model.frame_system.EventRecord>>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> EventCountParams
        ///  The number of events in the `Events<T>` list.
        /// </summary>
        public static string EventCountParams()
        {
            return RequestGenerator.GetStorage("System", "EventCount", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }

        /// <summary>
        /// >> EventCountDefault
        /// Default value as hex string
        /// </summary>
        public static string EventCountDefault()
        {
            return "0x00000000";
        }

        /// <summary>
        /// >> EventCount
        ///  The number of events in the `Events<T>` list.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.U32> EventCount(CancellationToken token)
        {
            string parameters = SystemStorage.EventCountParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.U32>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> EventTopicsParams
        ///  Mapping between a topic (represented by T::Hash) and a vector of indexes
        ///  of events in the `<Events<T>>` list.
        /// 
        ///  All topic vectors have deterministic storage locations depending on the topic. This
        ///  allows light-clients to leverage the changes trie storage tracking mechanism and
        ///  in case of changes fetch the list of events of interest.
        /// 
        ///  The value has the type `(T::BlockNumber, EventIndex)` because if we used only just
        ///  the `EventIndex` then in case if the topic has the same contents on the next block
        ///  no notification will be triggered thus the event might be lost.
        /// </summary>
        public static string EventTopicsParams(Substrate.NetApi.Generated.Model.primitive_types.H256 key)
        {
            return RequestGenerator.GetStorage("System", "EventTopics", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }

        /// <summary>
        /// >> EventTopicsDefault
        /// Default value as hex string
        /// </summary>
        public static string EventTopicsDefault()
        {
            return "0x00";
        }

        /// <summary>
        /// >> EventTopics
        ///  Mapping between a topic (represented by T::Hash) and a vector of indexes
        ///  of events in the `<Events<T>>` list.
        /// 
        ///  All topic vectors have deterministic storage locations depending on the topic. This
        ///  allows light-clients to leverage the changes trie storage tracking mechanism and
        ///  in case of changes fetch the list of events of interest.
        /// 
        ///  The value has the type `(T::BlockNumber, EventIndex)` because if we used only just
        ///  the `EventIndex` then in case if the topic has the same contents on the next block
        ///  no notification will be triggered thus the event might be lost.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>> EventTopics(Substrate.NetApi.Generated.Model.primitive_types.H256 key, CancellationToken token)
        {
            string parameters = SystemStorage.EventTopicsParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> LastRuntimeUpgradeParams
        ///  Stores the `spec_version` and `spec_name` of when the last runtime upgrade happened.
        /// </summary>
        public static string LastRuntimeUpgradeParams()
        {
            return RequestGenerator.GetStorage("System", "LastRuntimeUpgrade", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }

        /// <summary>
        /// >> LastRuntimeUpgradeDefault
        /// Default value as hex string
        /// </summary>
        public static string LastRuntimeUpgradeDefault()
        {
            return "0x00";
        }

        /// <summary>
        /// >> LastRuntimeUpgrade
        ///  Stores the `spec_version` and `spec_name` of when the last runtime upgrade happened.
        /// </summary>
        public async Task<Substrate.NetApi.Generated.Model.frame_system.LastRuntimeUpgradeInfo> LastRuntimeUpgrade(CancellationToken token)
        {
            string parameters = SystemStorage.LastRuntimeUpgradeParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Generated.Model.frame_system.LastRuntimeUpgradeInfo>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> UpgradedToU32RefCountParams
        ///  True if we have upgraded so that `type RefCount` is `u32`. False (default) if not.
        /// </summary>
        public static string UpgradedToU32RefCountParams()
        {
            return RequestGenerator.GetStorage("System", "UpgradedToU32RefCount", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }

        /// <summary>
        /// >> UpgradedToU32RefCountDefault
        /// Default value as hex string
        /// </summary>
        public static string UpgradedToU32RefCountDefault()
        {
            return "0x00";
        }

        /// <summary>
        /// >> UpgradedToU32RefCount
        ///  True if we have upgraded so that `type RefCount` is `u32`. False (default) if not.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.Bool> UpgradedToU32RefCount(CancellationToken token)
        {
            string parameters = SystemStorage.UpgradedToU32RefCountParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.Bool>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> UpgradedToTripleRefCountParams
        ///  True if we have upgraded so that AccountInfo contains three types of `RefCount`. False
        ///  (default) if not.
        /// </summary>
        public static string UpgradedToTripleRefCountParams()
        {
            return RequestGenerator.GetStorage("System", "UpgradedToTripleRefCount", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }

        /// <summary>
        /// >> UpgradedToTripleRefCountDefault
        /// Default value as hex string
        /// </summary>
        public static string UpgradedToTripleRefCountDefault()
        {
            return "0x00";
        }

        /// <summary>
        /// >> UpgradedToTripleRefCount
        ///  True if we have upgraded so that AccountInfo contains three types of `RefCount`. False
        ///  (default) if not.
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Primitive.Bool> UpgradedToTripleRefCount(CancellationToken token)
        {
            string parameters = SystemStorage.UpgradedToTripleRefCountParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Primitive.Bool>(parameters, token);
            return result;
        }

        /// <summary>
        /// >> ExecutionPhaseParams
        ///  The execution phase of the block.
        /// </summary>
        public static string ExecutionPhaseParams()
        {
            return RequestGenerator.GetStorage("System", "ExecutionPhase", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }

        /// <summary>
        /// >> ExecutionPhaseDefault
        /// Default value as hex string
        /// </summary>
        public static string ExecutionPhaseDefault()
        {
            return "0x00";
        }

        /// <summary>
        /// >> ExecutionPhase
        ///  The execution phase of the block.
        /// </summary>
        public async Task<Substrate.NetApi.Generated.Model.frame_system.EnumPhase> ExecutionPhase(CancellationToken token)
        {
            string parameters = SystemStorage.ExecutionPhaseParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Generated.Model.frame_system.EnumPhase>(parameters, token);
            return result;
        }
    }

    public sealed class SystemCalls
    {

        /// <summary>
        /// >> fill_block
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method FillBlock(Substrate.NetApi.Generated.Model.sp_arithmetic.per_things.Perbill ratio)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(ratio.Encode());
            return new Method(0, "System", 0, "fill_block", byteArray.ToArray());
        }

        /// <summary>
        /// >> remark
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method Remark(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> remark)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(remark.Encode());
            return new Method(0, "System", 1, "remark", byteArray.ToArray());
        }

        /// <summary>
        /// >> set_heap_pages
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method SetHeapPages(Substrate.NetApi.Model.Types.Primitive.U64 pages)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(pages.Encode());
            return new Method(0, "System", 2, "set_heap_pages", byteArray.ToArray());
        }

        /// <summary>
        /// >> set_code
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method SetCode(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> code)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(code.Encode());
            return new Method(0, "System", 3, "set_code", byteArray.ToArray());
        }

        /// <summary>
        /// >> set_code_without_checks
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method SetCodeWithoutChecks(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> code)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(code.Encode());
            return new Method(0, "System", 4, "set_code_without_checks", byteArray.ToArray());
        }

        /// <summary>
        /// >> set_storage
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method SetStorage(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>>> items)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(items.Encode());
            return new Method(0, "System", 5, "set_storage", byteArray.ToArray());
        }

        /// <summary>
        /// >> kill_storage
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method KillStorage(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>> keys)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(keys.Encode());
            return new Method(0, "System", 6, "kill_storage", byteArray.ToArray());
        }

        /// <summary>
        /// >> kill_prefix
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method KillPrefix(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> prefix, Substrate.NetApi.Model.Types.Primitive.U32 subkeys)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(prefix.Encode());
            byteArray.AddRange(subkeys.Encode());
            return new Method(0, "System", 7, "kill_prefix", byteArray.ToArray());
        }

        /// <summary>
        /// >> remark_with_event
        /// Contains one variant per dispatchable that can be called by an extrinsic.
        /// </summary>
        public static Method RemarkWithEvent(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> remark)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(remark.Encode());
            return new Method(0, "System", 8, "remark_with_event", byteArray.ToArray());
        }
    }

    public sealed class SystemConstants
    {

        /// <summary>
        /// >> BlockWeights
        ///  Block & extrinsics weights: base values and limits.
        /// </summary>
        public Substrate.NetApi.Generated.Model.frame_system.limits.BlockWeights BlockWeights()
        {
            var result = new Substrate.NetApi.Generated.Model.frame_system.limits.BlockWeights();
            result.Create(@"0x07E0D1A93E01000B00204AA9D10113FFFFFFFFFFFFFFFF4236931400010B70FAE4A82E011366666666666666A6010B0098F73E5D0113FFFFFFFFFFFFFFBF0100004236931400010B70823713A3011366666666666666E6010B00204AA9D10113FFFFFFFFFFFFFFFF01070088526A741300000000000000404236931400000000");
            return result;
        }

        /// <summary>
        /// >> BlockLength
        ///  The maximum length of a block (in bytes).
        /// </summary>
        public Substrate.NetApi.Generated.Model.frame_system.limits.BlockLength BlockLength()
        {
            var result = new Substrate.NetApi.Generated.Model.frame_system.limits.BlockLength();
            result.Create("0x00003C000000500000005000");
            return result;
        }

        /// <summary>
        /// >> BlockHashCount
        ///  Maximum number of block number to block hash mappings to keep (oldest pruned first).
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 BlockHashCount()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U32();
            result.Create("0x60090000");
            return result;
        }

        /// <summary>
        /// >> DbWeight
        ///  The weight of runtime database operations the runtime can invoke.
        /// </summary>
        public Substrate.NetApi.Generated.Model.sp_weights.RuntimeDbWeight DbWeight()
        {
            var result = new Substrate.NetApi.Generated.Model.sp_weights.RuntimeDbWeight();
            result.Create("0x40787D010000000000E1F50500000000");
            return result;
        }

        /// <summary>
        /// >> Version
        ///  Get the chain's current version.
        /// </summary>
        public Substrate.NetApi.Generated.Model.sp_version.RuntimeVersion Version()
        {
            var result = new Substrate.NetApi.Generated.Model.sp_version.RuntimeVersion();
            result.Create(@"0x346E6F64652D74656D706C617465346E6F64652D74656D706C6174650100000064000000010000002CDF6ACB689907609B0400000037E397FC7C91F5E40100000040FE3AD401F8959A06000000D2BC9897EED08F1503000000F78B278BE53F454C02000000DD718D5CC53262D401000000AB3C0572291FEB8B01000000ED99C5ACB25EEDF503000000BC9D89904F5B923F0100000037C8BB1350A9A2A801000000F3FF14D5AB527059010000000100000001");
            return result;
        }

        /// <summary>
        /// >> SS58Prefix
        ///  The designated SS58 prefix of this chain.
        /// 
        ///  This replaces the "ss58Format" property declared in the chain spec. Reason is
        ///  that the runtime should know about the prefix in order to make use of it as
        ///  an identifier of the chain.
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U16 SS58Prefix()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U16();
            result.Create("0x2A00");
            return result;
        }
    }

    public enum SystemErrors
    {

        /// <summary>
        /// >> InvalidSpecName
        /// The name of specification does not match between the current runtime
        /// and the new runtime.
        /// </summary>
        InvalidSpecName,

        /// <summary>
        /// >> SpecVersionNeedsToIncrease
        /// The specification version is not allowed to decrease between the current runtime
        /// and the new runtime.
        /// </summary>
        SpecVersionNeedsToIncrease,

        /// <summary>
        /// >> FailedToExtractRuntimeVersion
        /// Failed to extract the runtime version from the new runtime.
        /// 
        /// Either calling `Core_version` or decoding `RuntimeVersion` failed.
        /// </summary>
        FailedToExtractRuntimeVersion,

        /// <summary>
        /// >> NonDefaultComposite
        /// Suicide called when the account has non-default composite data.
        /// </summary>
        NonDefaultComposite,

        /// <summary>
        /// >> NonZeroRefCount
        /// There is a non-zero reference count preventing the account from being purged.
        /// </summary>
        NonZeroRefCount,

        /// <summary>
        /// >> CallFiltered
        /// The origin filter prevent the call to be dispatched.
        /// </summary>
        CallFiltered,
    }
}


//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Ajuna.NetApi.Attributes;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.NetApi.Model.Types.Metadata.V14;
using System.Collections.Generic;


namespace PlutoWallet.NetApiExt.Generated.Model.polkadot_runtime_parachains.scheduler
{
    
    
    /// <summary>
    /// >> 654 - Composite[polkadot_runtime_parachains.scheduler.ParathreadClaimQueue]
    /// </summary>
    [AjunaNodeType(TypeDefEnum.Composite)]
    public sealed class ParathreadClaimQueue : BaseType
    {
        
        /// <summary>
        /// >> queue
        /// </summary>
        private Ajuna.NetApi.Model.Types.Base.BaseVec<PlutoWallet.NetApiExt.Generated.Model.polkadot_runtime_parachains.scheduler.QueuedParathread> _queue;
        
        /// <summary>
        /// >> next_core_offset
        /// </summary>
        private Ajuna.NetApi.Model.Types.Primitive.U32 _nextCoreOffset;
        
        public Ajuna.NetApi.Model.Types.Base.BaseVec<PlutoWallet.NetApiExt.Generated.Model.polkadot_runtime_parachains.scheduler.QueuedParathread> Queue
        {
            get
            {
                return this._queue;
            }
            set
            {
                this._queue = value;
            }
        }
        
        public Ajuna.NetApi.Model.Types.Primitive.U32 NextCoreOffset
        {
            get
            {
                return this._nextCoreOffset;
            }
            set
            {
                this._nextCoreOffset = value;
            }
        }
        
        public override string TypeName()
        {
            return "ParathreadClaimQueue";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Queue.Encode());
            result.AddRange(NextCoreOffset.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Queue = new Ajuna.NetApi.Model.Types.Base.BaseVec<PlutoWallet.NetApiExt.Generated.Model.polkadot_runtime_parachains.scheduler.QueuedParathread>();
            Queue.Decode(byteArray, ref p);
            NextCoreOffset = new Ajuna.NetApi.Model.Types.Primitive.U32();
            NextCoreOffset.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}

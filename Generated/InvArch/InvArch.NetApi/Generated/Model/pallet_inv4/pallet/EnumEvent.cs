//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Model.Types.Base;
using System.Collections.Generic;


namespace InvArch.NetApi.Generated.Model.pallet_inv4.pallet
{
    
    
    /// <summary>
    /// >> Event
    /// 
    ///			The [event](https://docs.substrate.io/main-docs/build/events-errors/) emitted
    ///			by this pallet.
    ///			
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> CoreCreated
        /// A core was created
        /// </summary>
        CoreCreated = 0,
        
        /// <summary>
        /// >> ParametersSet
        /// A core had parameters changed
        /// </summary>
        ParametersSet = 1,
        
        /// <summary>
        /// >> Minted
        /// A core's voting token was minted
        /// </summary>
        Minted = 2,
        
        /// <summary>
        /// >> Burned
        /// A core's voting token was burned
        /// </summary>
        Burned = 3,
        
        /// <summary>
        /// >> MultisigVoteStarted
        /// A multisig proposal has started, it needs more votes to pass
        /// </summary>
        MultisigVoteStarted = 4,
        
        /// <summary>
        /// >> MultisigVoteAdded
        /// A vote was added to an existing multisig proposal
        /// </summary>
        MultisigVoteAdded = 5,
        
        /// <summary>
        /// >> MultisigVoteWithdrawn
        /// A vote was removed from an existing multisig proposal
        /// </summary>
        MultisigVoteWithdrawn = 6,
        
        /// <summary>
        /// >> MultisigExecuted
        /// A multisig proposal passed and it's call was executed
        /// </summary>
        MultisigExecuted = 7,
        
        /// <summary>
        /// >> MultisigCanceled
        /// A multisig proposal was cancelled
        /// </summary>
        MultisigCanceled = 8,
    }
    
    /// <summary>
    /// >> 115 - Variant[pallet_inv4.pallet.Event]
    /// 
    ///			The [event](https://docs.substrate.io/main-docs/build/events-errors/) emitted
    ///			by this pallet.
    ///			
    /// </summary>
    public sealed class EnumEvent : BaseEnumRust<Event>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumEvent()
        {
				AddTypeDecoder<BaseTuple<InvArch.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>, InvArch.NetApi.Generated.Model.sp_arithmetic.per_things.Perbill, InvArch.NetApi.Generated.Model.sp_arithmetic.per_things.Perbill>>(Event.CoreCreated);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>>, Substrate.NetApi.Model.Types.Base.BaseOpt<InvArch.NetApi.Generated.Model.sp_arithmetic.per_things.Perbill>, Substrate.NetApi.Model.Types.Base.BaseOpt<InvArch.NetApi.Generated.Model.sp_arithmetic.per_things.Perbill>, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.Bool>>>(Event.ParametersSet);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, InvArch.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U128>>(Event.Minted);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, InvArch.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U128>>(Event.Burned);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, InvArch.NetApi.Generated.Model.sp_core.crypto.AccountId32, InvArch.NetApi.Generated.Model.sp_core.crypto.AccountId32, InvArch.NetApi.Generated.Model.pallet_inv4.voting.EnumVote, InvArch.NetApi.Generated.Model.primitive_types.H256>>(Event.MultisigVoteStarted);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, InvArch.NetApi.Generated.Model.sp_core.crypto.AccountId32, InvArch.NetApi.Generated.Model.sp_core.crypto.AccountId32, InvArch.NetApi.Generated.Model.pallet_inv4.voting.EnumVote, InvArch.NetApi.Generated.Model.pallet_inv4.voting.Tally, InvArch.NetApi.Generated.Model.primitive_types.H256>>(Event.MultisigVoteAdded);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, InvArch.NetApi.Generated.Model.sp_core.crypto.AccountId32, InvArch.NetApi.Generated.Model.sp_core.crypto.AccountId32, InvArch.NetApi.Generated.Model.pallet_inv4.voting.EnumVote, InvArch.NetApi.Generated.Model.primitive_types.H256>>(Event.MultisigVoteWithdrawn);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, InvArch.NetApi.Generated.Model.sp_core.crypto.AccountId32, InvArch.NetApi.Generated.Model.sp_core.crypto.AccountId32, InvArch.NetApi.Generated.Model.primitive_types.H256, InvArch.NetApi.Generated.Model.invarch_runtime.EnumRuntimeCall, InvArch.NetApi.Generated.Types.Base.EnumResult>>(Event.MultisigExecuted);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, InvArch.NetApi.Generated.Model.primitive_types.H256>>(Event.MultisigCanceled);
        }
    }
}

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


namespace Bifrost.NetApi.Generated.Model.bifrost_vtoken_voting.pallet
{
    
    
    /// <summary>
    /// >> Call
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public enum Call
    {
        
        /// <summary>
        /// >> vote
        /// </summary>
        vote = 0,
        
        /// <summary>
        /// >> unlock
        /// </summary>
        unlock = 1,
        
        /// <summary>
        /// >> remove_delegator_vote
        /// </summary>
        remove_delegator_vote = 2,
        
        /// <summary>
        /// >> kill_referendum
        /// </summary>
        kill_referendum = 3,
        
        /// <summary>
        /// >> add_delegator
        /// </summary>
        add_delegator = 4,
        
        /// <summary>
        /// >> set_referendum_status
        /// </summary>
        set_referendum_status = 5,
        
        /// <summary>
        /// >> set_vote_locking_period
        /// </summary>
        set_vote_locking_period = 6,
        
        /// <summary>
        /// >> set_undeciding_timeout
        /// </summary>
        set_undeciding_timeout = 7,
        
        /// <summary>
        /// >> notify_vote
        /// </summary>
        notify_vote = 8,
        
        /// <summary>
        /// >> notify_remove_delegator_vote
        /// </summary>
        notify_remove_delegator_vote = 10,
        
        /// <summary>
        /// >> set_vote_cap_ratio
        /// </summary>
        set_vote_cap_ratio = 11,
    }
    
    /// <summary>
    /// >> 431 - Variant[bifrost_vtoken_voting.pallet.Call]
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public sealed class EnumCall : BaseEnumRust<Call>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumCall()
        {
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, Bifrost.NetApi.Generated.Model.bifrost_vtoken_voting.vote.EnumAccountVote>>(Call.vote);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>>>(Call.unlock);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U16>, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U16>>>(Call.remove_delegator_vote);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>>>(Call.kill_referendum);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U16>>>(Call.add_delegator);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, Bifrost.NetApi.Generated.Model.bifrost_vtoken_voting.vote.EnumReferendumInfo>>(Call.set_referendum_status);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Primitive.U32>>(Call.set_vote_locking_period);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Substrate.NetApi.Model.Types.Primitive.U32>>(Call.set_undeciding_timeout);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Bifrost.NetApi.Generated.Model.staging_xcm.v4.EnumResponse>>(Call.notify_vote);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U64, Bifrost.NetApi.Generated.Model.staging_xcm.v4.EnumResponse>>(Call.notify_remove_delegator_vote);
				AddTypeDecoder<BaseTuple<Bifrost.NetApi.Generated.Model.bifrost_primitives.currency.EnumCurrencyId, Bifrost.NetApi.Generated.Model.sp_arithmetic.per_things.Perbill>>(Call.set_vote_cap_ratio);
        }
    }
}

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


namespace Bifrost.NetApi.Generated.Model.merkle_distributor.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> BadDescription
        /// Invalid metadata given.
        /// </summary>
        BadDescription = 0,
        
        /// <summary>
        /// >> InvalidMerkleDistributorId
        /// The id is not exist.
        /// </summary>
        InvalidMerkleDistributorId = 1,
        
        /// <summary>
        /// >> MerkleVerifyFailed
        /// The proof is invalid
        /// </summary>
        MerkleVerifyFailed = 2,
        
        /// <summary>
        /// >> Claimed
        /// The reward is already distributed.
        /// </summary>
        Claimed = 3,
        
        /// <summary>
        /// >> Charged
        /// The reward is already charged.
        /// </summary>
        Charged = 4,
        
        /// <summary>
        /// >> WithdrawAmountExceed
        /// Withdraw amount exceed charge amount.
        /// </summary>
        WithdrawAmountExceed = 5,
        
        /// <summary>
        /// >> BadChargeAccount
        /// 
        /// </summary>
        BadChargeAccount = 6,
        
        /// <summary>
        /// >> AlreadyInWhiteList
        /// Account has already in the set who can create merkle distributor
        /// </summary>
        AlreadyInWhiteList = 7,
        
        /// <summary>
        /// >> NotInWhiteList
        /// Account is no in the set who can create merkle distributor
        /// </summary>
        NotInWhiteList = 8,
    }
    
    /// <summary>
    /// >> 823 - Variant[merkle_distributor.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}

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


namespace Bifrost.NetApi.Generated.Model.bifrost_ve_minting.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> NotEnoughBalance
        /// </summary>
        NotEnoughBalance = 0,
        
        /// <summary>
        /// >> Expired
        /// </summary>
        Expired = 1,
        
        /// <summary>
        /// >> BelowMinimumMint
        /// </summary>
        BelowMinimumMint = 2,
        
        /// <summary>
        /// >> LockNotExist
        /// </summary>
        LockNotExist = 3,
        
        /// <summary>
        /// >> LockExist
        /// </summary>
        LockExist = 4,
        
        /// <summary>
        /// >> NoRewards
        /// </summary>
        NoRewards = 5,
        
        /// <summary>
        /// >> ArgumentsError
        /// </summary>
        ArgumentsError = 6,
        
        /// <summary>
        /// >> ExceedsMaxPositions
        /// </summary>
        ExceedsMaxPositions = 7,
        
        /// <summary>
        /// >> NoController
        /// </summary>
        NoController = 8,
    }
    
    /// <summary>
    /// >> 894 - Variant[bifrost_ve_minting.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}

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


namespace Bifrost.NetApi.Generated.Model.bifrost_stable_asset.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> InconsistentStorage
        /// </summary>
        InconsistentStorage = 0,
        
        /// <summary>
        /// >> InvalidPoolAsset
        /// </summary>
        InvalidPoolAsset = 1,
        
        /// <summary>
        /// >> ArgumentsMismatch
        /// </summary>
        ArgumentsMismatch = 2,
        
        /// <summary>
        /// >> ArgumentsError
        /// </summary>
        ArgumentsError = 3,
        
        /// <summary>
        /// >> PoolNotFound
        /// </summary>
        PoolNotFound = 4,
        
        /// <summary>
        /// >> Math
        /// </summary>
        Math = 5,
        
        /// <summary>
        /// >> InvalidPoolValue
        /// </summary>
        InvalidPoolValue = 6,
        
        /// <summary>
        /// >> MintUnderMin
        /// </summary>
        MintUnderMin = 7,
        
        /// <summary>
        /// >> SwapUnderMin
        /// </summary>
        SwapUnderMin = 8,
        
        /// <summary>
        /// >> RedeemUnderMin
        /// </summary>
        RedeemUnderMin = 9,
        
        /// <summary>
        /// >> RedeemOverMax
        /// </summary>
        RedeemOverMax = 10,
        
        /// <summary>
        /// >> TokenRateNotCleared
        /// </summary>
        TokenRateNotCleared = 11,
    }
    
    /// <summary>
    /// >> 912 - Variant[bifrost_stable_asset.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}

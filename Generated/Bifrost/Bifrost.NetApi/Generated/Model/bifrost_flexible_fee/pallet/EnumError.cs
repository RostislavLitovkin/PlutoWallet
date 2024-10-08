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


namespace Bifrost.NetApi.Generated.Model.bifrost_flexible_fee.pallet
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
        /// >> Overflow
        /// </summary>
        Overflow = 1,
        
        /// <summary>
        /// >> ConversionError
        /// </summary>
        ConversionError = 2,
        
        /// <summary>
        /// >> WrongListLength
        /// </summary>
        WrongListLength = 3,
        
        /// <summary>
        /// >> WeightAndFeeNotExist
        /// </summary>
        WeightAndFeeNotExist = 4,
        
        /// <summary>
        /// >> DexFailedToGetAmountInByPath
        /// </summary>
        DexFailedToGetAmountInByPath = 5,
        
        /// <summary>
        /// >> UnweighableMessage
        /// </summary>
        UnweighableMessage = 6,
        
        /// <summary>
        /// >> XcmExecutionFailed
        /// </summary>
        XcmExecutionFailed = 7,
    }
    
    /// <summary>
    /// >> 824 - Variant[bifrost_flexible_fee.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}

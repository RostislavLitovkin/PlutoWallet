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


namespace InvArch.NetApi.Generated.Model.pallet_ocif_staking.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// 
    ///			Custom [dispatch errors](https://docs.substrate.io/main-docs/build/events-errors/)
    ///			of this pallet.
    ///			
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> StakingNothing
        /// </summary>
        StakingNothing = 0,
        
        /// <summary>
        /// >> InsufficientBalance
        /// </summary>
        InsufficientBalance = 1,
        
        /// <summary>
        /// >> MaxStakersReached
        /// </summary>
        MaxStakersReached = 2,
        
        /// <summary>
        /// >> CoreNotFound
        /// </summary>
        CoreNotFound = 3,
        
        /// <summary>
        /// >> NoStakeAvailable
        /// </summary>
        NoStakeAvailable = 4,
        
        /// <summary>
        /// >> NotUnregisteredCore
        /// </summary>
        NotUnregisteredCore = 5,
        
        /// <summary>
        /// >> UnclaimedRewardsAvailable
        /// </summary>
        UnclaimedRewardsAvailable = 6,
        
        /// <summary>
        /// >> UnstakingNothing
        /// </summary>
        UnstakingNothing = 7,
        
        /// <summary>
        /// >> NothingToWithdraw
        /// </summary>
        NothingToWithdraw = 8,
        
        /// <summary>
        /// >> CoreAlreadyRegistered
        /// </summary>
        CoreAlreadyRegistered = 9,
        
        /// <summary>
        /// >> UnknownEraReward
        /// </summary>
        UnknownEraReward = 10,
        
        /// <summary>
        /// >> UnexpectedStakeInfoEra
        /// </summary>
        UnexpectedStakeInfoEra = 11,
        
        /// <summary>
        /// >> TooManyUnlockingChunks
        /// </summary>
        TooManyUnlockingChunks = 12,
        
        /// <summary>
        /// >> RewardAlreadyClaimed
        /// </summary>
        RewardAlreadyClaimed = 13,
        
        /// <summary>
        /// >> IncorrectEra
        /// </summary>
        IncorrectEra = 14,
        
        /// <summary>
        /// >> TooManyEraStakeValues
        /// </summary>
        TooManyEraStakeValues = 15,
        
        /// <summary>
        /// >> NotAStaker
        /// </summary>
        NotAStaker = 16,
        
        /// <summary>
        /// >> NoPermission
        /// </summary>
        NoPermission = 17,
        
        /// <summary>
        /// >> MaxNameExceeded
        /// </summary>
        MaxNameExceeded = 18,
        
        /// <summary>
        /// >> MaxDescriptionExceeded
        /// </summary>
        MaxDescriptionExceeded = 19,
        
        /// <summary>
        /// >> MaxImageExceeded
        /// </summary>
        MaxImageExceeded = 20,
        
        /// <summary>
        /// >> NotRegistered
        /// </summary>
        NotRegistered = 21,
        
        /// <summary>
        /// >> Halted
        /// </summary>
        Halted = 22,
        
        /// <summary>
        /// >> NoHaltChange
        /// </summary>
        NoHaltChange = 23,
        
        /// <summary>
        /// >> MoveStakeToSameCore
        /// </summary>
        MoveStakeToSameCore = 24,
    }
    
    /// <summary>
    /// >> 369 - Variant[pallet_ocif_staking.pallet.Error]
    /// 
    ///			Custom [dispatch errors](https://docs.substrate.io/main-docs/build/events-errors/)
    ///			of this pallet.
    ///			
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}

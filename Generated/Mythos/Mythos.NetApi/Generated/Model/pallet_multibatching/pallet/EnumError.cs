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


namespace Mythos.NetApi.Generated.Model.pallet_multibatching.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> AlreadyApplied
        /// </summary>
        AlreadyApplied = 0,
        
        /// <summary>
        /// >> BatchSenderIsNotOrigin
        /// </summary>
        BatchSenderIsNotOrigin = 1,
        
        /// <summary>
        /// >> NoCalls
        /// </summary>
        NoCalls = 2,
        
        /// <summary>
        /// >> NoApprovals
        /// </summary>
        NoApprovals = 3,
        
        /// <summary>
        /// >> InvalidDomain
        /// </summary>
        InvalidDomain = 4,
        
        /// <summary>
        /// >> InvalidCallOrigin
        /// </summary>
        InvalidCallOrigin = 5,
        
        /// <summary>
        /// >> InvalidSignature
        /// </summary>
        InvalidSignature = 6,
        
        /// <summary>
        /// >> Expired
        /// </summary>
        Expired = 7,
        
        /// <summary>
        /// >> UnsortedApprovals
        /// </summary>
        UnsortedApprovals = 8,
    }
    
    /// <summary>
    /// >> 377 - Variant[pallet_multibatching.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnumRust<Error>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumError()
        {
				AddTypeDecoder<BaseVoid>(Error.AlreadyApplied);
				AddTypeDecoder<BaseVoid>(Error.BatchSenderIsNotOrigin);
				AddTypeDecoder<BaseVoid>(Error.NoCalls);
				AddTypeDecoder<BaseVoid>(Error.NoApprovals);
				AddTypeDecoder<BaseVoid>(Error.InvalidDomain);
				AddTypeDecoder<Substrate.NetApi.Model.Types.Primitive.U16>(Error.InvalidCallOrigin);
				AddTypeDecoder<Substrate.NetApi.Model.Types.Primitive.U16>(Error.InvalidSignature);
				AddTypeDecoder<BaseVoid>(Error.Expired);
				AddTypeDecoder<BaseVoid>(Error.UnsortedApprovals);
        }
    }
}

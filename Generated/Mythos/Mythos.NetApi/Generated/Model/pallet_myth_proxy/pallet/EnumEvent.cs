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


namespace Mythos.NetApi.Generated.Model.pallet_myth_proxy.pallet
{
    
    
    /// <summary>
    /// >> Event
    /// The `Event` enum of this pallet
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> ProxyCreated
        /// A new proxy permission was added.
        /// </summary>
        ProxyCreated = 0,
        
        /// <summary>
        /// >> ProxyRemoved
        /// A proxy permission was removed.
        /// </summary>
        ProxyRemoved = 1,
        
        /// <summary>
        /// >> ProxySponsorshipApproved
        /// Proxy funding was approved.
        /// </summary>
        ProxySponsorshipApproved = 2,
        
        /// <summary>
        /// >> SponsorAgentRegistered
        /// A sponsor agent was registered.
        /// </summary>
        SponsorAgentRegistered = 3,
        
        /// <summary>
        /// >> SponsorAgentRevoked
        /// A sponsor agent was revoked.
        /// </summary>
        SponsorAgentRevoked = 4,
        
        /// <summary>
        /// >> ProxyExecuted
        /// Proxy call was executed.
        /// This event is emitted only when the proxy call is successful.
        /// </summary>
        ProxyExecuted = 5,
    }
    
    /// <summary>
    /// >> 151 - Variant[pallet_myth_proxy.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEvent : BaseEnumRust<Event>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumEvent()
        {
				AddTypeDecoder<BaseTuple<Mythos.NetApi.Generated.Model.account.AccountId20, Mythos.NetApi.Generated.Model.account.AccountId20, Mythos.NetApi.Generated.Model.mainnet_runtime.EnumProxyType, Substrate.NetApi.Model.Types.Base.BaseOpt<Mythos.NetApi.Generated.Model.account.AccountId20>>>(Event.ProxyCreated);
				AddTypeDecoder<BaseTuple<Mythos.NetApi.Generated.Model.account.AccountId20, Mythos.NetApi.Generated.Model.account.AccountId20, Substrate.NetApi.Model.Types.Base.BaseOpt<Mythos.NetApi.Generated.Model.account.AccountId20>>>(Event.ProxyRemoved);
				AddTypeDecoder<BaseTuple<Mythos.NetApi.Generated.Model.account.AccountId20, Mythos.NetApi.Generated.Model.account.AccountId20, Mythos.NetApi.Generated.Model.account.AccountId20>>(Event.ProxySponsorshipApproved);
				AddTypeDecoder<BaseTuple<Mythos.NetApi.Generated.Model.account.AccountId20, Mythos.NetApi.Generated.Model.account.AccountId20>>(Event.SponsorAgentRegistered);
				AddTypeDecoder<BaseTuple<Mythos.NetApi.Generated.Model.account.AccountId20, Mythos.NetApi.Generated.Model.account.AccountId20>>(Event.SponsorAgentRevoked);
				AddTypeDecoder<BaseTuple<Mythos.NetApi.Generated.Model.account.AccountId20, Mythos.NetApi.Generated.Model.account.AccountId20>>(Event.ProxyExecuted);
        }
    }
}

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


namespace InvArch.NetApi.Generated.Model.pallet_checked_inflation.inflation
{
    
    
    /// <summary>
    /// >> InflationMethod
    /// </summary>
    public enum InflationMethod
    {
        
        /// <summary>
        /// >> Rate
        /// </summary>
        Rate = 0,
        
        /// <summary>
        /// >> FixedYearly
        /// </summary>
        FixedYearly = 1,
        
        /// <summary>
        /// >> FixedPerEra
        /// </summary>
        FixedPerEra = 2,
    }
    
    /// <summary>
    /// >> 354 - Variant[pallet_checked_inflation.inflation.InflationMethod]
    /// </summary>
    public sealed class EnumInflationMethod : BaseEnumRust<InflationMethod>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumInflationMethod()
        {
				AddTypeDecoder<InvArch.NetApi.Generated.Model.sp_arithmetic.per_things.Perbill>(InflationMethod.Rate);
				AddTypeDecoder<Substrate.NetApi.Model.Types.Primitive.U128>(InflationMethod.FixedYearly);
				AddTypeDecoder<Substrate.NetApi.Model.Types.Primitive.U128>(InflationMethod.FixedPerEra);
        }
    }
}